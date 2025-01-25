using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour
{
    public void GoHome()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
