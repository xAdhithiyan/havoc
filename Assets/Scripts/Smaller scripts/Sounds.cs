using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;

[System.Serializable]
public class Sounds 
{
	public string name;
	public AudioClip clip;
	public float volume;

	[HideInInspector]
	public AudioSource source;
}
