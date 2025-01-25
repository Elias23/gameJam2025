using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour
{
    public void GoHome()
    {
        Debug.Log("GoHome method called. Loading the MainMenu scene...");
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void Restart()
    {
        Debug.Log("Restrart the game to try again...");
        SceneManager.LoadSceneAsync("MainGame");

    }

}
