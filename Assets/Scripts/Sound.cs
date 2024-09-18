using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[System.Serializable]
public class Sound
{
[HideInInspector]
public AudioSource source;
public string name;
public bool loop;
public AudioClip clip;
public float pitch = 1f;
public float volume = 1f;
}
