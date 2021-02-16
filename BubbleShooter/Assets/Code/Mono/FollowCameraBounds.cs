using Assets.Code.Mono;
using UnityEngine;

public class FollowCameraBounds : MonoBehaviour
{
    private enum TargetBound { Top, Bottom, Left, Right };

    [SerializeField]
    private TargetBound target;

    [Zenject.Inject]
    private CameraBounds cameraBounds;

    private void Update()
    {
        Vector3 targetPos = Vector3.zero;
        switch (target)
        {
            case TargetBound.Top:
                targetPos = cameraBounds.Top;
                break;
            case TargetBound.Bottom:
                targetPos = cameraBounds.Bottom;
                break;
            case TargetBound.Left:
                targetPos = cameraBounds.Left;
                break;
            case TargetBound.Right:
                targetPos = cameraBounds.Right;
                break;
            default:
                break;
        }

        transform.position = targetPos;
    }

}
