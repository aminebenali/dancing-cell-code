using UnityEngine;
using System;

public class DcRecordedSongsDlg : NvUIDialogBase 
{
	public UIPanel PanRecordedIn;
	public UIPanel PanRecordingFinished;
	public GameObject[] cFriends;
	
	protected override void OnDlgCreate()
	{
		
	}
	
	protected override void OnDlgInit ()
	{
		PanRecordedIn.gameObject.SetActiveRecursively(false);
		PanRecordingFinished.gameObject.SetActiveRecursively(false);
		
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		for (int i = 0; i < cFriends.Length; i++)
		{
			//cFriends[i].SetActiveRecursively(false);
		}
//		for (int i = 0; i < mgr.cShareData.lstRoomsData.Count; i++)
//		{
//			cFriends[i].SetActiveRecursively(true);
//			
//			for(int j = 0; j < cFriends[i].transform.childCount; j++)
//			{
//				GameObject child = cFriends[i].transform.GetChild(j).gameObject;
//				if(child.name == "LabName")
//				{
//					UILabel LabName = child.GetComponent(typeof(UILabel)) as UILabel;
//					LabName.text = /*cLocalization.GetValue("RoomName") + */mgr.cShareData.lstRoomsData[i].strRoomName;
//				}
//				else if(child.name == "LabMode")
//				{
//					UILabel LabMode = child.GetComponent(typeof(UILabel)) as UILabel;
//                    LabMode.text = /*cLocalization.GetValue("DancingMode") + */cLocalization.GetValue(mgr.cShareData.lstRoomsData[i].strMode);
//				}
//				
//			}
//		}
	}
	
    private void ReturnInterior()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Interior");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }

    private void ShowRecordedInPanel()
	{
		PanRecordedIn.gameObject.SetActiveRecursively(true);
		
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
    private void HideRecordedInPanel()
    {
		PanRecordedIn.gameObject.SetActiveRecursively(false);
		
        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void ShowRecordingFinishedPanel()
	{
		PanRecordingFinished.gameObject.SetActiveRecursively(true);
        HideRecordedInPanel();
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
    private void HideRecordingFinishedPanel()
    {
		PanRecordingFinished.gameObject.SetActiveRecursively(false);
        HideRecordedInPanel();
		
        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
}
