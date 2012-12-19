using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DcOutsideDlg : NvUIDialogBase 
{
	protected override void OnDlgCreate()
	{
        
	}
	
	protected override void OnDlgInit ()
	{

	}

    private void ReturnMap()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Map");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void GotoInterior()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Interior");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }
	
	private void GotoShop()
	{
		showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Shop");
		mgr.cShareData.nShopEntranceType = 1;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
	}
}
