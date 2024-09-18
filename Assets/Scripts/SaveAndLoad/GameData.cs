using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class GameData
{
public float PlayerMovSpeed;
public float MouseSensitivity;
public float AudioVolume;
public string level;

public float PlayerPosX , PlayerPosY , PlayerPosZ;
public float PlayerLookPointRotX , PlayerRotY;


public List<float> ObjectsPosX = new List<float>();
public List<float> ObjectsPosY = new List<float>();
public List<float> ObjectsPosZ = new List<float>();
public List<float> ObjectVelocityX = new List<float>();
public List<float> ObjectVelocityY = new List<float>();
public List<float> ObjectVelocityZ = new List<float>();
public List<float> ObjectAngularVelocityX = new List<float>();
public List<float> ObjectAngularVelocityY = new List<float>();
public List<float> ObjectAngularVelocityZ = new List<float>();
public List<float> ObjectRotationX = new List<float>();
public List<float> ObjectRotationY = new List<float>();
public List<float> ObjectRotationZ = new List<float>();

}
