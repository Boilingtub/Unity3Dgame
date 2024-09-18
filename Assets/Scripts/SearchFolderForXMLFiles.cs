using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Xml;
using TMPro;
using System;

public class SearchFolderForXMLFiles : MonoBehaviour
{
public List<string> FoundFilesList = new List<string>();
public List<GameObject> ButtonsList = new List<GameObject>();
public GameObject ButtonTemplate;
public GameObject CreateButton , RenameButton , OpenButton , DeleteButton , FileNameInputBox , CloseFileExplorerButton;
public string Selectedfilepath;

public string globalFolderPath , globalExtentionFilter;




public void RunFileExplorer(string FolderToSearchIn , string ExtentionFilter , GameObject SenderObject , string FunctionToRun)
{
 globalFolderPath = FolderToSearchIn;
 globalExtentionFilter = ExtentionFilter;   
    FileNameInputBox.SetActive(false);
    GetFilesAtDirectory(FolderToSearchIn , ExtentionFilter);
    OpenButton.GetComponent<Button>().onClick.AddListener(delegate(){OpenFile(SenderObject , FunctionToRun);});
    CreateButton.GetComponent<Button>().onClick.AddListener(delegate(){CreateNewXMLFile();});
    DeleteButton.GetComponent<Button>().onClick.AddListener(delegate(){DeleteXMLFile();});
    RenameButton.GetComponent<Button>().onClick.AddListener(delegate(){RenameXMLFile();});
    CloseFileExplorerButton.GetComponent<Button>().onClick.AddListener(delegate(){CloseFileExplorer();});
    
   
}

public void GetFilesAtDirectory(string FolderToSearchIn , string ExtentionFilter)
{
    foreach(GameObject obj in ButtonsList)
    {
        Destroy(obj);
    }

    ButtonsList.Clear();
    FoundFilesList.Clear();

    if(Directory.Exists(FolderToSearchIn))
    {
    ButtonTemplate.SetActive(true);
    
        foreach(string file in Directory.GetFiles(FolderToSearchIn , ExtentionFilter , SearchOption.AllDirectories))
        {
            FoundFilesList.Add(file);
            GameObject NewButton = Instantiate(ButtonTemplate) as GameObject;

            ButtonsList.Add(NewButton);
          
            NewButton.SetActive(true);

            TMP_Text NewButtonText = NewButton.GetComponentInChildren<TMP_Text>();

            NewButtonText.text = file.Substring(file.LastIndexOf("\\") + 1);
            
            NewButton.name = NewButton.name + "(" + FoundFilesList.Count + ")";
            NewButton.transform.SetParent(ButtonTemplate.transform.parent , false);
            NewButton.GetComponent<Button>().onClick.AddListener(delegate(){FileSelectButtonClick(NewButton.name);});
            
        }        
    ButtonTemplate.SetActive(false);
    }
    else
    {
        Debug.Log("Directory does not Exist ... Creating");
        Debug.Log(FolderToSearchIn);
        DirectoryInfo di = Directory.CreateDirectory(FolderToSearchIn);
        GetFilesAtDirectory(FolderToSearchIn , ExtentionFilter);

    }
}

void FileSelectButtonClick(string buttonname)
{
Selectedfilepath = FoundFilesList[ButtonsList.IndexOf(GameObject.Find(buttonname))];
}

public void OpenFile(GameObject SenderObject , string FunctionToRun)
{
GameObject.FindObjectOfType<WorldSaveAndLoad>().worldfilepath = Selectedfilepath;
if(GameObject.Find("LevelEditor") != null)
{
    GameObject.FindObjectOfType<LevelEditorCameraMove>().PlayerCanSelect = true;
}
SenderObject.SendMessage(FunctionToRun);
Destroy(this.gameObject);

}
public void CreateNewXMLFile()
{
int dupecount = 1;
int i = 0;
string newxmlfilepath = globalFolderPath + "\\New XmlFile(0).xml";
Debug.Log(globalFolderPath);
while(i < FoundFilesList.Count)
{

    if(newxmlfilepath == FoundFilesList[i])
    {
     dupecount++;
     string TempString;
      string ReplacementString = "(" + dupecount + ")";
      string TobeReplacedString;
      int startindex = newxmlfilepath.LastIndexOf("(");
      int endindex = newxmlfilepath.LastIndexOf(")");
      int extentionstartindex = newxmlfilepath.LastIndexOf(".");

      
      TobeReplacedString = newxmlfilepath.Substring(startindex , endindex + 1 - startindex);
      Debug.Log(ReplacementString);
     TempString = newxmlfilepath.Replace(TobeReplacedString , ReplacementString);
     newxmlfilepath = TempString;
      Debug.Log(newxmlfilepath);
    
  i = 0;
    }
    i++;
}
XmlWriter xmlwriter = XmlWriter.Create(newxmlfilepath);
xmlwriter.Close();

GetFilesAtDirectory(globalFolderPath , globalExtentionFilter);
}

public void DeleteXMLFile()
{
File.Delete(Selectedfilepath);
File.Delete(Selectedfilepath + ".meta");
Selectedfilepath = "";
GetFilesAtDirectory(globalFolderPath , globalExtentionFilter);
}

public void RenameXMLFile()
{
    if(Selectedfilepath != "")
    {
     FileNameInputBox.SetActive(true);
     FileNameInputBox.GetComponent<TMP_InputField>().text = Selectedfilepath.Substring(Selectedfilepath.LastIndexOf("\\") + 1 , Selectedfilepath.LastIndexOf(".") - Selectedfilepath.LastIndexOf("\\")-1);
     FileNameInputBox.GetComponent<TMP_InputField>().onEndEdit.AddListener(ChangeFileName);
    }
    else
    {
    return;    
    }

}
public void ChangeFileName(string text)
{
string newFilepath = Selectedfilepath.Remove(Selectedfilepath.LastIndexOf("\\") + 1 , Selectedfilepath.Length - Selectedfilepath.LastIndexOf("\\") - 1) + FileNameInputBox.GetComponent<TMP_InputField>().text + Selectedfilepath.Substring(Selectedfilepath.LastIndexOf(".") , Selectedfilepath.Length - Selectedfilepath.LastIndexOf("."));
File.Move(Selectedfilepath , newFilepath);
FileNameInputBox.GetComponent<TMP_InputField>().onEndEdit.RemoveAllListeners();
FileNameInputBox.SetActive(false);
GetFilesAtDirectory(globalFolderPath , globalExtentionFilter);
}

public void CloseFileExplorer()
{
 Selectedfilepath = null;
Destroy(this.gameObject);
}

}
