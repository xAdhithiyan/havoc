using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
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

	private Vector2 _directionOfPlayer;
  private Rigidbody2D _rb;

  private KeyCode _upKey = KeyCode.W;
  private KeyCode _downkey = KeyCode.S;
  private KeyCode _leftKey = KeyCode.A;
  private KeyCode _rightKey = KeyCode.D;
	private KeyCode _dashKey = KeyCode.LeftShift;

  void Start()
  {
    _rb = GetComponent<Rigidbody2D>();
    _cameraManager.assignCamera(transform);   
  }

	private void Update()
	{
		directionalMovement();
		dash();
  }
	private void FixedUpdate()
	{
		playerLookDirection();
	}

	private void directionalMovement()
  {
		if(_dashEnabled)
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
		// Debug.Log(_currentSpeed);
	}

	private void dash()
	{
		if (Input.GetKeyDown(_dashKey) && !_dashEnabled)
		{
			StartCoroutine(waitForDashToEnd());
			_dashEnabled = true;
			_rb.velocity = _rb.velocity.normalized * _dashSpeed;
			Debug.Log("Dashing");
		}
	}
	
	// co routine (similar to async function)
	private IEnumerator waitForDashToEnd()
	{
		yield return new WaitForSeconds(_dashTime);
		_dashEnabled = false;
		Debug.Log("Dashing ended");
	}

	private void playerLookDirection()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 directionOfCursor = mousePosition - transform.position;

		// gets the angle by calculating 'tan a = y / x' of rotation in radians 
		float angle = Mathf.Atan2(directionOfCursor.y, directionOfCursor.x);
		transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
	}

	// not being used.
	public void addForceForGrapple(Transform grappleTransform)
  {	
    Vector2 grappleDirection = grappleTransform.position - transform.position;
    Vector2 grappleDirectionNormalized = grappleDirection.normalized;
    _rb.AddForce(grappleDirectionNormalized * _grappleForce, ForceMode2D.Impulse);
  }
}
