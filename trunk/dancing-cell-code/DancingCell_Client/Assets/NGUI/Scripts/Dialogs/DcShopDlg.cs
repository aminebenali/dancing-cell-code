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
    private UISprite[] SprGoodsIcon = new UISprite[16];
	private UILabel[] SprGoodsPrice = new UILabel[16];
    private int nSelGoods = 0;
	
	protected override void OnDlgCreate()
	{
        
	}
	
	protected override void OnDlgInit ()
	{
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex, CharModel.eOne,CharForWhat.eChest,true);
		
		cMoneyNumber.Init(ref SprMoneyNumber,true);
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
                    SprGoodsPrice[i * 2] = child.GetComponent(typeof(UILabel)) as UILabel;
					SprGoodsPrice[i * 2].text = (i*2).ToString();
                }
                else if (child.name == "labItemPrice_1")
                {
                    SprGoodsPrice[i * 2 + 1] = child.GetComponent(typeof(UILabel)) as UILabel;
					SprGoodsPrice[i * 2 + 1].text = (i*2+1).ToString();
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
					Debug.LogError("AAAAAAAAAA:"+nSelTag);
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
