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
        showDialog(false);
		
        GameObject obj = GameObject.Find("Interior");
		if(obj)
		{
        	NvUIDialogManager m_DialogMgr = obj.GetComponent(typeof(NvUIDialogManager)) as NvUIDialogManager;
			DcInteriorDlg dlgI = m_DialogMgr.FindDialog("InteriorDlg")as DcInteriorDlg;
			dlgI.showDialog(true);
		}
		
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
