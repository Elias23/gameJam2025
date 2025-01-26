using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SoundManager.Instance.PlayMenuButtonSound();
        SceneManager.LoadSceneAsync("MainGame");
    }

    public void ShowCredits()
    {
        SoundManager.Instance.PlayMenuButtonSound();
        SceneManager.LoadSceneAsync("CreditsScene");
    }

    public void ResetTutorial()
    {
        SoundManager.Instance.PlayMenuButtonSound();
        GameStore.ResetTutorialState();
    }
}