using System;
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

    public void ToggleStore()
    {
        if(StoreUI.activeSelf) CloseStore();
        else OpenStore();
    }

    public void OpenStore()
    {
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
        RefreshInventory();
        InventoryUI.SetActive(true);
    }

    private void RefreshInventory()
    {
        InventoryUI.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText($"Seeds: {PlayerManager.Inventory.GetCountForType<PlayerInventory.Seeds>()}");
    }

    public void CloseInventory()
    {
        InventoryUI.SetActive(false);
    }
}
