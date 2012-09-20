using UnityEngine;
using System;

public class DcLobbyDlg : NvUIDialogBase 
{
	public UIPanel PanNewRoom;
	public UILabel LabSongName;
	public UIInput InpRoomName;
	
	public UILabel LabMode;
	private int nCurModeIdx = 0;
	
	public GameObject[] cRooms;
	public GameObject[] cSelModeDir;
	
	private NvLocalizationManager cLocalization;
	
	bool bPlaySound = true;
	
	protected override void OnDlgCreate()
	{
        cLocalization = Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager;
        if (cLocalization == null)
        {
            Debug.LogError("Can Not Find Localization File!!");
        }
	}
	
	protected override void OnDlgInit ()
	{
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		for (int i = 0; i < cRooms.Length; i++)
		{
			cRooms[i].SetActiveRecursively(false);
		}
		for (int i = 0; i < mgr.cShareData.lstRoomsData.Count; i++)
		{
			cRooms[i].SetActiveRecursively(true);
			
			for(int j = 0; j < cRooms[i].transform.childCount; j++)
			{
				GameObject child = cRooms[i].transform.GetChild(j).gameObject;
				if(child.name == "LabName")
				{
					UILabel LabName = child.GetComponent(typeof(UILabel)) as UILabel;
					LabName.text = /*cLocalization.GetValue("RoomName") + */mgr.cShareData.lstRoomsData[i].strRoomName;
				}
				else if(child.name == "LabMode")
				{
					UILabel LabMode = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabMode.text = /*cLocalization.GetValue("DancingMode") + */cLocalization.GetValue(mgr.cShareData.lstRoomsData[i].strMode);
				}
				else if(child.name == "LabSong")
				{
					UILabel LabSong = child.GetComponent(typeof(UILabel)) as UILabel;
					LabSong.text = /*cLocalization.GetValue("SongText") + */mgr.cShareData.lstSongData[mgr.cShareData.lstRoomsData[i].nSongID].strSongName;
				}
				else if(child.name == "LabNum")
				{
					UILabel LabNum = child.GetComponent(typeof(UILabel)) as UILabel;
					LabNum.text = /*cLocalization.GetValue("NumberOfPeople") + */mgr.cShareData.lstRoomsData[i].nNumOfPeople + "/3";
				}
                else if (child.name == "sprIcon")
                {
                    UISprite SprIcon = child.GetComponent(typeof(UISprite)) as UISprite;
                    if (mgr.cShareData.lstRoomsData[i].litPeopleSex[0] == 0)
                    {
                        SprIcon.spriteName = mgr.cShareData.strManCharIcon[(int)UnityEngine.Random.Range(0, mgr.cShareData.strManCharIcon.Length)];
                    }
                    else
                    {
                        SprIcon.spriteName = mgr.cShareData.strWomanCharIcon[(int)UnityEngine.Random.Range(0, mgr.cShareData.strWomanCharIcon.Length)];
                    }
                }
			}
		}
		
		bPlaySound = false;
		if(mgr.cShareData.bIsInBuildNewRoom)
		{
			ShowNewRoom();
			InpRoomName.text = mgr.cShareData.strCurRoomName;
			if(mgr.cShareData.cCurSongData != null)
			{
				LabSongName.text = mgr.cShareData.cCurSongData.strSongName;
			}
			else
			{
				LabSongName.text = "Random Songs";
			}
		}
		else
		{
			HideNewRoom();
		}
		bPlaySound = true;
//	for(int i=0;i<efx.transform.childCount;i++)
//	{
//		GameObject child = efx.transform.GetChild(i).gameObject;
//	}
		
//	aFinger = transform.Find("LeftShoulder/Arm/Hand/Finger");
//	aFinger.Rotate(Time.deltaTime*20, 0, 0);


	}
	
	public void OnDrop(Vector3 dir)
    {
        //Debug.LogWarning(" OnDrop:  " + dir);
        if (dir.x<0)
        {
           SelModeToLeft();
        }
        else if(dir.x>0)
        {
           SelModeToRight();
        }
    }
	
//	bool bIsValidGest = false;
//	void IsEnabledGesture(Vector3 posPos)
//	{
//		Debug.LogError("posPos" + posPos.x + " " + posPos.y);
//		if(posPos.x >= 65f && posPos.x <= 161f && posPos.y >= -11.5f && posPos.y <= 28.5f)
//		{
//			bIsValidGest = true;
//		}
//	}
	
