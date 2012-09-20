using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;

public class DcDancingMainDlg : NvUIDialogBase 
{
    private NvLocalizationManager cLocalization;
	
	public UIPanel PanReturnConfirm;
	
    public DcCharMgr cCharMgr = null;
    private bool bIsSucceedDancing = false;
    private int nSuccessionSD = 0;

	private DcSongData cCurSongData = null;

    private DCGesture cGesture;

	public UISprite SprReady;
	public UISprite SprGo;
	public UISprite SprBeatSlider;
	
	public UISprite SprPerfect;
	public UISprite SprGood;
	public UISprite SprBad;
	public UISprite SprMiss;

    public UISprite SprCombo;
    public GameObject cComboNum;

    public GameObject cShowTime;
    public GameObject cShowTimeAcc;
	
	public UIButton BtnDrumBeat;
	public UISprite SprDrumBeatHalo;
	
	public UILabel LabTest;

    public UISprite[] SprOperator;
    public UISprite SprGestureBg;
	
	private string[] strOpeCodelst = {"null","shang","xia","zuo","you","shandian","v","sanjiao","juxing","",
                                     "","","","shangzuo","shangyou","","","","","",
                                     "","","","xiazuo","xiayou","","","","","",
                                     "","zuoshang","zuoxia","","","","","","","",
                                     "","youshang","youxia","","","","","","",""};

	private DcOperatorData cCurOpeData;
	
	private int[] nScoreStandard = {1000,500,100};//Perfect,Good,Bad
	
	private int nPerfectNum = 0;
    private int nComboNum = 0;
    private int nComboNum4Result = 0;

    private int[,] nOpeSprCoord = new int[6, 5];

    Vector3 posOpeSprPos = new Vector3(0,-194,-50);

    private bool bIsLastGoodUp = false;

    public UISprite[] SprNumber;
    public UISprite[] SprComboNumber;
    private DcNumber cNumber = new DcNumber();
    private DcNumber cComboNumber = new DcNumber();

    private bool bIsInShowTime = false;
	private int nShowTimeCnt = 0;
	private const int ComboCnt4ShowTime = 20;
	private const int ShowTimeTurns = 3;

    private Animation cCamAnim = null;
	
	protected override void OnDlgCreate()
	{
        nOpeSprCoord[2, 0] = -36;
        nOpeSprCoord[2, 1] = 36;
        nOpeSprCoord[3, 0] = -70;
        nOpeSprCoord[3, 1] = 0;
        nOpeSprCoord[3, 2] = 70;
        nOpeSprCoord[4, 0] = -106;
        nOpeSprCoord[4, 1] = -35;
        nOpeSprCoord[4, 2] = 35;
        nOpeSprCoord[4, 3] = 106;
        nOpeSprCoord[5, 0] = -140;
        nOpeSprCoord[5, 1] = -70;
        nOpeSprCoord[5, 2] = 0;
        nOpeSprCoord[5, 3] = 70;
        nOpeSprCoord[5, 4] = 140;


        cNumber.Init(ref SprNumber);
        cComboNumber.Init(ref SprComboNumber);
        ShowCombo();
		
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		cCurSongData = mgr.cShareData.cCurSongData;

        DcGlobalFunc.CreateParticle(PEType.eBubbles);
		DcGlobalFunc.CreateParticle(PEType.eFireworks);
	}


    GameObject cCamObj = null;
    public void InitCharMgr(DcCharMgr cCM)
    {
        cCharMgr = cCM;
        if (cCharMgr)
        {
            cCharMgr.ForceMainCharIdleForDancing();
            cCharMgr.CosterIdleForDancing();
        }

        cCamObj = GameObject.Find("Camera");
        GameObject cCamParentObj = GameObject.Find("CameraParent");
        cCamAnim = cCamParentObj.animation;
    }
	
