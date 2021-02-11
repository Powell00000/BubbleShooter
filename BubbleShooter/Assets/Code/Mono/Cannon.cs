using Assets.Code.Bubbles.Mono;
using Assets.Code.Grid.Cells;
using Assets.Code.Mono;
using Assets.Code.PhysicsEx;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

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

    private bool spawnBubble;

    public void Initialize()
    {
        transform.localScale *= GameManager.CellDiameter;
        StartCoroutine(ShootLineRoutine());
    }

    private void Update()
    {
        if (GameManager.Initialized == false)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
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
            while (!exitLoop)
            {
                bool cellFound = PhysicsEx.TryCastForCell(
                    currentRayDirection,
                    float.MaxValue,
                    currentRayPosition,
                    out var reflectedDirection,
                    out var contactPosition,
                    out var foundCell);

                currentRayPosition = contactPosition;
                currentRayDirection = reflectedDirection;

                if (cellFound)
                {
                    exitLoop = true;
                    World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(foundCell.Entity, new MarkAsSelectedCmp());
                }
                points.Add(currentRayPosition);
            }

            shootLine.positionCount = points.Count;

            for (int i = 0; i < points.Count; i++)
            {
                shootLine.SetPosition(i, points[i]);

                if (i + 1 < points.Count)
                {
                    Debug.DrawLine(points[i], points[i + 1], Color.red);
                }
            }

            yield return null;

            if (spawnBubble)
            {
                ShootingBubble spawnedBubble = Instantiate(shootingBubblePrefab, shootPoint.position, Quaternion.identity, null);
                spawnedBubble.transform.localScale = Vector3.one * GameManager.CellDiameter;
                spawnedBubble.SetDirection(cannonDirection);
                spawnBubble = false;
            }
        }
    }
}
