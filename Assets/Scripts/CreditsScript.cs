using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public void GoHomeFromScripts()
    {
        SoundManager.Instance.PlayMenuButtonSound();
        Debug.Log("GoHomeFromScripts method called. Loading the MainMenu scene...");
        SceneManager.LoadSceneAsync("MainMenu");
    }
}