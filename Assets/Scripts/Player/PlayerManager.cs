using Player;
using Store;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public UIManager UIManager;
    public StoreManager StoreManager;
    public PlayerInventory Inventory;

    public int CurrentMoney { get; private set; } = GameplayConfig.StartingMoney;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIManager.ToggleStore();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void TryPurchaseSeeds()
    {
        TryPurchase(StoreItems.StoreItemType.Seeds);
    }

    public void TryPurchasePesticide()
    {
        TryPurchase(StoreItems.StoreItemType.Pesticide);
    }

    private bool TryPurchase(StoreItems.StoreItemType type)
    {
        int cost = StoreItems.TypeToCost(type);

        if (cost > CurrentMoney)
            return false;
        
        CurrentMoney -= StoreManager.Purchase(type, out PlayerInventory.IInventoryItem item);
        if (item is not null)
            Inventory.AddItem(item);

        return true;
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
    }

    public void RemoveMoney(int amount)
    {
        CurrentMoney -= amount;
    }

    private void ToggleInventory()
    {
        UIManager.ToggleInventory();
    }
}
