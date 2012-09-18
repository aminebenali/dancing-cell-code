using UnityEngine;
using System;

public class DcTheLatestSongDlg : NvUIDialogBase 
{
	public GameObject[] cSongs;
    public GameObject[] cBeFriend = new GameObject[8];
    public GameObject[] cSeize = new GameObject[8];
    public GameObject[] cHaveTake = new GameObject[8];
	
	protected override void OnDlgCreate()
	{
		
	}
	
	protected override void OnDlgInit ()
	{
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		for (int i = 0; i < cSongs.Length; i++)
		{
            cSongs[i].SetActiveRecursively(false);
		}
        for (int i = 0; i < mgr.cShareData.lstTheLatestSongsData.Count; i++)
        {
            cSongs[i].SetActiveRecursively(true);

            for (int j = 0; j < cSongs[i].transform.childCount; j++)
            {
                GameObject child = cSongs[i].transform.GetChild(j).gameObject;
                if (child.name == "BeFriend")
                {
                    cBeFriend[i] = child;
                    cBeFriend[i].SetActiveRecursively(!Convert.ToBoolean(mgr.cShareData.lstTheLatestSongsData[i].nIsMySelf));
                   // LabName.text = /*cLocalization.GetValue("RoomName") + */mgr.cShareData.lstRoomsData[i].strRoomName;
                }
                else if (child.name == "Seize")
                {
                    cSeize[i] = child;
                    cSeize[i].SetActiveRecursively(!Convert.ToBoolean(mgr.cShareData.lstTheLatestSongsData[i].nIsMySelf));
                }
                else if (child.name == "HaveTake")
                {
                    cHaveTake[i] = child;
                    cHaveTake[i].SetActiveRecursively(Convert.ToBoolean(mgr.cShareData.lstTheLatestSongsData[i].nIsMySelf));
                }
                else if (child.name == "LabSongName")
                {
                    UILabel LabSongName = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabSongName.text = mgr.cShareData.lstTheLatestSongsData[i].strSongName;
                }
                else if (child.name == "LabOwner")
                {
                    UILabel LabOwner = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabOwner.text = Convert.ToBoolean(mgr.cShareData.lstTheLatestSongsData[i].nIsMySelf) ? "" : mgr.cShareData.lstTheLatestSongsData[i].strOwner;
                }
            }
        }
	}
	
	void BeFriend()
	{
		Debug.LogError("BeFriend");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
	void Seize()
	{
		Debug.LogError("Seize");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
	void HaveTake()
	{
		Debug.LogError("HaveTake");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
    private void ReturnInterior()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Interior");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
}
