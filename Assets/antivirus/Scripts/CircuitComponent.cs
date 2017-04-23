using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitComponent : MonoBehaviour {

	public bool isInSocket = false;
	public GameObject firewall;
	public EnemySpawner enemySpawner;
	public Color highlightColor;
	
	bool highlight = false;

	Color originalColor;
	Renderer rend;

	void Awake()
	{
		rend = GetComponent<Renderer>();
		originalColor = rend.material.color;
	}

	public void enableCircuit()
	{
		enemySpawner.RaiseFirewall(firewall);
	}

	public void highlightOn()
	{
		highlight = true;
	}

	public void highlightOff()
	{
		highlight = false;
		rend.material.color = originalColor;
	}

	void Update() {
		if ( highlight )
		{
	        rend.material.color = Color.Lerp(originalColor, highlightColor, Mathf.PingPong(Time.time, 1));
		}
    }
}
