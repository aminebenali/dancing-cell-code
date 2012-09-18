using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class RewardShowItem
{
	public UISprite  m_Icon = null ;
	
	public UILabel   m_ShowItemLab = null ;
	
	[HideInInspector]
	public int num = 0;
	
	[HideInInspector]
	public string name = "";
	
	[HideInInspector]
	public string  m_ShowItemRes = "";
	
	public void Apply()
	{
		if(m_Icon == null)
		{
			//Debug.LogError(" m_Icon == null  ");
			return;
		}
		if(num <= 0 )
		{
			//Debug.LogError(" num <= 0  m_Icon:  "+m_Icon.transform.parent.name);
			
			m_Icon.gameObject.active = false;
			
			m_ShowItemLab.gameObject.active = false;
		}
		else 
		{
			//Debug.LogError(" m_ShowItemLab:  name"+name+"  m_ShowItemRes: "+m_ShowItemRes+" m_ShowItemLab.objname: "+m_Icon.transform.parent.name);
		
			m_ShowItemLab.gameObject.active = true;
			
			if(m_ShowItemRes != "")
			{
				m_Icon.gameObject.active = true;
				
				m_Icon.spriteName = m_ShowItemRes;
			}
			m_ShowItemLab.text = name+"+"+num.ToString();
		}
	}
}


[System.Serializable]
public class CommonRewardItem
{
	public enum CommonRewardType
	{
		EXP,
		GLOD,
		MCOIN,
		MATURITY
	}
	public UILabel                m_Lab = null;
	public UISprite               m_Coin = null;
	
	public string                              m_TypeName="";
	public string              				    m_SpriteName = "";
	
	private int                     m_num=0;
	
	public void SetNum(int newnum)
	{
		m_num = newnum;
	}
	
	public void Apply()
	{
		//Debug.LogWarning(" type: "+m_TypeName+"  num: "+m_num+"    m_Lab:  "+m_Lab.transform.parent.name+"/"+m_Lab.name);
		
		if(m_num>0)
		{
			m_Coin.gameObject.active = true;
			
			m_Coin.spriteName = m_SpriteName;
			
			m_Coin.transform.localScale = new Vector3(30,30,0);
			
			m_Lab.text = m_TypeName+"+"+m_num;
			
		//	Debug.LogError("text:   "+m_Lab.text);
		}
		else
		{
			//Debug.LogWarning(" type: "+m_TypeName+"  num==0 ");
			
			m_Lab.gameObject.active = false;
			
			m_Coin.gameObject.active = false;
		}
	}
}

[System.Serializable]
public class NvUIBattleReward : NvUIDialogBase 
{
	
	public RewardShowItem[]   m_RewardShowItemList = new RewardShowItem[4];
	
	/*
	public UILabel                m_ExpLab = null;
	public UISprite               m_ExpCoin = null;
	
	public UILabel                m_GoldLab = null;
	public UISprite                m_GoldCoin = null;
	
	public UILabel                m_McoinLab = null;
	public UISprite                m_McoinCoin = null;
	
	public UILabel                m_MaturityLab = null;
	public UISprite                m_MaturityCoin = null;
	*/
	/*
 		EXP,
		MATURITY,
		GOLD,
		ITEM,
 */
	public CommonRewardItem[]    m_CommonRewardItemList = new CommonRewardItem[4];

	public UIButtonMessage       m_ContinueBt ;
	public UIButtonMessage       m_OkBt ;
	public UIButtonMessage       m_BackToVillageBt ;
	public UIButtonMessage       m_BackToAreaBt ;
	
	public Transform                 m_LeftBtPos;
	public Transform                 m_RightBtPos;
	public Transform                 m_MidBtPos;
	
	//public FriendFrame              m_FriendFrame;
	
	public UISprite                     m_HelpSprite;
	
	public UISprite                     m_ApplyFriend;
	
	public UILabel                      m_ApplyResult;
	
	public UISprite                     m_NoHelpSprite;
	
	public UISprite                     m_DownPartLine;
	
	public delegate void             ButHandler();   
	
	public                             ButHandler EventOnBackToVillage;
	
	public                             ButHandler EventOnBackToArea;
	
