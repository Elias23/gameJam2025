using System.Collections.Generic;
using System.Linq;
using Core;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonManager : MonoBehaviour
{
    
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }

    private void HandleBackButton()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainGame":
                SceneManager.LoadScene("MainMenu");
                break;
            case "CreditScene":
                SceneManager.LoadScene("MainMenu");
                break;
            case "MainMenu":
                Application.Quit();
                break;
        }

    }
}