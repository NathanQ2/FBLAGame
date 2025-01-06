using Player;
using Store;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static readonly int StartingMoney = 500;

    public UIManager UIManager;
    public StoreManager StoreManager;
    public PlayerInventory Inventory;

    public int CurrentMoney { get; private set; } = StartingMoney;
    
    public void Start()
    {
    }

    public void Update()
    {
        // if (Input.GetButtonDown("ToggleStore"))
        // {
            // TODO: make this open a store gui
        // }

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

    private bool TryPurchase(StoreItems.StoreItemType type)
    {
        int cost = StoreItems.TypeToCost(type);

        if (cost > CurrentMoney)
            return false;
        
        CurrentMoney -= StoreManager.Purchase(StoreItems.StoreItemType.Seeds, out PlayerInventory.IInventoryItem item);
        Inventory.AddItem(item);

        return true;
    }

    public void AddMoney(int money)
    {
        CurrentMoney += money;
    }

    private void ToggleInventory()
    {
        UIManager.ToggleInventory();
    }
}
