using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DcSuccessDlg : NvUIDialogBase 
{
	protected override void OnDlgCreate()
	{
        
	}
	
	protected override void OnDlgInit ()
	{
		
	}

    private void ReturnInterior()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Interior");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }
	
	private void GetReward()
    {
        
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void Share()
    {
        
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
}
