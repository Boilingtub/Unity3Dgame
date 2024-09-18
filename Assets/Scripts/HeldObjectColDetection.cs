using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectColDetection : MonoBehaviour
{
void OnCollisionEnter(Collision col)
{
    if(col.gameObject.layer != LayerMask.NameToLayer("Player"))
    {
      this.gameObject.layer = 0;
    }
  

}
void OnCollisionExit(Collision col)
{
 this.gameObject.layer = 13;

}
}
