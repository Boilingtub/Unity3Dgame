using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsSliders : MonoBehaviour
{

  
public AudioMixer audiomixer;
public Slider MouseSensSlider;
public Slider VolumeSlider;


void OnEnable()
{
    UpdateSliderPos();
}



public void SetVolume(float slidervalue)
{
 audiomixer.SetFloat ("MixerVol" , Mathf.Log10 (slidervalue) * 20); 
}



public void SetMouseSens(float slidervalue)
{
  PlayerMovement.MouseSens   = slidervalue;
}



public void UpdateSliderPos()
{
MouseSensSlider.value = PlayerMovement.MouseSens; 
bool result = audiomixer.GetFloat("MixerVol" , out var vol);
vol = vol / 20;
vol = Mathf.Pow(10 , vol);
VolumeSlider.value = vol;
}



}
