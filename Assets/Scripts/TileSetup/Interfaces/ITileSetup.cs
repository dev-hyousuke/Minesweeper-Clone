using Assets.Scripts.Models.Structs;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.TileSetup.Interfaces
{
    public interface ITileSetup
    {
        Tile GetTile(Cell cell);
    }
}
