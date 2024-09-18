using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject MainPauseMenu;
    public GameObject OptionsMenu;
    public GameObject PauseMenuUI;
    public GameObject SaveMenu;
    public GameObject LoadMenu;

    public static bool isGamePaused = false;
   


 void Start()
  {
     UnPauseGame(); 
  }
    void Update()
{
    if(Input.GetKeyDown(KeyCode.Escape)) //Pausemenu Esc activate
    {
    if(isGamePaused == true)
    {
    UnPauseGame();
    }
    else
    {
      PauseGame();
    }
    }
} 
  public void QuitGame()
  {
      Application.Quit();
  }
  public void ReturnToMainMenu()
  {
      LoadingScreen.level = "MainMenu";
      SceneManager.LoadScene(0);
  }
  public void OpenOptions()
  {
      MainPauseMenu.SetActive(false);
      OptionsMenu.SetActive(true);
  }
  public void CloseOptions()
  {
      MainPauseMenu.SetActive(true);
      OptionsMenu.SetActive(false); 
  }

  public void OpenSaveMenu()
  {
   SaveMenu.SetActive(true);
   MainPauseMenu.SetActive(false);
  }
  public void CloseSaveMenu()
  {
    SaveMenu.SetActive(false);
   MainPauseMenu.SetActive(true);
  }

  public void OpenLoadMenu()
  {
    LoadMenu.SetActive(true);
   MainPauseMenu.SetActive(false);
  }
  public void CloseLoadMenu()
  {
   LoadMenu.SetActive(false);
   MainPauseMenu.SetActive(true);
  }
  public void PauseGame()
  {
        
        PauseMenuUI.SetActive(true);
        SaveMenu.SetActive(false);
        LoadMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        MainPauseMenu.SetActive(true);
       isGamePaused = true;
       Time.timeScale = 0f;
       Cursor.lockState = CursorLockMode.None;
       Cursor.visible = true;
  }
  public void UnPauseGame()
  {
       PauseMenuUI.SetActive(false);
      isGamePaused = false;
      Time.timeScale = 1f;
      if(GameObject.Find("LevelEditor") == null)
      {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
      }
  }

}

