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
 - animtaion and particle effect
 - dialoge box and health bar ui change and dialoges
 - add health for each enemy.
 - fix that bug (going through walls and enemy 5).
*/