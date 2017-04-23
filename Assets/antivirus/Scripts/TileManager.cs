using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {
	public float failThreshold = 0.5f;
	public Slider healthSlider;
	public GameObject levelFailedPanel;

	public GameObject failParticles;

	List<GameObject> healthyTiles;
	List<GameObject> damagedTiles;

	// Use this for initialization
	void Start () {
		levelFailedPanel.SetActive( false );
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
			levelFailedPanel.SetActive( true );
			PlayerController pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			pc.disabled = true;
			GameObject[] ps = new GameObject[10];
			for (int i = 0; i < 10; i++)
			{
				ps[i] = Instantiate(failParticles, GetAnyRandomTile().transform.position, Quaternion.identity);
			}
		}
		//Debug.Log("sliderValue=" + sliderValue);
	}
}
