using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void GoHome()
    {
        SoundManager.Instance.PlayMenuButtonSound();
        Debug.Log("GoHome method called. Loading the MainMenu scene...");
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void Restart()
    {
        SoundManager.Instance.PlayMenuButtonSound();
        Debug.Log("Restart the game to try again...");
        SceneManager.LoadSceneAsync("MainGame");
    }
}