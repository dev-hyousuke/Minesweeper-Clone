using Assets.Scripts.Models.Enums;
using Assets.Scripts.Models.Structs;
using Assets.Scripts.TileSetup.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.TileSetup
{
    public class TileSetup : MonoBehaviour, ITileSetup
    {
        [SerializeField] public Tile tileUnknown;
        [SerializeField] public Tile tileEmpty;
        [SerializeField] public Tile tileMine;
        [SerializeField] public Tile tileExploded;
        [SerializeField] public Tile tileFlag;
        [SerializeField] public Tile tileNum1;
        [SerializeField] public Tile tileNum2;
        [SerializeField] public Tile tileNum3;
        [SerializeField] public Tile tileNum4;
        [SerializeField] public Tile tileNum5;
        [SerializeField] public Tile tileNum6;
        [SerializeField] public Tile tileNum7;
        [SerializeField] public Tile tileNum8;

        public Tile GetTile(Cell cell)
        {
            if (cell.revealed)
                return GetRevealedTile(cell);
            else if (cell.flagged)
                return tileFlag;
            else
                return tileUnknown;
        }

        private Tile GetRevealedTile(Cell cell)
        {
            return cell.type switch
            {
                EType.Empty => tileEmpty,
                EType.Mine => cell.exploded ? tileExploded : tileMine,
                EType.Number => GetNumberTile(cell),
                _ => null
            };
        }

        private Tile GetNumberTile(Cell cell)
        {
            return cell.number switch
            {
                1 => tileNum1,
                2 => tileNum2,
                3 => tileNum3,
                4 => tileNum4,
                5 => tileNum5,
                6 => tileNum6,
                7 => tileNum7,
                8 => tileNum8,
                _ => null
            };
        }
    }
}