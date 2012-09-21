using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DcResultDlg : NvUIDialogBase 
{	
	public UILabel LabOwnership;
	
	public UISprite[] SprBasicNumber;
	public UISprite[] SprComboNumber;
	private DcNumber cBasicNumber = new DcNumber();
	private DcNumber cComboNumber = new DcNumber();
	
	public GameObject PanBgShake;
	public UISprite SprWin;
	public UISprite SprLost;
    public UISprite SprBlackLine;

    public GameObject[] cWinFlicker;

	private NvLocalizationManager cLocalization;
	
	bool bIsRelease = true;
	
	protected override void OnDlgCreate()
	{
        
	}
	
	protected override void OnDlgInit ()
	{
		if(bIsRelease)
		{
	        cLocalization = Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager;
	        if (cLocalization == null)
	        {
	            Debug.LogError("Can Not Find Localization File!!");
	        }
		}

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlayBgMusic("music_result");

        cBasicNumber.Init(ref SprBasicNumber, eNumberType.eSilver);
        cComboNumber.Init(ref SprComboNumber, eNumberType.eSilver);
 
        PanBgShake.animation.Stop();
        SprWin.animation.Stop();
        SprLost.animation.Stop();
        SprBlackLine.animation.Stop();
        SprWin.enabled = false;
        SprLost.enabled = false;
        SprBlackLine.enabled = false;

        for (int i = 0; i < SprBasicNumber.Length; i++)
        {
            SprBasicNumber[i].animation.Stop();
            SprComboNumber[i].animation.Stop();
        }

        for (int i = 0; i < cWinFlicker.Length; i++ )
        {
            cWinFlicker[i].SetActiveRecursively(false);
        }
        DcGlobalFunc.SetPEEmitState(PEType.eWinFireworks, false);

		StartCoroutine(ShowScore(1));
	}

    private void GotoMap()
    {
        Debug.Log("GotoMap");

        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        if (mgr.cShareData.nDancingEntranceType == 0)
        {
            mgr.ChangeScene("UI_Map");
        }
        else if (mgr.cShareData.nDancingEntranceType == 1)
        {
            mgr.ChangeScene("UI_MyHome_Interior");
        }
        else if (mgr.cShareData.nDancingEntranceType == 2)
        {
            mgr.ChangeScene("UI_WaitingRoom");
        }
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_enter");
    }
	
	IEnumerator ShowScore(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);
		
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		
 		cBasicNumber.ShowNumber(ref SprBasicNumber,bIsRelease ? mgr.cShareData.nBasicScore : 12345);
 		int nDigit = cComboNumber.ShowNumber(ref SprComboNumber, bIsRelease ? mgr.cShareData.nComboScore : 67890);
		
        for (int i = 0; i < SprBasicNumber.Length; i++)
        {
            SprBasicNumber[i].animation.Play();
            SprComboNumber[i].enabled = false;
        }
        StartCoroutine(DelayPlayComboNumberAnim(nDigit,0.8f));

		if(bIsRelease ? mgr.cShareData.bIsWin : true)
		{
            SprWin.enabled = true;
            SprWin.animation.Play();

            if(bIsRelease)
			{
				if (mgr.cShareData.bIsChallengeOthers)
	            {
	                if (mgr.cShareData.nBasicScore > mgr.cShareData.nHighScores)
	                {
	                    LabOwnership.text = cLocalization.GetValue("GetOwnership");
	                    mgr.cShareData.lstBuildingData[mgr.cShareData.nCurSelBuildingIdx].litSongMaster[mgr.cShareData.nCurSelSongIdx4Building] = mgr.cShareData.strUserName;
	                    mgr.cShareData.lstBuildingData[mgr.cShareData.nCurSelBuildingIdx].litHighScores[mgr.cShareData.nCurSelSongIdx4Building] = mgr.cShareData.nBasicScore;
	                    DcGlobalFunc.SaveJson(mgr.cShareData.lstBuildingData, "Assets/Resources/TXT/Table/Building.txt");
	                }
	                else
	                {
	                    LabOwnership.text = cLocalization.GetValue("ChallengeFail");
	                }
	            }
	            else
	            {
	                if (mgr.cShareData.nBasicScore > mgr.cShareData.nHighScores)
	                {
	                    LabOwnership.text = cLocalization.GetValue("NewHighScores");
	                    mgr.cShareData.lstBuildingData[mgr.cShareData.nCurSelBuildingIdx].litHighScores[mgr.cShareData.nCurSelSongIdx4Building] = mgr.cShareData.nBasicScore;
	                    DcGlobalFunc.SaveJson(mgr.cShareData.lstBuildingData, "Assets/Resources/TXT/Table/Building.txt");
	                }
	                else
	                {
	                    LabOwnership.text = cLocalization.GetValue("NoSurpass");
	                }
	            }
			}
		}
		else
		{
 			SprLost.enabled = true;
 			SprLost.animation.Play();

            PanBgShake.animation.CrossFade("ShopBgColorChange");

			if(bIsRelease)
			{
				LabOwnership.text = cLocalization.GetValue("IsSubstandard");  
			} 
		}
	}

    IEnumerator DelayPlayComboNumberAnim(int nDigit,float timevalue)
    {
        yield return new WaitForSeconds(timevalue);
        for (int i = 0; i < SprBasicNumber.Length; i++)
        {
            if (/*SprComboNumber[i].spriteName != "Silver0" && */i < nDigit)
            {
                SprComboNumber[i].enabled = true;
            }
            SprComboNumber[i].animation.Play();
        }
    }
}
