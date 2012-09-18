using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUIDialogAnimate : NvUIDialogEffectBase {
	
	public Animation mTarget = null;
	public string[] mOpenEffectClips = null;
	public string[] mCloseEffectClips = null;
	public bool m_CloseRerversly = false;
	public float m_playInAnimSpeed = 1.0f;
	public float m_playOutAnimSpeed = 1.0f;
	
	public enum EPlayMode
	{
		ePM_PlayDirect = 0,
		ePM_PlayFade
	}
	
	//public EPlayMode mMode = EPlayMode.ePM_PlayDirect;
	
	protected override UITweener startOpenEffectInternal( GameObject widget ) { return null; }
	protected override UITweener startCloseEffectInternal( GameObject widget ) { return null; }
	
	private List<string> mQueuedAnimation = new List<string>();
	private string currentPlayingAnimation;
	
	protected override void Update() 
	{
		// needn't do anything
		if ( m_effectStarted == false )
		{
			if ( mTarget != null && mTarget.enabled == true )
			{
				mTarget.enabled = false;
				//mTarget.playAutomatically = false;
			}
		}
		else
		{
			if ( mTarget.isPlaying == false )
			{
				mQueuedAnimation.Remove(currentPlayingAnimation);
				if ( mQueuedAnimation.Count > 0 )
				{
					currentPlayingAnimation = mQueuedAnimation[0];
					mTarget.Play( currentPlayingAnimation );
				}
				else
				{
					currentPlayingAnimation = "";
					m_effectStarted = false;
					mTarget.enabled = false;
					EnableAnchors(true);
						
					if ( eventReceiver != null && callWhenEffectDone != "" )
					{
						eventReceiver.SendMessage( callWhenEffectDone, this, SendMessageOptions.DontRequireReceiver );
					}
				}
			}
		}
	}
	
	private void EnableAnchors( bool flag )
	{
		if ( dialog != null )
		{
			dialog.enableAnchors( flag );
		}
	}
	
	protected override bool customStartEffect (bool isOpenEffect)
	{
		EnableAnchors(false);
		
		if ( mTarget != null )
		{
			mQueuedAnimation.Clear();
			mTarget.enabled = true;
			
			string[] clips;
			if ( m_CloseRerversly )
				clips = mOpenEffectClips;
			else
				clips = isOpenEffect ? mOpenEffectClips : mCloseEffectClips;
			
			foreach ( string clip in clips )
			{
				AnimationState state = mTarget[ clip ];
				
				if ( state != null )
				{
					if ( m_CloseRerversly && !isOpenEffect ){
						state.time = state.length;
						state.speed = -1.0f * (isOpenEffect ? m_playInAnimSpeed : m_playOutAnimSpeed);
					}else{
						state.speed = 1.0f * (isOpenEffect ? m_playInAnimSpeed : m_playOutAnimSpeed);
					}
						
					
					mQueuedAnimation.Add( clip );
				}
			}
			
			if ( mQueuedAnimation.Count <= 0 )
			{
				m_effectStarted = false;
				mTarget.enabled = false;
				EnableAnchors(true);
				
				if ( eventReceiver != null && callWhenEffectDone != "" )
				{
					eventReceiver.SendMessage( callWhenEffectDone, this, SendMessageOptions.DontRequireReceiver );
				}
			}
			else
			{
				currentPlayingAnimation = mQueuedAnimation[0];
				mTarget.Play( currentPlayingAnimation );
			}
		}
		
		return true;
	}
	
}
