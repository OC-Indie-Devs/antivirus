using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	public float pathEndThreshold = 0.1f;
	public float damageRate = 1f;

    private bool hasPath = false;

	Transform currentTarget;
	TileSpawner ts;
	NavMeshAgent agent;

	bool damagingTile = false;
	float travelTime;
	float travelTimeLimit = 20f;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		ts = GameObject.FindWithTag("Tiles").GetComponent<TileSpawner>();
	}
	
	// Update is called once per frame
	void Update () {
		if ( currentTarget == null ) 
		{
			currentTarget = ts.GetRandomTile().transform;
			agent.SetDestination(currentTarget.position);
			travelTime = 0f;
		} else {
			travelTime += Time.deltaTime;
			if ( AtEndOfPath() || travelTime > travelTimeLimit )
			{
				if ( !damagingTile )
				{
					//Debug.Log("Damaging a tile");
					damagingTile = true;
					Invoke("ClearTarget", 3.0f);
				} else {
					ts.DamageTile(currentTarget.gameObject, damageRate * Time.deltaTime);
				}
			}
		}
	}

	void ClearTarget()
	{
		currentTarget = null;
		damagingTile = false;
		hasPath = false;
	}

    bool AtEndOfPath()
    {
        hasPath |= agent.hasPath;
        if (hasPath && agent.remainingDistance <= agent.stoppingDistance + pathEndThreshold )
        {
            // Arrived
            hasPath = false;
            return true;
        }
 
        return false;
    }
}
