using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	public float pathEndThreshold = 0.1f;
	public float damageRate = 1f;

    private bool hasPath = false;

	Transform currentTarget;
	TileManager tileMgr;
	NavMeshAgent agent;

	bool damagingTile = false;
	float travelTime;
	float travelTimeLimit = 20f;

	Animator anim;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		tileMgr = GameObject.FindWithTag("Tiles").GetComponent<TileManager>();
		anim = GetComponentInChildren<Animator>();
		anim.SetBool("Crawl", true);
	}
	
	// Update is called once per frame
	void Update () {
		if ( currentTarget == null ) 
		{
			currentTarget = tileMgr.GetRandomTile().transform;
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
					anim.SetBool("AttackTile", true);
					Invoke("ClearTarget", 3.0f);
					tileMgr.DamageTile(currentTarget.gameObject);
				}
			}
		}
	}

	void ClearTarget()
	{
		currentTarget = null;
		damagingTile = false;
		hasPath = false;
		anim.SetBool("AttackTile", false);
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
