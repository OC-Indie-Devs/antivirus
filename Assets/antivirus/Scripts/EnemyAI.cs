using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	public float pathEndThreshold = 0.1f;

    private bool hasPath = false;

	Transform currentTarget;
	TileSpawner ts;
	NavMeshAgent agent;

	bool damagingTile = false;


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
		} else {
			if (AtEndOfPath() && !damagingTile)
			{
				damagingTile = true;
				//Debug.Log("Damaging a tile");
				Invoke("ClearTarget", 3.0f);
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
