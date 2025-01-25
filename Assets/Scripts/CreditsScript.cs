using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsScript : MonoBehaviour
{
    public void GoHomeFromScripts()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
