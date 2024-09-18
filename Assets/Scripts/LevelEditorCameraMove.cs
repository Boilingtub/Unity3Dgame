

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
 using System.Runtime.InteropServices;

public class LevelEditorCameraMove : MonoBehaviour
{
    GameObject PlayerSpawnPoint;
    public GameObject PlayerSpawnPointPropertiesPanel;
    [SerializeField] public Mesh[] ObjectModelsMeshes;
    public Material HighlightMaterial;
    public GameObject PropertiesPanel;
    public GameObject TextureTilingFieldsParent;
    public GameObject MassInputFieldParent;
    public GameObject ColourPickerPanel;
    public GameObject SpawnMenuPanel;
    public GameObject TextureMenuPanel;
    public GameObject LevelSavedText;
    float CameraMovementSpeed = 10f;
     float SelectedObjectMoveSpeed = 10f;
    GameObject EditorCamera;
    GameObject SelectedObject;
    bool MovingSelectedObject;
    bool ObjectSelected;
    bool GiveColorSlidersControl;
    float RotX;
    float RotY;
    public bool PlayerCanSelect = true;

   [DllImport("user32.dll")]
   private static extern void SaveFileDialog();
    void Start()
    {
        PlayerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
        EditorCamera = GameObject.Find("EditorCamera");
    }
    public void TogglePlayerSelect()
    {
        if(PlayerCanSelect == false)
        {
            PlayerCanSelect = true;
        }
        else
        if(PlayerCanSelect == true)
        {
         PlayerCanSelect = false;
        }
    }
    void Update()
    {
      
        if(GameObject.Find("PlayerSpawnPoint") != null)
        {
       PlayerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
        }

        float XInput = Input.GetAxisRaw("Horizontal");
        float ZInput = Input.GetAxisRaw("Vertical");

        Vector3 XMove = Vector3.right * XInput;
        Vector3 ZMove = Vector3.forward * ZInput;

        EditorCamera.transform.Translate(XMove * CameraMovementSpeed * Time.deltaTime);
        EditorCamera.transform.Translate(ZMove * CameraMovementSpeed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.Space))
        {
            EditorCamera.transform.Translate(Vector3.up * CameraMovementSpeed * Time.deltaTime);
        }


        if (Input.GetMouseButton(1))
        {

            RotX += Input.GetAxisRaw("Mouse X") * PlayerMovement.MouseSens * Time.deltaTime;
            RotY -= Input.GetAxisRaw("Mouse Y") * PlayerMovement.MouseSens * Time.deltaTime;
            RotY = Mathf.Clamp(RotY, -90f, 90f);

            EditorCamera.transform.eulerAngles = new Vector3(RotY, RotX, 0.0f);
        }



        if (SelectedObject != null && MovingSelectedObject == true)
        {
            if(PropertiesPanel != null)
            {
            float MouseX = Input.GetAxis("Mouse X") * SelectedObjectMoveSpeed * Time.deltaTime;
            float MouseY = Input.GetAxis("Mouse Y") * SelectedObjectMoveSpeed * Time.deltaTime;
            Vector3 SelectedObjectMoveX = MouseX * EditorCamera.transform.TransformDirection(Vector3.right);
            Vector3 SelectedObjectMoveY = MouseY * EditorCamera.transform.TransformDirection(Vector3.up);
            SelectedObject.transform.position += SelectedObjectMoveX + SelectedObjectMoveY;
            UpdatePropertiesMenuValues();
            }
            else
            if(PlayerSpawnPointPropertiesPanel != null)
            {
                UpdatePlayerSpawnpointMenuValues();
            }
             
        }
  
