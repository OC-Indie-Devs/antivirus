using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {
	public float failThreshold = 0.5f;
	public Slider healthSlider;
	public LevelManager levelManager;

	List<GameObject> healthyTiles;
	List<GameObject> damagedTiles;

	// Use this for initialization
	void Start () {
		levelManager.LevelStart();
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

	public GameObject GetAnyRandomTile()
	{
		int numTiles = healthyTiles.Count + damagedTiles.Count;
		int randTile = Random.Range(0, numTiles);
		if ( randTile >= healthyTiles.Count )
		{
			return damagedTiles[randTile - healthyTiles.Count];
		} else {
			return healthyTiles[randTile];
		}
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
			levelManager.LevelFailed();
		}
		//Debug.Log("sliderValue=" + sliderValue);
	}
}
