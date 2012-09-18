using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum QuestTotalType
{
	TODAYQUEST,
	CURQUEST,
	UNKNOW
}

[System.Serializable]
public class QuestItem
{
	public UISprite                 m_QuestBut = null;
	
	public UILabel                  m_QuestLabel = null;
		
	public UISprite                 m_SpriteIsFinish = null;
		
	[HideInInspector]
	public string                    m_QuestName = "";
		
	[HideInInspector]
	public int                         m_QuestType = (int)QuestType.UNKNOW;
		
	[HideInInspector]
	public string                    m_ColorOff = "[FFFFFF]";
		
	[HideInInspector]
	public string                    m_ColorOn = "[FFFA00]";
		
	[HideInInspector]
	public bool                      m_IsFinishQuest = false;
		
	[HideInInspector]
	public bool                      m_IsChecked = false;
		
	public void UpdateShow()
	{
		if(m_QuestType == (int)QuestType.UNKNOW)
		{
			SetEnable(false);
		}
		else
		{
			SetEnable(true);
				
			if(m_QuestLabel!=null)
			{
				m_QuestLabel.text = GetColor()+GetTypeStr()+m_QuestName;
			}
			if(!m_IsFinishQuest)
			{
				m_SpriteIsFinish.gameObject.SetActiveRecursively(false);
			}
			else
			{
				m_SpriteIsFinish.gameObject.SetActiveRecursively(true);
			}
				
			//NvGlobalFunc.SetCheckSprite(m_QuestBut,m_IsChecked);
		}
	}
		
		
	public void SetChecked(bool isset)
	{
		m_IsChecked = isset;
	}
		
	private string GetColor()
	{
		if(m_IsChecked)
		{
			return m_ColorOn;
		}
		else
		{
			return m_ColorOff;
		}
	}
		
	private string GetTypeStr()
	{
			//NvLocalizationManager LocalMgr = Singlton.getInstance("NvLocalizationManager")as NvLocalizationManager;
				
//			string tmpstr =	"";
//			
//			if(m_QuestType == (int)QuestType.MAIN)
//			{
//				tmpstr =	LocalMgr.GetValue("questtype_main");
//				
//				return "["+tmpstr+"]";
//			}
//			else if(m_QuestType == (int) QuestType.SECONDLY)
//			{
//				tmpstr =	LocalMgr.GetValue("questtype_secondly");
//				
//				return "["+tmpstr+"]";
//			}
//			else if(m_QuestType == (int) QuestType.ADVENTURE)
//			{
//				tmpstr =	LocalMgr.GetValue("questtype_adventure");
//				
//				return  "["+tmpstr+"]";
//			}
//			else if(m_QuestType == (int) QuestType.EVERYDAY)
//			{
//				tmpstr =	LocalMgr.GetValue("questtype_everyday");
//				
//				return  "["+tmpstr+"]";
//			}
			return "";
	}

	public void SetEnable(bool enable)
	{
				m_QuestBut.gameObject.SetActiveRecursively(enable);

				m_QuestLabel.gameObject.SetActiveRecursively(enable);

				m_SpriteIsFinish.gameObject.SetActiveRecursively(enable);

	}
}


[System.Serializable]
public class RewardItem
{
	[HideInInspector]
	public RewardType m_RewardType = RewardType.UNKNOW;
		
	[HideInInspector]
	public int m_id = 0; //EXP MATURITY GOLD id == 0
	
	[HideInInspector]
	public string    m_RewardItemName = "";//EXP MATURITY GOLD id == ""
		
	[HideInInspector]
	public string    m_RewardItemResName = "";
		
	[HideInInspector]
	public int  m_Num =0;
		
	[SerializeField]
	public UILabel      m_numLabel = null;
		
	[SerializeField]
	public UISprite     m_iconSprite = null;
								
