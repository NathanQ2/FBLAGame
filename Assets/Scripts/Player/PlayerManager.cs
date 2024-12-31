using Store;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static readonly int StartingMoney = 500;

    public StoreManager StoreManager;
    public PlayerInventory Inventory;

    public int CurrentMoney { get; private set; } = StartingMoney;
    
    public void Start()
    {
        // Test stuff
        TryPurchase(StoreItems.StoreItemType.Seeds);
    }

    public void Update()
    {
        // if (Input.GetButtonDown("ToggleStore"))
        // {
            // TODO: make this open a store gui
        // }
    }

    private bool TryPurchase(StoreItems.StoreItemType type)
    {
        int cost = StoreItems.TypeToCost(type);

        if (cost > CurrentMoney)
            return false;
        
        CurrentMoney -= StoreManager.Purchase(StoreItems.StoreItemType.Seeds, out PlayerInventory.IInventoryItem item);
        Inventory.AddItem(item);

        return true;
    }
}
