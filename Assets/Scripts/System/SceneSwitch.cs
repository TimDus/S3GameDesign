using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void MenuToGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
