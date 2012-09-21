using UnityEngine;
using System;

public class DcShopDlg : NvUIDialogBase 
{
	public UISprite[] SprMoneyNumber;
    private DcNumber cMoneyNumber = new DcNumber();
    public UISprite[] SprExpNumber;
    private DcNumber cExpNumber = new DcNumber();
	
	public UICheckbox[] ChbSelTag;
	private int nSelTag = 0;
	
	public GameObject[] cGoods;
    private UISprite[] SprGoods = new UISprite[16];
    private UISprite[] SprGoodsBuyBtn = new UISprite[16];
    private UISprite[] SprGoodsBuyText = new UISprite[16];
    private UISprite[] SprGoodsIcon = new UISprite[16];
    private UILabel[] LabGoodsPrice = new UILabel[16];
    private int nSelGoods = 0;
	
	protected override void OnDlgCreate()
	{
        
	}
	
	protected override void OnDlgInit ()
	{
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex, CharModel.eOne,CharForWhat.eChest,true);

        cMoneyNumber.Init(ref SprMoneyNumber, eNumberType.eSilver);
        cExpNumber.Init(ref SprExpNumber);
		
		//test
		cMoneyNumber.ShowNumber(ref SprMoneyNumber, 12345);
        cExpNumber.ShowNumber(ref SprExpNumber, 67890);
		
