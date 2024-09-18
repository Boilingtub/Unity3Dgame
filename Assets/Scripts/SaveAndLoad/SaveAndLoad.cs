using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.Audio;

public class SaveAndLoad : MonoBehaviour
{

    public static string filepath;
    public static bool LoadingnewScene = true;

    private string filepathauto
    {
        get
        {
            return Application.persistentDataPath + "/SavedGameDataAuto.xml";
        }
    }
    private string filepath01
    {
        get
        {
            return Application.persistentDataPath + "/SavedGameData01.xml";
        }
    }
    private string filepath02
    {
        get
        {
            return Application.persistentDataPath + "/SavedGameData02.xml";
        }
    }
    private string filepath03
    {
        get
        {
            return Application.persistentDataPath + "/SavedGameData03.xml";
        }
    }
    void Start()
    {
        if (LoadingnewScene == false)
        {
            LoadByDeSerialisation();
        }
    }

    public void SaveButton(int slot)
    {
      if(slot == 1)
      {
         filepath = filepath01;
      }
      else if(slot == 2)
      {
         filepath = filepath02;
      }
      else if(slot == 3)
      {
          filepath = filepath03;
      }
      else
      {
          filepath = filepathauto;
      }
      

        SaveBySerialisation();
        if (GameObject.Find("PauseMenu") != null)
        {
            GameObject.Find("PauseMenu").GetComponent<PauseMenu>().UnPauseGame();
        }
        Debug.Log("Saved ! " + " To slot " + slot);
    }
    public void LoadButton(int slot)
    {
      if(slot == 1)
      {
         filepath = filepath01;
      }
      else if(slot == 2)
      {
         filepath = filepath02;
      }
      else if(slot == 3)
      {
          filepath = filepath03;
      }
      else
      {
          filepath = filepathauto;
      }
      
        LoadByDeSerialisation();
        if (GameObject.Find("PauseMenu") != null)
        {
            GameObject.Find("PauseMenu").GetComponent<PauseMenu>().UnPauseGame();
        }
        Debug.Log("Loaded ! " + " from slot " + slot);
    
    }

    public AudioMixer audiomixer;

    private GameData createSaveGameDataObject()
    {
        GameData gamedata = new GameData();

        gamedata.level = SceneManager.GetActiveScene().name;
        gamedata.MouseSensitivity = PlayerMovement.MouseSens;
        bool result = audiomixer.GetFloat("MixerVol", out var vol);
        gamedata.AudioVolume = vol;



        if (GameObject.Find("Player") != null)
        {
            GameObject Player = GameObject.Find("Player");
            GameObject PlayerLookPoint = GameObject.Find("PlayerLookPoint");
            gamedata.PlayerMovSpeed = Player.GetComponent<PlayerMovement>().MovementSpeed;
            gamedata.PlayerPosX = Player.transform.position.x;
            gamedata.PlayerPosY = Player.transform.position.y;
            gamedata.PlayerPosZ = Player.transform.position.z;
            gamedata.PlayerLookPointRotX = PlayerLookPoint.transform.rotation.eulerAngles.x;
            gamedata.PlayerRotY = Player.transform.rotation.eulerAngles.y;

        }

        foreach (GameObject dynamicobjects in this.GetComponent<DynamicEnviormentData>().dynamicobjectslist)
        {
            gamedata.ObjectsPosX.Add(dynamicobjects.transform.position.x);
            gamedata.ObjectsPosY.Add(dynamicobjects.transform.position.y);
            gamedata.ObjectsPosZ.Add(dynamicobjects.transform.position.z);

            gamedata.ObjectVelocityX.Add(dynamicobjects.GetComponent<Rigidbody>().velocity.x);
            gamedata.ObjectVelocityY.Add(dynamicobjects.GetComponent<Rigidbody>().velocity.y);
            gamedata.ObjectVelocityZ.Add(dynamicobjects.GetComponent<Rigidbody>().velocity.z);

            gamedata.ObjectAngularVelocityX.Add(dynamicobjects.GetComponent<Rigidbody>().angularVelocity.x);
            gamedata.ObjectAngularVelocityY.Add(dynamicobjects.GetComponent<Rigidbody>().angularVelocity.y);
            gamedata.ObjectAngularVelocityZ.Add(dynamicobjects.GetComponent<Rigidbody>().angularVelocity.z);

            gamedata.ObjectRotationX.Add(dynamicobjects.transform.rotation.eulerAngles.x);
            gamedata.ObjectRotationY.Add(dynamicobjects.transform.rotation.eulerAngles.y);
            gamedata.ObjectRotationZ.Add(dynamicobjects.transform.rotation.eulerAngles.z);
        }


        return gamedata;
    }


