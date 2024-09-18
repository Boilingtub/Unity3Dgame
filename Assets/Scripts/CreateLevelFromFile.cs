using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class CreateLevelFromFile : MonoBehaviour
{
    public Material DefaultSykboxMaterial;
    public GameObject LevelEditor;
    public GameObject PlayerSpawnPoint;
    public GameObject LevelManagersEditMode;
    public GameObject LevelManagersPlayMode;
    string NewSceneName;
    public bool LoadLevelinEditorMode;


public void LoadExternalLevel()
{
    NewSceneName = GameObject.Find("LevelNameInputField").GetComponent<TMP_InputField>().text;
    if(NewSceneName == "")
    {
        GameObject.Find("LevelNameInputField").GetComponent<TMP_InputField>().text = "New Level Must Have a NAME";
        return;
    }
     SceneManager.CreateScene(NewSceneName);
     SceneManager.SetActiveScene(SceneManager.GetSceneByName(NewSceneName));  

   GameObject NewSceneSetupObject = new GameObject();
  NewSceneSetupObject.AddComponent<WorldSaveAndLoad>();
  NewSceneSetupObject.GetComponent<WorldSaveAndLoad>().OpenFileExplorer("ClearAndLoadWorld");

 
  StartCoroutine(WaitTillNewSceneSetupObjectHasFilePath(NewSceneSetupObject));
  StartCoroutine(CheckifFileExplorerClosed());

}

public IEnumerator WaitTillNewSceneSetupObjectHasFilePath(GameObject NewSceneSetupObject)
{
    
    yield return new WaitUntil(() => NewSceneSetupObject.GetComponent<WorldSaveAndLoad>().worldfilepath != null);
    InstantiateLevelNeccesaryObjects();
}
public IEnumerator CheckifFileExplorerClosed()
{
     yield return new WaitUntil(() => GameObject.Find("XmlFileSearchCanvas(Clone)") == null);
   
      SceneManager.UnloadSceneAsync(NewSceneName);
      SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu")); 
      StopAllCoroutines();
      yield break;
   
}
public void InstantiateLevelNeccesaryObjects()
{
   
  if(LoadLevelinEditorMode == true)
    {
        Instantiate(LevelManagersEditMode);
        Instantiate(LevelEditor).name = "LevelEditor";
    }
    else
    if(LoadLevelinEditorMode == false)
    {
        Instantiate(LevelManagersPlayMode);
        GameObject.Find("PlayerSpawnManager").GetComponent<InstantiatePlayerAtSpawnPoint>().SpawnPlayer();
    }
    RenderSettings.skybox = DefaultSykboxMaterial;
    RenderSettings.sun = new Light();
    
    
    SceneManager.UnloadSceneAsync("MainMenu");
    
    GameObject.Find("PlaySceneBTN").GetComponent<Button>().onClick.AddListener(delegate(){GameObject.Find("PlayerSpawnManager").GetComponent<InstantiatePlayerAtSpawnPoint>().SpawnPlayer();});
    GameObject.Find("SaveSceneBTN").GetComponent<Button>().onClick.AddListener(delegate(){GameObject.Find("level manager").GetComponent<WorldSaveAndLoad>().OpenFileExplorer("GetAndSaveObjects");});
    GameObject.Find("LoadSceneBTN").GetComponent<Button>().onClick.AddListener(delegate(){GameObject.Find("level manager").GetComponent<WorldSaveAndLoad>().OpenFileExplorer("ClearAndLoadWorld");});
    
}



public void LevelLoadMode()
{
int val = GameObject.Find("LevelLoadModeDropDown").GetComponent<TMP_Dropdown>().value;
if(val == 0)
{
    LoadLevelinEditorMode = false;
}
else if(val == 1)
{
    LoadLevelinEditorMode = true;
}
}

}
