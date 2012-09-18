using UnityEngine;
using System.Collections;
//Json
using LitJson;
using System.Collections.Generic;

//test
public class KKKKK
{
    public string strS1;
    public int nN;
    public string strS2;
}

public class DcUILogo : MonoBehaviour 
{
	public NvLocalizationManager m_Localization;
	
	private List<KKKKK> m_ScenariogListLocal = new List<KKKKK>();

    void startGame() 
	{
		//Debug.Log("startGame - startGame");
		_startGame();
	}

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
			//Debug.Log("Jump - startGame");
			_startGame();
        }
    }
	
	private void _startGame()
	{
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_BeginGame");
	}
	
	void test()
	{
		m_Localization = Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager;
        if (m_Localization == null)
        {
            Debug.LogError("Can Not Find Localization File!!");
        }
        Debug.LogError("ssss" + m_Localization.GetValue("seaside_song_2_name"));
		
		//
		
		UnityEngine.TextAsset s = (UnityEngine.TextAsset)Resources.Load("TXT/KKKKK/TestJson", typeof(UnityEngine.TextAsset));
        m_ScenariogListLocal = JsonMapper.ToObject<List<KKKKK>>(s.text);

        Debug.Log("************"+m_ScenariogListLocal.Count);
        for (int i = 0; i < m_ScenariogListLocal.Count; i++)
        {
            Debug.Log(i + "=>" + m_ScenariogListLocal[i].strS1 + "++" + m_ScenariogListLocal[i].nN + "++" + m_ScenariogListLocal[i].strS2);
        }
	}
}
