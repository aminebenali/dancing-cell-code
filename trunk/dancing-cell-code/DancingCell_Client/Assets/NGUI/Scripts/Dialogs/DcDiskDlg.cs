using UnityEngine;
using System;
using LitJson;

public class DcDiskDlg : NvUIDialogBase 
{
	public UISprite[] SprDisk;
	public UISprite[] SprInverted;
	public UILabel[] LabSongName;
	
	private int nAtMiddleIdx = 4;
    private int nNewMidIdx = 4;
    private int nState = -2;

    public GameObject[] cSelDiff;

    public UIPanel PanDiff;
    public UISprite SprDiffBg;
    public UISprite SprDiffCancelBtn;
    public UISprite SprDiffCancelText;

	protected override void OnDlgCreate()
	{
		InitSprDisk();
	}
	
	protected override void OnDlgInit ()
	{
        ShowDiffSel(false);
	}
	
	void InitSprDisk()
	{
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;

		if(nAtMiddleIdx >= 0 && nAtMiddleIdx < SprDisk.Length)
		{
			for(int i = 0; i < SprDisk.Length; i++)
			{
//				string name = "ZZZ";
//				UIAtlas t = Resources.Load("UI Prefabs/Atlases/UI/ZZZ/"+name) as UIAtlas;
//				if ( t == null )
//				{
//					Debug.LogError("No Such Portrait "+ name);
//				}	
                SprDisk[i].spriteName = mgr.cShareData.lstSongData[i].strSpriteName;
                SprInverted[i].spriteName = mgr.cShareData.lstSongData[i].strSpriteName;
			}
			
			NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
            soundctr.PlayBgMusic(mgr.cShareData.lstSongData[nAtMiddleIdx].strAudition);
			
			for(int i = 0; i < LabSongName.Length; i++)
			{
                LabSongName[i].text = mgr.cShareData.lstSongData[nAtMiddleIdx].strSongName;
			}
			
			SprDisk[nAtMiddleIdx].transform.localPosition = new Vector3(0,0,0);
			SprDisk[nAtMiddleIdx].transform.localScale = new Vector3(250,250,1);

            SprInverted[nAtMiddleIdx].transform.localPosition = new Vector3(0, -250, 1);
            SprInverted[nAtMiddleIdx].transform.localScale = new Vector3(250, 250, 1);

			for (int i = nAtMiddleIdx-1; i >= 0; i-- )
	        {
	            if(i == nAtMiddleIdx - 1)
				{
					SprDisk[i].transform.localPosition = new Vector3(-205,0,0);
					SprDisk[i].transform.localScale = new Vector3(150,150,1);

                    SprInverted[i].transform.localPosition = new Vector3(-205, -150, 0);
                    SprInverted[i].transform.localScale = new Vector3(150, 150, 1);
				}
				else
				{
					Vector3 posTemp = SprDisk[i+1].transform.localPosition;
					posTemp.x -= 40;
					SprDisk[i].transform.localPosition = posTemp;
					
					posTemp = SprDisk[i+1].transform.localScale;
					posTemp.x -= 30;
					posTemp.y -= 30;
					SprDisk[i].transform.localScale = posTemp;

                    SprInverted[i].transform.localPosition = new Vector3(SprDisk[i].transform.localPosition.x, -posTemp.y, 0);
                    SprInverted[i].transform.localScale = posTemp;
				}
	        }
			for (int i = nAtMiddleIdx+1; i < SprDisk.Length; i++ )
	        {
	            if(i == nAtMiddleIdx + 1)
				{
					SprDisk[i].transform.localPosition = new Vector3(205,0,0);
					SprDisk[i].transform.localScale = new Vector3(150,150,1);

                    SprInverted[i].transform.localPosition = new Vector3(205, -150, 0);
                    SprInverted[i].transform.localScale = new Vector3(150, 150, 1);
				}
				else
				{
					Vector3 posTemp = SprDisk[i-1].transform.localPosition;
					posTemp.x += 40;
					SprDisk[i].transform.localPosition = posTemp;
					
					posTemp = SprDisk[i-1].transform.localScale;
					posTemp.x -= 30;
					posTemp.y -= 30;
					SprDisk[i].transform.localScale = posTemp;

                    SprInverted[i].transform.localPosition = new Vector3(SprDisk[i].transform.localPosition.x, -posTemp.y, 0);
                    SprInverted[i].transform.localScale = posTemp;
				}
	        }

            UpDateDepth();
		}
	}

