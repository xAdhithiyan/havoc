using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class enemy1 : MonoBehaviour
{
	[SerializeField] Transform _playerTransform;
	[SerializeField] private float _lerpSpeed;
	[SerializeField] private float _backupSpeed;
	[SerializeField] private float _backupTime;
	[SerializeField] private Player _player;
	
	private Vector3 _towardsPlayerDirection;
	private bool _onContact = false;
	private bool _idlePosition = false;

	[Header("Health values")]
	private float _maxHealth = 4f;
	private float _currentHealth;
	private float _attackValue = 3f;
	private bool _dead = false;

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
		if(_idlePosition || _dead)
		{
			return;
		}

		if(_onContact)
		{
			transform.position += (transform.position - _playerTransform.position).normalized * _backupSpeed * Time.deltaTime;
			return;
		}
		_towardsPlayerDirection = (_playerTransform.position - transform.position).normalized;
		transform.position += _towardsPlayerDirection * _lerpSpeed * Time.deltaTime;

		// not using lerp because the interpolation value is very small(speed reduces) when distance is small
		//transform.position = Vector3.Lerp(transform.position, _playerTransform.position, Time.deltaTime );
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		_onContact = true;

		// player takes damage
		_player.takeDamage(_attackValue);
		StartCoroutine(waitForDashToEnd());
	}

	private IEnumerator waitForDashToEnd()
	{
		yield return new WaitForSeconds(_backupTime);
		_idlePosition = true;
		StartCoroutine(waitForIdleToEnd());
	}

	private IEnumerator waitForIdleToEnd()
	{
		yield return new WaitForSeconds(_backupTime);
		_idlePosition = false;
		_onContact= false;
	}

	public void takeDamage(float damage)
	{
		_currentHealth -= damage;
		_animator.SetTrigger("attack");

		if (_currentHealth <= 0)
		{
			_dead = true;
			die();
		}

		// for the enemy to backdash after getting hit(usally the backdash happens when the enemy hits the player)
		_onContact = true;
		StartCoroutine(waitForDashToEnd());
	}
	private void die()
	{
		Debug.Log("Enemy died");
		_animator.SetTrigger("death");
		StartCoroutine(WaitForDeathAnimation());
	}

	private IEnumerator WaitForDeathAnimation()
	{
		// gets info on the current animation 
		AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		float length = stateInfo.length;
		yield return new WaitForSeconds(length);

		Destroy(gameObject);
	}
}

	