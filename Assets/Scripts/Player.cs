using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
	[Header("Movement values")]
	[SerializeField] private float _maxSpeed = 8f;
	[SerializeField] private float _acceleration = 10f;
	[SerializeField] private float _decceleration = 12f;
	[SerializeField] private float _grappleForce = 2f;
	private float _currentSpeed = 0f;
	private float _horizontalValue;
	private float _verticalValue;

	[Header("dash values")]
	[SerializeField] private float _dashSpeed = 22f;
	[SerializeField] private float _dashTime = 0.1f;
	private bool _dashEnabled = false;

	[Header("Camera")]
	[SerializeField] private CameraManager _cameraManager;

	[Header("sword attack")]
	[SerializeField] private Transform _attackPoint;
	[SerializeField] private LayerMask _layerMask;
	[SerializeField] private float _attackRange = 0.5f;
	private float _swordAttackDamange = 2f;
	private float _attacksPerSeconds = 4f;
	private float _nextAttackTime = 0f;

	[Header("Health values")]
	private float _maxHeath = 20f;
	private float _currentHealth;

	[Header("shuriken stuff")]
	[SerializeField] private shuriken _shurikenPrefab;
	private shuriken _singleShuriken;

	[SerializeField] private HealthBar _healthBar;
	 
	private Vector2 _directionOfPlayer;
	private Rigidbody2D _rb;
	private Animator _animator;

	private KeyCode _upKey = KeyCode.W;
	private KeyCode _downkey = KeyCode.S;
	private KeyCode _leftKey = KeyCode.A;
	private KeyCode _rightKey = KeyCode.D;
	private KeyCode _dashKey = KeyCode.LeftShift;
	private KeyCode _attackKey = KeyCode.Mouse0;
	private KeyCode _secondaryAttackKey = KeyCode.Mouse1;

	void Start()
  {
    _rb = GetComponent<Rigidbody2D>();
    _cameraManager.assignCamera(transform);
		_animator = GetComponent<Animator>();
		_currentHealth = _maxHeath;
		_healthBar.setMaxHealth(_currentHealth);
  }

	private void Update()
	{
		directionalMovement();
		dash();
		attack();
		checkForShurikenThrow();
	}
	private void FixedUpdate()
	{
		playerLookDirection();
	}

	private void directionalMovement()
  {
		_animator.SetBool("dash", _dashEnabled);	
		_animator.SetBool("dash", _dashEnabled);	
		if (_dashEnabled)
		{
			return;
		}

		//gets the horizontal/vertical range from - 1 to 1
		_horizontalValue = Input.GetAxis("Horizontal");
		_verticalValue = Input.GetAxis("Vertical");
		// setting the direction 
		_rb.velocity = new Vector2(_horizontalValue, _verticalValue);

		if (_rb.velocity != Vector2.zero)  // checking magnitute is not zero (!= Vector2(0,0))
		{
			if (Input.GetKey(_upKey) || Input.GetKey(_downkey) || Input.GetKey(_leftKey) || Input.GetKey(_rightKey))
			{
				_directionOfPlayer = _rb.velocity;
				if (_currentSpeed < _maxSpeed)
				{
					_currentSpeed += _acceleration * Time.deltaTime;
				}
			}
		}
		else
		{
			if (_currentSpeed > 0f)
			{
				_currentSpeed -= _decceleration * Time.deltaTime;
			}
			else
			{
				_currentSpeed = 0f;
			}
		}

		// increasing the magnitute 
		_rb.velocity = _directionOfPlayer * _currentSpeed;
		_animator.SetFloat("speed", _currentSpeed);
		// Debug.Log(_currentSpeed);
	}

	private void dash()
	{
		if (Input.GetKeyDown(_dashKey) && !_dashEnabled)
		{
			StartCoroutine(waitForDashToEnd());
			_dashEnabled = true;
			_rb.velocity = _rb.velocity.normalized * _dashSpeed;
		}
	}
	
	// co routine (similar to async function)
	private IEnumerator waitForDashToEnd()
	{
		yield return new WaitForSeconds(_dashTime);
		_dashEnabled = false;
	}

	private void playerLookDirection()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 directionOfCursor = mousePosition - transform.position;

		// gets the angle by calculating 'tan a = y / x' of rotation in radians 
		float angle = Mathf.Atan2(directionOfCursor.y, directionOfCursor.x);
		transform.rotation = Quaternion.Euler(0, 0, (angle * Mathf.Rad2Deg));
	}

	// not being used.
	public void addForceForGrapple(Transform grappleTransform)
  {	
    Vector2 grappleDirection = grappleTransform.position - transform.position;
    Vector2 grappleDirectionNormalized = grappleDirection.normalized;
    _rb.AddForce(grappleDirectionNormalized * _grappleForce, ForceMode2D.Impulse);
  }

	private void attack()
	{
		if(Time.time >= _nextAttackTime)
		{
			if (Input.GetKeyDown(_attackKey))
			{
				_animator.SetTrigger("attack");
				// gives all the objects with collider that overlap the formed circle; 
				Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _layerMask);
				foreach (Collider2D enemy in hitEnemies)
				{
					// all the enemy class inherit the EnemyBase class. so EnemyBase acts as a middle way to the different enemy class
					EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
					if(enemyScript != null )
					{
						enemyScript.takeDamage(_swordAttackDamange);
					}
				}
				_nextAttackTime = Time.time + 1 / _attacksPerSeconds;
			}
		}
	}
	private void checkForShurikenThrow()
	{
		if(Input.GetKeyDown(_secondaryAttackKey))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// create a shuriken and throw it
			_singleShuriken = Instantiate(_shurikenPrefab, transform.position, transform.rotation);
			_singleShuriken.throwShuriken(transform.position, mousePosition);
		}
	}
	private void OnDrawGizmosSelected()
	{
		if(_attackPoint != null)
		{
			Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
		}
	}
	
	public void takeDamage(float damage)
	{
		_currentHealth -= damage;
		_healthBar.setHealth(_currentHealth);
		Debug.Log("player has taken damage" + _currentHealth);
	}
}
