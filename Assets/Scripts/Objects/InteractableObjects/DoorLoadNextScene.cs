using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLoadNextScene : MonoBehaviour
{
    public GameObject loadingScreen;

    public void LoadSceneByID()
    {
        int sceneID = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
