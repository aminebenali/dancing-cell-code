using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum ScenariogSideType
{
	Left,
	Right
}

public class NvScenariogItem
{
	public int                          m_Id;
	
	public int                          m_SideType;
	
	public string                     m_nameTex;
	
	public string                     m_Text;
	
	public string                     m_SpriteName;
	
	public int                          m_ResBg = 0; //0: normal 1: blackgroud
	
	public NvScenariogItem()
	{
		m_Id = 0;
		
		m_SideType = 0;
		
		m_nameTex = "";
		
		m_Text = "";
		
	    m_SpriteName ="";
		
		m_ResBg = 0; //0: normal 1: blackgroud
	}
	
	public NvScenariogItem(int m_id,int sidetype,string nametex,string mtext,string spritename,int resbg)
	{
		//Debug.LogError("===>NvScenariogItem:  nametex:  "+nametex);
		
		m_Id = m_id;
		
		m_SideType = sidetype;
		
		m_nameTex = nametex;
		
		m_Text = mtext;
		
	    m_SpriteName = spritename;
		
		m_ResBg = resbg; //0: normal 1: blackgroud
	}
	
//	public NvScenariogItem(MsgScenarioDlg msgitme)
//	{
//		//Debug.LogError("===>NvScenariogItem:  msgitme.title:  "+msgitme.title);
//		
//		m_Id = msgitme.dialog_id; 
//		
//		m_SideType = msgitme.res_side;
//		
//		NvLocalizationManager localmgr = Singlton.getInstance("NvLocalizationManager")as NvLocalizationManager;
//		
//		string Colon = localmgr.GetValue("Colon");
//		
//		if(msgitme.title !=null && msgitme.title!="")
//		{
//			m_nameTex = msgitme.title+Colon;
//			
//			m_Text = "    "+ msgitme.text;
//		}
//		else
//		{
//			m_nameTex = "";
//			
//			m_Text = msgitme.text;
//		}
//		
//		m_ResBg = msgitme.res_bg;
//		
//		m_SpriteName = msgitme.res_filename;
//		
//	}
}

public class NvUIScenarioDlg : NvUIDialogBase 
{
	private NvScenariogItem         m_ScenariogItem = null;
	
	public CommonUITexture[]     m_UITextureList = new  CommonUITexture[2];
	
	public UILabel[]                      m_LabelList = new  UILabel[2];
	
	public UISprite                       m_ImageButton = null;
	
	public Transform                   m_TrasFromLeft;
	
	public Transform                   m_TranFromRight;
	
	//private NvScenarioController  m_ScenarioController = null;
	
	private bool                          m_IsBeginToClose = false;
	
	public UILabel[]                     m_SkipBtn = new UILabel[2];
	
	public UILabel[]                     m_SpeakerName = new UILabel[2];
	
	public UISprite                       m_BackImage;
	
	public UILabel                        m_MidText;
	
	public UILabel[]                      m_NextText = new UILabel[2];
	
	public Animation                    m_MoveAnimtion = null;
	
	//private bool                           m_IsShowSkip = true;

	//private string                         m_SpeakerNameColor = "[F75000]";
	
	protected override void OnDlgCreate()
	{
		//m_ScenarioController = Singlton.getInstance("NvScenarioController")as NvScenarioController;
	}

	protected override void OnDlgInit()
	{
		Debug.LogWarning("=============>OnDlgInit !");
		
		m_IsBeginToClose = false;
	}
	
	protected override void OnDlgClose()
	{
	}
	
	void OnNextScenario ()
	{
		//Debug.Log("OnOverOneScenarioItem");
		
//		if(m_ScenarioController!=null)
//		{
//			m_ScenarioController.OnOverOneScenarioItem();
//		}
	}
	
	protected override void OnDlgEndEffect( EDialogEffect effect )
	{
		if(effect == EDialogEffect.eDE_CloseEffect)//when close dlg
		{
			OnNextScenario();
		}
	}
	
	protected override void OnDlgBeginEffect( EDialogEffect effect ) 
	{
		if(effect == EDialogEffect.eDE_OpenEffect)//when open dlg
		{		
			SetDiffSideAnim();
			
			ApplyShow();
		}
		else
		{
			
		}
	}
	
	private void SetDiffSideAnim()
	{
		NvUIDialogAnimate anim = gameObject.GetComponent<NvUIDialogAnimate>();
		
		if(anim==null)
		{
			Debug.LogError("Check out ");
			
			return;
		}
		if(m_ScenariogItem.m_SideType == (int)ScenariogSideType.Left)
		{
			anim.mOpenEffectClips[0] =  "scenarioleftin";
			
			anim.mCloseEffectClips[0] =  "scenarioleftout";
		}
		else
		{
			anim.mOpenEffectClips[0] =  "scenariorightin";
			
			anim.mCloseEffectClips[0] =  "scenariorightout";
		}
	}
	
	public bool IsBeginToClose()
	{
		return m_IsBeginToClose;
	}
	
