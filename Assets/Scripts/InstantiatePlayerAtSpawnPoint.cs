using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InstantiatePlayerAtSpawnPoint : MonoBehaviour
{
 
public GameObject Player; 
GameObject PlayerSpawnPointObject;
[SerializeField] public GameObject ExitPlayModeButton;
GameObject LevelEditor;
GameObject ActivePlayer;
public bool InstantiateOnAwake;
public bool inEditorPlayMode;
public static InstantiatePlayerAtSpawnPoint Playerspawnpointinstance;  



void Awake()
{
    
     if (Playerspawnpointinstance == null)
        {
            Playerspawnpointinstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if(InstantiateOnAwake == true)
        {
            SpawnPlayer();
        }
}
public void SpawnPlayer()
{
  
   if(GameObject.Find("LevelEditor") != null)
   {
   LevelEditor = GameObject.Find("LevelEditor");
   LevelEditor.SetActive(false);
   if(ExitPlayModeButton != null)
   {
    ExitPlayModeButton.SetActive(true);
   }
    inEditorPlayMode = true;
   }
   ActivePlayer = Instantiate(Player);
   ActivePlayer.layer = LayerMask.NameToLayer("Player");
   if(GameObject.Find("PlayerSpawnPoint") != null)
   {
   Player.transform.position = GameObject.Find("PlayerSpawnPoint").gameObject.transform.position;
   GameObject.Find("PlayerSpawnPoint").gameObject.GetComponent<Renderer>().enabled = false;
   }
   else
   {
       Player.transform.position = this.gameObject.transform.position;
   }
   Cursor.visible = false;
   
}
public void ExitPlayMode()
{
LevelEditor.SetActive(true);
Destroy(ActivePlayer);
ExitPlayModeButton.SetActive(false);
GameObject.Find("PlayerSpawnPoint").gameObject.GetComponent<Renderer>().enabled = true;
GameObject.Find("PauseMenu").GetComponent<PauseMenu>().UnPauseGame();
Cursor.visible = true;
Time.timeScale = 1f;
}
}
