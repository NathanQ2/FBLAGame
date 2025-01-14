using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject controlPanel;
    public GameObject settingPanel;

    public TMP_Dropdown modeDropdown;
    public TMP_Dropdown filterDropdown;
    
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowControls()
    {

        controlPanel.SetActive(true); // Show the panel

    }

    // Method to close the controls text box
    public void CloseControls()
    {

        controlPanel.SetActive(false); // Hide the panel

    }
    public void toggleControls()
    {
        if (controlPanel.activeSelf) CloseControls();
        else ShowControls();
    }
    public void ShowSettings()
    {
        settingPanel.SetActive(true);
    }
    public void CloseSettings() 
    {
        settingPanel.SetActive(false);
    }
    public void toggleSettings()
    {
        if (settingPanel.activeSelf) CloseSettings();
        else ShowSettings();
    }

    public void OnSettingsChanged()
    {
        PlayerPrefs.SetInt("ColorBlindMode", modeDropdown.value);
        PlayerPrefs.SetInt("ColorBlindFilter", filterDropdown.value);
    }
}