   if(Input.GetMouseButtonDown(0))
   {
       if(PlayerCanSelect == true && PauseMenu.isGamePaused == false)
       {
       TestForObject();
       }
   }
   if(Input.GetMouseButtonUp(0) && MovingSelectedObject == true)
   {
       MovingSelectedObject = false;
   }

    }

    void TestForObject()
    {
        Ray ray;
        RaycastHit hitinfo;
        
        
            ray = EditorCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitinfo , 1000f))
            {
                if(hitinfo.collider.gameObject.layer != 10)
                {
                if(hitinfo.collider.gameObject == SelectedObject && ObjectSelected == true)
                {
                   MovingSelectedObject = true;
                }
                if(ObjectSelected == true)
                {
                     UnselectObject();
                }
                if (ObjectSelected == false)
                {
                     SelectObject(hitinfo);
                }
                }
            }
          else if(ObjectSelected == true)
          {
             UnselectObject();
          }

    }
    public void UnselectBeforeSaveOrLoad()
    {
      if(SelectedObject != null)
      {
          UnselectObject();
      }
    }
    void UnselectObject()
    {
        Material[] SelectMaterials = new Material[SelectedObject.GetComponent<Renderer>().materials.Length - 1];
        for(int i = 0 ; i < SelectMaterials.Length ; i++)
        {
            SelectMaterials[i] = SelectedObject.GetComponent<Renderer>().materials[i];
        }
        
        if(SelectedObject.GetComponent<Renderer>() == true)
        {
           SelectedObject.GetComponent<Renderer>().materials = SelectMaterials;
        }

        if(SelectedObject.GetComponent<Rigidbody>() == true)
        {
        SelectedObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        TextureMenuPanel.SetActive(false);
        PropertiesPanel.SetActive(false);
       
        
        PlayerSpawnPointPropertiesPanel.SetActive(false);
        
        ColourPickerPanel.SetActive(false);
        SelectedObject = null;  
        ObjectSelected = false;
    }
    void SelectObject(RaycastHit hitinfo)
    {
        
        SelectedObject = hitinfo.collider.gameObject;
        if(SelectedObject == PlayerSpawnPoint)
        {
            PlayerSpawnPointPropertiesPanel.SetActive(true);
        }
        else
        {
        PropertiesPanel.SetActive(true);
        }
        Material[] SelectMaterials = new Material[SelectedObject.GetComponent<Renderer>().materials.Length + 1];

        for(int i = 0 ; i < SelectMaterials.Length - 1 ; i++)
        {
            SelectMaterials[i] = SelectedObject.GetComponent<Renderer>().materials[i];
        }
        
        SelectMaterials[SelectMaterials.Length - 1] = HighlightMaterial;
        if(SelectedObject.GetComponent<Renderer>() == true)
        {
           SelectedObject.GetComponent<Renderer>().materials = SelectMaterials;
        }

        if(SelectedObject.GetComponent<Rigidbody>() == true)
        {
        SelectedObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        if(PropertiesPanel != null)
        {
         UpdatePropertiesMenuValues();
        }
        ObjectSelected = true;
        
    }
     
     void UpdatePropertiesMenuValues()
     {
         GameObject.Find("ObjectNameHEadingInputBox").GetComponent<TMP_InputField>().text = SelectedObject.name;

        GameObject.Find("PosInputFieldX").GetComponent<InputField>().text = SelectedObject.transform.position.x.ToString();
        GameObject.Find("PosInputFieldY").GetComponent<InputField>().text = SelectedObject.transform.position.y.ToString();
        GameObject.Find("PosInputFieldZ").GetComponent<InputField>().text = SelectedObject.transform.position.z.ToString();
        if(PlayerSpawnPointPropertiesPanel != null && SelectedObject == PlayerSpawnPoint)
        {
            return;
        }
        GameObject.Find("RotInputFieldX").GetComponent<InputField>().text = SelectedObject.transform.localEulerAngles.x.ToString();
        GameObject.Find("RotInputFieldY").GetComponent<InputField>().text = SelectedObject.transform.localEulerAngles.y.ToString();
        GameObject.Find("RotInputFieldZ").GetComponent<InputField>().text = SelectedObject.transform.localEulerAngles.z.ToString();

        GameObject.Find("ScaleInputFieldX").GetComponent<InputField>().text = SelectedObject.transform.localScale.x.ToString();
        GameObject.Find("ScaleInputFieldY").GetComponent<InputField>().text = SelectedObject.transform.localScale.y.ToString();
        GameObject.Find("ScaleInputFieldZ").GetComponent<InputField>().text = SelectedObject.transform.localScale.z.ToString();

        GameObject.Find("ColorDropDown").GetComponent<TMP_Dropdown>().value = 0;

        if(SelectedObject.GetComponent<Rigidbody>() == false && SelectedObject.GetComponent<Collider>().isTrigger == true)
        {
            GameObject.Find("physicstypeDropdown").GetComponent<TMP_Dropdown>().value = 0;
        }
        else 
        if(SelectedObject.GetComponent<Rigidbody>() == false && SelectedObject.GetComponent<Collider>().isTrigger == false)
        {
            GameObject.Find("physicstypeDropdown").GetComponent<TMP_Dropdown>().value = 1;
        }
        else 
        if(SelectedObject.GetComponent<Rigidbody>() == true && SelectedObject.GetComponent<Collider>().isTrigger == false)
        {
            GameObject.Find("physicstypeDropdown").GetComponent<TMP_Dropdown>().value = 2;
        }

        if(SelectedObject.GetComponent<Renderer>().material.mainTexture != null)
        {
            GameObject.Find("TextureDropDown").GetComponent<TMP_Dropdown>().value = 1;
            TextureMenuDropDown();
        }
        else
        {
            GameObject.Find("TextureDropDown").GetComponent<TMP_Dropdown>().value = 0;
        }

        if(SelectedObject.tag == "Pickupable")
        {
            GameObject.Find("PlayerInteractDropdown").GetComponent<TMP_Dropdown>().value = 1;
        }
        else
        if(SelectedObject.tag == "Interactable")
        {
            GameObject.Find("PlayerInteractDropdown").GetComponent<TMP_Dropdown>().value = 2;
        }
        else
        {
          GameObject.Find("PlayerInteractDropdown").GetComponent<TMP_Dropdown>().value = 0; 
        }
         


        switch(SelectedObject.GetComponent<ObjectDATA>().objectdata.objecttype)
        {
            case ObjectDATA.ObjectType.Cube :
            GameObject.Find("ShapeDropdown").GetComponent<TMP_Dropdown>().value = 0;
            break;
             case ObjectDATA.ObjectType.Sphere :
            GameObject.Find("ShapeDropdown").GetComponent<TMP_Dropdown>().value = 1;
            break;
             case ObjectDATA.ObjectType.Capsule :
            GameObject.Find("ShapeDropdown").GetComponent<TMP_Dropdown>().value = 2;
            break;
             case ObjectDATA.ObjectType.Cylinder :
            GameObject.Find("ShapeDropdown").GetComponent<TMP_Dropdown>().value = 3;
            break;
            case ObjectDATA.ObjectType.Plane :
            GameObject.Find("ShapeDropdown").GetComponent<TMP_Dropdown>().value = 4;
            break;
            
        }
     }


    void UpdatePlayerSpawnpointMenuValues()
    {
        GameObject.Find("PosInputFieldX").GetComponent<InputField>().text = SelectedObject.transform.position.x.ToString();
        GameObject.Find("PosInputFieldY").GetComponent<InputField>().text = SelectedObject.transform.position.y.ToString();
        GameObject.Find("PosInputFieldZ").GetComponent<InputField>().text = SelectedObject.transform.position.z.ToString();
    }



    public void ChangeSelectedObjectProperties()
    {
     SelectedObject.name = GameObject.Find("ObjectNameHEadingInputBox").GetComponent<TMP_InputField>().text;
     SelectedObject.transform.position = new Vector3(float.Parse(GameObject.Find("PosInputFieldX").GetComponent<InputField>().text) , float.Parse(GameObject.Find("PosInputFieldY").GetComponent<InputField>().text) , float.Parse(GameObject.Find("PosInputFieldZ").GetComponent<InputField>().text));
      if(PlayerSpawnPointPropertiesPanel != null && SelectedObject == PlayerSpawnPoint)
        {
            return;
        }
     SelectedObject.transform.rotation = Quaternion.Euler(float.Parse(GameObject.Find("RotInputFieldX").GetComponent<InputField>().text) , float.Parse(GameObject.Find("RotInputFieldY").GetComponent<InputField>().text) , float.Parse(GameObject.Find("RotInputFieldZ").GetComponent<InputField>().text));
     SelectedObject.transform.localScale = new Vector3(float.Parse(GameObject.Find("ScaleInputFieldX").GetComponent<InputField>().text) , float.Parse(GameObject.Find("ScaleInputFieldY").GetComponent<InputField>().text) , float.Parse(GameObject.Find("ScaleInputFieldZ").GetComponent<InputField>().text));
    
    }

    public void DuplicateSelectedObject()
    {
        GameObject ObjectToBeDuplicated;
        ObjectToBeDuplicated = SelectedObject;
        UnselectObject();
        GameObject duplicate = Instantiate(ObjectToBeDuplicated) as GameObject;
        if(duplicate.GetComponent<Rigidbody>() == true)
        {
            duplicate.transform.SetParent(GameObject.Find("DynamicEnviorment").transform);
        }
        else
        {
            duplicate.transform.SetParent(GameObject.Find("StaticEnviorment").transform);
        }
        
    }
    public void DestorySelectedObject()
    {
        GameObject ObjectTobeDestoryed;
        ObjectTobeDestoryed = SelectedObject;
        UnselectObject();
        Destroy(ObjectTobeDestoryed);
    }
     
     public void ColourDropDown()
     {
         int val = GameObject.Find("ColorDropDown").GetComponent<TMP_Dropdown>().value;
         if(val == 1)
         {
             Debug.Log("ColorPicker Active");
             ColourPickerPanel.SetActive(true);
             SetupColorPicker();
             GiveColorSlidersControl = true;
         }
     }
     public void SetupColorPicker()
     {
     
      GameObject.Find("RedSlider").GetComponent<Slider>().value = SelectedObject.GetComponent<Renderer>().material.color.r ;
      GameObject.Find("GreenSlider").GetComponent<Slider>().value = SelectedObject.GetComponent<Renderer>().material.color.g ;
      GameObject.Find("BlueSlider").GetComponent<Slider>().value = SelectedObject.GetComponent<Renderer>().material.color.b ;
      GameObject.Find("AlphaSlider").GetComponent<Slider>().value = SelectedObject.GetComponent<Renderer>().material.color.a ;
      GameObject.Find("RedInputField").GetComponent<TMP_InputField>().text = (SelectedObject.GetComponent<Renderer>().material.color.r *255).ToString();
      GameObject.Find("GreenInputField").GetComponent<TMP_InputField>().text = (SelectedObject.GetComponent<Renderer>().material.color.g *255).ToString();
      GameObject.Find("BlueInputField").GetComponent<TMP_InputField>().text = (SelectedObject.GetComponent<Renderer>().material.color.b *255).ToString();
      GameObject.Find("AlphaInputField").GetComponent<TMP_InputField>().text = (SelectedObject.GetComponent<Renderer>().material.color.a *255).ToString();
     }
     public void CloseColorPicker()
     {
         ColourPickerPanel.SetActive(false);
         GiveColorSlidersControl = false;
     }

     public void SetObjectColorWithSliders()
     {
        if(GiveColorSlidersControl == true)
        {
         GameObject.Find("ColorShowImage").GetComponent<Image>().color = new Color32( (byte) (GameObject.Find("RedSlider").GetComponent<Slider>().value * 255) , (byte) (GameObject.Find("GreenSlider").GetComponent<Slider>().value* 255) , (byte) (GameObject.Find("BlueSlider").GetComponent<Slider>().value * 255) , (byte) (GameObject.Find("AlphaSlider").GetComponent<Slider>().value * 255));
         SelectedObject.GetComponent<Renderer>().material.color = new Color32( (byte) (GameObject.Find("RedSlider").GetComponent<Slider>().value* 255) , (byte) (GameObject.Find("GreenSlider").GetComponent<Slider>().value * 255 ) , (byte) (GameObject.Find("BlueSlider").GetComponent<Slider>().value  * 255) , (byte) (GameObject.Find("AlphaSlider").GetComponent<Slider>().value * 255));
         SetupColorPicker();
        }
     }
      public void SetObjectColorWithInputFields()
     {
        if(GiveColorSlidersControl == true)
        {
         GameObject.Find("ColorShowImage").GetComponent<Image>().color = new Color32( (byte) (float.Parse(GameObject.Find("RedInputField").GetComponent<TMP_InputField>().text)) , (byte) (float.Parse(GameObject.Find("GreenInputField").GetComponent<TMP_InputField>().text)) , (byte) (float.Parse(GameObject.Find("BlueInputField").GetComponent<TMP_InputField>().text)) , (byte) (float.Parse(GameObject.Find("AlphaInputField").GetComponent<TMP_InputField>().text)));
         SelectedObject.GetComponent<Renderer>().material.color = new Color32( (byte) (float.Parse(GameObject.Find("RedInputField").GetComponent<TMP_InputField>().text)) , (byte) (float.Parse(GameObject.Find("GreenInputField").GetComponent<TMP_InputField>().text)) , (byte) (float.Parse(GameObject.Find("BlueInputField").GetComponent<TMP_InputField>().text)) , (byte) (float.Parse(GameObject.Find("AlphaInputField").GetComponent<TMP_InputField>().text)));
         SetupColorPicker();
        }
     }

     public void TextureMenuDropDown()
     {
         int val = GameObject.Find("TextureDropDown").GetComponent<TMP_Dropdown>().value;
         if(val == 0)
         {
             SelectedObject.GetComponent<Renderer>().material.mainTexture = null;
             TextureMenuPanel.SetActive(false);
             TextureTilingFieldsParent.SetActive(false);
         }
         if(val == 1)
         {
            TextureTilingFieldsParent.SetActive(true);
            GameObject.Find("TextureTilingXInputField").GetComponent<InputField>().text = SelectedObject.GetComponent<Renderer>().material.mainTextureScale.x.ToString();
            GameObject.Find("TextureTilingYInputField").GetComponent<InputField>().text = SelectedObject.GetComponent<Renderer>().material.mainTextureScale.y.ToString();
         }
         if(val == 2)
         {
             TextureMenuPanel.SetActive(true);
             
         }
     }
     public void SetObjectTexture(string TexturePath)
     {
        SelectedObject.GetComponent<Renderer>().material.mainTexture = (Texture2D)Resources.Load(TexturePath);
        if(SelectedObject.GetComponent<ObjectDATA>() == true)
        {
                SelectedObject.GetComponent<ObjectDATA>().objectdata.TexturePath = TexturePath;
        }
        else
        {
            SelectedObject.AddComponent<ObjectDATA>().objectdata.TexturePath = TexturePath;
        }
        GameObject.Find("TextureDropDown").GetComponent<TMP_Dropdown>().value = 1;
          TextureMenuPanel.SetActive(false);
     }   
     public void ObjectTextureTiling()
     {
        SelectedObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2( float.Parse(GameObject.Find("TextureTilingXInputField").GetComponent<InputField>().text) , float.Parse(GameObject.Find("TextureTilingYInputField").GetComponent<InputField>().text)); 
     }

     

     public void OpenSpawnMenu()
     {
       SpawnMenuPanel.SetActive(true);
     }
     public void SpawnObjectbyName(string ObjectName)
     {
         switch(ObjectName)
         {
             case "PlainCube":
             GameObject PlainCube  = GameObject.CreatePrimitive(PrimitiveType.Cube);
             PlainCube.AddComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Cube;
             SetupNewlySpawnedObject(PlainCube);
             break;
             case "PlainSphere":
             GameObject PlainSphere  = GameObject.CreatePrimitive(PrimitiveType.Sphere);
             PlainSphere.AddComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Sphere;
             SetupNewlySpawnedObject(PlainSphere);
             break;
             case "PlainCapsule":
             GameObject PlainCapsule  = GameObject.CreatePrimitive(PrimitiveType.Capsule);
             PlainCapsule.AddComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Capsule;
             SetupNewlySpawnedObject(PlainCapsule);
             break;
             case "PlainCylinder":
             GameObject PlainCylinder  = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
             PlainCylinder.AddComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Cylinder;
             SetupNewlySpawnedObject(PlainCylinder);
             break;
             case "PlainPlane":
             GameObject PlainPlane  = GameObject.CreatePrimitive(PrimitiveType.Plane);
             PlainPlane.AddComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Plane;
             SetupNewlySpawnedObject(PlainPlane);
             break;
             case "PlayerSpawnPoint":
             break;
         }
     }
     void SetupNewlySpawnedObject(GameObject newlyspawnedObject)
     {
       newlyspawnedObject.transform.SetParent(GameObject.Find("StaticEnviorment").transform);
       newlyspawnedObject.transform.position = new Vector3(0,0,0);
     }
     public void CloseSpawnMenu()
     {
      SpawnMenuPanel.SetActive(false);
     }




     public void ObjectShapeDropDown()
     {
         
         int val = GameObject.Find("ShapeDropdown").GetComponent<TMP_Dropdown>().value;
          SelectedObject.GetComponent<MeshFilter>().mesh = ObjectModelsMeshes[val];

          if(SelectedObject.GetComponent<ObjectDATA>() == false)
          {
              SelectedObject.AddComponent<ObjectDATA>();
          }

          switch(val)
          {
          case 0:
          SelectedObject.GetComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Cube;
          break;
          case 1:
          SelectedObject.GetComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Sphere;
          break;
          case 2:
          SelectedObject.GetComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Capsule;
          break;
          case 3:
          SelectedObject.GetComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Cylinder;
          break;
          case 4:
          SelectedObject.GetComponent<ObjectDATA>().objectdata.objecttype = ObjectDATA.ObjectType.Plane;
          break;
          }

          Destroy(SelectedObject.GetComponent<Collider>());
          SelectedObject.AddComponent<MeshCollider>().convex = true;
        
     }
     





    public void PlayerInteractionDropDown()
    {
        int val = GameObject.Find("PlayerInteractDropdown").GetComponent<TMP_Dropdown>().value;
        if(val == 0)
        {
            SelectedObject.tag = "Untagged";
        }
        if(val == 1)
        {
            SelectedObject.tag = "Pickupable";
        }
        if(val == 2)
        {
            SelectedObject.tag = "Interactable";
        }
    }
    
    public void PhysicsTypeDropDown()
    {
        int val = GameObject.Find("physicstypeDropdown").GetComponent<TMP_Dropdown>().value;
       if(val == 0)
       {
           MassInputFieldParent.SetActive(false);
           SelectedObject.transform.SetParent(GameObject.Find("StaticEnviorment").transform);
           SelectedObject.layer = 12;
           if(SelectedObject.GetComponent<Rigidbody>() == true)
           {
               Destroy(SelectedObject.GetComponent<Rigidbody>());
           }
           if(SelectedObject.GetComponent<Collider>() == true)
           {
             SelectedObject.GetComponent<Collider>().isTrigger = true;
           }
       }
        if( val == 1)
       {
           MassInputFieldParent.SetActive(false);
            SelectedObject.transform.SetParent(GameObject.Find("StaticEnviorment").transform);
            SelectedObject.layer = 9;
           if(SelectedObject.GetComponent<Rigidbody>() == true)
           {
               Destroy(SelectedObject.GetComponent<Rigidbody>());
           }
           if(SelectedObject.GetComponent<Collider>() == false)
           {
             SelectedObject.AddComponent<MeshCollider>().isTrigger = false;
           }
       }
        if( val == 2)
       {
           MassInputFieldParent.SetActive(true);
            SelectedObject.transform.SetParent(GameObject.Find("DynamicEnviorment").transform);
            SelectedObject.layer = 0;
           if(SelectedObject.GetComponent<Rigidbody>() == false)
           {
               SelectedObject.AddComponent<Rigidbody>();
           }
           if(SelectedObject.GetComponent<Collider>() == false)
           {
             SelectedObject.AddComponent<MeshCollider>();
           }
           else
           {
               SelectedObject.GetComponent<Collider>().isTrigger = false;
           }
            GameObject.Find("MassInputField").GetComponent<InputField>().text = SelectedObject.GetComponent<Rigidbody>().mass.ToString();
       }
    }
    public void SetRigidBodyMass()
    {
      if(SelectedObject.GetComponent<Rigidbody>() == true)
      {
          SelectedObject.GetComponent<Rigidbody>().mass = float.Parse(GameObject.Find("MassInputField").GetComponent<InputField>().text);
      }
    }


}



