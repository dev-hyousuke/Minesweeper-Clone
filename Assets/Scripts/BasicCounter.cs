using UnityEngine;

namespace Assets.Scripts
{
    public class BasicCounter : ICounter
    {
        public const int MaxCount = 10;

        public BasicCounter(int count = 0) => Count = count;

        public void Increment() => Count = Mathf.Min(MaxCount, Count + 1);

        public int Count { get; set; }
    }
}