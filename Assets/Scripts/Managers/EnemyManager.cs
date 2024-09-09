using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class enemySpawn
{
	public Transform SpawnPoint;
	public GameObject enemyObjectPrefab;
}

public class EnemyManager : MonoBehaviour
{
	public enemySpawn[] enemies;
	[SerializeField] private float spawnBoxLength;
	[SerializeField] private int spawnTime;
	[SerializeField] private float triggerRadius;
	[SerializeField] private LayerMask playerLayer;
	private int totalEneimes = 0;
	private int currentTotalEnemies;
	private int maxEnemyIndex = 0;
	private int RandomEnemy;

	private Vector3 randomSpawn;
	private List<GameObject> activeEnimes = new List<GameObject>();

	private float maxEnimiesAtOnce = 3;
	private float maxEnimiesInGame = 20;

	private bool checkTrigger = true;
	private enemySpawn currentTriggerSpawn;
	private GameObject currentTriggerObject;
	private bool foundPlayer = false;
	private bool waitForNextTriggerBool = false;

	[SerializeField] TargetIndicator targetIndicator;

	private void Awake()
	{
		StartCoroutine(Spawing());	
		currentTotalEnemies = totalEneimes;
		currentTriggerSpawn = enemies[0];
	}

	private void Update()
	{
		if(waitForNextTriggerBool)
		{
			waitForNextTrigger();
		}
		if(checkTrigger)
		{
			TriggerEachEnemy(currentTriggerSpawn);
		}
	}

	private IEnumerator Spawing()
	{
		WaitForSeconds wait = new WaitForSeconds(spawnTime);

		while (true && totalEneimes < maxEnimiesInGame)
		{
			activeEnimes.RemoveAll(enemy => enemy == null);
			if(activeEnimes.Count < maxEnimiesAtOnce && !checkTrigger && !waitForNextTriggerBool)
			{
				GenerateRandom();
				GameObject spawned = Instantiate(enemies[RandomEnemy].enemyObjectPrefab, randomSpawn, Quaternion.identity);
				activeEnimes.Add(spawned);
				totalEneimes++;

				// need to change
				if (totalEneimes == currentTotalEnemies + 5)
				{
					maxEnemyIndex++;
					currentTotalEnemies += 5;
					currentTriggerSpawn = enemies[maxEnemyIndex];
					waitForNextTriggerBool = true;
				}
			}
			yield return wait;
		}
	}
	private void GenerateRandom()
	{
		RandomEnemy = Random.Range(0, maxEnemyIndex + 1);
		float randomX = Random.Range(enemies[RandomEnemy].SpawnPoint.position.x, enemies[RandomEnemy].SpawnPoint.position.x + spawnBoxLength);
		float randomY = Random.Range(enemies[RandomEnemy].SpawnPoint.position.y, enemies[RandomEnemy].SpawnPoint.position.y + spawnBoxLength);
		randomSpawn = new Vector3 (randomX, randomY, 0);
	}

	private void waitForNextTrigger()
	{
		if(activeEnimes.Count == 0) {
			checkTrigger = true;
			waitForNextTriggerBool = false;
		}
	}

	private void TriggerEachEnemy(enemySpawn currentTriggerEnemy)
	{
		Debug.Log("checkingfortrigger");
		if(!foundPlayer)
		{
			targetIndicator.gameObject.SetActive(true);
		}

		Collider2D player = Physics2D.OverlapCircle(currentTriggerEnemy.SpawnPoint.position , triggerRadius, playerLayer);
		if (player != null && !foundPlayer)
		{
			Debug.Log("Triggered");
			targetIndicator.StopArrow();
			currentTriggerObject = Instantiate(currentTriggerEnemy.enemyObjectPrefab, currentTriggerEnemy.SpawnPoint.position + new Vector3(2,2,0), Quaternion.identity);
			foundPlayer = true;
		}
		if(currentTriggerObject == null && foundPlayer) {
			checkTrigger = false;
			foundPlayer = false;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (currentTriggerSpawn != null)
		{
			Gizmos.DrawWireSphere(currentTriggerSpawn.SpawnPoint.position, triggerRadius);
		}
	}
}