	public                             ButHandler EventOnContinue;
	
	public                             ButHandler EventOnOk;
	
	private bool                    m_IsEnableContinue = true;
	
	private bool                    m_IsEnableQuit = true;
	
	//private                           UserBaseData m_Helperdata=null;
	
	private bool                    m_IsHaveAlreadyRequestFriend = false;
	
	//private bool                    m_IsCanApplyFriend = false;
	
	private bool                    m_IsApplyFriend = false;
	
	private bool                    m_IsCanPressBut = true;
	
	//private NvTutorial mTutorial;
	
	protected override void OnDlgCreate()
	{
	}
	
	protected override void OnDlgInit ()
	{
	}
	
	private void WaitToClose()
	{
		StartCoroutine(DoWaitToClose());
	}
	
	IEnumerator DoWaitToClose()
	{
		yield return new WaitForSeconds(2.0f);
		
		closeDialog();
	}
	protected override void OnDlgBeginEffect (EDialogEffect effect)
	{
		if (effect == NvUIDialogBase.EDialogEffect.eDE_OpenEffect) 
		{
			Apply();
		}
	}
	protected override void OnDlgEndEffect( EDialogEffect effect ) 
	{
		
	}
	
	private void Apply()
	{
		foreach(RewardShowItem showitem in  m_RewardShowItemList)
		{
			showitem.Apply();
		}
		foreach(CommonRewardItem item in m_CommonRewardItemList)
		{
			item.Apply();
		}
		
		m_ApplyFriend.color = Color.white;
				
		//NvBattleManager batmrg = Singlton.getInstance("NvBattleManager")as NvBattleManager;
		
		//if(batmrg.GetBattleType() == BattleMode.PkBattle)
		{

			//mTutorial = NvTutorialManager.getInstance().CurrentTutorial;
			//mTutorial.ActiveTutorialScene();			
			
			m_DownPartLine.gameObject.active = true;
			
			m_NoHelpSprite.gameObject.active = false;
		
//			Vector3 pos = m_ContinueBt.gameObject.transform.localPosition ;
//				
//			pos.y =-110;
//				
//			 m_ContinueBt.gameObject.transform.localPosition = pos;
//				
//			pos = m_CancelBt.gameObject.transform.localPosition ;
//				
//			pos.y =-110;
//				
//			m_CancelBt.gameObject.transform.localPosition = pos;
//			
//			pos = m_BackToAreaBt.gameObject.transform.localPosition ;
//				
//			pos.y =-110;
//				
//			m_BackToAreaBt.gameObject.transform.localPosition = pos;
				
//			if(m_Helperdata!=null && m_Helperdata.school>0)
//			{
//				if(m_IsCanApplyFriend)
//				{
//					m_ApplyFriend.gameObject.active = true;
//					
//				}
//				else
//				{
//					m_ApplyFriend.gameObject.active = false;
//				}
//				if(m_IsApplyFriend)
//				{
//					m_ApplyResult.gameObject.active = true;
//				}
//				else
//				{
//					m_ApplyResult.gameObject.active = false;
//				}
//				m_HelpSprite.gameObject.active = true;
//				
//				//m_FriendFrame.gameObject.SetActiveRecursively(true);
//				
//				//m_FriendFrame.SetFriendDetailInBattle(m_Helperdata);
//				
//				RequestAllFriends();
//			}
//			else
//			{
//				m_NoHelpSprite.gameObject.active = true;
//				
//				m_HelpSprite.gameObject.active = false;
//				
//				m_ApplyResult.gameObject.active = false;
//				
//				m_ApplyFriend.gameObject.active = false;
//				
//				//m_FriendFrame.gameObject.SetActiveRecursively(false);
//			}
			
		}
//		else
//		{
//			m_DownPartLine.gameObject.active = false;
//				
////			Vector3 pos = m_ContinueBt.gameObject.transform.localPosition ;
////				
////			pos.y =-35;
////				
////			 m_ContinueBt.gameObject.transform.localPosition = pos;
////				
////			pos = m_CancelBt.gameObject.transform.localPosition ;
////				
////			pos.y =-35;
////				
////			m_CancelBt.gameObject.transform.localPosition = pos;
////			
////			pos = m_BackToAreaBt.gameObject.transform.localPosition ;
////				
////			pos.y =-35;
////				
////			m_BackToAreaBt.gameObject.transform.localPosition = pos;
//			
//			m_NoHelpSprite.gameObject.active = false;
//		
//			m_ApplyFriend.gameObject.active = false;
//			
//			m_ApplyResult.gameObject.active = false;
//			
//			m_HelpSprite.gameObject.active = false;
//				
//			m_FriendFrame.gameObject.SetActiveRecursively(false);
//		}
		ApplyBackGroupBtn();
		
		m_IsCanPressBut = true;
	}
	
	
	private void ApplyBackGroupBtn()//ok continue toarea tovillage
	{
		if(!m_IsEnableQuit && !m_IsEnableContinue)
		{
			Debug.LogError("Check it!!  !m_IsEnableQuit && !m_IsEnableContinue");
		}
		//NvBattleManager battlemgr = Singlton.getInstance("NvBattleManager")as NvBattleManager;
		
		//if(battlemgr.GetBattleType() == BattleMode.PkBattle)
		{
			m_ContinueBt.gameObject.SetActiveRecursively(false);
			
			m_BackToAreaBt.gameObject.SetActiveRecursively(false);
			
			m_BackToVillageBt.gameObject.SetActiveRecursively(false);
			
			m_OkBt.gameObject.SetActiveRecursively(true);
			
			m_OkBt.transform.position = m_MidBtPos.position;
		}
		//else
		{
			if(!m_IsEnableQuit)
			{
				m_ContinueBt.gameObject.SetActiveRecursively(true);
			
				m_BackToAreaBt.gameObject.SetActiveRecursively(false);
				
				m_BackToVillageBt.gameObject.SetActiveRecursively(false);
				
				m_OkBt.gameObject.SetActiveRecursively(false);
				
				m_ContinueBt.transform.position = m_MidBtPos.position;
			}
			else if(!m_IsEnableContinue)
			{
				m_ContinueBt.gameObject.SetActiveRecursively(false);
			
				m_BackToAreaBt.gameObject.SetActiveRecursively(true);
				
				m_BackToVillageBt.gameObject.SetActiveRecursively(true);
				
				m_OkBt.gameObject.SetActiveRecursively(false);
				
				m_BackToVillageBt.transform.position = m_LeftBtPos.position;
				
				m_BackToAreaBt.transform.position = m_RightBtPos.position;
			}
			else
			{
				m_ContinueBt.gameObject.SetActiveRecursively(true);
			
				m_BackToAreaBt.gameObject.SetActiveRecursively(false);
				
				m_BackToVillageBt.gameObject.SetActiveRecursively(true);
				
				m_OkBt.gameObject.SetActiveRecursively(false);
				
				m_BackToVillageBt.transform.position = m_LeftBtPos.position;
				
				m_ContinueBt.transform.position = m_RightBtPos.position;
			}
			//if(battlemgr.GetBattleType() == BattleMode.PkBattle)
			{
				Vector3 pos = m_ContinueBt.gameObject.transform.localPosition ;
				
				pos.y =-110;
					
				m_ContinueBt.gameObject.transform.localPosition = pos;
					
				pos = m_OkBt.gameObject.transform.localPosition ;
					
				pos.y =-110;
					
				m_OkBt.gameObject.transform.localPosition = pos;
				
				pos = m_BackToVillageBt.gameObject.transform.localPosition ;
					
				pos.y =-110;
					
				m_BackToVillageBt.gameObject.transform.localPosition = pos;
				
				pos = m_BackToAreaBt.gameObject.transform.localPosition ;
					
				pos.y =-110;
					
				m_BackToAreaBt.gameObject.transform.localPosition = pos;
			}
			//else
			{
				Vector3 pos = m_ContinueBt.gameObject.transform.localPosition ;
				
				pos.y =-35;
					
				m_ContinueBt.gameObject.transform.localPosition = pos;
					
				pos = m_OkBt.gameObject.transform.localPosition ;
					
				pos.y =-35;
					
				m_OkBt.gameObject.transform.localPosition = pos;
				
				pos = m_BackToVillageBt.gameObject.transform.localPosition ;
					
				pos.y =-35;
					
				m_BackToVillageBt.gameObject.transform.localPosition = pos;
				
				pos = m_BackToAreaBt.gameObject.transform.localPosition ;
					
				pos.y =-35;
					
				m_BackToAreaBt.gameObject.transform.localPosition = pos;
			}
		}
//		if(!m_IsEnableContinue)
//		{
//			Debug.LogError("m_IsEnableContinue:  "+m_IsEnableContinue+"  m_IsEnableQuit: "+m_IsEnableQuit);
//			
//			m_ContinueBt.gameObject.SetActiveRecursively(false);
//			
//			m_BackToAreaBt.gameObject.SetActiveRecursively(true);
//			
//			Vector3 pos = m_BackToAreaBt.transform.position;
//				
//			pos.x = m_RightBtPos.position.x;
//			
//			m_BackToAreaBt.gameObject.transform.position = pos;
//		}
//		else
//		{
//			Debug.LogError("m_IsEnableContinue:  "+m_IsEnableContinue+"  m_IsEnableQuit: "+m_IsEnableQuit);
//			
//			m_ContinueBt.gameObject.SetActiveRecursively(true);
//			
//			m_BackToAreaBt.gameObject.SetActiveRecursively(false);
//			
//			if(m_IsEnableQuit)
//			{
//				Debug.LogError("m_IsEnableContinue:  "+m_IsEnableContinue+"  m_IsEnableQuit: "+m_IsEnableQuit);
//			
//				Vector3 pos = m_ContinueBt.transform.position;
//				
//				pos.x = m_RightBtPos.position.x;
//				
//				m_ContinueBt.transform.position = pos;
//			}
//			else
//			{
//				Debug.LogError("m_IsEnableContinue:  "+m_IsEnableContinue+"  m_IsEnableQuit: "+m_IsEnableQuit);
//			
//				Vector3 pos = m_ContinueBt.transform.position;
//				
//				pos.x = m_MidBtPos.position.x;
//				
//				m_ContinueBt.transform.position = pos;
//			}
//		}
//		if(!m_IsEnableQuit)
//		{
//			Debug.LogError("m_IsEnableContinue:  "+m_IsEnableContinue+"  m_IsEnableQuit: "+m_IsEnableQuit);
//			
//			m_CancelBt.gameObject.SetActiveRecursively(false);
//		}
//		else
//		{
//			Debug.LogError("m_IsEnableContinue:  "+m_IsEnableContinue+"  m_IsEnableQuit: "+m_IsEnableQuit);
//			
//			m_CancelBt.gameObject.SetActiveRecursively(true);
//			
//			Vector3 pos = m_CancelBt.transform.position;
//				
//			pos.x = m_LeftBtPos.position.x;
//				
//			m_CancelBt.transform.position = pos;
//		}
	}
	
