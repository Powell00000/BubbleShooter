using UnityEngine;

namespace Assets.Code.Mono
{
    internal class CameraBounds : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        [SerializeField]
        private float ZFactor = 10;

        public Vector3 Top { get; protected set; }
        public Vector3 Bottom { get; protected set; }
        public Vector3 Left { get; protected set; }
        public Vector3 Right { get; protected set; }

        private void Start()
        {
            Top = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, ZFactor));
            Bottom = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, ZFactor));
            Left = cam.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, ZFactor));
            Right = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, ZFactor));
        }
    }
}
