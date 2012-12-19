using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class DcWaitingRoomDlg : NvUIDialogBase 
{
    public GameObject BtnPrepare;
	public GameObject BtnStart;
	
	public UILabel[] LabCharName;
    public UISprite[] SprPrepare;
    public UISprite[] SprLight;

	private int nCurPeopleNum = 1;

    private bool bIsShowStart = false;
	
	protected override void OnDlgCreate()
	{
        
	}
	
	protected override void OnDlgInit ()
	{
        for (int i = 0; i < SprPrepare.Length; i++)
        {
            SprPrepare[i].enabled = false;
            SprLight[i].enabled = false;
        }

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        LabCharName[0].text = mgr.cShareData.strUserName;
		
		for (int i = 0; i < 2; i++ )
		{
			LabCharName[i + 1].enabled = false;
		}

	    for (int i = 0; i < mgr.cShareData.cCurRoomData.litPeopleName.Count; i++ )
	    {
			if(i + 1 < LabCharName.Length)
			{
				if (!string.IsNullOrEmpty(mgr.cShareData.cCurRoomData.litPeopleName[i]))
	            {
                    LabCharName[i + 1].enabled = true;
	            	LabCharName[i + 1].text = mgr.cShareData.cCurRoomData.litPeopleName[i];
	           	}
			}
		}

        BtnStart.gameObject.SetActiveRecursively(false);

        DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex, CharModel.eOne, CharForWhat.eWaitingRoomMain, true);
		
		if(mgr.cShareData.cCurRoomData.litPeopleSex.Count >= 1)
		{
			DcGlobalFunc.CreateChar(mgr.cShareData.cCurRoomData.litPeopleSex[0] == 0 ? CharSex.eMan : CharSex.eWoman,
                                    mgr.cShareData.cCurRoomData.litPeopleSex[0] == 0 ? CharModel.eOne : CharModel.eTwo,
                                    CharForWhat.eWaitingRoomLeft, true, false);
			
        	StartCoroutine(ImitateOthersPrepare((int)UnityEngine.Random.Range(1, 5), 1));
        	nCurPeopleNum++;
		}
		if(mgr.cShareData.cCurRoomData.litPeopleSex.Count >= 2)
		{
            DcGlobalFunc.CreateChar(mgr.cShareData.cCurRoomData.litPeopleSex[1] == 0 ? CharSex.eMan : CharSex.eWoman,
                                    mgr.cShareData.cCurRoomData.litPeopleSex[1] == 0 ? CharModel.eOne : CharModel.eTwo,
                                    CharForWhat.eWaitingRoomRight, true, false);
			
        	StartCoroutine(ImitateOthersPrepare((int)UnityEngine.Random.Range(1, 5), 2));
			nCurPeopleNum++;
		}
	}

    IEnumerator ImitateOthersPrepare(float timevalue,int nCharIdx)
    {
        yield return new WaitForSeconds(timevalue);
        SprPrepare[nCharIdx].enabled = true;
        SprLight[nCharIdx].enabled = true;
    }

    protected override void lazyUpdateImpl()
    {
        if (!bIsShowStart)
        {
            int nCnt = 0;
            for (int i = 0; i < nCurPeopleNum; i++)
            {
                if (SprPrepare[i].enabled)
                    nCnt++;
            }
            if(nCnt == nCurPeopleNum)
            {
                bIsShowStart = true;
            }
        }

        if (!BtnStart.active)
        {
            if (bIsShowStart)
            {
                BtnStart.gameObject.SetActiveRecursively(true);
            }
        }
    }

    private void ReturnLobby()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Lobby");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void Prepare()
    {
        SprPrepare[0].enabled = true;
        SprLight[0].enabled = true;
        BtnPrepare.gameObject.SetActiveRecursively(false);

		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void StartDancing()
    {
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene(mgr.cShareData.lstBuildingData[mgr.cShareData.cCurSongData.nWhereScene].str3DSceneName);
		mgr.cShareData.nDancingEntranceType = 2;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }
	
}