    void UpDateDepth()
    {
        if(nAtMiddleIdx >= 0 && nAtMiddleIdx < SprDisk.Length)
		    {
			    SprDisk[nAtMiddleIdx].depth = 100;
                SprInverted[nAtMiddleIdx].depth = 50;

			    for (int i = nAtMiddleIdx-1; i >= 0; i-- )
	            {
				    SprDisk[i].depth = SprDisk[i+1].depth - 1;
                    SprInverted[i].depth = SprInverted[i+1].depth - 1;
	            }
			    for (int i = nAtMiddleIdx+1; i < SprDisk.Length; i++ )
	            {
				    SprDisk[i].depth = SprDisk[i-1].depth - 1;
                    SprInverted[i].depth = SprInverted[i-1].depth - 1;
	            }
		    }
    }

    Vector3 SprEachChange(ref UISprite[] SprDisk, int nIdx, float fEachPosX, float fEachScale)
    {
        Vector3 posCurPos = SprDisk[nIdx].transform.localPosition;
        posCurPos.x += fEachPosX;
        SprDisk[nIdx].transform.localPosition = posCurPos;
        float fScale = SprDisk[nIdx].transform.localScale.x;
        fScale += fEachScale;
        SprDisk[nIdx].transform.localScale = new Vector3(fScale, fScale, 0);

        SprInverted[nIdx].transform.localPosition = new Vector3(posCurPos.x, -fScale, 1);
        SprInverted[nIdx].transform.localScale = new Vector3(fScale, fScale, 1);
        //SprInverted[nIdx].depth = 100;

        return posCurPos;
    }

	void Update ()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;

        switch (nState)
        {
            case 0://turn finish
                nAtMiddleIdx = nNewMidIdx;
                nState = -2;
                UpDateDepth();
				for(int i = 0; i < LabSongName.Length; i++)
				{
					LabSongName[i].enabled = true;
                    LabSongName[i].text = mgr.cShareData.lstSongData[nAtMiddleIdx].strSongName;
				}
				NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
                soundctr.PlayBgMusic(mgr.cShareData.lstSongData[nAtMiddleIdx].strAudition);
                break;
            case -1://left turn
                SprEachChange(ref SprDisk, nAtMiddleIdx, -20.5f, -10f);
                Vector3 posCurMidPos = SprEachChange(ref SprDisk, nAtMiddleIdx + 1, -20.5f, 10f);

                for (int i = nAtMiddleIdx - 1; i >= 0; i--)
                {
                    SprEachChange(ref SprDisk, i, -4f, -3f);
                }
                for (int i = nAtMiddleIdx + 2; i < SprDisk.Length; i++)
                {
                    SprEachChange(ref SprDisk, i, -4f, 3f);
                }
                
                if (posCurMidPos.x <= 0)
                {
                    nState = 0;
                }
                break;
            case 1://right turn
                SprEachChange(ref SprDisk, nAtMiddleIdx, 20.5f, -10f);
                posCurMidPos = SprEachChange(ref SprDisk, nAtMiddleIdx - 1, 20.5f, 10f);

                for (int i = nAtMiddleIdx - 2; i >= 0; i--)
                {
                    SprEachChange(ref SprDisk, i, 4f, 3f);
                }
                for (int i = nAtMiddleIdx + 1; i < SprDisk.Length; i++)
                {
                    SprEachChange(ref SprDisk, i, 4f, -3f);
                }

                if (posCurMidPos.x >= 0)
                {
                    nState = 0;
                }
                break;
        }
        
