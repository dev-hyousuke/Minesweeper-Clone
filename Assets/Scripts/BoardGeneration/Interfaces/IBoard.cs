using Assets.Scripts.Models.Structs;

namespace Assets.Scripts.BoardGeneration.Interfaces
{
    public interface IBoard
    {
        void PrepareBoard(int width, int height, int totalMines);
        void Draw(Cell[,] cells);
        void Flag();
        void Reveal();
    }
}
