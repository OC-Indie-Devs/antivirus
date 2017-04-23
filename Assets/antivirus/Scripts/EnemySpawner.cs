using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
	// Use this for initialization
	public Transform spawnPoints;
	public GameObject[] enemies;
	public Transform parent;

	public GameObject componentPrefab;

	public Text enemiesDestroyedText;
	public Text componentSpawnText;
	public Text levelPassedText;

	public GameObject[] firewalls;
	int fwIndex = 0;

	List<GameObject> enemyList;

	bool firewallDown = true;
	int enemiesDestroyed = 0;

	bool componentExists = false;

	int nextComponent;

	void Start () {
		enemyList = new List<GameObject>();
		int numEnemies = enemies.Length;
		foreach( Transform t in spawnPoints.GetComponentsInChildren<Transform>() )
		{
			GameObject thisEnemy = Instantiate(enemies[Random.Range(0,numEnemies)], t.position, Quaternion.identity, parent);
			enemyList.Add(thisEnemy);
		}
		nextComponent = nextComponentSpawn();
		for (int i = 0; i < firewalls.Length; i++)
		{
			firewalls[i].SetActive(false);
		}
		levelPassedText.enabled = false;
	}

	public void RemoveEnemy(GameObject enemy)
	{
		enemyList.Remove(enemy);

		if (!componentExists)
		{
			enemiesDestroyed++;
			if ( enemiesDestroyed == nextComponent )
			{
				SpawnComponent(enemy.transform.position);
				nextComponent = nextComponentSpawn();
				enemiesDestroyed = 0;
			}
		}

		Destroy(enemy);

		if ( firewallDown )
		{
			SpawnEnemy();
		}

		Debug.Log("enemyList.Count = " + enemyList.Count);
		if (enemyList.Count == 0)
		{
			// level complete
			levelPassedText.enabled = true;
		}
		enemiesDestroyedText.text = "enemiesDestroyed: " + enemiesDestroyed;
	}

	public void SpawnEnemy()
	{
		int numEnemies = enemies.Length;
		GameObject thisEnemy = Instantiate(enemies[Random.Range(0,numEnemies)], transform.position, Quaternion.identity, parent);
		enemyList.Add(thisEnemy);
	}

	// spawn the component needed to repair the circuit and set the firewall that will go up
	// when component is properly placed in the socket.
	void SpawnComponent(Vector3 position)
	{
		Debug.Log("Spawning component");
		GameObject thisComponent = Instantiate(componentPrefab, position, Quaternion.identity);
		CircuitComponent cc = thisComponent.GetComponent<CircuitComponent>();
		cc.highlightOn();
		cc.firewall = firewalls[fwIndex];
		cc.enemySpawner = this;
		componentExists = true;
		fwIndex++;
	}

	int nextComponentSpawn()
	{
		int ncs = Random.Range(3,enemyList.Count);
		componentSpawnText.text = "componentSpawn: " + ncs;
		return ncs;
	}

	public void RaiseFirewall(GameObject firewall)
	{
		firewall.SetActive(true);

		// check if all firewalls are active
		for (int i = 0; i < firewalls.Length; i++)
		{
			if ( firewalls[i].activeSelf == false )
			{
				return;
			}
		}
		firewallDown = false;
	}
}
