using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitComponent : MonoBehaviour {

	public bool isInSocket = false;
	public GameObject firewall;
	public EnemySpawner enemySpawner;

	public void enableCircuit()
	{
		enemySpawner.RaiseFirewall(firewall);
	}
}
