using UnityEngine;
using System;
using LitJson;

public class DcMapDlg : NvUIDialogBase 
{
    public UILabel[] LabBuildingName;
    public UISprite[] SprBuildingBg;
    public UISprite[] SprBuildingIcon;
	
	protected override void OnDlgCreate()
	{
		
	}
	
	protected override void OnDlgInit ()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        if (mgr != null && mgr.cShareData != null)
        {
            for (int i = 0; i < mgr.cShareData.lstBuildingData.Count; i++)
            {
                LabBuildingName[i].text = mgr.cShareData.lstBuildingData[i].strBuildingName;

                float fLabWidth = LabBuildingName[i].relativeSize.x * 24f + 48f + 6f;
                SprBuildingBg[i].transform.localScale = new Vector3(fLabWidth, 40f, 1f);
                SprBuildingIcon[i].transform.localPosition = new Vector3(-fLabWidth / 2f + 26f, 0f, -220f);
            }
        }
	}
	
    private void GotoMyHome()
    {
		Debug.Log("GotoMyHome");
		
		showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Outside");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }

    private void GotoChallengBuilding(object sender)
    {
        GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string strLastNum = DcGlobalFunc.GetLastString(obj.transform.name, splt);
        int nSelBuilding = Convert.ToInt32(strLastNum) - 1;

		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.cShareData.nCurSelBuildingIdx = nSelBuilding;
        AddChallengeListBegin();

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }
	
	void AddChallengeListBegin()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_ChallengeList"/*,AddChallengeListEnd*/);
    }
	
	void AddChallengeListEnd(string str)
    {
        GameObject obj = GameObject.Find("ChallengeList");
		if(obj)
		{
			//Debug.Log("AddChallengeListEnd Succeed");
        	NvUIDialogManager m_DialogMgr = obj.GetComponent(typeof(NvUIDialogManager)) as NvUIDialogManager;
			DcChallengeListDlg dlgCL = m_DialogMgr.FindDialog("ChallengeListDlg")as DcChallengeListDlg;
			dlgCL.SetChallengeChallenge();
            dlgCL.InitChbSel();
			//Debug.Log("dlgCL.nCurSelScene:" + dlgCL.nCurSelScene);
		}
		else
		{
			Debug.Log("AddChallengeListEnd Fail");
		}
    }
	
	private void GotoShop()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Shop");
		mgr.cShareData.nShopEntranceType = 0;

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }
	
	private void GotoTask()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Task");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }

    private void GotoLobby()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Lobby");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }
}