	protected override void OnDlgInit ()
	{
        cLocalization = Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager;
        if (cLocalization == null)
        {
            Debug.LogError("Can Not Find Localization File!!");
        }
		
		PanReturnConfirm.gameObject.SetActiveRecursively(false);
		
		SprReady.animation.Stop();
		SprGo.animation.Stop();
		SprPerfect.animation.Stop();
		SprGood.animation.Stop();
		SprBad.animation.Stop();
		SprMiss.animation.Stop();
        cShowTime.animation.Stop();
        cShowTimeAcc.animation.Stop();
		
		SprReady.enabled = false;
		SprGo.enabled = false;
		SprPerfect.enabled = false;
		SprGood.enabled = false;
		SprBad.enabled = false;
		SprMiss.enabled = false;
        cShowTime.SetActiveRecursively(false);
        cShowTimeAcc.SetActiveRecursively(false);

        BtnDrumBeat.gameObject.active = false;
		SprDrumBeatHalo.enabled = false;
        SprDrumBeatHalo.animation.Stop();
		
        for (int i = 0; i < SprOperator.Length; i++ )
        {
            SprOperator[i].enabled = false;
            //Debug.Log("SprOperator[" + i + "].sprite.name:" + SprOperator[i].sprite.name);
        }

        SprGestureBg.enabled = false;

        cGesture = Singlton.getInstance("DCGesture") as DCGesture;
        cGesture.SetGestureEnable(false);
        cGesture.FuncGetGesture = DistinguishGesture;
		cGesture.FuncNoHasPoints = SetEmitFalse;
		cGesture.FuncSamplingPoint = SetPos;
	}

    bool bIsNullOpe = false;

    void ShowOneGroup()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;

