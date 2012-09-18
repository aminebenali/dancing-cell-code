using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DcTaskDlg : NvUIDialogBase 
{
    public UISprite[] SprMoneyNumber;
    private DcNumber cMoneyNumber = new DcNumber();
    public UISprite[] SprExpNumber;
    private DcNumber cExpNumber = new DcNumber();

    public UICheckbox[] ChbSelTask;
    private int nSelTaskTag = 0;

    public UILabel LabDescribe;

	protected override void OnDlgCreate()
	{
        cMoneyNumber.Init(ref SprMoneyNumber, true);
        cExpNumber.Init(ref SprExpNumber);
	}
	
	protected override void OnDlgInit ()
	{

	}

    private bool bIsFirst = true;
    private void SelTask(bool bIsCheck)
    {
        if (bIsCheck)
        {
            for (int i = 0; i < ChbSelTask.Length; i++)
            {
                if (ChbSelTask[i].isChecked)
                {
                    nSelTaskTag = i;
                    ChangeCurShowTask();
                    Debug.LogError("TTTTTTTTTTT:" + nSelTaskTag);
                    break;
                }
            }

            if (bIsFirst)
            {
                bIsFirst = false;
                return;
            }

            NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
            soundctr.PlaySe("ui_check");
        }
    }

    void ChangeCurShowTask()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        LabDescribe.text = mgr.cShareData.lstTasksData[nSelTaskTag].strTaskDescribe;
        cMoneyNumber.ShowNumber(ref SprMoneyNumber, mgr.cShareData.lstTasksData[nSelTaskTag].nRewardMoney);
        cExpNumber.ShowNumber(ref SprExpNumber, mgr.cShareData.lstTasksData[nSelTaskTag].nRewardExp);
    }

    private void ReturnMap()
    {
        Debug.Log("GotoMap");

        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Map");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
}
