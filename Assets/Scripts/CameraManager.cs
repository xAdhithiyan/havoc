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
 - make 2 more enemies with one boss 
 - make dash and shuriken limit
 - make that gimick
 - annimation and particle effects
 - portal
 - the entire gameplay loop
 - make the arena (half left) and add flags with names

 - path finder
 - shaders
*/