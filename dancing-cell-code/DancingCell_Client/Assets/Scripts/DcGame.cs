using UnityEngine;
using System.Collections;

public class DcGame : MonoBehaviour, SceneSwitchListener 
{

    static private DcGame sInstance = null;

    static public DcGame getInstance()
	{
		return sInstance;
	}
	
	void Awake()
	{
		if ( sInstance == null )
		{
			sInstance = this;
			DontDestroyOnLoad( gameObject );
			
			if ( NvConst.kIsDebugBuild )
				Singlton.getInstance("ShowFPS");
			
			Singlton.getInstance("NvCommonUIManager");
			Singlton.getInstance("NvGameUIManager");
			Singlton.getInstance("NvLocalizationManager");
			Singlton.getInstance("NvSoundController");
            Singlton.getInstance("DCGesture");
			//Singlton.getInstance("NvSceneChangeColor");
			//Singlton.getInstance("NvAppearanceController");
			//Singlton.getInstance("NvMotionController");
		}
		else
		{
			GameObject.DestroyImmediate( gameObject );
		}
	}
	
	void Start()
	{	
		GameObject  o = GameObject.Instantiate(Resources.Load("TXT/LocalizationFile/Localization")) as GameObject;

		Localization loc = o.GetComponent(typeof(Localization)) as Localization;

		//loc.currentLanguage = "cn";

		o.transform.parent = gameObject.transform;		
		
		(Singlton.getInstance("SceneManager") as SceneManager).registerCallbackListener( this );
	}
    public void OnSceneSwitch(string levelName)
    {
    }
	
	public void OnPreSceneSwitch ( string levelName )
	{
		(Singlton.getInstance("NvGameUIManager") as NvGameUIManager).Reset();
	}
}
