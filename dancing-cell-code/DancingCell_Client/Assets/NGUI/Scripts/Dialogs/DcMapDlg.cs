using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class DcMapDlg : NvUIDialogBase 
{	
	public UILabel LabTemp1 = null;
	public UILabel LabTemp2 = null;
	public UILabel LabTemp3 = null;
	
	protected override void OnDlgCreate()
	{
		
	}
	
	protected override void OnDlgInit ()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        LabTemp1.text = mgr.cShareData.lstBuildingData[0].strBuildingName;
        LabTemp2.text = mgr.cShareData.lstBuildingData[1].strBuildingName;
        LabTemp3.text = mgr.cShareData.lstBuildingData[2].strBuildingName;
	}
	
    private void GotoMyHome()
    {
		Debug.Log("GotoMyHome");
		
		showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Outside");
		
		PlayBtnSound();
    }
	
	private void GotoChallenge_SeaSide()
    {
		//Debug.Log("GotoChallenge_SeaSide");
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		mgr.cShareData.nCurSelBuildingIdx = 0;
        AddChallengeListBegin();
    }
	
	private void GotoChallenge_Space()
    {
		//Debug.Log("GotoChallenge_Space");
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		mgr.cShareData.nCurSelBuildingIdx = 1;
 		AddChallengeListBegin();
    }
	
	private void GotoChallenge_Street()
    {
		//Debug.Log("GotoChallenge_Street");
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		mgr.cShareData.nCurSelBuildingIdx = 2;
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
