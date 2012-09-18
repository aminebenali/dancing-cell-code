using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum HelpSideType
{
	Left,
	Right
}

public class NvHelpAttackItem
{
	public int                          m_SideType;
	
	public string                     m_nameTex;
	
	public string                     m_SpriteName;
	

	public NvHelpAttackItem()
	{
		m_SideType = 0;
		
		m_nameTex = "";
		
	    m_SpriteName ="";
	}
	
	public NvHelpAttackItem(int sidetype,string name,string spritename)
	{
		m_SideType = sidetype;
		
		m_nameTex = name ;

	    m_SpriteName = spritename;
	}
}

public class NvUIHelpAttackDlg : NvUIDialogBase 
{
	private NvHelpAttackItem      m_HelpAttackItem = null;
	
	public CommonUITexture[]     m_UITextureList = new  CommonUITexture[2];
	
	public UILabel[]                      m_LabelList = new  UILabel[2];
	
	public UILabel[]                      m_LabeTextlList = new  UILabel[2];
	
	public GameObject[]              m_PanelList;
	
	public Animation                    m_MoveAnimtion = null;
	
	public delegate void              CallBackFinishShow();  
	
	public CallBackFinishShow      m_CallBack = null;
	
	private string                         m_SpeakerNameColor = "[FFFF00]";
	
	private string                         m_HelpText = "";
	
	
	protected override void OnDlgCreate()
	{
		//m_HelpText = (Singlton.getInstance( "NvLocalizationManager") as NvLocalizationManager).GetValue( "BattlePkPlayerHellpAttackScenarioText");;
	}

	protected override void OnDlgInit()
	{
		//WaitToClose((Singlton.getInstance("NvBattleManager")as NvBattleManager ).GetWaitHelpAttackUIOverTime());
	}
	
	protected override void OnDlgClose()
	{
	}
	
	
	protected override void OnDlgEndEffect( EDialogEffect effect )
	{
		if(effect == EDialogEffect.eDE_CloseEffect)//when close dlg
		{
			if(m_CallBack!=null)
			{
				m_CallBack();
			}
			else
			{
				Debug.Log("Check it!! ");
			}
		}
	}
	
	protected override void OnDlgBeginEffect( EDialogEffect effect ) 
	{
		if(effect == EDialogEffect.eDE_OpenEffect)//when open dlg
		{		
			SetDiffSideAnim();
			
			Apply();
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
		if(m_HelpAttackItem.m_SideType == (int)ScenariogSideType.Left)
		{
			anim.mOpenEffectClips[0] =  "HelpAttackAnimLeftIn";
			
			anim.mCloseEffectClips[0] =  "HelpAttackAnimLeftOut";
		}
		else
		{
			anim.mOpenEffectClips[0] =  "HelpAttackAnimRightIn";
			
			anim.mCloseEffectClips[0] =  "HelpAttackAnimRightOut";
		}
	}
	
	
	public void SetHelptem(NvHelpAttackItem item,CallBackFinishShow callback,string musicname)
	{
		m_HelpAttackItem = item;
		
		m_CallBack = callback;
		
		//NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
	
		//soundctr.PlaySeAndFadeBgm(musicname);
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
		return true;
	}
	
	
	private void Apply()
	{
		if(m_HelpAttackItem!=null)
		{
				m_PanelList[m_HelpAttackItem.m_SideType].SetActiveRecursively(true);
				
				Debug.LogWarning("m_PanelList[m_HelpAttackItem.m_SideType]:  "+m_PanelList[m_HelpAttackItem.m_SideType].name+" true");
			
				m_PanelList[1-m_HelpAttackItem.m_SideType].SetActiveRecursively(false);
				
				m_LabelList[m_HelpAttackItem.m_SideType].text = m_SpeakerNameColor+m_HelpAttackItem.m_nameTex;
				
				m_LabeTextlList[m_HelpAttackItem.m_SideType].text = m_HelpText;
			
				Material texmatro =  m_UITextureList[m_HelpAttackItem.m_SideType].ChangePortraitTexture(m_HelpAttackItem.m_SpriteName);
				
				texmatro.shader = Shader.Find("Unlit/Transparent Colored");
		}
	}

	private void CloseDlg()
	{
		Debug.LogWarning("CloseDlg");
		
		closeDialog();
	}
	
	private void WaitToClose(float timewait)
	{
		StartCoroutine(DoWaitToClose(timewait));
	}

	IEnumerator DoWaitToClose(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);
		
		closeDialog();
	}
	
}
