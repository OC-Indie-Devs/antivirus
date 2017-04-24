using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour {

	void Awake()
	{
		DynamicGI.UpdateEnvironment();
	}

	// Use this for initialization
	public void NextScene()
	{
		int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
		Debug.Log("nextIndex=" + nextIndex);
		if ( nextIndex >= SceneManager.sceneCountInBuildSettings )
		{
			// just reload same scene if at last scene
			nextIndex = 0;
		}
		Debug.Log("nextIndex=" + nextIndex);
		SceneManager.LoadScene( nextIndex );
	}
	
	public void LastScene()
	{
		int lastIndex = SceneManager.GetActiveScene().buildIndex - 1;
		if ( lastIndex < 0 )
		{
			// just reload if first scene
			lastIndex++;
		}
		SceneManager.LoadScene( lastIndex );
	}


	public void ReloadScene()
	{
		int sceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene( sceneIndex );
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
