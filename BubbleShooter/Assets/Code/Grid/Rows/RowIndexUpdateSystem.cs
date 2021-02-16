using Assets.Code.Bubbles.Hybrid;
using Assets.Code.DOTS;
using Assets.Code.Grid.Row;
using Assets.Code.Grid.Spawn;
using Unity.Entities;

namespace Assets.Code.Grid.Rows
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(BubbleSpawnSystem))]
    internal class RowIndexUpdateSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<RowSpawnedTagCmp>();
        }

        protected override void OnUpdate()
        {
            Entities
                .WithNone<JustSpawnedTagCmp, DestroyTagCmp>()
                .ForEach((Entity e, ref RowSharedCmp rowCmp) =>
                {
                    rowCmp.RowNumber++;
                })
                .Schedule();
        }
    }
}
