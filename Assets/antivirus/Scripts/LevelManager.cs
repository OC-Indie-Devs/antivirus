using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject levelPassedPanel;
	public GameObject levelFailedPanel;
	public GameObject failParticles;
	public GameObject passParticles;

	public TileManager tileManager;
	public EnemySpawner enemySpawner;

	public void LevelFailed()
	{
		levelFailedPanel.SetActive( true );
		PlayerController pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		pc.disabled = true;
		for (int i = 0; i < 10; i++)
		{
			StartCoroutine(playFailedEffect(failParticles));
		}		
	}

	IEnumerator playFailedEffect(GameObject prefab)
	{
		yield return new WaitForSeconds(Random.Range(0, 2));
		GameObject failParticles = Instantiate(prefab, tileManager.GetAnyRandomTile().transform.position, Quaternion.identity);
		yield return new WaitForSeconds(Random.Range(0, 2));
		Destroy(failParticles);
	}

	public void LevelPassed()
	{
		levelPassedPanel.SetActive(true);
		PlayerController pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		pc.disabled = true;
		passParticles.SetActive( true );
	}

	public void LevelStart()
	{
		levelPassedPanel.SetActive( false );
		levelFailedPanel.SetActive( false );
	}
}
