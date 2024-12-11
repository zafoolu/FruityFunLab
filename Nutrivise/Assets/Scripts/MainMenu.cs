using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("ui_click");
        SceneManager.LoadScene("Level1");      //
    }
    public void QuitGame()
    {
        Application.Quit();     //schlie�t Spiel
    }

    public void LoadLibrary()
    {
         FindObjectOfType<AudioManager>().Play("ui_click");
        SceneManager.LoadScene("CardCollection");
    }

}
