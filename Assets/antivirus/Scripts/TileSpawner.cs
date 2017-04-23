using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSpawner : MonoBehaviour {
	public int rows = 20;
	public int cols = 15;
	public float failThreshold = 0.5f;
	public GameObject tilePrefab;
	public Slider healthSlider;
	public Text failedText;

	List<GameObject> tiles;
	List<float> tileHealth;

	int damagedTiles;

	// Use this for initialization
	void Start () {
		failedText.enabled = false;
		tiles = new List<GameObject>();
		tileHealth = new List<float>();

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Vector3 tilePosition = new Vector3( j + 0.5f, -0.51f, -i - 0.5f );
				GameObject thisTile = Instantiate(tilePrefab, transform.position + tilePosition, Quaternion.identity, transform);
				tiles.Add(thisTile);
				tileHealth.Add(1.0f);
			}
		}
	}


	public int GetNumTiles()
	{
		return rows * cols;
	}

	public GameObject GetRandomTile()
	{
		bool healthyTile = false;
		int tileIndex = 0;
		int tries = 0;
		while (healthyTileExists() && !healthyTile && tries < 15)
		{
			tileIndex = Random.Range(0, GetNumTiles());
			if ( tileHealth[tileIndex] > 0f )
				healthyTile = true;
			tries++;
		}
		return tiles[tileIndex];
	}

	bool healthyTileExists()
	{
		foreach ( float health in tileHealth )
		{
			if ( health > 0 )
				return true;
		}
		return false;
	}

	public void DamageTile(GameObject tile, float damageAmount)
	{
		int tileIndex = tiles.IndexOf(tile);
		if ( tileHealth[tileIndex] > 0f)
		{
			tileHealth[tileIndex] -= damageAmount;
			if ( tileHealth[tileIndex] < 0f )
			{
				tileHealth[tileIndex] = 0f;
			}
			if ( tileHealth[tileIndex] == 0f )
			{
				damagedTiles++;
			}
			Text tileHealthText = tile.GetComponentInChildren<Text>();
			tileHealthText.text = tileHealth[tileIndex].ToString();
			UpdateLevelHealth();
		}
	}

	void UpdateLevelHealth()
	{
		float sliderValue = (float)( GetNumTiles() - damagedTiles ) / (float)GetNumTiles();
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
