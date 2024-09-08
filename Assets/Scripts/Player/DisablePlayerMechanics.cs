using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerMechanics : MonoBehaviour
{
	public bool startNextSceneCollider;

	[SerializeField] private Player player;
	private void Awake()
	{
		player.ArenaSceneActive = false;
	}

	public void disableMovement()
	{
		player.MovementActive = false;
	}

	public void enableMovement()
	{
		player.MovementActive = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(startNextSceneCollider)
		{
			FindObjectOfType<LevelLoader>().LoadNextLevel();
			startNextSceneCollider = false;
		}
	}
}
