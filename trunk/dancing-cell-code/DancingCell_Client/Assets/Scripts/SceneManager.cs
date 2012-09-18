// 
//  SceneManager.cs
//  
//  Author:
//    saviosun 
//
//  Purpose:
//  This class can handle process bar or loading menu while level loading.
// 
//  Copyright (c) 6, 3, 2012
// 
//  All rights reserved.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneAdditiveContext
{
	public delegate void onSceneLoaded (string sceneName);

	onSceneLoaded m_onSceneLoaded = null;
	string m_sSceneName = null;
	
	public SceneAdditiveContext (string name, onSceneLoaded loaded)
	{
		m_sSceneName = name;
		m_onSceneLoaded = loaded;
	}
	
	public string GetSceneName ()
	{
		return m_sSceneName;
	}
	
	public void runSceneLoaded ()
	{
		if (m_onSceneLoaded != null) {
			m_onSceneLoaded (m_sSceneName);
		}
	}
}

public interface SceneSwitchListener
{
	void OnPreSceneSwitch ( string levelName );
	void OnSceneSwitch ( string levelName );
}

public class SceneManager : MonoBehaviour
{
	public enum E_STATUS
	{
		E_INIT = 0,
		E_START_LOAD,
		E_START_LOAD_FADE,
		E_START_LOADADD,
		E_LOADING,
		E_LOADINGADD,
		E_END_LOAD,
		E_END_LOADADD,
		E_IDOL,
	};
	
	public string msNextLevel;
	public 	string msPreSceneName = null;
	public string msCurrentLoadingLevel;
	public List<SceneAdditiveContext> mAddLevelList = new List<SceneAdditiveContext> ();
	public List<string> mAddLevelNameList = new List<string> ();
	public E_STATUS meStatus = E_STATUS.E_INIT;
	AsyncOperation mAsyncOP = null;
	private GameObject m_GuiTexture;
	private List<SceneSwitchListener> m_callbackListeners = new List<SceneSwitchListener> ();
	
	private float m_StartFadeTime = 0;
	private float m_MaxFadeTime = 5.0f ;
	
	private NvCommonUIManager m_UIMgr;
	
	public DcShareData cShareData;
	
	void Awake ()
	{
		meStatus = E_STATUS.E_IDOL;
	}
	
	// Use this for initialization
	void Start ()
	{
		m_GuiTexture = iTween.CameraFadeAdd ();
		m_GuiTexture.layer = LayerMask.NameToLayer ("UILayer");
		DontDestroyOnLoad (m_GuiTexture);
		m_GuiTexture.active = false;
		
		m_UIMgr = Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager;
		//Debug.Log("Start SceneManager.");	
        cShareData = new DcShareData();
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (meStatus) {
			case E_STATUS.E_IDOL:
				if (mAddLevelList.Count > 0) {
					meStatus = E_STATUS.E_START_LOADADD;
				}
				break;
			
			case E_STATUS.E_START_LOAD:
			//meStatus = E_STATUS.E_LOADING;
			{
				(Singlton.getInstance ("NvSoundController") as NvSoundController).StopBgm ();
				FadeTo(gameObject, "OnFadeEnd",  0.9f, 0.5f);
				m_UIMgr.ShowLoadingIndicatorWithProgress(true);
				m_UIMgr.SetLoadingProgressAlpha(1);
				m_UIMgr.SetLoadingProgress(0);
				iTween.ValueTo(gameObject, iTween.Hash("name", "LoadingPgBar","from", 0, "to", 1, "time", 5, "easetype", iTween.EaseType.linear, 
				"onupdate", "SetPg"/*, "oncompletetarget", gameObject, "oncomplete", "LoadingProgressEnd"*/));
				//m_UIMgr.SetLoadingProgress( mAsyncOP.progress );
				meStatus = E_STATUS.E_START_LOAD_FADE;
				m_StartFadeTime = 0;
			}
				break;

			case E_STATUS.E_START_LOAD_FADE:
				m_StartFadeTime += Time.deltaTime;
				if (m_StartFadeTime > m_MaxFadeTime ){
					Debug.LogWarning("YYYYYYYYYYYYYY Start Load" + msNextLevel );
					mAsyncOP = Application.LoadLevelAsync (msNextLevel);
					meStatus = E_STATUS.E_LOADING;
				}
				//GUITexture tx = m_GuiTexture.GetComponent (typeof(GUITexture)) as GUITexture;
				//Debug.Log (" Fade is " + tx.color.a);
			//GUITexture tx = m_GuiTexture.GetComponent (typeof(GUITexture)) as GUITexture;
			//if (tx.color.a > 0.9f) {
			//	mAsyncOP = Application.LoadLevelAsync (msNextLevel);
			//	meStatus = E_STATUS.E_LOADING;
			//}
				
				break;
			
			case E_STATUS.E_START_LOADADD:
				msCurrentLoadingLevel = mAddLevelList [0].GetSceneName ();
				mAsyncOP = Application.LoadLevelAdditiveAsync (msCurrentLoadingLevel);
				//Debug.Log ("Start Load Additive Level " + msCurrentLoadingLevel);
				meStatus = E_STATUS.E_LOADINGADD;
				break;
			
			case E_STATUS.E_LOADING:
				if (mAsyncOP != null && mAsyncOP.isDone) {					
					meStatus = E_STATUS.E_END_LOAD;
					OnSceneSwitch ();
				}
				break;
			
			case E_STATUS.E_LOADINGADD:	
				if (mAsyncOP != null && mAsyncOP.isDone) {
					FinishAddScene (msCurrentLoadingLevel);
					msCurrentLoadingLevel = null;
					meStatus = E_STATUS.E_END_LOADADD;
				}
				break;
			
			case E_STATUS.E_END_LOADADD:
				{
					mAsyncOP = null;
					meStatus = E_STATUS.E_IDOL;
				}
				break;
			
			case E_STATUS.E_END_LOAD:
				{
					mAsyncOP = null;
					m_UIMgr.SetLoadingProgress( 1 );
					m_UIMgr.EnableCLRCam(false);
					iTween.StopByName(gameObject, "LoadingPgBar");
					//m_UIMgr.ShowLoadingIndicatorWithProgress(false);

					iTween.ValueTo(gameObject, iTween.Hash("name", "FadePgBar", "from", 1, "to", 0, "time", 1, "easetype", iTween.EaseType.linear, 
				"onupdate", "FadePgBar", "oncompletetarget", gameObject, "oncomplete", "PgBarFadeEnd"));
				
					FadeTo(gameObject, "OnFadeInEnd", 0.01f, 1 );
					meStatus = E_STATUS.E_IDOL;
				}
				break;
				
			default:
				break;
		}
	}
	
