using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class enemy2 : EnemyBase
{
	[Header("movement")]
	[SerializeField] private Transform _playerPosiiton;
	[SerializeField] private float _lerpSpeed;
	private Vector3 _towardsPlayerDirection;

	[Header("front dash values")]
	[SerializeField] private float _dashRange;
	[SerializeField] private LayerMask _layerMask;
	[SerializeField] private float _frontDashSpeed;
	[SerializeField] private float _waitingTime;
	[SerializeField] private float _dashTime;
	private Vector3 _currentPosition;
	private bool _inDash = false;
	private bool _inWaiting = false;

	[Header("health values")]
	private float _maxHealth = 6f;
	private float _currentHealth;
	
	private Rigidbody2D _rb;
	private Animator _animator;

	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_currentHealth = _maxHealth;
	}
	private void Update()
	{
		_towardsPlayerDirection = (_playerPosiiton.position - transform.position).normalized; 
		if(!_inDash)
		{
			movement();
			checkForFrontDash();
		}
	}

	private void movement()
	{
		_currentPosition = _towardsPlayerDirection * _lerpSpeed * Time.deltaTime;
		transform.position += _currentPosition;
	}

	private void checkForFrontDash()
	{
		Collider2D player = Physics2D.OverlapCircle(transform.position, _dashRange, _layerMask);
		if(player != null)
		{
			_animator.SetBool("angry", true);
			StartCoroutine(waitingTime(false));
		}
	}

	private IEnumerator waitingTime(bool dashFinished)
	{
		_inDash = true;
		yield return new WaitForSeconds(_waitingTime);
		if(!dashFinished )
		{
			dash();
		} else
		{
			_inDash = false;
		}
	}

	private IEnumerator dashTime()
	{
		yield return new WaitForSeconds(_dashTime);
		_rb.velocity = Vector3.zero;
		_animator.SetBool("angry", false);
		_inWaiting = false;
		StartCoroutine(waitingTime(true));
	}
		
	private void dash()
	{
		_rb.velocity = _towardsPlayerDirection * _frontDashSpeed;
		_inWaiting = true;
		StartCoroutine(dashTime());
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(_inWaiting)
		{
			StopAllCoroutines();
			_inWaiting = false;
			_rb.velocity = Vector3.zero;
			_animator.SetBool("angry", false);
			StartCoroutine(waitingTime(true));
		}
	}

	public override void takeDamage(float damage)
	{
		_currentHealth -= damage;
		_animator.SetTrigger("attack");
		if ( _currentHealth <= 0)
		{
			die();
		}
	}

	private void die()
	{
		Debug.Log("Enemy died");
		_animator.SetTrigger("death");
		StartCoroutine(WaitForDeathAnimation());
	}
	private IEnumerator WaitForDeathAnimation()
	{
		// Get the runtime animator controller
		RuntimeAnimatorController ac = _animator.runtimeAnimatorController;

		// Find the death animation clip length
		float length = 0;
		foreach (var clip in ac.animationClips)
		{
			if (clip.name == "enemy-2_death")
			{
				length = clip.length;
				break;
			}
		}

		yield return new WaitForSeconds(length);

		Destroy(gameObject);
	}
private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, _dashRange);
	}
}