    private void SaveByXML() //REDUNDEND
    {
        GameData gamedata = createSaveGameDataObject();
        XmlDocument xmldocument = new XmlDocument();

        XmlElement root = xmldocument.CreateElement("GameData");//elements will be inside "GameData" tag <GameData>...elements...<GameData>
        root.SetAttribute("FileName", "SaveFile_01");//<GameData FileName="SAveFile_01">......<GameData>

        #region Create XMLElements and Save each variable in class GameData as an XMLElement

        XmlElement LevelElement = xmldocument.CreateElement("level");//<level>...float of speed value...<level>
        LevelElement.InnerText = gamedata.level.ToString();
        root.AppendChild(LevelElement);//Append inside the <GameData> braces (AS a CHILD / Element)

        XmlElement PlayerMovSpeedElement = xmldocument.CreateElement("PlayerMovementSpeed");//<PlayerMovementSpeed>...float of speed value...<PlayerMovementSpeed>
        PlayerMovSpeedElement.InnerText = gamedata.PlayerMovSpeed.ToString();
        root.AppendChild(PlayerMovSpeedElement);//Append inside the <GameData> braces (AS a CHILD / Element)

        XmlElement PlayerPosXElement = xmldocument.CreateElement("PlayerPosX");//<PlayerPosX>...float of transform value...<PlayerPosX>
        PlayerPosXElement.InnerText = gamedata.PlayerPosX.ToString();
        root.AppendChild(PlayerPosXElement);//Append inside the <GameData> braces (AS a CHILD / Element)

        XmlElement PlayerPosYElement = xmldocument.CreateElement("PlayerPosY");
        PlayerPosYElement.InnerText = gamedata.PlayerPosY.ToString();
        root.AppendChild(PlayerPosYElement);

        XmlElement PlayerPosZElement = xmldocument.CreateElement("PlayerPosZ");
        PlayerPosZElement.InnerText = gamedata.PlayerPosZ.ToString();
        root.AppendChild(PlayerPosZElement);


        #endregion
        xmldocument.AppendChild(root);//Add the root and its children elements to the XML

        xmldocument.Save(filepath);
        if (File.Exists(filepath))
        {
            Debug.Log("XML FILE SAVED");
        }

    }

     
    private void LoadByXML()//REDUNDEND
    {
        if (File.Exists(filepath))
        {
            GameData gamedata = new GameData();

            //LOAD SAVEGAMEFILE DATA

            XmlDocument xmldocument = new XmlDocument();
            xmldocument.Load(filepath);

            #region Get SAVEGAMEFILE DATA FROM FILE
            XmlNodeList LevelElement = xmldocument.GetElementsByTagName("level");
            int level = int.Parse(LevelElement[0].InnerText);
            gamedata.level = level.ToString();

            XmlNodeList PlayerMovSpeedElement = xmldocument.GetElementsByTagName("PlayerMovementSpeed");
            float PlayerMovSpeed = float.Parse(PlayerMovSpeedElement[0].InnerText);
            gamedata.PlayerMovSpeed = PlayerMovSpeed;

            XmlNodeList PlayerPosXElement = xmldocument.GetElementsByTagName("PlayerPosX");
            float PlayerPosX = float.Parse(PlayerPosXElement[0].InnerText);
            gamedata.PlayerPosX = PlayerPosX;

            XmlNodeList PlayerPosYElement = xmldocument.GetElementsByTagName("PlayerPosY");
            float PlayerPosY = float.Parse(PlayerPosYElement[0].InnerText);
            gamedata.PlayerPosY = PlayerPosY;

            XmlNodeList PlayerPosZElement = xmldocument.GetElementsByTagName("PlayerPosZ");
            float PlayerPosZ = float.Parse(PlayerPosZElement[0].InnerText);
            gamedata.PlayerPosZ = PlayerPosZ;

            #endregion

            //ASSIGN the saved Data to the used Ingame data
            #region 



            if (GameObject.Find("Player") != null)
            {
                GameObject Player = GameObject.Find("Player");
                Player.GetComponent<PlayerMovement>().MovementSpeed = gamedata.PlayerMovSpeed;
                Player.transform.position = new Vector3(gamedata.PlayerPosX, gamedata.PlayerPosY, gamedata.PlayerPosZ);
            }



            #endregion

        }
        else
        {
            Debug.Log("COULD NOT FIND 'SaveGameData.xml' in " + filepath);
        }
    }





