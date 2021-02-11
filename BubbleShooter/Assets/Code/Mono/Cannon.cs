using Assets.Code.Bubbles.Mono;
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

    private ShootingBubble shootingBubblePrefab;

    [SerializeField]
    private LineRenderer shootLine;

    private void Start()
    {
        StartCoroutine(ShootLineRoutine());
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {

        }
    }

    private IEnumerator ShootLineRoutine()
    {
        while (true)
        {
            Vector3 mouseWorldPosition = cameraBounds.Cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

            Vector3 normalToTarget = (position - pivot.position).normalized;

            List<Vector3> points = new List<Vector3>();

            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, normalToTarget);
            float zAngle = Vector3.SignedAngle(Vector3.up, normalToTarget, Vector3.forward);
            zAngle = Mathf.Clamp(zAngle, -60, 60);
            Quaternion clampedRotation = Quaternion.Euler(desiredRotation.eulerAngles.x, desiredRotation.eulerAngles.y, zAngle);
            pivot.rotation = clampedRotation;

            Vector3 lookNormal = clampedRotation * Vector3.up;

            Vector3 rayForward = lookNormal;
            Vector3 rayStartPosition = shootPoint.position;

            Debug.DrawRay(rayStartPosition, rayForward * 20, Color.red);

            points.Add(rayStartPosition);

            yield return null;

            bool exitLoop = false;
            while (!exitLoop)
            {
                Ray ray = new Ray()
                {
                    direction = rayForward,
                    origin = rayStartPosition
                };

                if (Physics.Raycast(ray, out var raycastHit))
                {
                    rayForward = Vector3.Reflect(ray.direction, raycastHit.normal);
                    rayStartPosition = raycastHit.point;

                    Debug.DrawRay(rayStartPosition, rayForward * 20, Color.red);
                    points.Add(raycastHit.point);

                    if (raycastHit.rigidbody.gameObject.name == "TopWall")
                    {
                        exitLoop = true;
                    }
                }
            }

            shootLine.positionCount = points.Count;

            for (int i = 0; i < points.Count; i++)
            {
                shootLine.SetPosition(i, points[i]);
            }

            yield return null;
        }
    }
}
