using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicEnviormentData : MonoBehaviour
{
   [SerializeField]public List<GameObject> dynamicobjectslist = new List<GameObject>();
   void Awake()
   {
       #region Dynamic Objects
   if(GameObject.Find("DynamicEnviorment"))
   {
       GameObject DynamicEnviormentParent = GameObject.Find("DynamicEnviorment");
       foreach(Transform childtrans in DynamicEnviormentParent.transform)
       {
        dynamicobjectslist.Add(childtrans.gameObject);
       }
   }
    
       #endregion

   }
}

