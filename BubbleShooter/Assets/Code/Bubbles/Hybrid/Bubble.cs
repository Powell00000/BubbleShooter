using Assets.Code.Bubbles.Nodes;
using Assets.Code.DOTS;
using Assets.Code.Hybrid;
using Assets.Code.Movement.Follow;
using Assets.Code.Visuals;
using DG.Tweening;
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

        [SerializeField]
        private GameObject visualsParent;

        private int number;

        public int Number => number;

        public void CreateAndSetupBubbleEntity(Entity entityToFollow, Scale scale)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new BubbleCmp());
            entityManager.AddComponentData(entity, new NumberCmp());
            entityManager.AddComponentData(entity, new FollowEntityCmp { EntityToFollow = entityToFollow });
            entityManager.AddSharedComponentData(entity, new Grid.Row.RowSharedCmp());
            entityManager.SetComponentData(entity, scale);
            entityManager.AddComponentObject(entity, transform);
            entityManager.AddBuffer<NodeNeighboursCmp>(entity);

            entityManager.SetComponentData(entity, entityManager.GetComponentData<Translation>(entityToFollow));
        }

        private void PingScale()
        {
            visualsParent.transform.localScale = Vector3.one * 3;
            visualsParent.transform.DOScale(Vector3.one, 0.4f).OnComplete(() => entityManager.RemoveComponent<IsAnimatingTagCmp>(entity));
        }

        public void RefreshNumber(int number, EntityCommandBuffer ecb)
        {
            if (this.number < number && !entityManager.HasComponent<JustSpawnedTagCmp>(entity))
            {
                ecb.AddComponent(entity, new IsAnimatingTagCmp());
                PingScale();
            }

            this.number = number;
            numberText.text = number.ToString();
        }
    }
}
