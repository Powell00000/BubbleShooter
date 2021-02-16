using Assets.Code.Bubbles.Mono;
using Assets.Code.Grid.Cells;
using Assets.Code.Mono;
using Assets.Code.Physics;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static Assets.Code.Physics.PhysicsEx;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private Transform pivot;

    [SerializeField]
    private Transform shootPoint;

    [Zenject.Inject]
    private CameraBounds cameraBounds;

    [Zenject.Inject]
    private ShootingBubble shootingBubblePrefab;

    [SerializeField]
    private LineRenderer shootLine;

    [SerializeField]
    private SpriteRenderer circle;

    [SerializeField]
    private TMPro.TMP_Text nextNumberText;

    private int nextNumber;

    private GameManager gameManager;

    private bool spawnBubble;
    private bool canShoot = false;

    public void Initialize(GameManager gameManager)
    {
        transform.localScale *= GameManager.CellDiameter;
        this.gameManager = gameManager;
        PreloadNextNumber();
        StartCoroutine(ShootLineRoutine());

        gameManager.OnGameEnded += OnGameEnded;
    }

    public void OnGameEnded()
    {
        StopAllCoroutines();
        shootLine.enabled = false;
    }

    public void ShootBubble()
    {
        if (canShoot)
        {
            spawnBubble = true;
        }
    }

    private IEnumerator ShootLineRoutine()
    {
        while (true)
        {
            if (GameManager.Initialized == false)
            {
                yield return null;
                continue;
            }
            //if (!Input.GetMouseButton(1))
            //{
            //    yield return null;
            //    continue;
            //}

            Vector3 mouseWorldPosition = cameraBounds.Cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

            Vector3 normalToTarget = (position - pivot.position).normalized;

            List<Vector3> points = new List<Vector3>();

            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, normalToTarget);
            float zAngle = Vector3.SignedAngle(Vector3.up, normalToTarget, Vector3.forward);
            zAngle = Mathf.Clamp(zAngle, -60, 60);
            Quaternion clampedRotation = Quaternion.Euler(desiredRotation.eulerAngles.x, desiredRotation.eulerAngles.y, zAngle);
            pivot.rotation = clampedRotation;

            Vector3 cannonDirection = clampedRotation * Vector3.up;
            Vector3 currentRayPosition = shootPoint.position;
            Vector3 currentRayDirection = cannonDirection;

            points.Add(currentRayPosition);

            yield return null;

            bool exitLoop = false;
            canShoot = false;
            while (!exitLoop)
            {
                CastResult castResult = PhysicsEx.Cast(currentRayDirection, float.MaxValue, currentRayPosition);

                if (castResult.SomethingHit)
                {
                    currentRayPosition = castResult.ContactPoint;
                    currentRayDirection = castResult.ReflectedDirection;

                    if (castResult.FinalObjectHit)
                    {
                        exitLoop = true;
                    }

                    if (castResult.FoundCell != null)
                    {
                        CellCmp cellCmp = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<CellCmp>(castResult.FoundCell.Entity);
                        if (cellCmp.IsEmpty)
                        {
                            canShoot = true;
                            circle.transform.position = castResult.FoundCell.transform.position;
                            circle.transform.localScale = Vector3.one * GameManager.CellDiameter;
                        }
                    }

                    points.Add(currentRayPosition);
                }

            }

            circle.gameObject.SetActive(canShoot);

            shootLine.positionCount = points.Count;

            for (int i = 0; i < points.Count; i++)
            {
                shootLine.SetPosition(i, points[i]);

                if (i + 1 < points.Count)
                {
                    Debug.DrawLine(points[i], points[i + 1], Color.red);
                }
            }

            shootLine.startColor = shootLine.endColor = canShoot ? Color.green : Color.red;

            yield return null;

            if (spawnBubble)
            {
                ShootingBubble spawnedBubble = Instantiate(shootingBubblePrefab, shootPoint.position, Quaternion.identity, null);
                spawnedBubble.transform.localScale = Vector3.one * GameManager.CellDiameter;
                spawnedBubble.SetNumber(nextNumber);
                spawnedBubble.SetDirection(cannonDirection);
                spawnBubble = false;

                PreloadNextNumber();
            }
        }
    }

    private void PreloadNextNumber()
    {
        nextNumber = gameManager.GetRandomBubbleNumber();
        nextNumberText.text = nextNumber.ToString();
    }
}
