using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void OnClick()
    {
        int currIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currIndex+ 1);
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
