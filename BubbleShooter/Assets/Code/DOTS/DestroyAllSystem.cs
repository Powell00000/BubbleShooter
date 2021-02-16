using Unity.Entities;

namespace Assets.Code.DOTS
{
    internal class DestroyAllSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<DestroyAllTagCmp>();
        }
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e) =>
                {
                    EntityManager.AddComponentData(e, new DestroyTagCmp());
                })
                .Run();

            EntityManager.DestroyEntity(GetSingletonEntity<DestroyAllTagCmp>());
        }
    }
}
