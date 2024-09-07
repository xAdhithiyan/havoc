using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy3Attack : MonoBehaviour
{
	private Rigidbody2D _rb;
	private GameObject _player;
	private Vector3 _towardsPlayer;

	[SerializeField] private float _enemy3AttackSpeed;
	[SerializeField] private LayerMask _playerLayerMask;
	[SerializeField] private LayerMask _collionLayerMask;
	private float _attackValue = 3f;

	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		movement();
	}
	private void movement()
	{
		_player = GameObject.FindWithTag("MainPlayer");
		if (_player == null)
		{
			Debug.Log("_player is NULL");
		}
		else
		{
			_towardsPlayer = (_player.transform.position - transform.position).normalized;
			transform.position += _towardsPlayer;
			transform.right = -_towardsPlayer;
			_rb.velocity = _towardsPlayer * _enemy3AttackSpeed;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		int collionLayer = collision.gameObject.layer;
		if(_collionLayerMask == (1 << collionLayer) || _playerLayerMask == (1 << collionLayer))
		{
			if(_playerLayerMask == (1 << collionLayer))
			{
				_player.GetComponent<Player>().takeDamage(_attackValue);
			}
			Destroy(gameObject);
		}
	}
}
