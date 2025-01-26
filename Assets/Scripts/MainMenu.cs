using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("MainGame");
    }

    public void ShowCredits()
    {
        SceneManager.LoadSceneAsync("CreditsScene");
    }

    public void ResetTutorial()
    {
        GameStore.ResetTutorialState();
    }
}