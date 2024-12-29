using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public enum ItemType
    {
        Seeds
    }
    public interface IInventoryItem
    {
        public Sprite GetIcon();
        public int GetCount();
        public ItemType GetType();
    }

    public class Seeds : IInventoryItem
    {
        public static Sprite Icon; 
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
        public new ItemType GetType() => ItemType.Seeds;
    }

    private List<IInventoryItem> m_inventoryItems = new List<IInventoryItem>();

    public void Start()
    {
        m_inventoryItems.AddRange(Seeds.FromCount(250));
    }
}