	public void FadeTo( GameObject o, string funcName, float amout, float time )
	{
		m_GuiTexture.active = true;
		iTween.StopByName(gameObject, "ScreenFade");
		Hashtable param = new Hashtable ();
		param.Add ("name", "ScreenFade");
		param.Add ("amount", amout);
		param.Add ("time", time);
		param.Add ("oncomplete", funcName);
		param.Add ("oncompletetarget", o);
		iTween.CameraFadeTo ( param );
	}
	
	public void ChangeScene (string sNextScene)
	{
		if (meStatus != E_STATUS.E_IDOL) {
			Debug.LogWarning ("You can't load two scene sync");
			return;
		}
		mAddLevelList.Clear ();
		mAddLevelNameList.Clear ();
		msPreSceneName = msNextLevel;
		msNextLevel = sNextScene;
		meStatus = E_STATUS.E_START_LOAD;
		
		OnPreSceneSwitch();
		
		//mAsyncOP = Application.LoadLevelAsync(sNextScene);
		//Debug.Log ("Start Load Level " + sNextScene);
	}
	
	public void AddScene (string sAddScene, SceneAdditiveContext.onSceneLoaded loaded = null)
	{
		//Debug.Log("@@@@@@@@@@@@@++++++AddScene: "+sAddScene);
		mAddLevelNameList.Add (sAddScene);
		SceneAdditiveContext ctx = new SceneAdditiveContext (sAddScene, loaded);
		mAddLevelList.Add (ctx);
		if (meStatus == E_STATUS.E_IDOL) {
			meStatus = E_STATUS.E_START_LOADADD;
		}
	}
	
	void FinishAddScene (string sAddScene)
	{
		//Debug.Log ("#######Finsih Load: " + sAddScene);
		foreach (SceneAdditiveContext ctx in mAddLevelList) {
			if (ctx.GetSceneName () == sAddScene) {
				ctx.runSceneLoaded ();
				mAddLevelList.Remove (ctx);
				break;
			}
		}
	}
	
	public bool ExsitScene (string sName)
	{
		foreach (string name in mAddLevelNameList) {
			if (name == sName) {
				return true;
			}
		}
		return false;
	}
	
	public void OnFadeEnd ()
	{
		//Debug.LogWarning("XXXXXXXXXXXXXXXX Start Load" + msNextLevel );
		mAsyncOP = Application.LoadLevelAsync (msNextLevel);
		meStatus = E_STATUS.E_LOADING;
	}
	
	void OnFadeInEnd ()
	{
		m_GuiTexture.active = false;
		//m_UIMgr.ShowLoadingIndicatorWithProgress(false);
	}
	
//	void OnGUI ()
//	{
//		if (meStatus == E_STATUS.E_LOADING && mAsyncOP != null) {
//			GUI.Label (new Rect (Screen.width * 0.5f - 100.0f, Screen.height * 0.5f - 25.0f, 200.0f, 50.0f), "Now Loading " + (mAsyncOP.progress * 100.0f));
//		}
//	}
	
	public void registerCallbackListener (SceneSwitchListener listener)
	{
		if (listener != null && m_callbackListeners.Contains (listener) == false) {
			m_callbackListeners.Add (listener);
		}
	}
	
	public void unregisterCallbackListener (SceneSwitchListener listener)
	{
		if (m_callbackListeners.Contains (listener)) {
			m_callbackListeners.Remove (listener);
		}
	}
	
	private void OnSceneSwitch ()
	{
		foreach (SceneSwitchListener listener in m_callbackListeners) {
			listener.OnSceneSwitch (msNextLevel);
		}
	}
	
	private void OnPreSceneSwitch()
	{
		foreach (SceneSwitchListener listener in m_callbackListeners) {
			listener.OnPreSceneSwitch (msNextLevel);
		}
	}
	
	void LoadingProgressEnd()
	{
		m_UIMgr.ShowLoadingIndicatorWithProgress(false);
	}
	
	void SetPg(float v)
	{		
		m_UIMgr.SetLoadingProgress( v );
	}
	
	void FadePgBar(float v)
	{
		m_UIMgr.SetLoadingProgressAlpha(v);
	}
	
	void PgBarFadeEnd()
	{
		m_UIMgr.ShowLoadingIndicatorWithProgress(false);		
	}
}
