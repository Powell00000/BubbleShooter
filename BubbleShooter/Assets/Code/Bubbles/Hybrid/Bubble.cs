using Assets.Code.Bubbles.Connections;
using Assets.Code.Bubbles.Explosion;
using Assets.Code.Bubbles.Nodes;
using Assets.Code.DOTS;
using Assets.Code.Hybrid;
using Assets.Code.Movement.Follow;
using Assets.Code.Visuals;
using DG.Tweening;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor;
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

        [SerializeField]
        private GameObject visuals;

        [SerializeField]
        private ParticleSystem explosionParticles;

        private int visualsPlaying = 0;
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

        private void AddVisuals()
        {
            if (visualsPlaying == 0)
            {
                entityManager.AddComponentData(entity, new IsAnimatingTagCmp());
            }

            visualsPlaying++;
        }

        private void RemoveVisuals()
        {
            visualsPlaying--;
            if (visualsPlaying == 0)
            {
                entityManager.RemoveComponent<IsAnimatingTagCmp>(entity);
            }
        }

        private void PingScale()
        {
            AddVisuals();
            visualsParent.transform.localScale = Vector3.one * 3;
            visualsParent.transform.DOScale(Vector3.one, 0.4f).OnComplete(() => RemoveVisuals());
        }

        public void Explode()
        {
            entityManager.RemoveComponent<ExplodeTagCmp>(entity);
            AddVisuals();
            explosionParticles.Play();
            visuals.transform.DOScale(Vector3.zero, explosionParticles.main.startLifetime.constant).OnComplete(() =>
            {
                RemoveVisuals();
                entityManager.AddComponentData(entity, new DestroyTagCmp());
            });
        }

        public void RefreshNumber(int number)
        {
            if (this.number < number && !entityManager.HasComponent<JustSpawnedTagCmp>(entity))
            {
                PingScale();
            }

            this.number = number;
            numberText.text = number.ToString();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Handles.Label(transform.position + Vector3.down * 0.1f, $"Has connection: {entityManager.HasComponent<HasConnectionWithTopRowTagCmp>(entity)}");
            }
        }
#endif
    }
}
