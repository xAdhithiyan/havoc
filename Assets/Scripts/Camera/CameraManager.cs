using Cinemachine; // used for camera movements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
  [SerializeField] private CinemachineVirtualCamera followCamera;
	public void assignCamera(Transform PlayerPosition)
  {
    followCamera.Follow = PlayerPosition;
  }
}

/* 
 - make the characters stay in the arena
 - one boss 
 - make that gimick
 - annimation and particle effects and sound
 - the entire gameplay loop
 - make the arena (half left) and add flags with names
 - resume

 - path finder
 - shaders
*/