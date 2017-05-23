using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
	public enum Scenes
	{
		CHOOSE_PATH		=		0,
		MAIN			=		1
	}

	private Scenes mCurrentScene;

	private int mCarIndex = 0;

	private static ScenesManager sInstance = null;
	public static ScenesManager Instance
	{
		get{return sInstance;}
		private set{}
	}
	
	void Awake()
	{
		if(sInstance == null)
		{
			sInstance = GetComponent<ScenesManager>();
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void ChangeScene(Scenes pNewScene)
	{
		switch (pNewScene) 
		{
		case Scenes.CHOOSE_PATH:
		default:
			SceneManager.LoadScene ("LoadPath");
			break;
		case Scenes.MAIN:
			SceneManager.LoadScene ("MaioAmarelo (v2.0)_new");


			break;

		}

		mCurrentScene = pNewScene;
	}


	public void GoToLoadPath()
	{
		ChangeScene (Scenes.CHOOSE_PATH);
	}

	public void GoToGame(int pCarIndex)
	{
		mCarIndex = pCarIndex;
		ChangeScene (Scenes.MAIN);
	}

	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("Level Loaded");
		Debug.Log(scene.name);
		Debug.Log(mode);

		switch (mCurrentScene) 
		{
		case Scenes.CHOOSE_PATH:
		default:
			
			break;
		case Scenes.MAIN:
			GameManager.Instance.Initialize (mCarIndex);
			break;

		}
	}
}