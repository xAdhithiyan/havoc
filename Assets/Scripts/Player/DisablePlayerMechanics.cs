using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerMechanics : MonoBehaviour
{
	public bool startNextSceneCollider;
	public Animator animator;

	[SerializeField] private Player player;
	private bool checkForWalkSound = true;

	private void Awake()
	{
		player.ArenaSceneActive = false;
	}

	private void Update()
	{
		if(checkForWalkSound && player.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
		{
			FindObjectOfType<AudioManager>().Play("PlayerWalking", true);
			checkForWalkSound = false;
		} else if(!checkForWalkSound && player.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
		{
			FindObjectOfType<AudioManager>().Stop("PlayerWalking");
			checkForWalkSound = true;
		}
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
