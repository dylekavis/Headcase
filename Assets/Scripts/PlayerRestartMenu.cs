using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRestartMenu : MonoBehaviour
{
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Canvas menuCanvas;
    [SerializeField] GameObject playerObj;

    public void OnRestart()
    {
        int currScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currScene);
        Time.timeScale = 1;
    }

    public void OnQuit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
