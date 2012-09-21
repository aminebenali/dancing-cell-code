using UnityEngine;
using System;

public class DcChestDlg : NvUIDialogBase 
{
	public UISprite[] SprMoneyNumber;
    private DcNumber cMoneyNumber = new DcNumber();
    public UISprite[] SprExpNumber;
    private DcNumber cExpNumber = new DcNumber();

    public GameObject[] cItems;
    private UISprite[] SprItemBg = new UISprite[10];
    private UISprite[] SprItemIcon = new UISprite[10];

    private int nSelItem = 0;
	
	protected override void OnDlgCreate()
	{
        cMoneyNumber.Init(ref SprMoneyNumber, eNumberType.eSilver);
        cExpNumber.Init(ref SprExpNumber);
		
		//test
		cMoneyNumber.ShowNumber(ref SprMoneyNumber, 12345);
        cExpNumber.ShowNumber(ref SprExpNumber, 67890);
	}
	
	protected override void OnDlgInit ()
	{
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        for (int i = 0; i < cItems.Length; i++)
        {
            cItems[i].SetActiveRecursively(false);
        }
        for (int i = 0; i < 5/*mgr.cShareData.lstFriendsData.Count*/; i++)
        {
            cItems[i].SetActiveRecursively(true);

            for (int j = 0; j < cItems[i].transform.childCount; j++)
            {
                GameObject child = cItems[i].transform.GetChild(j).gameObject;
                if (child.name == "sprItemBg_0")
                {
                    SprItemBg[i * 2] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                else if (child.name == "sprItemBg_1")
                {
                    SprItemBg[i * 2 + 1] = child.GetComponent(typeof(UISprite)) as UISprite;
                } 
                else if (child.name == "sprItemIcon_0")
                {
                    SprItemIcon[i * 2] = child.GetComponent(typeof(UISprite)) as UISprite;
                    SprItemIcon[i * 2].spriteName = "Head portrait girl 01";
                }
                else if (child.name == "sprItemIcon_1")
                {
                    SprItemIcon[i * 2 + 1] = child.GetComponent(typeof(UISprite)) as UISprite;
                    SprItemIcon[i * 2 + 1].spriteName = "Head portrait girl 03";
                }
            }
        }

        TempInitClothesAndPantsIcon(0);
        TempInitClothesAndPantsIcon(5);

        DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex, CharModel.eOne, CharForWhat.eChest,true);
	}

    void TempInitClothesAndPantsIcon(int nSelTag)
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        GameObject cObjPrefab = null;
        GameObject cGOPrefab = null;
        int nCurIconsLen = 5;
        switch (nSelTag)
        {
            case 0:
                if (mgr.cShareData.eUserSex == CharSex.eMan)
                {
                    cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/BoyClothes", typeof(GameObject)) as GameObject;
                }
                else
                {
                    cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/GirlClothes", typeof(GameObject)) as GameObject;
                }
                break;
            case 5:
                cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/Pants", typeof(GameObject)) as GameObject;
                break;
        }
        cGOPrefab = Utility.Instantiate(cObjPrefab) as GameObject;

        for (int i = 0+nSelTag; i < nCurIconsLen+nSelTag; i++)
        {   
            SprItemBg[i].enabled = true;
            SprItemIcon[i].enabled = true;

            SprItemIcon[i].atlas = cGOPrefab.GetComponent(typeof(UIAtlas)) as UIAtlas;

            switch (nSelTag)
            {
                case 0:
                    if (mgr.cShareData.eUserSex == CharSex.eMan)
                        SprItemIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_BoyClothes[i];
                    else
                        SprItemIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_GirlClothes[i];
                    break;
                case 5:
                    SprItemIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_Pants[i - nSelTag];
                    break;
            }
        }
    }

    Vector3 GetMousePos()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            return new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Touch input = Input.GetTouch(0);

            //Debug.Log("==========> GetTouchPoint: input.position:  " + input.position);
            return new Vector3(input.position.x, input.position.y, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }

    void SelItem(object sender)
    {
        GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string lastnum = DcGlobalFunc.GetLastString(obj.transform.name, splt);
        nSelItem = Convert.ToInt32(lastnum) - 1;
        int nItemIdx = 0;
        if (GetMousePos().y > 357)
        {
            nItemIdx = 0;
        }
        else if (GetMousePos().y < 327)
        {
            nItemIdx = 1;
        }

        nSelItem = nSelItem * 2 + nItemIdx;
        for (int i = 0; i < SprItemBg.Length; i++)
        {
            SprItemBg[i].spriteName = "Item Bg S1";
        }
        SprItemBg[nSelItem].spriteName = "Item Bg S2";

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }
	
	private void GotoDress()
	{
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
	}
	
    private void ReturnInterior()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Interior");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
	
	private void GotoShop()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_Shop");
		mgr.cShareData.nShopEntranceType = 3;
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }
}
