using Assets.Code.Bubbles.Mono;
using Assets.Code.Grid.Hybrid;
using Assets.Code.Mono;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
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
            Vector3 rayStartPosition = shootPoint.position;
            Vector3 currentRayDirection = cannonDirection;

            Debug.DrawRay(rayStartPosition, cannonDirection * 20, Color.red);

            points.Add(rayStartPosition);

            yield return null;

            bool exitLoop = false;
            while (!exitLoop)
            {
                float radius = GameManager.CellDiameter / 2;
                if (Physics.SphereCast(rayStartPosition, radius, currentRayDirection, out var raycastHit, 200, LayerMask.GetMask("Default", "Bubble")))
                {
                    Vector3 sphereContactPoint = raycastHit.point + raycastHit.normal * radius;

                    currentRayDirection = Vector3.Reflect(currentRayDirection, raycastHit.normal);
                    rayStartPosition = sphereContactPoint;

                    Debug.DrawRay(rayStartPosition, currentRayDirection * 20, Color.red);

                    points.Add(sphereContactPoint);

                    if (raycastHit.rigidbody.gameObject.name == "TopWall" || raycastHit.rigidbody.gameObject.layer == LayerMask.NameToLayer("Bubble"))
                    {
                        exitLoop = true;
                        Cell cell = ShootingBubble.GetCellNearPosition(raycastHit.point + (currentRayDirection * -1), radius);
                        if (cell)
                        {
                            cell.gizmos = true;
                        }
                    }
                }
            }

            shootLine.positionCount = points.Count;

            for (int i = 0; i < points.Count; i++)
            {
                shootLine.SetPosition(i, points[i]);
            }

            yield return null;

            if (spawnBubble)
            {
                ShootingBubble spawnedBubble = Instantiate(shootingBubblePrefab, shootPoint.position, Quaternion.identity, null);
                spawnedBubble.SetDirection(cannonDirection);
                spawnBubble = false;
            }
        }
    }
}