	public void SetScenarioItem(NvScenariogItem item)
	{
		m_ScenariogItem = item;
	}
	
	public bool CheckDlg()
	{
		if(m_UITextureList[0]==null || m_UITextureList[1]==null)
		{
			return false;
		}
		if(m_LabelList[0]==null || m_LabelList[1]==null)
		{
			return false;
		}
		if(m_ImageButton == null)
		{
			return false;
		}
		return true;
	}
	
//	void SetSpeakerNameVisableAndValue(ScenarioBackgroudType type,ScenariogSideType side)
//	{
//		//Debug.LogError("  type:  "+type+" side: "+side);
//		
//		m_SpeakerName[(int)ScenariogSideType.Left].gameObject.active = false;
//		
//		m_SpeakerName[(int)ScenariogSideType.Right].gameObject.active = false;
//		
//		if(type == ScenarioBackgroudType.talktype)
//		{
//			if(m_ScenariogItem.m_nameTex != null && m_ScenariogItem.m_nameTex != "")
//			{
//				if(side == ScenariogSideType.Left)
//				{
//					m_SpeakerName[(int)ScenariogSideType.Left].gameObject.active = true;
//					
//					m_SpeakerName[(int)ScenariogSideType.Left].text = m_SpeakerNameColor+m_ScenariogItem.m_nameTex;
//					
//					//Debug.LogError(" Left = m_SpeakerNameColor+m_ScenariogItem.m_nameTex; "+  m_SpeakerNameColor+m_ScenariogItem.m_nameTex);
//				}
//				else
//				{
//					m_SpeakerName[(int)ScenariogSideType.Right].gameObject.active = true;
//					
//					m_SpeakerName[(int)ScenariogSideType.Right].text = m_SpeakerNameColor+m_ScenariogItem.m_nameTex;
//					
//					//Debug.LogError(" Right = m_SpeakerNameColor+m_ScenariogItem.m_nameTex; "+  m_SpeakerNameColor+m_ScenariogItem.m_nameTex);
//				}
//			}
//		}
//	}
	
//	void SetSkipBtnVisable(ScenarioBackgroudType type,ScenariogSideType side)
//	{
//		m_SkipBtn[0].gameObject.active = false;
//					
//		m_SkipBtn[1].gameObject.active = false;
//		
//		return;
//		
//		/*
//		if(type == ScenarioBackgroudType.talktype)
//		{
//			if(m_IsShowSkip)
//			{
//				if(side == ScenariogSideType.Left)
//				{
//					m_SkipBtn[0].gameObject.active = false;
//					
//					m_SkipBtn[1].gameObject.active = true;
//				}
//				else
//				{
//					m_SkipBtn[0].gameObject.active = true;
//					
//					m_SkipBtn[1].gameObject.active = false;
//				}
//			}
//			else
//			{
//				m_SkipBtn[0].gameObject.active = false;
//			
//				m_SkipBtn[1].gameObject.active = false;
//			}
//		}
//		else if(type == ScenarioBackgroudType.asidetype)
//		{
//			m_SkipBtn[0].gameObject.active = false;
//			
//			m_SkipBtn[1].gameObject.active = false;
//		}
//		*/
//	}
	
