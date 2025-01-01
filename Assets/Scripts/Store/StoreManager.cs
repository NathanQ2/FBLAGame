using Player;
using UnityEngine;

namespace Store
{
    public class StoreManager : MonoBehaviour
    {
        public int Purchase(StoreItems.StoreItemType type, out PlayerInventory.IInventoryItem inventoryItem)
        {
            switch (type)
            {
            case StoreItems.StoreItemType.Seeds:
                inventoryItem = PlayerInventory.Seeds.OneStack;

                return StoreItems.TypeToCost(type);
            // TODO: add the rest of items
            default:
                inventoryItem = null;
                return 0;
            }
        }
    }
}
