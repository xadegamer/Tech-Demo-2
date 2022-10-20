using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenuCam;
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
        MainMenuCam.GetComponent<Animator>().SetBool("MainMenu", true);
        MainMenuCam.GetComponent<Animator>().SetBool("Settings", false);
    }

    public void LoadSetings()
    {
        MainMenuCam.GetComponent<Animator>().SetBool("Settings", true);
        MainMenuCam.GetComponent<Animator>().SetBool("MainMenu", false);
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
