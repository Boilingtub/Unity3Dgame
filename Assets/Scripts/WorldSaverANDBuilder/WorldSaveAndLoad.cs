using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.InteropServices;

public class WorldSaveAndLoad : MonoBehaviour
{

public string LevelSaveDirectory
{
    get
    {
     return Application.dataPath + "/Resources/Levels";
    }
}

public string worldfilepath;

public void OpenFileExplorer(string FunctionToRun)
{

GameObject FileExplorer = (GameObject)Instantiate(Resources.Load("Prefabs/XmlFileSearchCanvas"));
FileExplorer.GetComponent<SearchFolderForXMLFiles>().RunFileExplorer(LevelSaveDirectory , "*.xml" , this.gameObject , FunctionToRun);

}

IEnumerator DisplayLevelSavedTextForSeconds(float seconds)
{
    GameObject.Find("EditorCamera").GetComponent<LevelEditorCameraMove>().LevelSavedText.SetActive(true);
    yield return new WaitForSeconds(seconds);
    GameObject.Find("EditorCamera").GetComponent<LevelEditorCameraMove>().LevelSavedText.SetActive(false);
}
public void GetAndSaveObjects()
{
 if(GameObject.Find("LevelEditor") == true)
 {
     StartCoroutine(DisplayLevelSavedTextForSeconds(1f));
 }

if(GameObject.Find("StaticEnviorment") != null)
{
foreach(Transform childobj in GameObject.Find("StaticEnviorment").transform)
{
    if(childobj.GetComponent<ObjectDATA>() == false)
    {
    childobj.gameObject.AddComponent<ObjectDATA>();
    }
}

}

if(GameObject.Find("DynamicEnviorment") != null)
{
foreach(Transform childobj in GameObject.Find("DynamicEnviorment").transform)
{
     if(childobj.GetComponent<ObjectDATA>() == false)
    {
    childobj.gameObject.AddComponent<ObjectDATA>();
    }
}

}

createWorldDATAobject();
SerializeAndSaveWorld();

}

public WorldDATA createWorldDATAobject()
{
    
    WorldDATA worlddata = new WorldDATA();
    worlddata.WorldObjects = new List<ObjectDATA.ObjectVariables>();
    List<GameObject> worlddatagameobjects = new List<GameObject>();
    
    ObjectDATA[] foundobjects = FindObjectsOfType<ObjectDATA>();
    foreach(ObjectDATA obj in foundobjects)
    {
        worlddata.WorldObjects.Add(obj.objectdata);
        worlddatagameobjects.Add(obj.gameObject);
        
    }

    for(int i = 0 ; i < worlddata.WorldObjects.Count ; i++)
    {
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.objectname = worlddatagameobjects[i].name;

        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.positionX = worlddatagameobjects[i].transform.position.x;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.positionY = worlddatagameobjects[i].transform.position.y;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.positionZ = worlddatagameobjects[i].transform.position.z;

        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.rotationX = worlddatagameobjects[i].transform.localEulerAngles.x;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.rotationY = worlddatagameobjects[i].transform.localEulerAngles.y;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.rotationZ = worlddatagameobjects[i].transform.localEulerAngles.z;

        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.scaleX = worlddatagameobjects[i].transform.localScale.x;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.scaleY = worlddatagameobjects[i].transform.localScale.y;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.scaleZ = worlddatagameobjects[i].transform.localScale.z;

        if(worlddatagameobjects[i].GetComponent<Rigidbody>() == true)
        {
          worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.PhysicsIsDynamic = true; 
          worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.RigidBodyMass = worlddatagameobjects[i].GetComponent<Rigidbody>().mass;
        }

        else 
        if(worlddatagameobjects[i].GetComponent<Collider>().isTrigger == false)
        {
            worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.PhysicsIsStatic = true;
        }

        if(worlddatagameobjects[i].tag == "Pickupable")
        {
              worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.PlayerInteractionIsPickupable = true; 
        }

        if(worlddatagameobjects[i].tag == "Interactable")
        {
             worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.PlayerInteractionIsInteractable = true;
        }

        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.ColorR = worlddatagameobjects[i].GetComponent<Renderer>().material.color.r * 255;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.ColorG = worlddatagameobjects[i].GetComponent<Renderer>().material.color.g * 255;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.ColorB = worlddatagameobjects[i].GetComponent<Renderer>().material.color.b * 255;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.ColorA = worlddatagameobjects[i].GetComponent<Renderer>().material.color.a * 255;
        
        
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.TextureScaleX = worlddatagameobjects[i].GetComponent<Renderer>().material.mainTextureScale.x;
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.TextureScaleY = worlddatagameobjects[i].GetComponent<Renderer>().material.mainTextureScale.y;
        
        worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.ShaderName = worlddatagameobjects[i].GetComponent<Renderer>().material.shader.name;
        
        //Check that all objects has differentnames
         for(int _i = 0 ; _i + i < worlddata.WorldObjects.Count; _i++)
        {
           if(worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.objectname == worlddatagameobjects[_i].GetComponent<ObjectDATA>().objectdata.objectname)
           {
             worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.objectname = worlddatagameobjects[i].GetComponent<ObjectDATA>().objectdata.objectname + "(" + i.ToString() + ")";
           }
        }

    }
   


    return worlddata;
}



private void SerializeAndSaveWorld()
{
       WorldDATA worlddata = createWorldDATAobject();
        XmlSerializer xmlserializer = new XmlSerializer(typeof(WorldDATA));
        FileStream filestream = File.Create(worldfilepath);
        xmlserializer.Serialize(filestream, worlddata);
        filestream.Close();
        
}


public void ClearAndLoadWorld()
{
    Debug.Log("BUILDING WORLD FORM FILE !!!!");

  if(GameObject.Find("DynamicEnviorment"))
  {
      DestroyImmediate(GameObject.Find("DynamicEnviorment"));
  }
  if(GameObject.Find("StaticEnviorment"))
  {
      DestroyImmediate(GameObject.Find("StaticEnviorment"));
  }


  GameObject DynamicEnviorment = new GameObject();
  DynamicEnviorment.name = "DynamicEnviorment";
             
   GameObject StaticEnviorment = new GameObject();
   StaticEnviorment.name = "StaticEnviorment";
   DeSerializeAndBuildWorld();
}

private void DeSerializeAndBuildWorld()
{
    if (File.Exists(worldfilepath))
        {
            // LOAD VALUES FROM FILE TO GameData
            XmlSerializer xmlserializer = new XmlSerializer(typeof(WorldDATA));
            FileStream filestream = File.Open(worldfilepath, FileMode.Open);

            WorldDATA worlddata = xmlserializer.Deserialize(filestream) as WorldDATA;
            filestream.Close();
             
              for(int i = 0 ; i < worlddata.WorldObjects.Count ; i++)
            {

                if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.Cube)
                {
                    
                    GameObject NewCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    ShapeNewObjectAsSpecifiedinFile(NewCube , worlddata , i);
                }
                else
                if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.Sphere)
                {
                    
                    GameObject NewSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    ShapeNewObjectAsSpecifiedinFile(NewSphere , worlddata , i);
                }
                else
                if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.Capsule)
                {
                   
                    GameObject NewCapsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    ShapeNewObjectAsSpecifiedinFile(NewCapsule , worlddata , i);
                }
                else
                if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.Cylinder)
                {
                   
                   GameObject  NewCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                   ShapeNewObjectAsSpecifiedinFile(NewCylinder , worlddata , i);
                }
                else
                if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.Plane)
                {
                  
                   GameObject  NewPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                  ShapeNewObjectAsSpecifiedinFile(NewPlane , worlddata , i);
                }
                else
                if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.Empty)
                {
                   
                   GameObject  NewEmpty = new GameObject();
                   ShapeNewObjectAsSpecifiedinFile(NewEmpty , worlddata , i);
                }
                else
                if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.PlayerSpawnPoint)
                {
                    GameObject NewPlayerSpawnPoint = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    ShapeNewObjectAsSpecifiedinFile(NewPlayerSpawnPoint , worlddata , i);
                }
            }
             createWorldDATAobject();
        }
        else
        {
            Debug.Log("FILE NOT FOUND IN : " + worldfilepath);
        }
}

