using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class NvUIBattleEnd : NvUIDialogBase 
{
	public delegate void                 CloseHandler();   
	public event                            CloseHandler EventOnCloseAnimEnd;
	
	public  UISprite[]                      m_ResultImg = new UISprite[2];
	
	private string                           m_Result = "";
	
	protected override void OnDlgCreate()
	{

	}
	
	protected override void OnDlgInit ()
	{
		if(!IsNeedShowQuestFinishOrLevelUp())
		{
			WaitToClose(1.0f);
		}
		else
		{
			WaitToPlayQuestOrLevUp();
		}
		
		//NvSoundController SoundCtr = Singlton.getInstance("NvSoundController") as NvSoundController;
		
		if(m_Result.Contains("w"))
		{
			//SoundCtr.PlaySeAndFadeBgm("se_win");
		}
		else if(m_Result.Contains("l"))
		{
			//SoundCtr.PlaySeAndFadeBgm("se_lose");
		}
	}
	
	private void WaitToClose(float timepass)
	{
		StartCoroutine(DoWaitToClose(timepass));
	}
	
	private void WaitToPlayQuestOrLevUp()
	{
		PopQuestFinish();
	}
	
	IEnumerator DoWaitToClose(float timepass)
	{
		yield return new WaitForSeconds(timepass);
		
		closeDialog();
	}
	
	protected override void OnDlgBeginEffect( EDialogEffect effect ) 
	{
		if(effect == EDialogEffect.eDE_OpenEffect)//when open dlg
		{
			Apply();
		}
	}
	protected override void OnDlgEndEffect( EDialogEffect effect ) 
	{
		if (effect == NvUIDialogBase.EDialogEffect.eDE_CloseEffect)
		{
			EventOnCloseAnimEnd();
		}
	}
	
	private void Apply()
	{
		if(m_Result == "w1")
		{
			m_ResultImg[0].spriteName = "byanarrowmat";
			
			m_ResultImg[1].spriteName = "win";
		}
		else if(m_Result == "w2")
		{
			m_ResultImg[0].spriteName = "whup";
			
			m_ResultImg[1].spriteName = "win";
		}
		else if(m_Result == "w3")
		{
			m_ResultImg[0].spriteName = "completely";
			
			m_ResultImg[1].spriteName = "win";
		}
		else if(m_Result == "l1")
		{
			m_ResultImg[0].spriteName = "pity";
			
			m_ResultImg[1].spriteName = "lose";
		}
		else if(m_Result == "l2")
		{
			m_ResultImg[0].spriteName = "fiasco";
			
			m_ResultImg[1].spriteName = "lose";
		}
		else if(m_Result == "l3")
		{
			m_ResultImg[0].spriteName = "completely_gray";
			
			m_ResultImg[1].spriteName = "lose";
		}
		
		m_ResultImg[0].transform.localScale = new Vector3(180,180,-35);
	}
	
	public void SetResult(string res)
	{
		m_Result = res;
	}
	
	
	
	private bool IsNeedShowQuestFinishOrLevelUp()
	{
		//NvPlayerManager m_PlayerMgr = NvPlayerManager.getInstance();
		
		//Debug.LogError("m_PlayerMgr.LevelupEffectStack:  "+m_PlayerMgr.LevelupEffectStack+" m_PlayerMgr.QuestFinishMsgCount: "+m_PlayerMgr.QuestFinishMsgCount);
//		if(m_PlayerMgr && (m_PlayerMgr.LevelupEffectStack>0 || m_PlayerMgr.QuestFinishMsgCount>0))
//		{
//			return true;
//		}
		return false;
	}
	
	public void PopQuestFinish()
	{
		//NvPlayerManager m_PlayerMgr = NvPlayerManager.getInstance();
		
//		if(m_PlayerMgr==null)
//		{
//			WaitToClose(1.0f);
//			
//			return;
//		}
//		if(m_PlayerMgr.QuestFinishMsgCount>0)
//		{
//			m_PlayerMgr.PopQuestFinishMessage();
//		}
//		else
//		{
//			PopLevelUp();
//		}
	}
	
	public void PopLevelUp()
	{
//		NvPlayerManager m_PlayerMgr = NvPlayerManager.getInstance();
//		
//		if(m_PlayerMgr==null)
//		{
//			WaitToClose(1.0f);
//			
//			return;
//		}
//		if(m_PlayerMgr.LevelupEffectStack>0)
//		{
//			m_PlayerMgr.PopLevelupEffect();
//		}
//		else
//		{
//			WaitToClose(0.5f);
//		}
		
	}
	
	public void OnQuestFinish( )
	{
		//Debug.LogError("OnQuestFinish!!");
		PopLevelUp();
	}
	
	public void OnLevelUp(  )
	{
		//Debug.LogError("OnQuestFinish!!");
		WaitToClose(0.2f);
	}
}
