using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StepShow
{
	public NvUIImage[]                   m_numSprite;
	
	public int                                 Step;                    
	
	public int                                 m_MaxStep = 3;
	
	public Transform                      m_LeftPos;
	
	public Transform                      m_MidPos;
	
	public UISprite                         m_UIBattleBeginPic;
	
	public void ApplyShow()
	{
		if((Step>0 && Step<4) && (m_MaxStep>0 && m_MaxStep<4))
		{
			//Debug.LogError(" Step: "+Step +" MaxStep: "+m_MaxStep);
			m_numSprite[0].setVisible(true);
			m_numSprite[0].setEnable(true);
			
			m_numSprite[1].setVisible(true);
			m_numSprite[1].setEnable(true);
			
			m_numSprite[2].setVisible(true);
			m_numSprite[2].setEnable(true);
			
			m_UIBattleBeginPic.transform.position = m_LeftPos.position;
			
			m_numSprite[0].spriteName = Step.ToString();
			
			m_numSprite[2].spriteName = m_MaxStep.ToString();
			
		}
		else
		{
			//Debug.LogError(" Step: "+Step +" MaxStep: "+m_MaxStep);
			m_numSprite[0].setVisible(false);
			m_numSprite[0].setEnable(false);
			
			m_numSprite[1].setVisible(false);
			m_numSprite[1].setEnable(false);
			
			m_numSprite[2].setVisible(false);
			m_numSprite[2].setEnable(false);
			
			m_UIBattleBeginPic.transform.position = m_MidPos.position;
		}
	}
}

public class NvUIBattleBegin : NvUIDialogBase 
{
	public delegate void                 CloseHandler();   
	public event                            CloseHandler EventOnCloseAnimEnd;
	
	public StepShow                       m_numSprite;
	
	
	protected override void OnDlgInit ()
	{
		WaitToClose(1.0f);
	}
		
	protected override void OnDlgBeginEffect( EDialogEffect effect ) 
	{
		if(effect == EDialogEffect.eDE_OpenEffect)//when open dlg
		{
			m_numSprite.ApplyShow();
		}
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
	
	public void SetBattleStep(int step,int maxstep)
	{
		//Debug.LogError("step :  "+step+"  maxstep: "+maxstep);
		
		m_numSprite.Step = step;
		
		m_numSprite.m_MaxStep = maxstep;
		
		//m_numSprite.ApplyShow();
	}
	
	protected override void OnDlgEndEffect( EDialogEffect effect ) 
	{
		//Debug.LogWarning("=======OnDlgEndEffect");
		if (effect == NvUIDialogBase.EDialogEffect.eDE_CloseEffect)
		{
			EventOnCloseAnimEnd();
		}
	}
	
	
}
