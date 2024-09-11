using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueHairScripts : MonoBehaviour
{
	private GameObject restartMenu;
	private GameObject WinMenu;
	private Player player;
	
	private void Start()
	{
		player = FindObjectOfType<Player>();

		restartMenu = GameObject.Find("RestartMenu");

		if (restartMenu != null)
		{
			WinMenu = restartMenu.transform.Find("WinMenu")?.gameObject;
		}

		if (WinMenu != null)
		{
			WinMenu.SetActive(false);
		} else
		{
			Debug.LogError("DeathMenu not found under RestartMenu");
		}

	}
	private void Update()
	{
		transform.Rotate(0, 0, 60 * Time.deltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(gameObject);
		if(player != null)
		{
			Destroy(player.gameObject);
		}
		WinMenu.SetActive(true);
	}
}
