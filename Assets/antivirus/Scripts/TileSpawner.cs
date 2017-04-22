using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {
	public int rows = 20;
	public int cols = 15;

	public GameObject tilePrefab;

	List<GameObject> tiles;

	// Use this for initialization
	void Start () {
		tiles = new List<GameObject>();

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Vector3 tilePosition = new Vector3( j + 0.5f, -0.51f, -i - 0.5f );
				GameObject thisTile = Instantiate(tilePrefab, transform.position + tilePosition, Quaternion.identity, transform);
				tiles.Add(thisTile);
			}
		}
	}


	public int GetNumTiles()
	{
		return rows * cols;
	}

	public GameObject GetRandomTile()
	{
		return tiles[Random.Range(0, GetNumTiles()-1)];
	}
}
