using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
  [SerializeField] private AudioManager audioManager;
  void Start()
  {
    audioManager.Play("MainTheme");
  }
}
