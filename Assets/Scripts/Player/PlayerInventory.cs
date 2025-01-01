using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public interface IInventoryItem
        {
            public Sprite GetIcon();
            public int GetCount();
        }

        public class Seeds : IInventoryItem
        {
            public static Sprite Icon;
            public static Seeds OneStack => new Seeds(StackSize);
            public static readonly int StackSize = 100;

            private int m_count;

            public Seeds(int count)
            {
                m_count = Mathf.Clamp(count, 0, StackSize);
            }

            public static Seeds[] FromCount(int count)
            {
                Seeds[] result = new Seeds[Mathf.CeilToInt((float)count / StackSize)];
                int i = 0;
                while (count > 0)
                {
                    result[i] = new Seeds(count);
                    count -= StackSize;
                    i++;
                }

                return result;
            }
        
            public Sprite GetIcon() => Icon;
            public int GetCount() => m_count;

            public override string ToString() => $"ItemType: Seeds, Count: {GetCount()}";
        }

        private List<IInventoryItem> m_inventoryItems = new List<IInventoryItem>();

        public void AddItem(IInventoryItem item)
        {
            m_inventoryItems.Add(item);
        }

        public T[] GetItemsByType<T>() where T : IInventoryItem => m_inventoryItems.OfType<T>().ToArray();

        public int GetCountForType<T>() where T : IInventoryItem => m_inventoryItems.OfType<T>().Sum(inventoryItem => inventoryItem.GetCount());
    }
}