	void SelModeToLeft()
	{
		cSelModeDir[1].SetActiveRecursively(true);
		if(--nCurModeIdx <= 0)
		{
			nCurModeIdx = 0;
			cSelModeDir[0].SetActiveRecursively(false);
		}
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		LabMode.text = mgr.cShareData.strDancingMode[nCurModeIdx];
	}
	
	void SelModeToRight()
	{
		cSelModeDir[0].SetActiveRecursively(true);
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		if(++nCurModeIdx >= mgr.cShareData.strDancingMode.Length - 1)
		{
			nCurModeIdx = mgr.cShareData.strDancingMode.Length - 1;
			cSelModeDir[1].SetActiveRecursively(false);
		}
		LabMode.text = mgr.cShareData.strDancingMode[nCurModeIdx];
	}
	
    private void ReturnMap()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Map");
		
		mgr.cShareData.bIsInBuildNewRoom = false;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }

    private void SelRoom(object sender)
    {
        GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string lastnum = DcGlobalFunc.GetLastString(obj.transform.parent.name, splt);
        int nSelRoom = Convert.ToInt32(lastnum)-1;
        //Debug.LogError(lastnum);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.cShareData.cCurRoomData = mgr.cShareData.lstRoomsData[nSelRoom];
        mgr.cShareData.cCurSongData = mgr.cShareData.lstSongData[mgr.cShareData.lstRoomsData[nSelRoom].nSongID];
        mgr.cShareData.strDiffLv = mgr.cShareData.lstRoomsData[nSelRoom].strDiff;
		mgr.cShareData.strMode = mgr.cShareData.strDancingMode[nCurModeIdx];
        mgr.cShareData.cCurSongData.nWhereScene = 0;

        GotoWaitingRoom();
    }

    private void CreateRoom()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        DcImitateRoomData cNewRoom = new DcImitateRoomData();
        //cNewRoom.litPeopleName.Add("");
        //cNewRoom.litPeopleSex.Add((int)mgr.cShareData.eUserSex);
        cNewRoom.nNumOfPeople = 1;
        cNewRoom.nRoomID = 1000;
		if(LabSongName.text == "Random Songs")
		{
			mgr.cShareData.cCurSongData = mgr.cShareData.lstSongData[0];
         	mgr.cShareData.strDiffLv = "Simple";
		}
//         cNewRoom.nSongID;
//         cNewRoom.strDiff;
         cNewRoom.strRoomName = InpRoomName.text;
        mgr.cShareData.cCurRoomData = cNewRoom;
        mgr.cShareData.cCurSongData.nWhereScene = 0;
// 
         GotoWaitingRoom();
    }
	
	private void SelSong()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
    	mgr.ChangeScene("UI_MyHome_Disk");
		mgr.cShareData.nDiskEntranceType = 1;
	
		mgr.cShareData.bIsInBuildNewRoom = true;
		mgr.cShareData.strCurRoomName = InpRoomName.text;

		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void ShowNewRoom()
	{
		PanNewRoom.gameObject.SetActiveRecursively(true);
		
		cSelModeDir[0].SetActiveRecursively(false);
		cSelModeDir[1].SetActiveRecursively(true);
		
		nCurModeIdx = 0;
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		LabMode.text = mgr.cShareData.strDancingMode[nCurModeIdx];
		InpRoomName.text = "Dancing Ferly";
		LabSongName.text = "Random Songs";
		
		if(bPlaySound)
		{
			NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
            soundctr.PlaySe("ui_enter");
		}
	}
	
    private void HideNewRoom()
    {
		PanNewRoom.gameObject.SetActiveRecursively(false);
		
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		mgr.cShareData.bIsInBuildNewRoom = false;
		
		if(bPlaySound)
		{
        	NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
			soundctr.PlaySe("ui_touch");
		}
    }
	
	private void GotoWaitingRoom()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_WaitingRoom");
		
		mgr.cShareData.bIsInBuildNewRoom = false;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }
}