	public void ApplyShow()
	{
//		if(m_ScenariogItem!=null && CheckDlg())
//		{
//			if(m_ScenariogItem.m_ResBg == (int)ScenarioBackgroudType.talktype)
//			{
//				
//				SetSkipBtnVisable((ScenarioBackgroudType)m_ScenariogItem.m_ResBg ,(ScenariogSideType)m_ScenariogItem.m_SideType);
//				
//				SetSpeakerNameVisableAndValue((ScenarioBackgroudType)m_ScenariogItem.m_ResBg ,(ScenariogSideType)m_ScenariogItem.m_SideType);
//				
//				m_ImageButton.gameObject.active = true;
//				
//				m_UITextureList[m_ScenariogItem.m_SideType].gameObject.active = true;
//				
//				m_UITextureList[1-m_ScenariogItem.m_SideType].gameObject.active = false;
//				
//				m_LabelList[m_ScenariogItem.m_SideType].gameObject.active = true;
//				
//				m_LabelList[1-m_ScenariogItem.m_SideType].gameObject.active = false;
//				
//				Material texmatro =  m_UITextureList[m_ScenariogItem.m_SideType].ChangePortraitTexture(m_ScenariogItem.m_SpriteName);
//				
//				texmatro.shader = Shader.Find("Unlit/Transparent Colored");
//				
//				m_LabelList[m_ScenariogItem.m_SideType].text = m_ScenariogItem.m_Text;
//				
//				m_BackImage.gameObject.active = false;
//	
//				m_MidText.gameObject.active = false;
//				
//				if(m_IsShowSkip)
//				{
//					if(m_ScenariogItem.m_SideType == (int)ScenariogSideType.Left)
//					{
//						m_NextText[(int)ScenariogSideType.Left].gameObject.active = true;
//						
//						m_NextText[(int)ScenariogSideType.Right].gameObject.active = false;
//					}
//					else
//					{
//						m_NextText[(int)ScenariogSideType.Left].gameObject.active = false;
//						
//						m_NextText[(int)ScenariogSideType.Right].gameObject.active = true;
//					}
//				}
//				else
//				{
//					m_NextText[(int)ScenariogSideType.Left].gameObject.active = false;
//					
//					m_NextText[(int)ScenariogSideType.Right].gameObject.active = false;
//				}
//				TryToDoAutoNextScenarioItem();
//			}
//			else if(m_ScenariogItem.m_ResBg == (int)ScenarioBackgroudType.asidetype)
//			{
//				m_UITextureList[0].gameObject.active = false;
//				
//				m_UITextureList[1].gameObject.active = false;
//				
//				m_LabelList[0].gameObject.active = false;
//				
//				m_LabelList[1].gameObject.active = false;
//				
//				m_ImageButton.gameObject.active = false;
//				
//				SetSkipBtnVisable((ScenarioBackgroudType)m_ScenariogItem.m_ResBg ,(ScenariogSideType)m_ScenariogItem.m_SideType);
//				
//				SetSpeakerNameVisableAndValue((ScenarioBackgroudType)m_ScenariogItem.m_ResBg ,(ScenariogSideType)m_ScenariogItem.m_SideType);
//				
//				m_BackImage.gameObject.active = true;
//				
//				m_MidText.gameObject.active = true;
//				
//				m_MidText.text = m_ScenariogItem.m_Text;
//				
//				m_NextText[(int)ScenariogSideType.Left].gameObject.active = false;
//					
//				m_NextText[(int)ScenariogSideType.Right].gameObject.active = false;
//				
//				SetFadeShowAside(0.5f,1.0f,0.0f,"DoShowAside");
//			}
//		}
//		SetSkipBtnVisable((ScenarioBackgroudType)m_ScenariogItem.m_ResBg ,(ScenariogSideType)m_ScenariogItem.m_SideType);
	}
	
	void DoShowAside()
	{
		StartCoroutine(WaitForShowAsideOver());
	}
	
	void TryToDoAutoNextScenarioItem()
	{
//		float timelgth = m_ScenarioController.GetTimeShowNext();
//		
//		if(timelgth>0.2f)
//		{
//			StartCoroutine(WaitForNextScenarioItem());
//		}
	}
	
	IEnumerator WaitForShowAsideOver()
	{
		yield return new WaitForSeconds(1.5f);
	
		SetFadeShowAside(0.5f,0,1,"OnPressScenairo");
	}
		
//	IEnumerator WaitForNextScenarioItem()
//	{
////		float timelgth = m_ScenarioController.GetTimeShowNext();
////		
////		yield return new WaitForSeconds(timelgth);
////	
////		OnPressScenairo();
//	}
	
	void SetFadeShowAside(float timepass, float fadeTo,float fadefrom ,string callwhenfinished)
	{
//		m_BackImage.alpha = fadefrom;
//		
//		Color fadetocolor = new Color(m_BackImage.color.r,m_BackImage.color.g,m_BackImage.color.b,fadeTo);
//			
//		Color fadefromcolor = new Color(m_BackImage.color.r,m_BackImage.color.g,m_BackImage.color.b,fadefrom);
//		
//		TweenColor tc = TweenColor.Begin( m_BackImage.gameObject, timepass, fadetocolor );
//		
//		tc.style = UITweener.Style.Once;
//		
//		tc.eventReceiver = gameObject;
//		
//		tc.callWhenFinished = callwhenfinished;
//			
//		tc.from = fadefromcolor;	
//		
//		fadetocolor = new Color(m_MidText.color.r,m_MidText.color.g,m_MidText.color.b,fadeTo);
//			
//		fadefromcolor = new Color(m_MidText.color.r,m_MidText.color.g,m_MidText.color.b,fadefrom);
//		
//		tc = TweenColor.Begin( m_MidText.gameObject, timepass, fadetocolor );
//		
//		tc.style = UITweener.Style.Once;
//			
//		tc.from = fadefromcolor;	
		
	}
		
	private void CloseDlg()
	{
		Debug.LogWarning("CloseDlg");
		
		closeDialog();
	}
	
	
	public void OnPressSkip()
	{
		Debug.LogWarning("OnPressSkip");
		
		if(isOpened)
		{
			Debug.Log("isOpened == true");
			
//			if(m_ScenarioController!=null)
//			{
//				Debug.Log("SkipScenairo");
//				
//				m_ScenarioController.SkipScenairo();
//			}
		}
	}
	
	public void OnPressScenairo()
	{
		if(isOpened)
		{
			//Debug.Log("OnPressScenairo");
			
			m_IsBeginToClose = true;
			
			CloseDlg();
		}
	}
	
	public void SetIsShowSkip(bool IsSkip)
	{
		//m_IsShowSkip = IsSkip;
	}
}