	public void UpdateShow()
	{
		if(IsNeedShow())
		{
			SetEnable(true);
				
				m_iconSprite.spriteName = m_RewardItemResName;
				
				if(m_RewardType != RewardType.ITEM)
				{
					m_iconSprite.transform.localScale = new Vector3(24,24,1);
				}
				else
				{
					m_iconSprite.transform.localScale = new Vector3(32,32,1);
				}
				if(m_RewardType == RewardType.ITEM)
				{
					m_numLabel.text = "[000000]"+m_RewardItemName+"  x "+m_Num.ToString();
				}
				else
				{
					m_numLabel.text = "[000000]"+GetRewardTypeName(m_RewardType)+" + "+m_Num.ToString();
				}
			}
			else
			{
				
				SetEnable(false);
			}
		}
		
		private string GetRewardTypeName(RewardType type)
		{
			 //NvLocalizationManager m_Localization = Singlton.getInstance("NvLocalizationManager")as NvLocalizationManager;
		
			string ret = "";
		
			switch(type)
			{
				case RewardType.EXP:
				{
					//ret = m_Localization.GetValue("Exp");
				}
				break;
				case RewardType.GOLD:
				{
					//ret = m_Localization.GetValue("Gold");
				}
				break;
				case RewardType.MATURITY:
				{
					//ret = m_Localization.GetValue("Maturity");
				}
				break;

			}
			return ret;
		}
		
		private void SetEnable(bool enable)
		{
			m_numLabel.gameObject.SetActiveRecursively(enable);
				
			m_iconSprite.gameObject.SetActiveRecursively(enable);
		}
		
