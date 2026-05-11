using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ReloadGameScene() => SceneManager.LoadScene(0);
}