        for (int i = 0; i < SprOperator.Length; i++)
        {
            //Debug.Log("cCurOpeData.litOpeCode["+i+"]:" + cCurOpeData.litOpeCode[i]);
            if (i < nRecordCurOpeCnt)
            {
                if (cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo[i] == 0)
                {
                    SprOperator[i].enabled = false;
                    if (SprGestureBg.enabled)
                    {
                        SprGestureBg.enabled = false;
                    }
                    bIsNullOpe = true;
                }
                else
                {
                    if (!SprGestureBg.enabled)
                    {
                        SprGestureBg.enabled = true;
                    }
                    SprOperator[i].enabled = true;
                    SprOperator[i].spriteName = strOpeCodelst[cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo[i]];
                    bIsNullOpe = false;
                    posOpeSprPos.x = nOpeSprCoord[nRecordCurOpeCnt, i];
                    SprOperator[i].transform.localPosition = posOpeSprPos;
                }
            }
            else
            {
                SprOperator[i].enabled = false;
            }
            
        }
        if (nCurOpeDataIdx != 0 && !bIsNullOpe)
        {
            if (!bIsSucceedDancing)
            {
                cCharMgr.MainCharIdleForDancing();
            }
            bIsSucceedDancing = false;
        }
    }

	Vector3 posSliderPos = new Vector3(-150f,211f,-30f);
	float fTime;
	int nTime;
	Color cC1 = new Color(1,1,0);
	Color cC2 = new Color(1,0,1);
	
	float fBeatTime = 0.00f;
	float fSliderStep;

    int nCurOpeDataIdx = 0;
	int nCurOpeIdx = 0;
    int nRecordCurOpeCnt = 0;

    bool bIsStart = false;
	bool bIsCanPressDrum = false;
	
    int nCurScore = 0;

    //int nnn = 0;

    Vector3 posOffset = new Vector3(0,50.6f,0);

	protected override void lazyUpdateImpl()
	{
        if (!bIsStart)
            return;
		
		if(cCamObj && cCharMgr)
		{
       		cCamObj.transform.LookAt(cCharMgr.cMainChar.gameObject.transform.position + posOffset);
		}
		
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;

		//Debug.Log("lazyUpdateImpl");
		fTime += Time.deltaTime;
		//Debug.Log("ftime" + ftime);
		nTime = (int)fTime;
        //LabTest.text = "Time:" + nTime.ToString() + "  Beat:" + fBeatTime + "  Score:" + nScore;
		
		fSliderStep = (300/fBeatTime)*Time.deltaTime;
		posSliderPos.x += fSliderStep;
		if(posSliderPos.x >= 150f)
		{
			posSliderPos.x = -150f;
			Debug.LogWarning("fTime" + fTime);
		}
		
		if(posSliderPos.x >= 80 && posSliderPos.x <= 100)
		{
			LabTest.color = cC1;
		}
		else
		{
			LabTest.color = cC2;
		}
		SprBeatSlider.transform.localPosition = posSliderPos;

        if (posSliderPos.x >= 115 && posSliderPos.x <= 115 + fSliderStep)
        {
            if ((nCurOpeIdx != nRecordCurOpeCnt && !bIsNullOpe) || bIsCanPressDrum)
            {
                NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
                soundctr.PlaySe("beatmiss");
            }

            if (nCurOpeDataIdx != 0 && !bIsNullOpe)
            {
                if (!bIsLastGoodUp)
                {
                    Miss();
                }

                bIsCanPressDrum = false;

                for (int i = 0; i < SprOperator.Length; i++)
                {
                    SprOperator[i].enabled = false;
                }
                BtnDrumBeat.gameObject.active = false;
                SprDrumBeatHalo.enabled = false;
                SprDrumBeatHalo.animation.Stop();

                cGesture.SetGestureEnable(false);
            }
        }
		
		//Debug.LogWarning("cCurSongData.litBeatDataIdx = " + cCurSongData.litBeatDataIdx + "     Count = " + mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData.Count);
		
        if (mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData.Count > nCurOpeDataIdx)
        {
            //Debug.Log("nCurOpeDataIdx:" + nCurOpeDataIdx);
            if (posSliderPos.x >= 140 && posSliderPos.x <= 140 + fSliderStep
                /*Mathf.Abs((float)cCurSongData.litOpeCode[nCurOpeDataIdx].nShowTime / 100f - fTime) < Time.deltaTime*/)
            {
                //Debug.LogError("nTime:" + nTime);
                if (fTime > (float)mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData[0].nShowTime / 100f - 1)
                {
					int nTempTurns = 0;
                    if (bIsInShowTime)
                    {
						if(++nShowTimeCnt > ShowTimeTurns)
						{
							ShowTimeAccomplish();
							cCurOpeData = mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData[nCurOpeDataIdx];
							nRecordCurOpeCnt = cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Count;
							nTempTurns = 1;
						}
						else
						{
							cCurOpeData = mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData[nCurOpeDataIdx];
	                        cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Clear();
							cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Add((int)UnityEngine.Random.Range(5,8));
							cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Add((int)UnityEngine.Random.Range(5,8));
							cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Add((int)UnityEngine.Random.Range(5,8));
                            if ((int)UnityEngine.Random.Range(0, 10) > 4)
                            {
                                cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Add((int)UnityEngine.Random.Range(5, 8));
                            }
                            if ((int)UnityEngine.Random.Range(0, 10) > 6)
                            {
                                cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Add((int)UnityEngine.Random.Range(5, 8));
                            }
	                        nRecordCurOpeCnt = 3;
							nTempTurns = 2;
						}
                    }
                    else
                    {
                        cCurOpeData = mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData[nCurOpeDataIdx];
                        nRecordCurOpeCnt = cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo.Count;
						nTempTurns = 1;
                    }
                    ShowOneGroup();
                    nCurOpeIdx = 0;
                    nCurOpeDataIdx += nTempTurns;
                    bIsLastGoodUp = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrumBeat();
        }

		if(cCurOpeData != null)
		{
            if (nCurOpeIdx >= nRecordCurOpeCnt)
			{
                BtnDrumBeat.gameObject.active = true;
				SprDrumBeatHalo.enabled = true;
                SprDrumBeatHalo.animation.Play();
				bIsCanPressDrum = true;
				cCurOpeData = null;
                cGesture.SetGestureEnable(false);
			}
			else
			{
                cGesture.SetGestureEnable(true);
				if (Input.GetKeyDown (KeyCode.UpArrow))
				{
					VerdictOperator(1);
				}
				if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					VerdictOperator(2);
				}
				if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					VerdictOperator(3);
				}
				if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					VerdictOperator(4);
				}
			}	
		}
	}
	
	private void ShowReturnConfirm(object sender)
	{
		PanReturnConfirm.gameObject.SetActiveRecursively(true);
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
    private void HideReturnConfirm()
    {
		PanReturnConfirm.gameObject.SetActiveRecursively(false);
		
        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
    private void ReturnMap()
    {
		//Debug.Log("ReturnMap");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");

        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        if (mgr.cShareData.nDancingEntranceType == 0)
		{
        	mgr.ChangeScene("UI_Map");
		}
		else if(mgr.cShareData.nDancingEntranceType == 1)
		{
        	mgr.ChangeScene("UI_MyHome_Interior");
		}
		else if(mgr.cShareData.nDancingEntranceType == 2)
		{
        	mgr.ChangeScene("UI_WaitingRoom");
		}
		
        cGesture.SetGestureEnable(false);
    }
	
	private void DrumBeat()
    {
		//Debug.Log("DrumBeat");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		
		if(bIsCanPressDrum)
		{
			if(posSliderPos.x >= 85 && posSliderPos.x <= 95)
			{
				Debug.LogWarning("Perfect:"+posSliderPos.x);
                soundctr.PlaySe("perfect");
                nCurScore += bIsInShowTime ? nScoreStandard[0]*2 : nScoreStandard[0];
                cNumber.ShowNumber(ref SprNumber, nCurScore);
				nPerfectNum++;
                GoodUp(true);
			}
			else if(posSliderPos.x >= 70 && posSliderPos.x <= 110)
			{
				Debug.LogWarning("Good:"+posSliderPos.x);
                soundctr.PlaySe("good");
                nCurScore += bIsInShowTime ? nScoreStandard[1]*2 : nScoreStandard[1];
                cNumber.ShowNumber(ref SprNumber, nCurScore);
                GoodUp(false);
			}
			else if(posSliderPos.x >= 65 && posSliderPos.x <= 115)
			{
				Debug.LogWarning("Bad:"+posSliderPos.x);
                soundctr.PlaySe("bad");
                nCurScore += bIsInShowTime ? nScoreStandard[2]*2 : nScoreStandard[2];
                cNumber.ShowNumber(ref SprNumber, nCurScore);
				SprBad.animation.Play();
				SprBad.enabled = true;
                bIsSucceedDancing = false;
                nSuccessionSD = 0;
                cCharMgr.MainCharIdleForDancing();
                bIsLastGoodUp = true;
			}
			else
			{
                soundctr.PlaySe("miss");
                Miss();
			}
			
			bIsCanPressDrum = false;

            if(nComboNum >= ComboCnt4ShowTime)
            {
				SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
				if(mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData.Count > nCurOpeDataIdx+ShowTimeTurns*2)
				{
                	ShowTimeStart();
				}
            }
		}
    }

    void GoodUp(bool bIsPerfect)
    {
        if (bIsPerfect)
        {
            SprPerfect.animation.Play();
            SprPerfect.enabled = true;
        }
        else
        {
            SprGood.animation.Play();
            SprGood.enabled = true;
        }
        bIsSucceedDancing = true;
        if (nSuccessionSD == 0 || nSuccessionSD > 4)
        {
            cCharMgr.MainCharDancing();
            cCharMgr.CosterDancing();
            if (nSuccessionSD > 4)
            {
                nSuccessionSD = 1;
            }
        }
        nSuccessionSD++;
        bIsLastGoodUp = true;
    }

    void Miss()
    {
        Debug.LogWarning("Miss:" + posSliderPos.x);
        SprMiss.animation.Play();
        SprMiss.enabled = true;
        bIsSucceedDancing = false;
        nSuccessionSD = 0;

        cCharMgr.MainCharIdleForDancing();

        //if (nCurOpeIdx != nRecordCurOpeCnt)
        {
            nComboNum = 0;
            ShowCombo();
        }
    }

	void VerdictOperator(int nOpeIdx)
	{
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        if (cCurOpeData.mapOpeCode[mgr.cShareData.strDiffLv].litOpeInfo[nCurOpeIdx] == nOpeIdx)
        {
            SprOperator[nCurOpeIdx].spriteName = SprOperator[nCurOpeIdx].spriteName + "_r";
			nCurOpeIdx++;
			
			if (!bIsInShowTime)
			{
				nComboNum++;
            	ShowCombo();
			}
        }
        else
        {
			ShowOneGroup();
            nCurOpeIdx = 0;
            bIsLastGoodUp = false;
            nSuccessionSD = 0;
			
            nComboNum = 0;
            ShowCombo();
        }
	}
	
	public void StartDancing()
	{
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlayBgMusic(cCurSongData.strOfficial);

        bIsStart = true;
        fBeatTime = (float)cCurSongData.nBeatTime / 1000f;

        if (cCurSongData.nLength != 0)
        {
            StartCoroutine(ShowReady(cCurSongData.nShowReadyTime));
            StartCoroutine(ShowGo(cCurSongData.nShowGoTime));
            StartCoroutine(EndDancing(cCurSongData.nLength));
        }
        else
        {
            LabTest.text = cLocalization.GetValue("NotArrangement");
        }
	}
	
	IEnumerator ShowReady(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("v_ready");
		
		SprReady.animation.Play();
		SprReady.enabled = true;
	}
	
	IEnumerator ShowGo(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("v_go");
		
		SprGo.animation.Play();
		SprGo.enabled = true;

        if (cCharMgr)
        {
            cCharMgr.MainCharDancing(true);
            cCharMgr.CosterDancing();
        }
	}
	
	IEnumerator EndDancing(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.StopBgmImmediately();
		
        bIsSucceedDancing = false;
        nSuccessionSD = 0;
        if (cCharMgr)
        {
            cCharMgr.CosterIdleForDancing();
        }
		
        if (IsWin())
		{
            if (cCharMgr)
            {
                cCharMgr.MainCharCheer();
            }

			DcGlobalFunc.SetPEEmitState(PEType.eFireworks,true);
			
			soundctr.PlaySe("music_win");
			StartCoroutine(ChangeFireworksPosition(1.5f));
			StartCoroutine(ChangeFireworksPosition(3));
			StartCoroutine(ChangeFireworksPosition(4.5f));
		}
		else
		{
            if (cCharMgr)
            {
                cCharMgr.ForceMainCharIdleForDancing();
            }
			soundctr.PlaySe("music_lost");
		}
        
        cGesture.SetGestureEnable(false);

        nComboNum4Result += nComboNum;
        nComboNum = 0;
        ShowCombo();
		
		StartCoroutine(ShowResult(5));
	}
	
	IEnumerator ChangeFireworksPosition(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);
		DcGlobalFunc.ChangePEPosition(PEType.eFireworks);
	}
	
	IEnumerator ShowResult(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);

        showDialog(false);

		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.cShareData.bIsWin = IsWin();
        mgr.cShareData.nBasicScore = nCurScore;
        mgr.cShareData.nComboScore = nComboNum4Result * nScoreStandard[1]/*nPerfectNum*nScoreStandard[0]*/;
       	mgr.ChangeScene("UI_Result");

        cGesture.SetGestureEnable(false);
	}

    public void DistinguishGesture(string str)
    {
        //Debug.Log("DistinguishGesture:" + str);
        switch (str)
        {
            case "Up":          VerdictOperator(1); break;
            case "Down":        VerdictOperator(2); break;
            case "Left":        VerdictOperator(3); break;
            case "Right":       VerdictOperator(4); break;

            case "Lighting":    VerdictOperator(5); break;
            case "V":           VerdictOperator(6); break;
            case "Triangle":    VerdictOperator(7); break;
            case "Rectangle":   VerdictOperator(8); break;

            case "UpLeft":      VerdictOperator(13); break;
            case "UpRight":     VerdictOperator(14); break;
            case "DownLeft":    VerdictOperator(23); break;
            case "DownRight":   VerdictOperator(24); break;
            case "LeftUp":      VerdictOperator(31); break;
            case "LeftDown":    VerdictOperator(32); break;
            case "RightUp":     VerdictOperator(41); break;
            case "RightDown":   VerdictOperator(42); break;
        }
    }

    bool IsWin()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        return (nCurScore >= (float)mgr.cShareData.lstBeatData[cCurSongData.litBeatDataIdx].litOpeData.Count * 0.6f * (float)nScoreStandard[1]);
    }

    void ShowCombo()
    {
        if (cComboNumber.ShowNumber(ref SprComboNumber, nComboNum) == 0)
        {
            SprCombo.animation.Stop();
            SprCombo.enabled = false;
            cComboNum.animation.Stop();
            cComboNum.SetActiveRecursively(false);
            cComboNumber.Hide(ref SprComboNumber);
        }
        else
        {
            SprCombo.animation.Stop();
            SprCombo.animation.Play();
            SprCombo.enabled = true;
            cComboNum.animation.Stop();
            cComboNum.animation.Play();
            cComboNum.SetActiveRecursively(true);
        }
    }

    void ShowTimeStart()
    {
        cShowTime.SetActiveRecursively(true);
        cShowTime.animation.Play();
        nComboNum4Result += nComboNum;
        nComboNum = 0;
        ShowCombo();
        bIsInShowTime = true;
		nShowTimeCnt = 0;

        fBeatTime *= 2f;

        DcGlobalFunc.SetPEEmitState(PEType.eBubbles,true);

        cCamAnim.Stop();
        cCamAnim.CrossFade("Cam_1");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("v_show_time");
    }

    void ShowTimeAccomplish()
    {
        //Debug.LogError("ShowTimeAccomplish");
        cShowTime.SetActiveRecursively(true);
        cShowTime.animation.Stop();
        cShowTimeAcc.SetActiveRecursively(true);
        cShowTimeAcc.animation.Play();
        bIsInShowTime = false;
		nComboNum = 0;

        fBeatTime = (float)cCurSongData.nBeatTime / 1000f;

        DcGlobalFunc.SetPEEmitState(PEType.eBubbles,false);

        cCamAnim.Stop();
        cCamAnim.CrossFade("Cam_2");
    }
	
	GameObject cPEObj = null;
	ParticleEmitter cPE = null;
	
	void SetEmitFalse()
	{
		if(!cPEObj)
		{
			cPEObj = GameObject.Find("Track");
			if(cPEObj)
			{
				cPE = cPEObj.GetComponent(typeof(ParticleEmitter)) as ParticleEmitter;
			}
		}
		else
		{
			cPE.emit = false;
		}
	}
	
	void SetPos(Vector3 posPos)
	{
		if(!cPEObj)
		{
			cPEObj = GameObject.Find("Track");
			cPE = cPEObj.GetComponent(typeof(ParticleEmitter)) as ParticleEmitter;
		}
		else
		{
			cPE.emit = true;
			cPE.transform.localPosition = posPos;
		}
	}
}

