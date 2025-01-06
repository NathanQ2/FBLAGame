using System.Linq;
using Player;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum MenuType
    {
        Store,
        Inventory
    }
    
    public GameObject StoreUI;
    public GameObject InventoryUI;

    private GameObject[] Menus => new[] { StoreUI, InventoryUI };

    public PlayerManager PlayerManager;

    public void Start()
    {
        CloseAll();
    }

    public void Open(MenuType menuType)
    {
        CloseAll();
        
        switch (menuType)
        {
        case MenuType.Store:
            OpenStore();

            break;
        case MenuType.Inventory:
            OpenInventory();

            break;
        default:
            break;
        }
    }

    public void Toggle(MenuType menuType)
    {
        switch (menuType)
        {
        case MenuType.Store:
            ToggleStore();

            break;
        case MenuType.Inventory:
            ToggleInventory();

            break;
        }
    }

    public void CloseAll()
    {
        StoreUI.SetActive(false);
        InventoryUI.SetActive(false);
    }

    public bool IsAnyOpen() => Menus.Any(obj => obj.activeSelf);

    public void ToggleStore()
    {
        if(StoreUI.activeSelf) CloseStore();
        else OpenStore();
    }

    public void OpenStore()
    {
        CloseAll();
        StoreUI.SetActive(true);
    }

    public void CloseStore()
    {
        StoreUI.SetActive(false);
    }

    public void ToggleInventory()
    {
        if (InventoryUI.activeSelf) CloseInventory();
        else OpenInventory();
    }

    public void OpenInventory()
    {
        CloseAll();
        RefreshInventory();
        InventoryUI.SetActive(true);
    }

    private void RefreshInventory()
    {
        TextMeshProUGUI[] texts = InventoryUI.GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].SetText($"Money: {PlayerManager.CurrentMoney}");
        texts[2].SetText($"Seeds: {PlayerManager.Inventory.GetCountForType<PlayerInventory.Seeds>()}");
        texts[3].SetText($"Wheat: {PlayerManager.Inventory.GetCountForType<PlayerInventory.Wheat>()}");
    }

    public void CloseInventory()
    {
        InventoryUI.SetActive(false);
    }
}