		if(Input.GetKeyUp(KeyCode.LeftArrow))
		{
            Turn(1);
		}
        if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			Turn(-1);
		}
	}
	
	void Turn(int nDirection)
	{
		if (nState == -2)
        {
            nNewMidIdx += nDirection;
            if (nNewMidIdx < 0)
			{
				nNewMidIdx = 0;
			}
			else if (nNewMidIdx >= SprDisk.Length)
			{
				nNewMidIdx = SprDisk.Length - 1;
			}
            else
			{				
                nState = -nDirection;
				for(int i = 0; i < LabSongName.Length; i++)
				{
					LabSongName[i].enabled = false;
				}
			}
		}
	}
	
    private void ReturnInterior()
    {
        showDialog(false);		
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
		
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		if(mgr.cShareData.nDiskEntranceType == 0)
		{
            mgr.ChangeScene("UI_MyHome_Interior");
		}
		else if(mgr.cShareData.nDiskEntranceType == 1)
		{
            mgr.ChangeScene("UI_Lobby");
		}
		mgr.cShareData.cCurSongData = null;
    }
	
	private void SelSong()
    {
        //Debug.LogError(nAtMiddleIdx);
        ShowDiffSel(true);
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_enter");
    }
	
	private void DiffCancel()
    {
        //Debug.LogError(nAtMiddleIdx);
        ShowDiffSel(false);
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_enter");
    }
	
	private void SelDiff(object sender)
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;

        GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string lastnum = DcGlobalFunc.GetLastString(obj.transform.name, splt);
        Debug.LogError(lastnum);
        int nIdx = Convert.ToInt32(lastnum);

        switch (nIdx)
        {
            case 0:
                mgr.cShareData.strDiffLv = "Simple";
                break;
            case 1:
                mgr.cShareData.strDiffLv = "Normal";
                break;
            case 2:
                mgr.cShareData.strDiffLv = "Diff";
                break;
        }
        Debug.LogError(mgr.cShareData.strDiffLv);
		
		//showDialog(false);

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");

        mgr.cShareData.cCurSongData = mgr.cShareData.lstSongData[nAtMiddleIdx];
        mgr.cShareData.strMode = "Default";
		if(mgr.cShareData.nDiskEntranceType == 0)
		{
            mgr.ChangeScene(mgr.cShareData.lstBuildingData[mgr.cShareData.lstSongData[nAtMiddleIdx].nWhereScene].str3DSceneName);
            //Debug.LogError(mgr.cShareData.lstBuildingData[mgr.cShareData.lstSongData[nAtMiddleIdx].nWhereScene].str3DSceneName);
			mgr.cShareData.nDancingEntranceType = 1;
		}
		else if(mgr.cShareData.nDiskEntranceType == 1)
		{
			mgr.ChangeScene("UI_Lobby");
		}
    }

    public void OnDrop(Vector3 dir)
    {
        //Debug.LogWarning(" OnDrop:  " + dir);
        if (dir.x<0)
        {
           Turn(1);
        }
        else if(dir.x>0)
        {
           Turn(-1);
        }
    }

    public void ShowDiffSel(bool bIsShow)
    {
        //PanDiff.gameObject.SetActiveRecursively(bIsShow);
        DcDrag cDrag = GetComponent(typeof(DcDrag)) as DcDrag;
        cDrag.enabled = !bIsShow;

        if(bIsShow)
        {
            PanDiff.transform.localScale = new Vector3(1f,1f,1f);
            SprDiffBg.enabled = true;
            SprDiffCancelBtn.enabled = true;
            SprDiffCancelText.enabled = true;
            for (int i = 0; i < cSelDiff.Length; i++)
            {
                //cSelDiff[i].SetActiveRecursively(true);
                cSelDiff[i].animation.Play();
            }
        }
        else
        {
            PanDiff.transform.localScale = new Vector3(0f, 0f, 0f);
            SprDiffBg.enabled = false;
            SprDiffCancelBtn.enabled = false;
            SprDiffCancelText.enabled = false;
            for (int i = 0; i < cSelDiff.Length; i++)
            {
                cSelDiff[i].animation.Stop();
                //cSelDiff[i].SetActiveRecursively(false);
            }
        }
    }
}
