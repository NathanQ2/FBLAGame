using System;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject StoreUI;

    public void Start()
    {
        CloseStore();
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
}
