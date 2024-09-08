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
	[SerializeField] private float _dashIncrement;
	private float _maxDash = 2f;
	private float _currentDash;
	public bool _dashEnabled = false;
	

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
	[SerializeField] private float _shurikenIncrement;
	private shuriken _singleShuriken;
	private float _maxShuriken = 3;
	private float _currentShuriken = 0f;


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

	// controlled from disablePlayerMechanics script
	public bool ArenaSceneActive = true;
	public bool MovementActive = true;

	void Start()
  {
    _rb = GetComponent<Rigidbody2D>();
    _cameraManager.assignCamera(transform);
		_animator = GetComponent<Animator>();
		_currentHealth = _maxHeath;
		_currentDash = _maxDash;
		_currentShuriken = _maxShuriken;
		_currentShuriken = _maxShuriken;
		MovementActive = true;
		if(ArenaSceneActive)
		{
		_healthBar.setMaxHealth(_currentHealth);
		_healthBar.setMaxDashValue(_currentDash);
		_healthBar.setMaxShurikenValue(_currentShuriken);
		}
	}

	private void Update()
	{
		directionalMovement();
		if(ArenaSceneActive)
		{
			dash();
			attack();
			checkForShurikenThrow();
			dashRefresh();
			shurikenRefresh();
		}
	}
	private void FixedUpdate()
	{
		playerLookDirection();
	}

	private void directionalMovement()
  {
		if(!MovementActive)
		{
			_rb.velocity = Vector2.zero;
			_currentSpeed = 0;
			_animator.SetFloat("speed", _currentSpeed);
			return;
		}

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
		if (Input.GetKeyDown(_dashKey) && !_dashEnabled && _currentDash >= 1)
		{
			StartCoroutine(waitForDashToEnd());
			_dashEnabled = true;
			_rb.velocity = _rb.velocity.normalized * _dashSpeed;
			_currentDash -= 1;
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
		if(!MovementActive)
		{
			return;
		}

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
		if(Input.GetKeyDown(_secondaryAttackKey) && _currentShuriken >= 1)
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// create a shuriken and throw it
			_singleShuriken = Instantiate(_shurikenPrefab, transform.position, transform.rotation);
			_singleShuriken.throwShuriken(transform.position, mousePosition);
			_currentShuriken -= 1;
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

	private void dashRefresh()
	{
		if(_currentDash < _maxDash)
		{
			_currentDash += _dashIncrement * Time.deltaTime;
			_healthBar.setDashValue(_currentDash);
		}
	}

	private void shurikenRefresh()
	{
		if(_currentShuriken < _maxShuriken)
		{
			_currentShuriken += _shurikenIncrement * Time.deltaTime;
			_healthBar.setShurikenValue(_currentShuriken);
		}
	}
}
