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
 - animation and particle effects and sound
 - the entire gameplay loop
 - 2nd cutscene 
 - menu

 - path finder
 - shaders
*/