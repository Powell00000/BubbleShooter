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
        private TMPro.TMP_Text numberText;

        public void CreateAndSetupBubbleEntity(Entity entityToFollow, Scale scale)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new BubbleCmp());
            entityManager.AddComponentData(entity, new NumberCmp());
            entityManager.AddComponentData(entity, new FollowEntityCmp { EntityToFollow = entityToFollow });
            entityManager.SetComponentData(entity, scale);
            entityManager.AddComponentObject(entity, transform);

            entityManager.SetComponentData(entity, entityManager.GetComponentData<Translation>(entityToFollow));
        }

        public void RefreshNumber(int number)
        {
            numberText.text = number.ToString();
        }
    }
}