        for (int i = 0; i < cGoods.Length; i++)
        {
            cGoods[i].SetActiveRecursively(false);
        }
        for (int i = 0; i < 8/*mgr.cShareData.lstFriendsData.Count*/; i++)
        {
            cGoods[i].SetActiveRecursively(true);

            for (int j = 0; j < cGoods[i].transform.childCount; j++)
            {
                GameObject child = cGoods[i].transform.GetChild(j).gameObject;
                if (child.name == "sprItemBg_0")
                {
                    SprGoods[i * 2] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                else if (child.name == "sprItemBg_1")
                {
                    SprGoods[i * 2 + 1] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                if (child.name == "btnBuy_0")
                {
                    SprGoodsBuyBtn[i * 2] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                else if (child.name == "btnBuy_1")
                {
                    SprGoodsBuyBtn[i * 2 + 1] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                if (child.name == "sprBuy_0")
                {
                    SprGoodsBuyText[i * 2] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                else if (child.name == "sprBuy_1")
                {
                    SprGoodsBuyText[i * 2 + 1] = child.GetComponent(typeof(UISprite)) as UISprite;
                }

                if (child.name == "sprItemIcon_0")
                {
                    SprGoodsIcon[i * 2] = child.GetComponent(typeof(UISprite)) as UISprite;
                }
                else if (child.name == "sprItemIcon_1")
                {
                    SprGoodsIcon[i * 2 + 1] = child.GetComponent(typeof(UISprite)) as UISprite;
                } 
                if (child.name == "labItemPrice_0")
                {
                    LabGoodsPrice[i * 2] = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabGoodsPrice[i * 2].text = (i * 2).ToString();
                }
                else if (child.name == "labItemPrice_1")
                {
                    LabGoodsPrice[i * 2 + 1] = child.GetComponent(typeof(UILabel)) as UILabel;
                    LabGoodsPrice[i * 2 + 1].text = (i * 2 + 1).ToString();
                } 
            }
        }
	}
	
	private bool bIsFirst = true;
	private void SelTag(bool bIsCheck)
	{
		if(bIsCheck)
		{
			for(int i = 0; i < ChbSelTag.Length; i++)
			{
				if(ChbSelTag[i].isChecked)
				{
					nSelTag = i;
					ChangeGoods();
					//Debug.LogError("AAAAAAAAAA:"+nSelTag);
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
	
	void ChangeGoods()
	{
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        for (int i = 0; i < cGoods.Length; i++)
        {
            cGoods[i].SetActiveRecursively(false);
        }
        GameObject cObjPrefab = null;
        GameObject cGOPrefab = null;
        int nCurIconsLen = 0;
        switch (nSelTag)
        {
            case 0:
                if (mgr.cShareData.eUserSex == CharSex.eMan)
                {
                    cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/BoyHair", typeof(GameObject)) as GameObject;
                    cGOPrefab = Utility.Instantiate(cObjPrefab) as GameObject;
                    nCurIconsLen = mgr.cShareData.strAvatar_ItemIcon_BoyHair.Length;
                }
                else
                {
                    cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/GirlHair", typeof(GameObject)) as GameObject;
                    cGOPrefab = Utility.Instantiate(cObjPrefab) as GameObject;
                    nCurIconsLen = mgr.cShareData.strAvatar_ItemIcon_GirlHair.Length;
                }
                
                break;
            case 1:
                cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/Face", typeof(GameObject)) as GameObject;
                cGOPrefab = Utility.Instantiate(cObjPrefab) as GameObject;
                nCurIconsLen = mgr.cShareData.strAvatar_ItemIcon_Face.Length;
                break;
            case 2:
                if (mgr.cShareData.eUserSex == CharSex.eMan)
                {
                    cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/BoyClothes", typeof(GameObject)) as GameObject; 
                    cGOPrefab = Utility.Instantiate(cObjPrefab) as GameObject;
                    nCurIconsLen = mgr.cShareData.strAvatar_ItemIcon_BoyClothes.Length;
                }
                else
                {
                    cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/GirlClothes", typeof(GameObject)) as GameObject;
                    cGOPrefab = Utility.Instantiate(cObjPrefab) as GameObject;
                    nCurIconsLen = mgr.cShareData.strAvatar_ItemIcon_GirlClothes.Length;
                }
                break;
            case 3:
                cObjPrefab = Resources.Load("UI Prefabs/Atlases/UI/ItemIcon/Pants", typeof(GameObject)) as GameObject;
                cGOPrefab = Utility.Instantiate(cObjPrefab) as GameObject;
                nCurIconsLen = mgr.cShareData.strAvatar_ItemIcon_Pants.Length;
                break;
            case 4:
                nCurIconsLen = 0;
                break;
        }
        
		for (int i = 0; i < SprGoodsIcon.Length; i++)
        {
            if (i < nCurIconsLen)
            {
                cGoods[i/2].SetActiveRecursively(true);
                SprGoods[i].enabled = true;
                SprGoodsBuyBtn[i].enabled = true;
                SprGoodsBuyText[i].enabled = true;
                SprGoodsIcon[i].enabled = true;
                LabGoodsPrice[i].enabled = true;

                SprGoodsIcon[i].atlas = cGOPrefab.GetComponent(typeof(UIAtlas)) as UIAtlas;

                switch (nSelTag)
                {
                    case 0:
                        if (mgr.cShareData.eUserSex == CharSex.eMan)
                            SprGoodsIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_BoyHair[i];
                        else
                            SprGoodsIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_GirlHair[i];
                        break;
                    case 1:
                        SprGoodsIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_Face[i];
                        break;
                    case 2:
                        if (mgr.cShareData.eUserSex == CharSex.eMan)
                            SprGoodsIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_BoyClothes[i];
                        else
                            SprGoodsIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_GirlClothes[i];
                        break;
                    case 3:
                        SprGoodsIcon[i].spriteName = mgr.cShareData.strAvatar_ItemIcon_Pants[i];
                        break;
                    case 4:
                        //SprGoodsIcon[i].spriteName = "";
                        break;
                }
            }
            else
            {
                SprGoods[i].enabled = false;
                SprGoodsBuyBtn[i].enabled = false;
                SprGoodsBuyText[i].enabled = false;
                SprGoodsIcon[i].enabled = false;
                LabGoodsPrice[i].enabled = false;
            }
        }
	}
	
	void Buy(object sender)
    {
        GameObject obj = sender as GameObject;
        char[] splt = { '_' };
		string strParentLastNum = DcGlobalFunc.GetLastString(obj.transform.parent.name, splt);
        int nParentLastNum = Convert.ToInt32(strParentLastNum) - 1;
        string strLastNum = DcGlobalFunc.GetLastString(obj.transform.name, splt);
        nSelGoods = nParentLastNum*2 + Convert.ToInt32(strLastNum);
        
        Debug.LogError("SSSSSSSSS:"+nSelGoods);

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }
	
    private void ReturnMap()
    {
        Debug.Log("GotoMap");

        showDialog(false);

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		if(mgr.cShareData.nShopEntranceType == 0)
		{
        	mgr.ChangeScene("UI_Map");	
		}
		else if(mgr.cShareData.nShopEntranceType == 1)
		{
        	mgr.ChangeScene("UI_MyHome_Outside");	
		}
		else if(mgr.cShareData.nShopEntranceType == 2)
		{
        	mgr.ChangeScene("UI_MyHome_Interior");	
		}
		else if(mgr.cShareData.nShopEntranceType == 3)
		{
        	mgr.ChangeScene("UI_MyHome_Chest");	
		}
		else if(mgr.cShareData.nShopEntranceType == 4)
		{
        	mgr.ChangeScene("UI_MyHome_Dresser");
		}
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
}
