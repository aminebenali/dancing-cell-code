using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NvUIDialogEffectBase : MonoBehaviour {
	
	public NvUIDialogBase dialog = null;
	public GameObject eventReceiver = null;
	public string callWhenEffectStarted = "OnEffectStarted";
	public string callWhenEffectDone = "OnEffectDone";
	
	
	private List<UITweener> m_tweenerObjects = new List<UITweener>();	
	protected bool m_effectStarted = false;
	
	public void startOpenEffect()
	{
		startEffect( true );
	}
	
	public void startCloseEffect()
	{
		startEffect ( false );
	}
	
	private void startEffect( bool isOpenEffect )
	{
		if ( !m_effectStarted )
		{
			m_effectStarted = true;
			
			if ( callWhenEffectStarted != null && eventReceiver != null )
			{
				eventReceiver.SendMessage( callWhenEffectStarted, this, SendMessageOptions.DontRequireReceiver );
			}
			
			if ( !customStartEffect(isOpenEffect) )
			{
				if ( dialog != null )
				{
					m_tweenerObjects.Clear();
					
					List<Transform> stack = new List<Transform>();
					stack.Add( dialog.transform );
					
					while ( stack.Count > 0 )
					{
						Transform tr = stack[0];
						stack.RemoveAt(0);
						UITweener tweener = null;
						
						if ( tr.gameObject.active )
						{
							if ( isOpenEffect )
							{
								tweener = startOpenEffectInternal( tr.gameObject );
							}
							else 
							{
								tweener = startCloseEffectInternal( tr.gameObject );
							}
							
							if ( tweener != null )
							{
								tweener.callWhenFinished = "OnTweenDone";
								tweener.eventReceiver = gameObject;
								m_tweenerObjects.Add( tweener );
							}
						}
						
						for ( int ii = 0; ii < tr.childCount; ++ii )
						{
							stack.Add( tr.GetChild(ii) );
						}
					}
					
					if ( m_tweenerObjects.Count <= 0 )
					{
						m_effectStarted = false;
						if ( callWhenEffectDone != null && eventReceiver != null )
						{
							eventReceiver.SendMessage( callWhenEffectDone, this, SendMessageOptions.DontRequireReceiver );
						}
					}
				}
			}
		}
	}
	
	abstract protected UITweener startOpenEffectInternal( GameObject widget );
	abstract protected UITweener startCloseEffectInternal( GameObject widget );
	
	virtual protected bool customStartEffect( bool isOpenEffect ) { return false; }
	
	private void OnTweenDone( UITweener tweener )
	{
		m_tweenerObjects.Remove( tweener );
		GameObject.Destroy( tweener );
		
		if ( m_tweenerObjects.Count <= 0 )
		{
			m_effectStarted = false;
			if ( callWhenEffectDone != null && eventReceiver != null )
			{
				eventReceiver.SendMessage( callWhenEffectDone, this, SendMessageOptions.DontRequireReceiver );
			}
		}
	}
	
	virtual protected void Update()
	{
		if ( m_effectStarted )
		{
			List<UITweener> tmplist = new List<UITweener>();
			foreach(UITweener tweener in m_tweenerObjects)
			{
				if ( tweener.gameObject.active == false )
				{
					tmplist.Add(tweener);
				}
			}
			
			foreach ( UITweener tweener in tmplist )
			{
				m_tweenerObjects.Remove( tweener );
			}
		}
	}
}