	private void OnBackToVillage()
	{
		Debug.LogWarning("OnCancel");
		
		if(isOpened &&m_IsCanPressBut)
		{
			Debug.LogWarning("!!!!!   closeDialog");
			closeDialog();
		
			EventOnBackToVillage();
			
			m_IsCanPressBut = false;
		}
	}
	
	private void OnBackToArea()
	{
		Debug.LogWarning("OnCancel");
		
		if(isOpened &&m_IsCanPressBut)
		{
			Debug.LogWarning("!!!!!   closeDialog");
			
			closeDialog();
		
			EventOnBackToArea();
			
			m_IsCanPressBut = false;
		}
	}
	
	private void OnContinue()
	{
		//Debug.LogWarning("@@@@@@@@@@@@@OnContinuev     m_IsCanPressBut:  "+m_IsCanPressBut+"  m_IsEnableContinue:  "+m_IsEnableContinue);
		
		if(isOpened && m_IsCanPressBut)
		{
			if(m_IsEnableContinue)
			{
				//Debug.LogWarning("@@@@@@@@@@@@@!!!!!   closeDialog");
				
				closeDialog();
			
				EventOnContinue();
			}
			m_IsCanPressBut = false;
		}
	}
	
	private void OnOk()//just for pk
	{
		Debug.LogWarning("OnCancel");
		
		if(isOpened &&m_IsCanPressBut)
		{
			Debug.LogWarning("!!!!!   closeDialog");
			
			closeDialog();
		
			EventOnOk();
			
			m_IsCanPressBut = false;
		}
	}
	
//	public void SetReward(MsgBase msg, int exp,int gold,int mcoin,int maturity,List<RewardItemBase> m_showItemList,UserBaseData userdata = null)
//	{
//		NvLocalizationManager localMgr = Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager;
//
//		m_CommonRewardItemList[(int)CommonRewardItem.CommonRewardType.EXP].SetNum(exp);
//		
//		m_CommonRewardItemList[(int)CommonRewardItem.CommonRewardType.GLOD].SetNum(gold);
//		
//		m_CommonRewardItemList[(int)CommonRewardItem.CommonRewardType.MCOIN].SetNum(mcoin);
//		
//		m_CommonRewardItemList[(int)CommonRewardItem.CommonRewardType.MATURITY].SetNum(maturity);
//		
//		m_Helperdata = userdata;
//		
//		int i=0;
//		
//		foreach(RewardItemBase item in m_showItemList )
//		{	
//			if(i<m_RewardShowItemList.Length)
//			{
//				m_RewardShowItemList[i].num = item.amount;
//				
//				CommonRes resitem = msg.GetCommRes(item.item_id.ToString() ,"0");
//				
//				string name = msg.GetDictName(item.item_id);
//				
//				Debug.Log("====>  m_showItemList:  name:  "+ name);
//				
//				if(resitem!=null)
//				{
//					m_RewardShowItemList[i].m_ShowItemRes = resitem.res_filename;
//				}
//				m_RewardShowItemList[i].name = name;
//					
//				i++;
//			}
//		}
//		//Apply();
//	}
	