void ShapeNewObjectAsSpecifiedinFile(GameObject NewObject , WorldDATA worlddata , int i)
{

              

               NewObject.AddComponent<ObjectDATA>();
               NewObject.GetComponent<ObjectDATA>().objectdata.objecttype =  worlddata.WorldObjects[i].objecttype;

               for(int _i = 0 ; _i + i < worlddata.WorldObjects.Count; _i++)
        {
           if(worlddata.WorldObjects[i].objectname == worlddata.WorldObjects[_i].objectname)
           {
             NewObject.name = worlddata.WorldObjects[i].objectname + "(" + i.ToString() + ")";
           }
           if(worlddata.WorldObjects[i].objecttype == ObjectDATA.ObjectType.PlayerSpawnPoint)
           {
               NewObject.name = "PlayerSpawnPoint";
           }
        }
               
               NewObject.transform.position = new Vector3(worlddata.WorldObjects[i].positionX , worlddata.WorldObjects[i].positionY , worlddata.WorldObjects[i].positionZ);
               
               NewObject.transform.rotation = Quaternion.Euler(worlddata.WorldObjects[i].rotationX , worlddata.WorldObjects[i].rotationY , worlddata.WorldObjects[i].rotationZ);
              
               NewObject.transform.localScale = new Vector3(worlddata.WorldObjects[i].scaleX , worlddata.WorldObjects[i].scaleY , worlddata.WorldObjects[i].scaleZ);
               
            
               if(worlddata.WorldObjects[i].PhysicsIsDynamic == true)
               {
                   NewObject.AddComponent<Rigidbody>();
                   NewObject.GetComponent<Rigidbody>().mass = worlddata.WorldObjects[i].RigidBodyMass;
                   NewObject.transform.position = new Vector3(worlddata.WorldObjects[i].positionX , worlddata.WorldObjects[i].positionY + 1 , worlddata.WorldObjects[i].positionZ);
                   if(NewObject.GetComponent<Collider>() == false)
                   {
                   NewObject.AddComponent<MeshCollider>().convex = true;
                   }
                   NewObject.transform.SetParent(GameObject.Find("DynamicEnviorment").transform);
                   NewObject.layer = 0;
                   
               }
               else if(worlddata.WorldObjects[i].PhysicsIsStatic == true)
               {
                   if(NewObject.GetComponent<Collider>() == false)
                   {
                   NewObject.AddComponent<MeshCollider>().convex = true;
                   }
                  NewObject.transform.SetParent(GameObject.Find("StaticEnviorment").transform);
                  NewObject.layer = 9;
                  
               }
               else
               {
                   if(NewObject.GetComponent<Collider>() == false)
                   {
                   NewObject.AddComponent<MeshCollider>();
                   NewObject.GetComponent<MeshCollider>().convex = true;
                   }
                   NewObject.GetComponent<Collider>().isTrigger = true;
                   NewObject.transform.SetParent(GameObject.Find("StaticEnviorment").transform);
                   NewObject.layer = 12;
                   
               }
               
               if(worlddata.WorldObjects[i].PlayerInteractionIsPickupable == true)
               {
                 NewObject.tag = "Pickupable";
               }
               else if(worlddata.WorldObjects[i].PlayerInteractionIsInteractable == true)
               {
                   NewObject.tag = "Interactable";
               }
              
             
             NewObject.GetComponent<Renderer>().material.color = new Color32((byte)worlddata.WorldObjects[i].ColorR , (byte)worlddata.WorldObjects[i].ColorG , (byte)worlddata.WorldObjects[i].ColorB , (byte)worlddata.WorldObjects[i].ColorA);
             
             NewObject.GetComponent<Renderer>().material.shader = Shader.Find(worlddata.WorldObjects[i].ShaderName);
             NewObject.GetComponent<Renderer>().material.mainTexture = (Texture2D)Resources.Load(worlddata.WorldObjects[i].TexturePath);
             NewObject.GetComponent<ObjectDATA>().objectdata.TexturePath = worlddata.WorldObjects[i].TexturePath;
             
             NewObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(worlddata.WorldObjects[i].TextureScaleX , worlddata.WorldObjects[i].TextureScaleY);
}

}


