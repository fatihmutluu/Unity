using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public GameObject settingsPanel;

    public void PlayClicked()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SettingsClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsClicked()
    {
        settingsPanel.SetActive(false);
    }

    public void MuteClicked()
    {
        AudioListener.pause = !AudioListener.pause;
        if (AudioListener.pause)
        {
            settingsPanel.transform.Find("Mute Button").GetComponent<UnityEngine.UI.Image>().color =
                Color.white;
            settingsPanel
                .transform.Find("Mute Button")
                .GetChild(0)
                .GetComponent<UnityEngine.UI.Image>()
                .enabled = false;
        }
        else
        {
            settingsPanel.transform.Find("Mute Button").GetComponent<UnityEngine.UI.Image>().color =
                Color.red;

            settingsPanel
                .transform.Find("Mute Button")
                .GetChild(0)
                .GetComponent<UnityEngine.UI.Image>()
                .enabled = true;
        }
    }
}
