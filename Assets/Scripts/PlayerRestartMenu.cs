using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRestartMenu : MonoBehaviour
{
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Canvas menuCanvas;
    [SerializeField] GameObject playerObj;

    public void OnSettingsChosen()
    {
        settingsCanvas.gameObject.SetActive(true);
        menuCanvas.enabled = false;

    }

    public void OnSettingsClose()
    {
        settingsCanvas.gameObject.SetActive(false);
        menuCanvas.enabled = true;
    }

    public void OnRestart()
    {
        int currScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currScene);
    }

    public void OnQuit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SoundToggle(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
