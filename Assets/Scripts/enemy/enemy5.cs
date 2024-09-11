using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class enemy5 : EnemyBase
{
	[Header("movement")]
	[SerializeField] private Player _player;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _checkRadius;
	[SerializeField] private LayerMask _playerLayerMask;
	private float _jumpValue = 40;

	[Header("Emeny 5 Base")]
	[SerializeField] private enemy5Base _enemy5BasePrefab;
	[SerializeField] private float indicateValueOffset;
	[SerializeField] private float dogeWindow;
	private enemy5Base _singleEnemy5Base;
	private enemy5Base _currentEnemy5Base;

	[Header("Emeny 5 Health Values")]
	[SerializeField] private float _attackValue;
	[SerializeField] private float _maxHealth;
	private float _currentHealth;

	[Header("Core Values")]
	[SerializeField] private Animator _animator;
	private Rigidbody2D _rb;

	[Header("")]

	private bool _playerFound = false;
	public bool _endJump = false;
	private bool hasDashed = false;


	private void Start()
	{
		_player = FindObjectOfType<Player>();
		_rb = GetComponent<Rigidbody2D>();
		_currentHealth = _maxHealth;
	}
	private void Update()
	{
		if (_player == null)
		{
			return;
		}

		if (!_playerFound)
		{
			movement();
			checkForPlayer();
		}
		if (_endJump)
		{
			endJump();
		}
	}
	private void movement()
	{
		Vector2 moveDirection = (_player.transform.position - transform.position).normalized;
		_rb.velocity = moveDirection * _moveSpeed;
	}

	private void checkForPlayer()
	{
		Collider2D player = Physics2D.OverlapCircle(transform.position, _checkRadius, _playerLayerMask);
		if (player != null)
		{
			_playerFound = true;
			_rb.velocity = Vector2.zero;
			StartCoroutine(waitForJump());
		}
	}

	private IEnumerator waitForJump()
	{
		yield return new WaitForSeconds(1f);
		_rb.velocity = Vector2.up * _jumpValue;
		
		yield return new WaitForSeconds(2f);
		_rb.velocity = Vector2.zero;

		_singleEnemy5Base = Instantiate(_enemy5BasePrefab, _player.transform.position, Quaternion.identity, transform);
	}

	private void endJump()
	{
		_currentEnemy5Base = GetComponentInChildren<enemy5Base>();

		if( _currentEnemy5Base != null)
		{
			Vector3 towardsEnemy5Base = (_currentEnemy5Base.transform.position - transform.position);
			_rb.velocity = towardsEnemy5Base.normalized * _jumpValue;

			if ((transform.position.y - _currentEnemy5Base.transform.position.y) < indicateValueOffset)
			{
				_currentEnemy5Base.playAlertAnimation();
			}

			if(transform.position.y < _currentEnemy5Base.transform.position.y)
			{
				_rb.velocity = Vector2.zero;
				
				if(!hasDashed)
				{
					_player.takeDamage(_attackValue);
				}
				else
				{
					Debug.Log("escaped");
				}

				_endJump = false;
				hasDashed = false;
				_currentEnemy5Base.playHitAnimation();
				StartCoroutine(Restart());
			} else if ((transform.position.y - _currentEnemy5Base.transform.position.y) < dogeWindow)
			{
					if(_player._dashEnabled)
				{
					hasDashed = true;
				}
			}
		}
		else
		{
			Debug.Log("_currentEnemy5Base is NULL");
		}
	}

	private IEnumerator Restart()
	{
		yield return new WaitForSeconds(2f);
		_playerFound = false;
	}


	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, _checkRadius);
	}

	public override void takeDamage(float damage)
	{
		_currentHealth -= damage;
		_animator.SetTrigger("hit");
		if(_currentHealth <= 0){
			death();
		}
	}

	public void death()
	{
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
			if (clip.name == "enemy5_death")
			{
				length = clip.length;
				break;
			}
		}

		yield return new WaitForSeconds(length);

		Destroy(gameObject);
	}
}


