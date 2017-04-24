using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	public float pathEndThreshold = 0.1f;
	public float damageRate = 1f;

	public GameObject[] particleSystems;
	public GameObject[] lights;

    private bool hasPath = false;

	Transform currentTarget;
	TileManager tileMgr;
	NavMeshAgent agent;

	bool damagingTile = false;
	bool idle = true;

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
					damagingTile = true;
					if ( currentTarget.gameObject.CompareTag("Component"))
					{
						//Debug.Log("Damaging a component");
						anim.SetBool("AttackComponent", true);
					} else {
						//Debug.Log("Damaging a tile");
						anim.SetBool("AttackTile", true);
					}
					StartCoroutine(damageEffects());
					Invoke("ClearTarget", 3.0f);
					tileMgr.DamageTile(currentTarget.gameObject);
				}
			}
		}

		if ( !idle )
		{
			if ( agent.velocity.magnitude < 0.1 )
			{
				anim.SetBool("Crawl", false);
				idle = true;
			}
		} else {
			if ( agent.velocity.magnitude >= 0.1 )
			{
				anim.SetBool("Crawl", true);
				idle = false;
			}
		}
	}

	IEnumerator damageEffects()
	{
		ParticleSystem[] ps = new ParticleSystem[particleSystems.Length];
		for (int i = 0; i < particleSystems.Length; i++)
		{
			ps[i] = particleSystems[i].GetComponent<ParticleSystem>();
		}
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < particleSystems.Length; j++)
			{
				ps[j].Play();
				blinkLights();
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	void blinkLights()
	{
		lightsOn();
		Invoke("lightsOff", 0.1f);
	}

	void lightsOn()
	{
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].SetActive(true);
		}
	}

	void lightsOff()
	{
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].SetActive(false);
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
