using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class AppstoreHandler : MonoBehaviour
{
	#if UNITY_IPHONE
	[DllImport ("__Internal")] private static extern void _OpenAppInStore(int appID);
	#endif
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif

	private static AppstoreHandler _instance;

	public static AppstoreHandler Instance
	{
		get { return _instance; }
		private set { _instance = value; }
	}

	void Awake()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else {
			Destroy(gameObject);
			return;
		}

		if(!Application.isEditor)
		{
			#if UNITY_ANDROID
			jo = new AndroidJavaObject("com.purplelilgirl.nativeappstore.NativeAppstore");
			#endif
		}	else
		{	Debug.Log("AppstoreHandler:: Cannot open Appstore in Editor.");
		}
	}

	public void openAppInStore(string appID)
	{
		if(!Application.isEditor)
		{
			#if UNITY_IPHONE
			int appIDIOS;

			if(int.TryParse(appID, out appIDIOS))
			{	_OpenAppInStore(appIDIOS);
			}
			#endif
			
			#if UNITY_ANDROID
			jo.Call("OpenInAppStore", "market://details?id="+appID);
			#endif
		}	else
		{	Debug.Log("AppstoreHandler:: Cannot open Appstore in Editor.");
		}
	}

	public void appstoreClosed()
	{	Debug.Log("AppstoreHandler:: Appstore closed.");
	}
}	
