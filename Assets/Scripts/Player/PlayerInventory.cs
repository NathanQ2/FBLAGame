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
            public void SetCount(int count);
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

            public static IInventoryItem[] FromCount(int count)
            {
                IInventoryItem[] result = new IInventoryItem[Mathf.CeilToInt((float)count / StackSize)];
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

            public void SetCount(int count)
            {
                m_count = Mathf.Clamp(count, 0, StackSize);
            }

            public override string ToString() => $"ItemType: Seeds, Count: {GetCount()}";
        }

        public class Wheat : IInventoryItem
        {
            public static Sprite Icon;
            
            public static Wheat OneStack => new Wheat(StackSize);
            public static readonly int StackSize = 50;

            private int m_count;

            public Wheat(int count)
            {
                m_count = Mathf.Clamp(count, 0, StackSize);
            }

            public static IInventoryItem[] FromCount(int count)
            {
                Wheat[] result = new Wheat[Mathf.CeilToInt((float)count / StackSize)];
                int i = 0;

                while (count > 0)
                {
                    result[i] = new Wheat(count);
                    count -= StackSize;
                    i++;
                }

                return result;
            }
            
            public Sprite GetIcon() => Icon;
            public int GetCount() => m_count; 

            public void SetCount(int count)
            {
                m_count = Mathf.Clamp(count, 0, StackSize);
            }
            
            public override string ToString() => $"ItemType: Seeds, Count: {GetCount()}";
        }

        public class Pesticide : IInventoryItem
        {
            public static Sprite Icon;
            public static Pesticide OneStack => new Pesticide(StackSize);
            public static readonly int StackSize = 10;

            private int m_count;

            public Pesticide(int count)
            {
                m_count = Mathf.Clamp(count, 0, StackSize);
            }

            public static IInventoryItem[] FromCount(int count)
            {
                IInventoryItem[] result = new IInventoryItem[Mathf.CeilToInt((float)count / StackSize)];
                int i = 0;

                while (count > 0)
                {
                    result[i] = new Pesticide(count);
                    count -= StackSize;
                    i++;
                }

                return result;
            }

            public Sprite GetIcon() => Icon;
            public int GetCount() => m_count;

            public void SetCount(int count)
            {
                m_count = Mathf.Clamp(count, 0, StackSize);
            }

            public override string ToString() => $"ItemType: Pesticide, Count: {GetCount()}";
        }
        
        private List<IInventoryItem> m_inventoryItems = new List<IInventoryItem>();

        public void AddItem(IInventoryItem item)
        {
            m_inventoryItems.Add(item);
        }

        public void AddItems(IInventoryItem[] items)
        {
            foreach (IInventoryItem item in items)
            {
                AddItem(item);
            }
        }

        public void RemoveTypeByCount<T>(int count)
        {
            foreach (IInventoryItem item in m_inventoryItems.OfType<T>().ToList())
            {
                if (item.GetCount() - count <= 0)
                {
                    m_inventoryItems.Remove(item);
                }
                else
                {
                    item.SetCount(item.GetCount() - count);
                }
            }
        }

        public T[] GetItemsByType<T>() where T : IInventoryItem => m_inventoryItems.OfType<T>().ToArray();

        public int GetCountForType<T>() where T : IInventoryItem => m_inventoryItems.OfType<T>().Sum(inventoryItem => inventoryItem.GetCount());
    }
}