    private void SaveBySerialisation()
    {
        //SAVE VALUES FROM GAMEDATA TO FILE    
        GameData gamedata = createSaveGameDataObject();
        XmlSerializer xmlserializer = new XmlSerializer(typeof(GameData));
        FileStream filestream = File.Create(filepath);
        xmlserializer.Serialize(filestream, gamedata);
        filestream.Close();
    }





    public void LoadByDeSerialisation()
    {
        if (File.Exists(filepath))
        {
            // LOAD VALUES FROM FILE TO GameData
            XmlSerializer xmlserializer = new XmlSerializer(typeof(GameData));
            FileStream filestream = File.Open(filepath, FileMode.Open);

            GameData gamedata = xmlserializer.Deserialize(filestream) as GameData;
            filestream.Close();

            //Apply Readed Values
            if (LoadingnewScene == true)
            {
                LoadingnewScene = false;
                LoadingScreen.level = gamedata.level;

                SceneManager.LoadScene(0);
            }
            else
            {

                PlayerMovement.MouseSens = gamedata.MouseSensitivity;
                audiomixer.SetFloat("MixerVol", gamedata.AudioVolume);

                if (GameObject.Find("Player") != null)

                {
                    GameObject Player = GameObject.Find("Player");
                    GameObject PlayerLookPoint = GameObject.Find("PlayerLookPoint");
                    Player.GetComponent<PlayerMovement>().MovementSpeed = gamedata.PlayerMovSpeed;

                    Player.transform.position = new Vector3(gamedata.PlayerPosX, gamedata.PlayerPosY, gamedata.PlayerPosZ);
                    Player.transform.localRotation = Quaternion.Euler(0, gamedata.PlayerRotY, 0);
                    PlayerLookPoint.transform.rotation = Quaternion.Euler(gamedata.PlayerLookPointRotX, 0, 0);
                }

                for (int loopcounter = 0; loopcounter < this.GetComponent<DynamicEnviormentData>().dynamicobjectslist.Count; loopcounter++)
                {
                    float ObjectPosX = gamedata.ObjectsPosX[loopcounter];
                    float ObjectPosY = gamedata.ObjectsPosY[loopcounter];
                    float ObjectPosZ = gamedata.ObjectsPosZ[loopcounter];
                    this.GetComponent<DynamicEnviormentData>().dynamicobjectslist[loopcounter].transform.position = new Vector3(ObjectPosX, ObjectPosY, ObjectPosZ);
                    float ObjectVelocityX = gamedata.ObjectVelocityX[loopcounter];
                    float ObjectVelocityY = gamedata.ObjectVelocityY[loopcounter];
                    float ObjectVelocityZ = gamedata.ObjectVelocityZ[loopcounter];
                    this.GetComponent<DynamicEnviormentData>().dynamicobjectslist[loopcounter].GetComponent<Rigidbody>().velocity = new Vector3(ObjectVelocityX, ObjectVelocityY, ObjectVelocityZ);
                    float ObjectAngularVelocityX = gamedata.ObjectAngularVelocityX[loopcounter];
                    float ObjectAngularVelocityY = gamedata.ObjectAngularVelocityY[loopcounter];
                    float ObjectAngularVelocityZ = gamedata.ObjectAngularVelocityZ[loopcounter];
                    this.GetComponent<DynamicEnviormentData>().dynamicobjectslist[loopcounter].GetComponent<Rigidbody>().angularVelocity = new Vector3(ObjectAngularVelocityX, ObjectAngularVelocityY, ObjectAngularVelocityZ);
                    float ObjectRotationX = gamedata.ObjectRotationX[loopcounter];
                    float ObjectRotationY = gamedata.ObjectRotationY[loopcounter];
                    float ObjectRotationZ = gamedata.ObjectRotationZ[loopcounter];
                    this.GetComponent<DynamicEnviormentData>().dynamicobjectslist[loopcounter].transform.localRotation = Quaternion.Euler(ObjectRotationX, ObjectRotationY, ObjectRotationZ);
                }
                LoadingnewScene = true;

            }

        }
        else
        {
            Debug.Log("FIle NOT FOUND in " + filepath);
        }
    }
}
