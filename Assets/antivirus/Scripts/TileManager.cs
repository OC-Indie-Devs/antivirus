using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {
	public float failThreshold = 0.5f;
	public Slider healthSlider;
	public Text failedText;

	List<GameObject> healthyTiles;
	List<GameObject> damagedTiles;

	// Use this for initialization
	void Start () {
		failedText.enabled = false;
		healthyTiles = new List<GameObject>();
		damagedTiles = new List<GameObject>();
		
		Transform[] ht = GetComponentsInChildren<Transform>();
		for (int i = 0; i < ht.Length; i++)
		{
			healthyTiles.Add(ht[i].gameObject);
		}
	}


	public int GetNumTiles()
	{
		return healthyTiles.Count + damagedTiles.Count;
	}

	// returns a random healthy tile if any exist, otherwise returns random damaged tile
	public GameObject GetRandomTile()
	{
		int numTiles = healthyTiles.Count;
		if ( numTiles > 0 )
		{
			return healthyTiles[Random.Range(0, numTiles)];
		}
		return damagedTiles[Random.Range(0, damagedTiles.Count)];
	}

	public void DamageTile(GameObject tile)
	{
		int tileIndex = healthyTiles.IndexOf(tile);
		if ( tileIndex >= 0 )
		{
			damagedTiles.Add( healthyTiles[tileIndex]);
			healthyTiles.Remove( tile );
		}
		UpdateLevelHealth();
	}

	void UpdateLevelHealth()
	{
		int numTiles = healthyTiles.Count + damagedTiles.Count;
		float sliderValue = (float)healthyTiles.Count / (float)numTiles;
		healthSlider.value = sliderValue;
		if ( sliderValue < failThreshold )
		{
			failedText.enabled = true;
			PlayerController pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			pc.disabled = true;
		}
		//Debug.Log("sliderValue=" + sliderValue);
	}
}
