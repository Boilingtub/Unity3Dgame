using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
public static string level = "MainMenu";
public void Start()
{
SceneManager.LoadSceneAsync(level);
}
}
