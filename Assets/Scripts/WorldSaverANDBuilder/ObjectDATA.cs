using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectDATA : MonoBehaviour
{
public enum ObjectType {Cylinder , Sphere , Cube , Capsule , Plane , Empty , PlayerSpawnPoint};
[Serializable]
public struct ObjectVariables
{

public string objectname;
public ObjectType objecttype;
public float positionX , positionY , positionZ;
public float rotationX , rotationY , rotationZ;
public float scaleX , scaleY , scaleZ;
public bool PhysicsIsDynamic , PhysicsIsStatic ;
public bool PlayerInteractionIsPickupable , PlayerInteractionIsInteractable;
public float RigidBodyMass;
public float ColorR , ColorG , ColorB , ColorA;
public string TexturePath;
public string ShaderName;
public float TextureScaleX , TextureScaleY;

}
public ObjectVariables objectdata;

}
