using Assets.Scripts.Models.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.Structs
{
    public struct Cell
    {
        public Vector3Int position;
        public EType type;
        public int number;
        public bool revealed;
        public bool flagged;
        public bool exploded;
    }
}
