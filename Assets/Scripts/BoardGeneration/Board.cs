using Assets.Scripts.BoardGeneration.Interfaces;
using Assets.Scripts.Models.Enums;
using Assets.Scripts.Models.Structs;
using Assets.Scripts.TileSetup.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.BoardGeneration
{
    public class Board : MonoBehaviour, IBoard
    {
        public Tilemap Tilemap { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MineCount { get; set; }
        public Cell[,] State { get; set; }
        public ITileSetup TileSetup { get; set; }

        private void OnValidate() => MineCount = Mathf.Clamp(MineCount, 0, Width * Height);

        private void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
            TileSetup = GetComponentInParent<TileSetup.TileSetup>();
        }

        public void PrepareBoard(int width, int height, int totalMines)
        {
            Width = width;
            Height = height;
            MineCount = totalMines;
            State = new Cell[Width, Height];

            GenerateCells();
            GenerateMines();
            GenerateNumbers();
            Draw(State);
        }

        public void Draw(Cell[,] cells)
        {
            var width = cells.GetLength(0);
            var height = cells.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cell cell = cells[x, y];
                    Tilemap.SetTile(cell.position, TileSetup.GetTile(cell));
                }
            }
        }

        private void GenerateCells()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cell cell = new()
                    {
                        position = new Vector3Int(x, y, 0),
                        type = EType.Empty
                    };
                    State[x, y] = cell;
                }
            }
        }

        private void GenerateMines()
        {
            for (int i = 0; i < MineCount; i++)
            {
                int x = Random.Range(0, Width);
                int y = Random.Range(0, Height);

                while (State[x, y].type == EType.Mine)
                {
                    x++;

                    if (x >= Width)
                    {
                        x = 0;
                        y++;

                        if (y >= Height)
                        {
                            y = 0;
                        }
                    }
                }

                State[x, y].type = EType.Mine;
            }
        }

        private void GenerateNumbers()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cell cell = State[x, y];

                    if (cell.type == EType.Mine)
                    {
                        continue;
                    }

                    cell.number = CountMines(x, y);

                    if (cell.number > 0)
                    {
                        cell.type = EType.Number;
                    }

                    State[x, y] = cell;
                }
            }
        }

        private int CountMines(int cellX, int cellY)
        {
            int count = 0;

            for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
            {
                for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
                {
                    if (adjacentX == 0 && adjacentY == 0)
                    {
                        continue;
                    }

                    int x = cellX + adjacentX;
                    int y = cellY + adjacentY;

                    if (GetCell(x, y).type == EType.Mine)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public void Flag()
        {
            var cell = GetCellBasedOnMousePosition();

            if (cell.type == EType.Invalid || cell.revealed)
                return;

            cell.flagged = !cell.flagged;
            State[cell.position.x, cell.position.y] = cell;
            Draw(State);
        }

        public void Reveal()
        {
            var cell = GetCellBasedOnMousePosition();

            if (cell.type == EType.Invalid || cell.revealed || cell.flagged)
                return;

            switch (cell.type)
            {
                case EType.Mine:
                    Explode(cell);
                    break;

                case EType.Empty:
                    Flood(cell);
                    CheckWinCondition();
                    break;

                default:
                    cell.revealed = true;
                    State[cell.position.x, cell.position.y] = cell;
                    CheckWinCondition();
                    break;
            }

            Draw(State);
        }

        private void Flood(Cell cell)
        {
            if (cell.revealed) return;
            if (cell.type == EType.Mine || cell.type == EType.Invalid) return;

            // Reveal the cell
            cell.revealed = true;
            State[cell.position.x, cell.position.y] = cell;

            // Keep flooding if the cell is empty, otherwise stop at numbers
            if (cell.type == EType.Empty)
            {
                Flood(GetCell(cell.position.x - 1, cell.position.y));
                Flood(GetCell(cell.position.x + 1, cell.position.y));
                Flood(GetCell(cell.position.x, cell.position.y - 1));
                Flood(GetCell(cell.position.x, cell.position.y + 1));
            }
        }

        private void Explode(Cell cell)
        {
            Debug.Log("Game Over!");
            //gameover = true;

            // Set the mine as exploded
            cell.exploded = true;
            cell.revealed = true;
            State[cell.position.x, cell.position.y] = cell;

            // Reveal all other mines
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cell = State[x, y];

                    if (cell.type == EType.Mine)
                    {
                        cell.revealed = true;
                        State[x, y] = cell;
                    }
                }
            }
        }

        private void CheckWinCondition()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cell cell = State[x, y];

                    // All non-mine cells must be revealed to have won
                    if (cell.type != EType.Mine && !cell.revealed)
                    {
                        return; // no win
                    }
                }
            }

            Debug.Log("Winner!");
            //gameover = true;

            // Flag all the mines
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cell cell = State[x, y];

                    if (cell.type == EType.Mine)
                    {
                        cell.flagged = true;
                        State[x, y] = cell;
                    }
                }
            }
        }

        private Cell GetCellBasedOnMousePosition()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = Tilemap.WorldToCell(worldPosition);
            return GetCell(cellPosition.x, cellPosition.y);
        }

        private Cell GetCell(int x, int y)
        {
            if (IsValid(x, y))
                return State[x, y];
            else
                return new Cell();
        }

        private bool IsValid(int x, int y) => x >= 0 && x<Width && y >= 0 && y<Height;
    }
}