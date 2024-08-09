using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuriken : MonoBehaviour
{
	[SerializeField] private float _shurikenSpeed;
	private Vector3 _throwDirection;
	private Rigidbody2D _rb;

	[SerializeField] private LayerMask _enemyLayerMask;
	[SerializeField] private LayerMask _colldiersLayerMask;

	private float _shurikenDamage = 2f;
	private float _rotationSpeed = 1080f;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
	}
	public void throwShuriken(Vector2 _playerPosition, Vector2 throwPoint) 
	{
		Vector2 _throwDirection = (_playerPosition - throwPoint).normalized;
		_rb.velocity = -_throwDirection * _shurikenSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// current collision layer
		int collisionLayer = collision.gameObject.layer;

		// here _enemyLayerMask is represented as bitmask and collisionLayer is represented as an integer. The collisionLayer is converted to a bitmask by using the left shift operator. for example if collisonLayer is 2 then 1 << 2 = 0100. we are moving 1 twice towards the left
		if (_enemyLayerMask == (1 << collisionLayer))
		{
			EnemyBase enemy = collision.GetComponent<EnemyBase>();
			enemy.takeDamage(_shurikenDamage);
			Destroy(gameObject);
		}
		
		if(_colldiersLayerMask == (1 << collisionLayer))
		{
			Destroy(gameObject);
		}
	}
}
