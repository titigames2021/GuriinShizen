using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public GameObject continueUI;

    private void Update()
    {
        
    }


    public void StartLevelMainMenu(int level)
    {
        SceneManager.LoadScene(level);
    }

   

    public void ExitGameMenu()
    {
        Application.Quit();

    }



}
