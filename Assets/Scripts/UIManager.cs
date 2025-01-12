using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Timer = Util.Timer;

public class UIManager : MonoBehaviour
{
    public enum MenuType
    {
        Store,
        Inventory
    }
    
    public GameObject StoreUI;
    public GameObject InventoryUI;

    [FormerlySerializedAs("MoneyText")]
    public TextMeshProUGUI InfoText;

    private GameObject[] Menus => new[] { StoreUI, InventoryUI };

    public PlayerManager PlayerManager;
    
    public GameObject PenaltyNotificationPrefab;

    private List<Timer> m_timers = new List<Timer>();

    public void Start()
    {
        CloseAll();
    }

    public void Update()
    {
        m_timers.ForEach(timer => timer.Update(Time.deltaTime));
        m_timers.RemoveAll(timer => timer.IsFinished());

        InfoText.text = @$"Current Day: {PlayerManager.GameManager.CurrentDay}
Score: {PlayerManager.Score}
Money: ${PlayerManager.CurrentMoney}
Control Mode: {ControlModeUtils.ToString(PlayerManager.PlayerController.ActiveMode)}
Seeds: {PlayerManager.PlayerInventory.GetCountForType<PlayerInventory.Seeds>()}
";
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

    public void InstantiatePenaltyNotification(Penalties.IPenalty penalty, int cost)
    {
        GameObject obj = Instantiate(PenaltyNotificationPrefab, gameObject.transform);
        obj.SetActive(false);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1920 / 2 + 250.0f, 1080 / 2 - 100.0f);
        
        TextMeshProUGUI[] texts = obj.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = $"{penalty.Cause} Penalty!";
        texts[1].text = penalty.Description;
        texts[2].text = $"Cost: ${cost}";
        
        m_timers.Add(new Timer(10, () => Destroy(obj)).Start());
        
        obj.SetActive(true);
    }
    
    private void RefreshInventory()
    {
        TextMeshProUGUI[] texts = InventoryUI.GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].SetText($"Money: ${PlayerManager.CurrentMoney}");
        texts[2].SetText($"Seeds: {PlayerManager.PlayerInventory.GetCountForType<PlayerInventory.Seeds>()}");
        texts[3].SetText($"Pesticides Used: {PlayerManager.GameManager.PesticidesUsed}");
        // texts[3].SetText($"Wheat: {PlayerManager.Inventory.GetCountForType<PlayerInventory.Wheat>()}");
    }

    public void CloseInventory()
    {
        InventoryUI.SetActive(false);
    }
}
