using Assets.Code.Bubbles.Nodes;
using Assets.Code.Hybrid;
using Assets.Code.Movement.Follow;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Bubbles.Hybrid
{
    internal class Bubble : HybridMonoBase
    {
        [SerializeField]
        private SphereCollider sphereCollider;

        [SerializeField]
        private TMPro.TMP_Text numberText;

        private int number;

        public int Number => number;

        public void CreateAndSetupBubbleEntity(Entity entityToFollow, Scale scale)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new BubbleCmp());
            entityManager.AddComponentData(entity, new NumberCmp());
            entityManager.AddComponentData(entity, new FollowEntityCmp { EntityToFollow = entityToFollow });
            entityManager.SetComponentData(entity, scale);
            entityManager.AddComponentObject(entity, transform);
            entityManager.AddBuffer<NodeNeighboursCmp>(entity);

            entityManager.SetComponentData(entity, entityManager.GetComponentData<Translation>(entityToFollow));
        }

        public void RefreshNumber(int number)
        {
            this.number = number;
            numberText.text = number.ToString();
        }

        private void OnDrawGizmosSelected()
        {
            //Handles.Label(transform.position, $"Has connection: {entityManager.HasComponent<HasConnectionWithTopRowTagCmp>(entity)}");
        }
    }
}
