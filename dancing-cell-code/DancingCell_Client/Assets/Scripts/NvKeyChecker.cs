
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
//using NetworkUtils;

public class NvKeyChecker : MonoBehaviour
{
	private NvCommonUIManager mCommonUIManager = null;
	[SerializeField]
	private bool mMessageboxDisplayed = false;
	void Awake()
	{
		mCommonUIManager = Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager;
	}

	// Update is called once per frame
	void Update ()
	{
		if ( DcGame.gEnableKeyChecker == false )
		{
			return ;
		}
		
		if (Application.platform != RuntimePlatform.IPhonePlayer) 
		{
		    if (Input.GetKey(KeyCode.Escape)/* && !mMessageboxDisplayed*/) 
			{
				Debug.Log ("KeyChecker : " + Input.GetKey (KeyCode.Escape));
				string desc =  (Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager).GetValue("ExitHintContext");
				mCommonUIManager.showMessageBox(desc, "", NvMessageBox.EMessageBoxStyle.eStyle_YesNo, OnEndModal, true);
				mMessageboxDisplayed = true;
			}
	    }
		
		if ( Application.isEditor )
		{
			if ( Input.GetKeyUp(KeyCode.F11) )
			{
				string strScreenshotPath = "Screenshot/"+System.DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss")+".png";
				Debug.LogWarning("CaptureScreenshot: "+strScreenshotPath);
				Application.CaptureScreenshot(strScreenshotPath);
				
				NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
				soundctr.PlaySe("shutter");
			}
		}
		
		if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            if ( Input.GetKeyUp(KeyCode.F9) )
			{
				SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
				if(Application.loadedLevelName == "UI_Map")
				{
        			mgr.ChangeScene("UI_ProductionTeam");
				}
				else if(Application.loadedLevelName == "UI_ProductionTeam")
				{
        			mgr.ChangeScene("UI_Map");
				}

				NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        		soundctr.PlaySe("ui_touch");
			}
        }
	}
	
	private void OnEndModal( int result )
	{
		if ( result == (int)NvMessageBox.EResult.eResult_Yes )
		{
            Debug.LogError("Quit");
			Application.Quit();
		}
		
		mMessageboxDisplayed = false;
	}
#if UNITY_ANDROID || UNITY_IPHONE	
	void OnApplicationFocus(bool bFocus)
	{
		if(bFocus)
		{
			Debug.Log("OnApplicationFocus True.");
		}
		else
		{
			Debug.Log("OnApplicationFocus False.");
		}
	}
	
	void OnApplicationPause(bool bPause)
	{
		if(bPause)
		{
			Debug.Log("OnApplicationPause True.");
		}
		else
		{
			if(Application.isEditor)
			{
				return;
			}
			Debug.Log("OnApplicationPause False.");
			//check session
			//NvNetworkHandler.getInstance().postCommand( (uint)ENetworkCmd.eNC_MSG_CHK_SESSION, null, OnSessionOk, OnSessionError );
		}		
	}
#endif	
// 	void OnSessionOk(uint msgId, object obj, object userObj)
// 	{
// 		//MsgBase tbase = (MsgBase)(obj);
// 		//if(tbase.isSuccess)
// 		//{
// 		//	MsgChkSession msg = obj as MsgChkSession;
// 			Debug.Log( "=======> MsgChkSession!!!!!!!!! "); //+ msg.session_expire
// 		//	if(msg.session_expire == 1)
// 		//	{
// 		//		string desc =  (Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager).GetValue("LostSession");
// 		//		mCommonUIManager.showMessageBox(desc, "", NvMessageBox.EMessageBoxStyle.eStyle_OkayOnly, OnToTitle);
// 		//	}
// 		//}
// 		//else
// 		//{
// 		//	SceneManager sc = Singlton.getInstance("SceneManager") as SceneManager;
// 		//	sc.ChangeScene("TitleScene");	
// 		//}
// 		
// 	}
	
// 	void OnSessionError(uint msgId, object obj, object userObj)
// 	{
// 		//MsgSessionMobage msg = obj as MsgSessionMobage;
// 		Debug.LogError("MsgChkSession =======> Error!!!!!!!!!");	
// 	}
	
// 	void OnToTitle( int result )
// 	{
// 		if ( result == (int)NvMessageBox.EResult.eResult_Okay )
// 		{
// 			SceneManager sc = Singlton.getInstance("SceneManager") as SceneManager;
// 			sc.ChangeScene("TitleScene");
// 		}
// 	}
//	private void OnDlgClose( object sender )
//	{
//		NvMessageBox dlg = sender as NvMessageBox;
//		if ( dlg != null && dlg.result == NvMessageBox.EResult.eResult_Yes )
//		{
//			Application.Quit();
//		}
//		else if ( dlg != null && dlg.result == NvMessageBox.EResult.eResult_No )
//		{
//			
//		}
//	}
	
}
