using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy5Base : MonoBehaviour
{
	private GameObject _player;
	private float _maxDistance = 1.5f;
	private float _offsetDistance = 0;
	private int _x = 0;
	private int _y = 1;
	private int _directionX = 1;
	private int _directionY = 1;

	private Vector3 _offsetPlayerPosition;
	private GameObject _currentEnemy5;
	private Animator _animator;

	private bool _movingAwayFromPlayer = true;
	private bool _moving = true;
	//private bool checkTrigger = false;

	private void Start()
	{
		_player = GameObject.FindWithTag("MainPlayer");
		_animator = GetComponent<Animator>();
		StartCoroutine(standForAttack());
	}

	private void Update()
	{
		_offsetPlayerPosition = _player.transform.position + new Vector3(0,0.5f,0);
		if (_moving)
		{
			if (_movingAwayFromPlayer)
			{
				movingAwayFromPlayer();
			}
			else
			{
				movingtowardsPlayer();
			}
		}
	}

	private void movingAwayFromPlayer()
	{
		if (_offsetDistance < _maxDistance)
		{
			transform.position = _offsetPlayerPosition + new Vector3(_x * _directionX * _offsetDistance, _y * _directionY * _offsetDistance, 0);
			_offsetDistance += Time.deltaTime;
			//Debug.Log("Away " + _directionX + " " + _directionY);
		}
		else
		{
			_movingAwayFromPlayer = false;
		}
	}

	private void movingtowardsPlayer()
	{
		if(_offsetDistance > 0)
		{
			transform.position = _offsetPlayerPosition + new Vector3(_x * _directionX * _offsetDistance, _y * _directionY * _offsetDistance, 0);
			_offsetDistance -= Time.deltaTime;
			//Debug.Log("Towards " + _directionX +  " " + _directionY);
		}
		else
		{
			do
			{
				_movingAwayFromPlayer = true;
				_x = Random.Range(0, 2);
				_y = Random.Range(0, 2);
				_directionX = Random.Range(0, 2) * 2 - 1; // either 1 or -1 
				_directionY = Random.Range(0, 2) * 2 - 1; 
			} while (_x == 0 && _y == 0);
		}
	}

	public void playAlertAnimation()
	{
		_animator.SetTrigger("alert");
	}

	public void playHitAnimation()
	{
		_moving = false;
		_animator.SetTrigger("hit");
		StartCoroutine(WaitForDeathAnimation());
	}

	private IEnumerator standForAttack()
	{
		_currentEnemy5 = GameObject.FindWithTag("enemy5");
		yield return new WaitForSeconds(2f);
		_currentEnemy5.GetComponent<enemy5>()._endJump = true;	
	}

	private IEnumerator WaitForDeathAnimation()
	{
		// Get the runtime animator controller
		RuntimeAnimatorController ac = _animator.runtimeAnimatorController;

		// Find the death animation clip length
		float length = 0;
		foreach (var clip in ac.animationClips)
		{
			if (clip.name == "enemy5Base_hit")
			{
				length = clip.length;
				break;
			}
		}

		yield return new WaitForSeconds(length);

		Destroy(gameObject);
	}
}