	public void SetEnableContinue(bool enable)
	{
		m_IsEnableContinue = enable;
	}
	
	public void SetEnableQuit(bool enable)
	{
		m_IsEnableQuit = enable;
	}
	
	public void ApplyFriend()
	{
		if(isOpened &&m_IsCanPressBut)
		{
			if(!m_IsApplyFriend)
			{
				//FriendsSceneModel friendmod = m_FriendFrame.GetComponent<FriendsSceneModel>();
				
				//friendmod.RequestFriend(m_Helperdata.player_id.ToString(),ApplyOk);
				
				m_ApplyResult.gameObject.active = true;
					
				//m_ApplyResult.text =  (Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager).GetValue("BattleRewardFriendApplying");
				
				m_IsApplyFriend = true;
				
				//mTutorial.TutorialChkInside ( "ApplyFriend" );
			}
		}
	}
	
	private void RequestAllFriends()
	{
		if(!m_IsHaveAlreadyRequestFriend)
		{
			m_IsHaveAlreadyRequestFriend = true;
			
			//FriendsSceneModel friendmod = m_FriendFrame.GetComponent<FriendsSceneModel>();
			
			//friendmod.RequesAllFriendList(ReceiveAllFriends);
			
			(Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager).ShowLoadingIndicator( true );
		}
	}
	
	private void ReceiveAllFriends()
	{
		(Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager).ShowLoadingIndicator( false );
		
//		if(!IsMyFriend(m_Helperdata.player_id))
//		{
//			m_ApplyFriend.gameObject.active = true;
//			
//			m_ApplyResult.gameObject.active = false;
//			
//			m_IsCanApplyFriend = true;
//		}
	}
	
	private bool IsMyFriend(int playid)
	{
		//FriendsSceneModel friendmod = m_FriendFrame.GetComponent<FriendsSceneModel>();
		
//		foreach(List<FriendBase> friends in friendmod.m_FriendsAll.friend_list.Values)
//		{
//			foreach(FriendBase friend in friends)
//			{
//				if(friend.player_id == playid)
//				{
//					return true;
//				}
//			}
//		}
		return false;
	}
	
	private void ApplyOk()
	{
		m_ApplyFriend.color = new Color(128.0f/255,128.0f/255,128.0f/255,1.0f);
				
		m_ApplyResult.gameObject.active = true;
			
		//m_ApplyResult.text =  (Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager).GetValue("BattleRewardFriendApplyed");
	}
}