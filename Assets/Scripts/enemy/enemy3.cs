using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class enemy3 : EnemyBase
{
	[Header("movement")]
	[SerializeField] private Transform _playerPosition;
	[SerializeField] private float _checkRadius;
	[SerializeField] private LayerMask _playerLayerMask;
	[SerializeField] private float _moveSpeed;

	[Header("health values")]
	private float _maxHealth = 8f;
	private float _currentHealth;

	[Header("prefabs")]
	[SerializeField] private Enemy3Attack _enemy3AttackPrefab;
	private Enemy3Attack _singleEnemy3Atttack;

	private Rigidbody2D _rb;
	private Animator _animator;
	private bool _playerFound = false;
	private bool _isMoving = true;
	private int _count = 0;

	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_currentHealth = _maxHealth;
	}

	private void Update()
	{
		lookDirection();
		if (_isMoving)
		{
			movement();
		}
		if (!_playerFound)
		{
			checkForPlayer();
		}
	}		

	private void movement()
	{
		Vector2 moveDirection = (_playerPosition.position - transform.position).normalized;
		_rb.velocity = moveDirection * _moveSpeed;
		_animator.SetBool("move", true);
	}
	private void lookDirection()
	{

		Vector3 directionTowardsPlayer = _playerPosition.position - transform.position;

		// gets the angle by calculating 'tan a = y / x' of rotation in radians 
		float angle = Mathf.Atan2(directionTowardsPlayer.y, directionTowardsPlayer.x);
		transform.rotation = Quaternion.Euler(0, 0, (angle * Mathf.Rad2Deg));
	}

	private void checkForPlayer()
	{
		Collider2D player = Physics2D.OverlapCircle(transform.position, _checkRadius, _playerLayerMask);
		if(player != null)
		{
			_animator.SetBool("move", false);
			
			_playerFound = true;
			_isMoving = false;
			_rb.velocity = Vector2.zero;
			StartCoroutine(waitForAttack());
		}
	}

	private IEnumerator waitForAttack()
	{
		while (_count < 3)
		{
			_animator.SetTrigger("attack");
			yield return new WaitForSeconds(0.8f);
			_singleEnemy3Atttack = Instantiate(_enemy3AttackPrefab, transform.position, Quaternion.identity);
			_count++;
		}
		_count = 0;
		StartCoroutine(afterAttackTime());	
	}

	private IEnumerator afterAttackTime()
	{
		yield return new WaitForSeconds(1f);
		_isMoving = true;
     yield return new WaitForSeconds(2f);
		_playerFound = false;
	}

	public override void takeDamage(float damage)
	{
		_currentHealth -= damage;
		_animator.SetTrigger("hit");
		if( _currentHealth <= 0)
		{
			Die();
		}
	}
	public void Die()
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
			if (clip.name == "enemy3_death")
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
		Gizmos.DrawWireSphere(transform.position, _checkRadius);	
	}
}
