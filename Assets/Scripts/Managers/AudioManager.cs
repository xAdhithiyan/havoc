using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
	public Sounds[] sounds;

	public void Play(string name, bool loop = false)
	{
		Sounds currentSound = Array.Find(sounds, sound => sound.name == name);
		if(currentSound != null)
		{
			if(currentSound.source == null)
			{
				currentSound.source = gameObject.AddComponent<AudioSource>();
				currentSound.source.clip = currentSound.clip;

				currentSound.source.volume = currentSound.volume;
			}

			if (loop)
			{
				currentSound.source.loop = true;
			}
			currentSound.source.Play();
		}
	}
	public void Stop(string name)
	{
		Sounds currentSound = Array.Find(sounds, sound => sound.name == name);
		if (currentSound != null)
		{
			currentSound.source.Stop();
		}
	}

}
