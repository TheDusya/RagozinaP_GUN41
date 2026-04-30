using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OpenMainScene() => SceneManager.LoadScene(0);
    public void OpenGameScene() => SceneManager.LoadScene(1, LoadSceneMode.Additive);
    public void ReloadGameScene()
    {
        SceneManager.UnloadSceneAsync(1);
        OpenGameScene();
    }
}
