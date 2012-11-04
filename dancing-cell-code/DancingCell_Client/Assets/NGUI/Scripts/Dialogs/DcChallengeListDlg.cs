using UnityEngine;
using System;
using LitJson;

public class DcChallengeListDlg : NvUIDialogBase 
{
    public UILabel LabSongMaster = null;
	public UILabel LabSongName = null;
	public UILabel LabSongDifficulty = null;
	public UILabel LabHighScores = null;
    public UILabel LabMode = null;

    public GameObject[] cSongSel;
    private UISprite[] SprSongSelCharIcon = new UISprite[5];
    private UISprite[] SprSongSelHalo = new UISprite[5];

    public UISprite[] SprMoneyNumber;
    private DcNumber cMoneyNumber = new DcNumber();
    public UISprite[] SprExpNumber;
    private DcNumber cExpNumber = new DcNumber();
	
	private int nCurSelChallengeIndex = 0;
    private int nLastCSCIdx = 0;
	
	public NvLocalizationManager cLocalization;
	
	protected override void OnDlgCreate()
	{
        cMoneyNumber.Init(ref SprMoneyNumber, eNumberType.eSilver);
        cExpNumber.Init(ref SprExpNumber);
	}
	
	protected override void OnDlgInit ()
	{		
		cLocalization = Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager;
        if (cLocalization == null)
        {
            Debug.LogError("Can Not Find Localization File!!");
        }

        for (int i = 0; i < cSongSel.Length; i++)
        {
            cSongSel[i].SetActiveRecursively(false);
            for (int j = 0; j < cSongSel[i].transform.childCount; j++)
            {
                GameObject child = cSongSel[i].transform.GetChild(j).gameObject;
                if (child.name == "sprBG")
                {
                    SprSongSelCharIcon[i] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                else if (child.name == "sprHalo")
                {
                    SprSongSelHalo[i] = child.GetComponent(typeof(UISprite)) as UISprite;
                    SprSongSelHalo[i].enabled = (nCurSelChallengeIndex == i ? true : false);
                }
            }
        }

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        DcBuildingData cTempBuildingData = mgr.cShareData.lstBuildingData[mgr.cShareData.nCurSelBuildingIdx];
        for (int i = 0; i < cTempBuildingData.litSongIDs.Count; i++)
        {
            cSongSel[i].SetActiveRecursively(true);
            if(cTempBuildingData.litMasterSex[i] == 0)
            {
                SprSongSelCharIcon[i].spriteName = mgr.cShareData.strManCharIcon[(int)UnityEngine.Random.Range(0,mgr.cShareData.strManCharIcon.Length)];
            }
            else
            {
                SprSongSelCharIcon[i].spriteName = mgr.cShareData.strWomanCharIcon[(int)UnityEngine.Random.Range(0, mgr.cShareData.strWomanCharIcon.Length)];
            }
        }

        SetChallengeChallenge();
	}
	
    public void InitChbSel()
    {
        nLastCSCIdx = nCurSelChallengeIndex;
        //ChbSel_1.isChecked = true;
    }

    private void ReturnMap()
    {
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Map");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }

    private void SelChallengeIndex(object sender)
	{
        GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string lastnum = DcGlobalFunc.GetLastString(obj.transform.name, splt);
        nCurSelChallengeIndex = Convert.ToInt32(lastnum) - 1;
		//Debug.Log("SelChallengeIndex:" + nCurSelChallengeIndex);
        if (nCurSelChallengeIndex != nLastCSCIdx)
		{
            SetChallengeChallenge();
            nLastCSCIdx = nCurSelChallengeIndex;

            for (int i = 0; i < SprSongSelHalo.Length; i++)
            {
                SprSongSelHalo[i].enabled = (nCurSelChallengeIndex == i ? true : false);
            }

			NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
			soundctr.PlaySe("ui_check");
		}
	}
	
	private void BeginChallenge()
	{
		//Debug.Log("BeginChallenge");

        showDialog(false);

		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene(mgr.cShareData.lstBuildingData[mgr.cShareData.nCurSelBuildingIdx].str3DSceneName);
        mgr.cShareData.nDancingEntranceType = 0;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_enter");
	}
	
	public void SetChallengeChallenge()
	{
		//Debug.Log("SetChallengeChallenge---------------------");
		
		//Debug.Log("lstSceneData[nCurSelScene].litSongIDs[nCurSelChallengeIndex]:" + lstSceneData[nCurSelScene].litSongIDs[nCurSelChallengeIndex]);
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;

        if (mgr.cShareData.nCurSelBuildingIdx >= mgr.cShareData.lstBuildingData.Count)
		{
			//Debug.Log("nCurSelScene >= lstSceneData.Count");
			return;
		}
        DcBuildingData cTempBuildingData = mgr.cShareData.lstBuildingData[mgr.cShareData.nCurSelBuildingIdx];
        if (nCurSelChallengeIndex >= cTempBuildingData.litSongIDs.Count)
		{
			//Debug.Log("nCurSelChallengeIndex >= cTempSceneData.litSongIDs.Count");
			return;
		}
        if (cTempBuildingData.litSongIDs[nCurSelChallengeIndex] >= mgr.cShareData.lstSongData.Count)
		{
			//Debug.Log("cTempSceneData.litSongIDs[nCurSelChallengeIndex] >= lstSongData.Count");
			return;
		}
//		Debug.Log("nCurSelScene:" + nCurSelScene);
//		Debug.Log("nCurSelChallengeIndex:" + nCurSelChallengeIndex);
//		Debug.Log("cTempSceneData.litSongIDs.Count:" + cTempSceneData.litSongIDs.Count);
        mgr.cShareData.cCurSongData = mgr.cShareData.lstSongData[cTempBuildingData.litSongIDs[nCurSelChallengeIndex]];
        mgr.cShareData.strDiffLv = cTempBuildingData.litDiffLvs[nCurSelChallengeIndex];
        mgr.cShareData.strMode = cTempBuildingData.litMode[nCurSelChallengeIndex];
		mgr.cShareData.nHighScores = cTempBuildingData.litHighScores[nCurSelChallengeIndex];
		
        LabSongMaster.text = cTempBuildingData.litSongMaster[nCurSelChallengeIndex];
		if(LabSongMaster.text != mgr.cShareData.strUserName)
		{
			mgr.cShareData.bIsChallengeOthers = true;
			mgr.cShareData.nCurSelSongIdx4Building = nCurSelChallengeIndex;
		}
		else
		{
			mgr.cShareData.bIsChallengeOthers = false;
		}
		LabSongName.text = /*cLocalization.GetValue("ChallengeSongs") + */mgr.cShareData.cCurSongData.strSongName;
        LabSongDifficulty.text = /*cLocalization.GetValue("ChallengeTheDifficulty") + */cLocalization.GetValue(mgr.cShareData.strDiffLv);
        LabMode.text = /*cLocalization.GetValue("ChallengeMode") + */cLocalization.GetValue(mgr.cShareData.strMode);
		LabHighScores.text = /*cLocalization.GetValue("ChallengeHighScores") + */mgr.cShareData.nHighScores.ToString();
        cMoneyNumber.ShowNumber(ref SprMoneyNumber, mgr.cShareData.cCurSongData.nRewardMoney);
        cExpNumber.ShowNumber(ref SprExpNumber, mgr.cShareData.cCurSongData.nRewardExp);

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlayBgMusic(mgr.cShareData.cCurSongData.strAudition);
	}
}
