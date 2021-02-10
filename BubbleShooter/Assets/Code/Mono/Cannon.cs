using Assets.Code.Bubbles.Mono;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private Transform pivot;

    [SerializeField]
    private Transform shootPoint;

    //[Zenject.Inject]
    private ShootingBubble shootingBubblePrefab;

    [SerializeField]
    private BansheeGz.BGSpline.Curve.BGCurve shootCurve;

    private void Start()
    {
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

        Vector3 forwardLook = (position - pivot.position).normalized;

        pivot.rotation = Quaternion.LookRotation(Vector3.forward, forwardLook);

    }
}
