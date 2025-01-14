using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject controlPanel;
    public GameObject settingPanel;
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
}

