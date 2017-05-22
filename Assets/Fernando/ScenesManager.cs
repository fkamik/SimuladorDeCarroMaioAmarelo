using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
	public enum Scenes
	{
		CHOOSE_PATH		=		0,
		MAIN			=		1
	}

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
	
	
}