using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelloadtrigger : MonoBehaviour
{
   void OnTriggerEnter(Collider playercol)
   {
       if(playercol.gameObject.tag == "Player")
       {
           LoadingScreen.level = "lvl1";
           SceneManager.LoadScene(0);

       }
   }
}
