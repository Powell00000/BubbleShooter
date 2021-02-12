using Assets.Code.Visuals;
using Unity.Entities;
using UnityEngine;

namespace Assets.Code.DOTS
{
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class InputSystem : SystemBase
    {
        [Zenject.Inject]
        private Cannon cannon;

        private EntityQueryDesc preventInputQueryDesc = new EntityQueryDesc
        {
            Any = new ComponentType[]
            {
                typeof(VisualsInProgressTagCmp),
            }
        };
        private EntityQuery preventInputQuery;

        protected override void OnCreate()
        {
            preventInputQuery = GetEntityQuery(preventInputQueryDesc);
        }

        protected override void OnUpdate()
        {
            if (!preventInputQuery.IsEmpty)
            {
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                cannon.ShootBubble();
            }
        }
    }
}
