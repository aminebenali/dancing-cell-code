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
        for (int i = 0; i < mgr.cShareData.lstBuildingData.Count; i++)
        {
            LabBuildingName[i].text = mgr.cShareData.lstBuildingData[i].strBuildingName;

            float fLabWidth = LabBuildingName[i].relativeSize.x * 24f + 48f + 6f;
            SprBuildingBg[i].transform.localScale = new Vector3(fLabWidth, 40f, 1f);
            SprBuildingIcon[i].transform.localPosition = new Vector3(-fLabWidth / 2f + 26f, 0f, -220f);
        }
	}
	
    private void GotoMyHome()
    {
		Debug.Log("GotoMyHome");
		
		showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Outside");
		
		PlayBtnSound();
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
    }
	
	void AddChallengeListBegin()
    {
        PlayBtnSound();
// 		GameObject obj = GameObject.Find("ChallengeList");
// 		if(obj)
// 		{
//             showDialog(false);
// 			NvUIDialogManager m_DialogMgr = obj.GetComponent(typeof(NvUIDialogManager)) as NvUIDialogManager;
// 			DcChallengeListDlg dlgCL = m_DialogMgr.FindDialog("ChallengeListDlg")as DcChallengeListDlg;
// 			dlgCL.showDialog(true);
// 			dlgCL.SetChallengeChallenge();
//             dlgCL.InitChbSel();
// 			//Debug.Log("dlgCL.nCurSelScene:" + dlgCL.nCurSelScene);
// 		}
//        	else
		{
			SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        	mgr.ChangeScene("UI_ChallengeList"/*,AddChallengeListEnd*/);
		}
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
		//Debug.Log("GotoShop");
		showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Shop");
		mgr.cShareData.nShopEntranceType = 0;
		
		PlayBtnSound();
    }
	
	private void GotoTask()
    {
		//Debug.Log("GotoTask");
		showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Task");
		
		PlayBtnSound();
    }

    private void GotoLobby()
    {
        //Debug.Log("GotoLobby");
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Lobby");

        PlayBtnSound();
    }
	
	private void PlayBtnSound()
	{
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
}
