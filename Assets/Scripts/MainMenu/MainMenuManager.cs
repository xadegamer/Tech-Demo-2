using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenuCam;
    public GameObject SettingsCam;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MainMenu()
    {
        SettingsCam.SetActive(false);
        MainMenuCam.SetActive(true);
    }

    public void LoadSetings()
    {
        SettingsCam.SetActive(true);
        MainMenuCam.SetActive(false);
    }

    public void LoadGame(int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
