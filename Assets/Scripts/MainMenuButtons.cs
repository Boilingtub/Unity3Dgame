using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LevelCreateMenu;
    public GameObject OptionsMenu;
    public GameObject LevelsMenu;
    public GameObject LoadMenu;

 void Start()
  {
       FindObjectOfType<AudioManagerScript>().Play("MainMenuTheme");
       MainMenu = GameObject.Find("MainMenuPanel");
       LevelCreateMenu = GameObject.Find("LevelCreateMenuPanel");
       OptionsMenu = GameObject.Find("OptionsMenuPanel");
       LevelsMenu = GameObject.Find("LevelSelectPanel");
       LoadMenu = GameObject.Find("LoadMenuPanel");
       LevelCreateMenu.SetActive(false);
       OptionsMenu.SetActive(false);
       LevelsMenu.SetActive(false);
       LoadMenu.SetActive(false);
       StartCoroutine(MainMenuLoadValues());
  }

IEnumerator MainMenuLoadValues()
{
yield return 0f;
SaveAndLoad.LoadingnewScene = false;
GameObject.Find("Level Manager").GetComponent<SaveAndLoad>().LoadByDeSerialisation();
}

  public void LoadLevel(string Leveltoload)
  {
    LoadingScreen.level = Leveltoload;
    SceneManager.LoadScene(0);
  }  
  public void QuitGame()
  {
      Application.Quit();
  }
  public void OpenOptions()
  {
      MainMenu.SetActive(false);
      OptionsMenu.SetActive(true);
  }
  public void CloseOptions()
  {
      MainMenu.SetActive(true);
      OptionsMenu.SetActive(false); 
     
  }
  public void OpenLevelsMenu()
  {
    LevelsMenu.SetActive(true);
    MainMenu.SetActive(false);
  }
  public void CloseLevelsMenu()
  {
      LevelsMenu.SetActive(false);
    MainMenu.SetActive(true);
  }
  public void OpenLoadMenu()
  {
    LoadMenu.SetActive(true);
    MainMenu.SetActive(false);
  }
  public void CloseLoadMenu()
  {
      LoadMenu.SetActive(false);
    MainMenu.SetActive(true);
  }
  public void OpenLevelCreateMenu()
  {
   LevelCreateMenu.SetActive(true);
   LevelsMenu.SetActive(false);
  }
  public void CloseLevelCreateMenu()
  {
LevelCreateMenu.SetActive(false);
   LevelsMenu.SetActive(true);
  }
}
