using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public Animator animator;
	public float transitionTime;
	public void LoadNextLevel()
	{
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}

	private IEnumerator LoadLevel(int levelIndex)
	{
		animator.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(levelIndex);
	}

	public void LoadArena()
	{
		SceneManager.LoadScene(3);
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}