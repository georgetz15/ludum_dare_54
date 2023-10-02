using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private Toggle MenuToggle;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private TMP_Text muteButtonText;

    public void ToggleMenu()
    {
        if (Menu.activeSelf)
        {
            Menu.SetActive(false);
            MenuToggle.isOn = false;
        }
        else
        {
            Menu.SetActive(true);
            MenuToggle.isOn = true;
        }
    }

    public void RestartSecene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CloseMenu()
    {
        if (Menu.activeSelf)
        {
            ToggleMenu();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Help()
    {
        MessageBox.Instance.DisplayMsgWithoutAutohide("- Click on a planet to travel\n\n" +
                                                      "- Hover over missions to see origin and destination\n\n" +
                                                      "- Click on a mission to load cargo\n\n" +
                                                      "- Travel to destination to auto-complete mission\n\n" +
                                                      "- Upgrade your ship's cargo capacity and range of travel");
    }

    public void Mute()
    {
        audioListener.enabled = !audioListener.enabled;
        muteButtonText.text = audioListener.enabled ? "MUTE" : "UN-MUTE";
    }
}