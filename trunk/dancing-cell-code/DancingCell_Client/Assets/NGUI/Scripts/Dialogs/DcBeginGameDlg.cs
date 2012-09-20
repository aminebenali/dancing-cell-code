using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DcBeginGameDlg : NvUIDialogBase 
{
	protected override void OnDlgCreate()
	{
        Debug.Log("OnDlgCreate" + gameObject.name);
	}
	
	protected override void OnDlgInit ()
	{

	}

    //for debug
    protected override void lazyUpdateImpl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.DeleteKey("UserName");
            PlayerPrefs.DeleteKey("UserSex");
        }
    }

    private void GotoMap()
    {
        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        if (PlayerPrefs.HasKey("UserName"))
		{
            mgr.cShareData.strUserName = PlayerPrefs.GetString("UserName");
			mgr.cShareData.eUserSex = (CharSex)PlayerPrefs.GetInt("UserSex");
			//Debug.LogError(mgr.cShareData.eUserSex);
			
            mgr.ChangeScene("UI_Map");
		}
        else
        {
            mgr.ChangeScene("UI_CreateCharacter");
        }
    }
}