		private bool IsNeedShow()
		{
			if( m_Num>0 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

}
public enum RewardType
{
		EXP,
		GOLD,
		MATURITY,
		ITEM,
		UNKNOW	
}
	
public enum QuestType
{
		MAIN,
		SECONDLY,
		EVERYDAY,
		ADVENTURE,
		UNKNOW
}



public class NvUIQuestDlg : NvUIDialogBase 
{
	
	private static int MaxQuestNum = 6;
	private static int RewardNum = 5;

	/*
	private EMessageBoxStyle m_style = EMessageBoxStyle.eStyle_OkayOnly;
	
	private NvUILabel m_title = null;
	private NvUILabel m_description = null;
	private NvUIButton m_btnOk = null;
	private NvUIButton m_btnCancel = null;
	//private Transform m_bg = null;
	*/
	
	[SerializeField]
	public  List<RewardItem> m_rewardItemList ;
	
	[SerializeField]
	public  List<QuestItem>   m_QuestList ;
	
	[SerializeField]     
	public  UISprite         m_btnGoToFight;
	
	public  UISprite         m_btnGiveUp;
	
	public  UISprite[]       m_btnQuestTitle ;

	public  UILabel          m_QuestDescLab;
	
	public  UILabel          m_QuestFinishDescLab;
	
	public Transform[]    m_BtnTransformPosList = new Transform[3];
	
	//private NvConstructionView m_View = null;
	
	//private QuestTotalType m_CurTitleType = QuestTotalType.CURQUEST;
	
	//private List<MsgMyQuestItem> m_SortdQuestList= new List<MsgMyQuestItem>();
		
	//private  List<MsgMyQuestItem> curAcceptQuestList = null;
		
	//private int      m_CurChoosedQuest = 0;
	
	//private NvTutorial mTutorial;
	
	private bool m_IsCanPress = true;
	
	protected override void OnDlgCreate()
	{
		for(int i=0;i<MaxQuestNum;i++)
		{
			m_QuestList[i].SetChecked(false);
			
			m_QuestList[i].UpdateShow();
		}
		//m_QuestList[m_CurChoosedQuest].m_QuestBut.setChecked(true);
		
		for(int i=0;i<RewardNum;i++)
		{
			m_rewardItemList[i].UpdateShow();
		}
		m_QuestDescLab.text = "";
			
		m_QuestFinishDescLab.text = "";
		
		//mTutorial = NvTutorialManager.getInstance().CurrentTutorial;
	}
	

	public void SetShowNull()
	{
		for(int i=0;i<MaxQuestNum;i++)
		{
			m_QuestList[i].m_QuestType  = (int)QuestType.UNKNOW;
			
			m_QuestList[i].UpdateShow();
		}
		for(int i=0;i<RewardNum;i++)
		{
			m_rewardItemList[i].m_Num = 0;
			
			m_rewardItemList[i].m_RewardType = RewardType.UNKNOW;
			
			m_rewardItemList[i].UpdateShow();
		}
	
		m_QuestDescLab.text = "";
		
		m_QuestFinishDescLab.text = "";
	}
	
	protected override void OnDlgInit()
	{
		//get curquestlist
		
		//mTutorial.TutorialChkInside ( "QuestDlg" );
		
	}
	
	 protected override void OnDlgBeginEffect( EDialogEffect effect ) 
	{
		if(effect == EDialogEffect.eDE_OpenEffect)//when open dlg
		{
			//NvConstructionController controller = NvGlobalFunc.GetNvConstructionController();
			//m_View = controller.findView("NvConstructionView")as NvConstructionView;
			//Debug.Log (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> m_View = " + m_View);
			ApplyShow();
		}
	}
	
	public void ApplyShow()
	{
		//if(m_View == null)
		//{
			return;
		//}
		//m_btnQuestTitle[(int)QuestTotalType.TODAYQUEST].gameObject.active  = false;
			
		//curAcceptQuestList = m_View.GetCurAcceptQuestList();
		
		//NvGlobalFunc.SetCheckSprite(m_btnQuestTitle[(int)m_CurTitleType],true);
		
		//int index = 1-(int)m_CurTitleType;
		
		//NvGlobalFunc.SetCheckSprite(m_btnQuestTitle[index],false);
		
		//List<MsgMyQuestItem> showlist = new List<MsgMyQuestItem>();
		
		//m_CurChoosedQuest =0;
		
		//m_QuestList[m_CurChoosedQuest].m_QuestBut.setChecked(true);
		
//		foreach(MsgMyQuestItem item in curAcceptQuestList )
//		{
//			if(m_CurTitleType == QuestTotalType.CURQUEST &&  item.quest_type == (int)QuestType.EVERYDAY)
//			{
//				continue;
//			}
//			if(m_CurTitleType == QuestTotalType.TODAYQUEST &&  item.quest_type != (int)QuestType.EVERYDAY)
//			{
//				continue;
//			}
//			showlist.Add(item);
//		}
		//m_SortdQuestList = GetQuesrettBubbleSortList(showlist);
			
		//Debug.Log("   m_SortdQuestList.Count:   "+m_SortdQuestList.Count);
		
		//for(int i=0;i<MaxQuestNum;i++)
		//{
//			if(m_SortdQuestList.Count > i)
//			{
//				m_QuestList[i].m_QuestType = m_SortdQuestList[i].quest_type; 
//				
//		    	//m_QuestList[i].m_QuestName =   m_View.getQuestDictName(m_SortdQuestList[i].quest_id) ;
//				
//				if(i==m_CurChoosedQuest)
//				{
//					m_QuestList[i].SetChecked(true);
//				}
//				else
//				{
//					m_QuestList[i].SetChecked(false);
//				}
//				m_QuestList[i].m_IsFinishQuest = m_SortdQuestList[i].status==1?true:false;
//				
//				m_QuestList[i].UpdateShow();
//			}
//			else
//			{
//				m_QuestList[i].m_QuestType = (int)QuestType.UNKNOW;
//				
//		    	m_QuestList[i].m_QuestName = "";
//				
//				m_QuestList[i].UpdateShow();
//			}
	   // }
		//ApplyCurQuest();
		
		//m_IsCanPress = true;
	}
	
	private void ApplyCurQuest()
	{
//		if(m_CurChoosedQuest < m_SortdQuestList.Count)
//		{
//			MsgMyQuestItem item = m_SortdQuestList[m_CurChoosedQuest];
//			
//			//m_QuestDescLab.text = "[000000]" + Utility.GetQuestDetail( m_View.getQuestDictDesc(item.quest_id));
//			
//			string rewardstr = "";
//			
////			if(m_View.getQuestDictName(item.target1_id)!="")
////			{
////				rewardstr += GetColorbyNum(item.target1_num,item.target1_amount)+m_View.getQuestDictName( item.target1_id)+" ("+item.target1_num+"/"+item.target1_amount+")   ";
////			}
////			if(m_View.getQuestDictName(item.target2_id)!="")
////			{
////				if(rewardstr != "")
////				{
////					rewardstr += "[000000],   ";
////				}
////				rewardstr += GetColorbyNum(item.target2_num,item.target2_amount)+m_View.getQuestDictName( item.target2_id)+" ("+item.target2_num+"/"+item.target2_amount+")   ";
////			}
////			if(m_View.getQuestDictName(item.target3_id)!="")
////			{
////				if(rewardstr != "")
////				{
////					rewardstr += "[000000],   ";
////				}
////				rewardstr += GetColorbyNum(item.target3_num,item.target3_amount)+m_View.getQuestDictName( item.target3_id)+" ("+item.target3_num+"/"+item.target3_amount+") ";
////			}
//			
//			m_QuestFinishDescLab.text = rewardstr;
//			
//			Debug.Log("=====================>  name: "+m_QuestFinishDescLab.name+"   m_QuestFinishDescLab.text:  "+m_QuestFinishDescLab.text);
//			
//			for(int i=0;i<RewardNum;i++)
//			{
//				m_rewardItemList[i].m_Num = GetNumRewardItem(i,item);
//				
//				if(i<3)
//				{
//					m_rewardItemList[i].m_id = 0;
//					
//					if(i == 0)
//					{
//						m_rewardItemList[i].m_RewardType = RewardType.EXP;
//						
//						m_rewardItemList[i].m_RewardItemResName = "exp";
//					}
//					if(i == 1)
//					{
//						m_rewardItemList[i].m_RewardType = RewardType.GOLD;
//						
//						m_rewardItemList[i].m_RewardItemResName = "money";
//					}
//					if(i == 2)
//					{
//						m_rewardItemList[i].m_RewardType = RewardType.MATURITY;
//						
//						m_rewardItemList[i].m_RewardItemResName = "maturity";
//					}
//					m_rewardItemList[i].m_RewardItemName = "";
//				}
//				else
//				{
//					string fieldname = "item"+(i-2)+"_id";
//					
//					System.Type  t = item.GetType();
//					
//					System.Reflection.FieldInfo field = t .GetField(fieldname);
//					
//					int id = (int)field.GetValue(item);
//					
//					m_rewardItemList[i].m_id = id;
//					
//					m_rewardItemList[i].m_RewardType = RewardType.ITEM;
//					
//					//string name = m_View.getQuestDictName(id);
//					
//					//string resname = m_View.getQuestResName(id);
//					
//					if(resname=="")
//					{
//						resname = "mt_wa_0_0";
//					}
//					m_rewardItemList[i].m_RewardItemResName = resname;
//					
//					m_rewardItemList[i].m_RewardItemName = name;
//				}
//				m_rewardItemList[i].UpdateShow();
//			}
//			
//		}
//		else
//		{
//			m_QuestDescLab.text = "";
//			
//			m_QuestFinishDescLab.text = "";
//			
//			for(int i=0;i<RewardNum;i++)
//			{
//				m_rewardItemList[i].m_Num = 0;
//				
//				m_rewardItemList[i].m_id =0;
//				
//				m_rewardItemList[i].m_RewardType = RewardType.UNKNOW;
//				
//				m_rewardItemList[i].UpdateShow();
//				
//				m_rewardItemList[i].m_RewardItemResName = "";
//				
//				m_rewardItemList[i].m_RewardItemName = "";
//			}
//		}
//		if(m_SortdQuestList.Count>0)
//		{
//			m_btnGoToFight.gameObject.active = true;
//			
//			m_btnGiveUp.gameObject.active = true;
//			
//			if(m_SortdQuestList.Count>m_CurChoosedQuest)
//			{
//				MsgMyQuestItem curitem = m_SortdQuestList[m_CurChoosedQuest];
//					
//				int dungeonid = curitem.relate_dungeon_id;
//				
//				if(dungeonid<=0)
//				{
//					m_btnGoToFight.spriteName = "to go _off";
//					
//					m_btnGoToFight.gameObject.active = false;
//			
//					m_btnGiveUp.gameObject.active = true;
//					
//					m_btnGiveUp.gameObject.transform.position = m_BtnTransformPosList[1].position;
//				}
//				else
//				{
//					m_btnGoToFight.spriteName = "to go _on";
//					
//					m_btnGoToFight.gameObject.active = true;
//			
//					m_btnGiveUp.gameObject.active = true;
//					
//					m_btnGiveUp.gameObject.transform.position = m_BtnTransformPosList[0].position;
//					
//					m_btnGoToFight.gameObject.transform.position = m_BtnTransformPosList[2].position;
//				}
//			}
//			else
//			{
//				//m_btnGoToFight.spriteName = "to go _on";
//				
//				Debug.LogError("Check it! ");
//			}
//		}
//		else
//		{
//			m_btnGoToFight.gameObject.active = false;
//			
//			m_btnGiveUp.gameObject.active = false;
//		}
	}
	
//	private int GetNumRewardItem(int index,MsgMyQuestItem item)
//	{
//		if(index == 0)
//		{
//			return item.exp;
//		}
//		else if(index == 1)
//		{
//			return item.gold;
//		}
//		else if(index == 2)
//		{
//			return item.maturity;
//		}
//		else 
//		{
//			if(index == 3)
//			{
//				return item.item1_amount;
//			}
//			else if(index == 4)
//			{
//				return item.item2_amount;
//			}
//			else if(index == 5)
//			{
//				return item.item3_amount;
//			}
//		}
//		return 0;
//	}
	private string GetColorbyNum(int num ,int amount)
	{
		if(num>=amount)
		{
			return "[6B8E23]";
		}
		else
		{
			return "[000000]";
		}
	}
	
//	private MsgMyQuestItem GetMinWeightQuestItem(MsgMyQuestItem leftquest, MsgMyQuestItem rightquest)
//	{
//		if(leftquest.quest_type == rightquest.quest_type)
//		{
//			if(leftquest.quest_id > rightquest.quest_id  )
//			{
//				//Debug.LogError(" return right!!!=====>    Leftquet:  "+m_View.getQuestDictName( leftquest.quest_id)+"  rightquest:  "+m_View.getQuestDictName( rightquest.quest_id));
//				
//				return rightquest;
//			}
//			else
//			{
//				//Debug.LogError(" return left!!!=====>     Leftquet:  "+m_View.getQuestDictName( leftquest.quest_id)+"  rightquest:  "+m_View.getQuestDictName( rightquest.quest_id));
//				
//				return leftquest;
//			}
//		}
//		else
//		{
//			if( leftquest.quest_type < rightquest.quest_type)//QuestType
//			{
//				//Debug.LogError(" return left!!!=====>    Leftquet:  "+m_View.getQuestDictName( leftquest.quest_id)+"  rightquest:  "+m_View.getQuestDictName( rightquest.quest_id));
//				
//				return leftquest;
//			}
//			else
//			{
//				//Debug.LogError(" return right!!!=====>     Leftquet:  "+m_View.getQuestDictName( leftquest.quest_id)+"  rightquest:  "+m_View.getQuestDictName( rightquest.quest_id));
//				
//				return rightquest;
//			}
//		}
//	}
	
//	private List<MsgMyQuestItem> GetQuesrettBubbleSortList(List<MsgMyQuestItem> curlist )
//	{
//		if(curlist.Count>1)
//		{
//			for(int i=0;i<curlist.Count;i++)
//			{
//				for(int j = curlist.Count-2; j >= i ; j--)
//				{
//					if(GetMinWeightQuestItem(curlist[j+1],curlist[j]) == curlist[j+1])
//					{
//						MsgMyQuestItem tmp = curlist[j];
//						
//						curlist[j] = curlist[j+1];
//						
//						curlist[j+1] = tmp;
//					}
//				}
//			}
//		}
//		return curlist;
//		
//	}
	

	private void OnGiveUp()
	{
		//Debug.Log("OnGiveUp~~~~~~~~~~~~~");	
		if(isOpened && m_IsCanPress)
		{
			//if(m_View!=null)
			{
//				if(m_SortdQuestList.Count > m_CurChoosedQuest)
//				{
//					NvCommonUIManager mCommonUIManager = Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager;
//		
//					string desc =  (Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager).GetValue("questgiveupmsgboxshow");
//					
//					mCommonUIManager.showMessageBox(desc, "", NvMessageBox.EMessageBoxStyle.eStyle_YesNo, OnDoGiveUp);
//				}
			}
		}
	}
	
	private void RealGiveUp()
	{
		//m_View.RequestGiveUpQuest(m_SortdQuestList[m_CurChoosedQuest].quest_id);
					
		m_IsCanPress = false;
	}

	
	private void OnDoGiveUp( int result )
	{
		if ( result == (int)NvMessageBox.EResult.eResult_Yes )
		{
			RealGiveUp();
		}
	}


	private void OnClose()
	{
		Debug.Log("OnClose~~~~~~~~~~~~~");
		
		if(isOpened && m_IsCanPress)
		{
			CloseDlg();
			
			m_IsCanPress = false;
		}
	}
	
	private void OnOk()
	{
		if(isOpened && m_IsCanPress)
		{
//			if(m_SortdQuestList.Count>m_CurChoosedQuest)
//			{
//				//Debug.Log("OnOk~~~~~~~~~~~~~");
//				
//				MsgMyQuestItem item = m_SortdQuestList[m_CurChoosedQuest];
//					
//				int dungeonid = item.relate_dungeon_id;
//				
//				if(dungeonid>0)
//				{
//					NvPlayerManager playermgr = Singlton.getInstance("NvPlayerManager")as NvPlayerManager;
//					
//					playermgr.SetDungeonId(dungeonid);
//					
//					((SceneManager)(Singlton.getInstance("SceneManager"))).ChangeScene("Scene_UI_Area");
//					
//					mTutorial.TutorialChkInside ( "QuestDlg" );
//					
//					m_IsCanPress = false;
//				}
//			}
		}
	}
	
	private void OnQuestType(object sender)
	{
		//Debug.LogWarning("   sender: "+(sender as GameObject).name+"    m_btnQuestTitle[(int)QuestTotalType.CURQUEST]: "+ m_btnQuestTitle[(int)QuestTotalType.CURQUEST].name+
		//	"  m_btnQuestTitle[(int)QuestTotalType.TODAYQUEST]: "+m_btnQuestTitle[(int)QuestTotalType.TODAYQUEST].name);
		if((sender as GameObject).name == m_btnQuestTitle[(int)QuestTotalType.CURQUEST].name)
		{
		//	Debug.Log("(int)QuestTotalType.CURQUEST!!~~~~~~~~~~~~~sender: "+(sender as GameObject).name);

			//m_CurTitleType = QuestTotalType.CURQUEST;
		}
		else if((sender as GameObject).name == m_btnQuestTitle[(int)QuestTotalType.TODAYQUEST].name)
		{
		//	Debug.Log("int)QuestTotalType.TODAYQUEST!!~~~~~~~~~~~~~sender: "+(sender as GameObject).name);

			//m_CurTitleType = QuestTotalType.TODAYQUEST;
		}
		ApplyShow();
	}
	
	
	private void OnPressQuestList(object sender)
	{
		if(isOpened)
		{
			//GameObject obj = sender as GameObject;
			
			//char[] splt = {'_'};
			
			//string lastnum = NvGlobalFunc.GetLastString(obj.transform.parent.name,splt);
			
			//Debug.Log("OnPressQuestList~~~~~~~~~~~~~lastnum: "+ lastnum);
			
			//int index = int.Parse(lastnum);
			
			//Debug.Log("OnPressQuestList~~~~~~~~~~~~~index: "+ index);
			
			//m_CurChoosedQuest = index;
			
//			for(int i=0;i<MaxQuestNum;i++)
//			{
//				if(i == m_CurChoosedQuest )
//				{
//					m_QuestList[i].SetChecked(true);
//				}
//				else
//				{
//					m_QuestList[i].SetChecked(false);
//				}
//				m_QuestList[i].UpdateShow();
//			}
//			ApplyCurQuest();
		}
	}
	
	private void CloseDlg()
	{
		closeDialog();
	}
	
}
