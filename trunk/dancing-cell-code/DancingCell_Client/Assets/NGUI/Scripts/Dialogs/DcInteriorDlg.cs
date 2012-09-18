using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DcInteriorDlg : NvUIDialogBase 
{
	protected override void OnDlgCreate()
	{
	}
	
	protected override void OnDlgInit ()
	{
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex, CharModel.eOne, CharForWhat.eInterior,true);
	}
	
	private void ReturnOutside()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Outside");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
    private void GotoDisk()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Disk");
		mgr.cShareData.nDiskEntranceType = 0;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void GotoChest()
    {
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Chest");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }

    private void GotoDresser()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Dresser");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }
	
	private void GotoShop()
	{
		showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Shop");
		mgr.cShareData.nShopEntranceType = 2;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
	public void GotoPersonal()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Personal");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	public void GotoSuccess()
    {
        showDialog(false);
		
		GameObject obj = GameObject.Find("Success");
		if(obj)
		{
        	NvUIDialogManager m_DialogMgr = obj.GetComponent(typeof(NvUIDialogManager)) as NvUIDialogManager;
			DcSuccessDlg dlgS = m_DialogMgr.FindDialog("SuccessDlg")as DcSuccessDlg;
			dlgS.showDialog(true);
		}
		else
		{
			SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        	mgr.AddScene("UI_MyHome_Success");
		}
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void GotoFriend()
	{
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Friend");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
	private void GotoRecordedSongs()
	{
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_RecordedSongs");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
	private void GotoTheLatestSong()
	{
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_TheLatestSong");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
}
