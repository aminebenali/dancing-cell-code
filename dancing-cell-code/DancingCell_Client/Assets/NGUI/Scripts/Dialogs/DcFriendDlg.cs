using UnityEngine;
using System;

public class DcFriendDlg : NvUIDialogBase 
{
	public UIPanel PanMessage;
	public UIPanel PanFriendHome;
	public GameObject[] cFriends;

    public BoxCollider BoxAddFriend;
    public UISprite SprAddFriend;

    private UISprite[] SprBgOg = new UISprite[5];
	private UISprite[] SprIcon = new UISprite[5];
	
	public UISprite SprMsgFriendIcon;
	public UILabel LabMsgFriendName;
	public UILabel LabMsgFriendLv;

	protected override void OnDlgCreate()
	{
		
	}
	
	protected override void OnDlgInit ()
	{
		PanMessage.gameObject.SetActiveRecursively(false);
		PanFriendHome.gameObject.SetActiveRecursively(false);

        //BoxAddFriend.size = new Vector3(0, 0, 0);
        //SprAddFriend.color = new Color(1, 0, 0);

		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		for (int i = 0; i < cFriends.Length; i++)
		{
			cFriends[i].SetActiveRecursively(false);
		}
        for (int i = 0; i < mgr.cShareData.lstFriendsData.Count; i++)
        {
            cFriends[i].SetActiveRecursively(true);

            for (int j = 0; j < cFriends[i].transform.childCount; j++)
            {
                GameObject child = cFriends[i].transform.GetChild(j).gameObject;
				if (child.name == "sprBG")
                {
                    UISprite SprBg = child.GetComponent(typeof(UISprite)) as UISprite;
                    SprBg.spriteName = mgr.cShareData.lstFriendsData[i].nFriendSex == 0 ? "Friend Boy Bg" : "Friend Girl Bg";
					//Debug.LogError(SprBg.spriteName);
                }
                else if (child.name == "sprBgOg")
                {
                    SprBgOg[i] = child.GetComponent(typeof(UISprite)) as UISprite;
					SprBgOg[i].spriteName = mgr.cShareData.lstFriendsData[i].nFriendSex == 0 ? "Friend Boy Og" : "Friend Girl Og";
                    SprBgOg[i].enabled = false;
                }
				else if (child.name == "sprIcon")
                {
                    SprIcon[i] = child.GetComponent(typeof(UISprite)) as UISprite;
                    if (mgr.cShareData.lstFriendsData[i].nFriendSex == 0)
                    {
                        SprIcon[i].spriteName = mgr.cShareData.strManCharIcon[(int)UnityEngine.Random.Range(0, mgr.cShareData.strManCharIcon.Length)];
                    }
                    else
                    {
                        SprIcon[i].spriteName = mgr.cShareData.strWomanCharIcon[(int)UnityEngine.Random.Range(0, mgr.cShareData.strWomanCharIcon.Length)];
                    }
                }
                else if (child.name == "LabName")
                {
                    UILabel LabName = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabName.text = mgr.cShareData.lstFriendsData[i].strFriendName;
                }
				else if (child.name == "LabHasSongsNum")
                {
                    UILabel LabHasSongsNum = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabHasSongsNum.text = mgr.cShareData.lstFriendsData[i].nHasMusic.ToString();
                }
				else if (child.name == "LabLv")
                {
                    UILabel LabLv = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabLv.text = mgr.cShareData.lstFriendsData[i].nFriendLv.ToString();
                }
				else if (child.name == "nDancingCnt")
                {
                    UILabel nDancingCnt = child.GetComponent(typeof(UILabel)) as UILabel;
                    nDancingCnt.text = mgr.cShareData.lstFriendsData[i].nDancingCnt.ToString();
                }
            }
        }
	}

    void SelOneFriend(object sender)
    {
        GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string lastnum = DcGlobalFunc.GetLastString(obj.transform.name, splt);
        int nSelFriend = Convert.ToInt32(lastnum) - 1;

        for (int i = 0; i < SprBgOg.Length; i++)
        {
            SprBgOg[i].enabled = false;
        }
        SprBgOg[nSelFriend].enabled = true;

        //BoxAddFriend.size = new Vector3(1, 1, 1);
        //SprAddFriend.color = new Color(1, 1, 1);
    }

    private void ReturnInterior()
    {
        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Interior");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }

    private void ShowFriendHomePanel(object sender)
	{
		//PanFriendHome.gameObject.SetActiveRecursively(true);
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
    private void HideFriendHomePanel()
    {
		PanFriendHome.gameObject.SetActiveRecursively(false);
		
        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void ShowMessagePanel(object sender)
	{
		PanMessage.gameObject.SetActiveRecursively(true);
		
		GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string lastnum = DcGlobalFunc.GetLastString(obj.transform.parent.name, splt);
        int nSelFriend = Convert.ToInt32(lastnum) - 1;
		
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		mgr.cShareData.cCurFriendData = mgr.cShareData.lstFriendsData[nSelFriend];
		
		SprMsgFriendIcon.spriteName = SprIcon[nSelFriend].spriteName;
		LabMsgFriendName.text = mgr.cShareData.cCurFriendData.strFriendName;
		LabMsgFriendLv.text = mgr.cShareData.cCurFriendData.nFriendLv.ToString();
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
    private void HideMessagePanel()
    {
		PanMessage.gameObject.SetActiveRecursively(false);
		
        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
}
