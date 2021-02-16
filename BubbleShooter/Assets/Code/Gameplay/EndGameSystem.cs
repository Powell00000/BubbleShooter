using Assets.Code.Bubbles;
using Assets.Code.DOTS;
using Assets.Code.Grid.Cells;
using Assets.Code.Grid.Row;
using Unity.Entities;

namespace Assets.Code.Gameplay
{
    internal class EndGameSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private GameManager gameManager;

        protected override void OnUpdate()
        {

            Entities
                .WithoutBurst()
                .WithAll<BubbleCmp>()
                .ForEach((Entity e, RowSharedCmp rowCmp) =>
                {
                    if (rowCmp.RowNumber >= GameManager.MaxRowsCount)
                    {
                        gameManager.EndGame();
                    }
                })
                .Run();

            Entities
                .WithoutBurst()
                .WithAll<DestroyTagCmp>()
                .ForEach((Entity e, in CellCmp cellCmp) =>
                {
                    if (cellCmp.IsEmpty == false)
                    {
                        gameManager.EndGame();
                    }
                })
                .Run();
        }
    }
}
