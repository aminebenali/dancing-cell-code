using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUIDialogBase : NvUIBase 
{
	private bool m_run = false;
	
	protected int m_modalResult = -1;
	private bool m_modal = false;
	
	[SerializeField]
	private bool m_standalone = false;
	
	[SerializeField]
	protected NvUIDialogEffectBase m_dialogEffector = null;
	
	[SerializeField]
	private  bool m_IsCanMouseCheckScene = false;
	
	[SerializeField]
	private bool m_destroyOnClose = false;
	
	[SerializeField]
	protected NvUIButton m_CloseButton = null;
	
	// Layer Owner for dialog
	[HideInInspector]
	public NvUIDialogLayer LayerOwner = null;
	
	// Manager Owner
	[HideInInspector]
	public NvUIDialogManager DlgManagerOwner = null;
	
	[SerializeField]
	protected Collider m_LockDownCollider = null;
	
	[SerializeField]
	protected bool m_CloseMenuWhenOpen = false;
	
	
	private NvCommonUIManager m_CommonUIManager = null;
	
	public enum EState
	{
		E_STATE_UNKNOWN = 0,
		E_STATE_CLOSED,
		E_STATE_CLOSING,
		E_STATE_OPENED,
		E_STATE_OPENING
	}
	
	protected EState m_State = EState.E_STATE_UNKNOWN;
	
	// tab
	[HideInInspector]
	public NvUITab mTab = null;
	
	
	// close scene camera when dialog opened
	public bool m_closeSceneCamWhenOpened = false;
	
	
	// 
	public bool m_PlaySoundEffect = false;
	
	public bool isOpened
	{
		get { return m_State == EState.E_STATE_OPENED; }
	}
	
	public bool HasTabControl
	{
		get { return mTab != null; }
	}
	
	public NvUIDialogEffectBase dialogEffector
	{
		get { return m_dialogEffector; }
		set {
			
			if ( m_dialogEffector != value )
			{
				m_dialogEffector = value;
				
				if ( m_dialogEffector != null )
				{
					m_dialogEffector.eventReceiver = gameObject;
					m_dialogEffector.dialog = this;
				}
			}
		}
	}
	
	public void SelectTab( int index )
	{
		if ( HasTabControl )
		{
			mTab.m_defaultIndex = index;
		}
	}
	
	virtual public void Lock()
	{
		if ( m_LockDownCollider != null )
		{
			m_LockDownCollider.enabled = true;
		}
		
		m_CommonUIManager.ShowLoadingIndicator( true );
		
	}
	
	virtual public void Unlock()
	{
		if ( m_LockDownCollider != null )
		{
			m_LockDownCollider.enabled = false;
		}
		
		m_CommonUIManager.ShowLoadingIndicator( false );
	}
	
	public void SetSceneCheckEnable(bool isenable)
	{
		m_IsCanMouseCheckScene = isenable;
	}
	
	public bool IsSceneCheckEnable()
	{
		return m_IsCanMouseCheckScene;
	}
	
	public bool standalone
	{
		get { return m_standalone; }
	}
	
	private bool m_created = false;
	
	public void OnCreated()
	{
		if ( m_created == false )
		{
			m_created = true;
			
			// forcely apply anchor
			applyAnchors();
			
			OnDlgCreate();
			notify("OnDlgCreate");
		}
	}
		
	private void OnUpdate()
	{
		if ( m_run )
		{
			OnDlgRun();
		}
	}
	
	public override int depth
	{
		get { return m_depth; }
		set {
			
			if ( m_depth != value )
			{
				m_depth = value;	
				calcDepth();
			}
			
		}
	}
	
	public int modalResult
	{
		get { return m_modalResult; }
	}
	
	// post notification to attached objects
	private List<GameObject> m_notificationListeners = new List<GameObject>();
	
	// delegate for end modal
	public delegate void OnEndModal( int result );
	public event OnEndModal eventOnEndModal;
	
	
	virtual protected void OnDlgCreate() {}
	virtual protected void OnDlgBeginEffect( EDialogEffect effect ) {}
	virtual protected void OnDlgInit() {}
	virtual protected void OnDlgEndEffect( EDialogEffect effect ) {}
	virtual protected void OnDlgRun() {}
	
	
	virtual protected void OnDlgClose() {}
	virtual protected void OnDlgDestroy() {}
	
	
	private void OnDlgCloseInternal()
	{
		OnDlgClose();
		notify("OnDlgClose");
		
		if ( m_destroyOnClose )
		{
			destroyDialog();
		}
	}
	
	private bool m_doCleanup = false;
	
	//private Dictionary< Transform, bool > m_visibleTable = new Dictionary<Transform, bool>();
	
	public enum EDialogEffect
	{
		eDE_OpenEffect,
		eDE_CloseEffect
	}
	
	private EDialogEffect m_effect = EDialogEffect.eDE_OpenEffect;
	
	// current dialog effect
	public EDialogEffect dialogEffect
	{
		get { return m_effect; }
	}
	
	
	public bool isOpening
	{
		get { return m_State == EState.E_STATE_OPENING; }
	}
	
	public bool isClosing
	{
		get { return m_State == EState.E_STATE_CLOSING; }
	}
	
//	private bool buildVisibleTableCallback( Transform tr, object args )
//	{
//		NvUIBase elem = tr.GetComponent<NvUIBase>();
//		if ( elem == null )
//		{
//			saveVisibleToTable( tr, tr.gameObject.active );
//		}
//		else 
//		{
//			return true;
//		}
//		
//		return false;
//	}
	
//	protected override void AwakeImpl ()
//	{
//		// do nothing in dialog awake
//		enumWidgets(buildVisibleTableCallback);
//	}
	
	protected override void AwakeImpl()
	{
		m_CommonUIManager = Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager;
		
		Unlock();
		
		if ( m_CloseButton != null )
		{
			m_CloseButton.eventOnClick += (sender, context) => {
				
				closeDialog();
				
			};
		}
		
		m_State = EState.E_STATE_CLOSED;
	}
	
	public void openDialog( int tabIndex = 0 )
	{
		if ( !isOpened && !isOpening && !isClosing )
		{
			if ( m_PlaySoundEffect )
			{
				(Singlton.getInstance("NvSoundController") as NvSoundController).PlaySe("se_gather");	
			}
			
			if ( HasTabControl && tabIndex >= 0 )
			{
				mTab.m_defaultIndex = tabIndex;
			}
			
			OpenOrCloseUICamera(false);
			if ( m_dialogEffector != null )
			{
				// enable dialog
				//revertEnable();
				//revertVisible();
				
				// set visible for this object
				//base.setVisible(true);
				//base.setEnable(true);
				setEnable(true);
				setVisible(true);
				
				m_effect = EDialogEffect.eDE_OpenEffect;
				
				OnDlgBeginEffect(EDialogEffect.eDE_OpenEffect);
				notify("OnDlgBeginEffect");
				
				m_dialogEffector.startOpenEffect();
				m_State = EState.E_STATE_OPENING;
			}
			else
			{
				// enable dialog
				//revertEnable();
				//revertVisible();
				
				// set visible for this object
				//base.setVisible(true);
				//base.setEnable(true);
				
				setEnable(true);
				setVisible(true);
				
				// make dialog run
				m_run = true;
				
				OnDlgInit();
				notify("OnDlgInit");
				
				OpenOrCloseSceneCam(false);
				OpenOrCloseUICamera(true);
				m_State = EState.E_STATE_OPENED;
			}
		}
	}
	
//	private delegate bool widgetCallBack( Transform tr, object args );
//	
//	private void enumWidgets( widgetCallBack cb, object args = null )
//	{
//		List<Transform> stack = new List<Transform>();
//		for ( int ii = 0; ii < transform.childCount; ++ii )
//		{
//			stack.Add( transform.GetChild(ii) );
//		}
//		
//		while ( stack.Count > 0 )
//		{
//			Transform tr = stack[0];
//			stack.RemoveAt(0);
//			
//			if ( !cb( tr, args ) )
//			{
//				for ( int ii = 0; ii < tr.childCount; ++ii )
//				{
//					stack.Add( tr.GetChild(ii) );
//				}
//			}
//		}
//	}
	
	private bool setEnableCallback( Transform tr, object args )
	{
		bool flag = (bool)args;
		
		NvUIBase elem = tr.GetComponent<NvUIBase>();
		if ( elem != null )
		{
			elem.setEnable( flag );
			return true;
		}
		else
		{
			if ( tr.collider != null )
			{
				tr.collider.enabled = flag;
			}
		}
		
		return false;
	}
	
	public override void setEnable( bool flag )
	{
		base.setEnable( flag );
		
		Utility.enumWidgets( transform, setEnableCallback, flag as object );
	}
	
//	private bool revertEnableCallback( Transform tr, object args )
//	{
//		NvUIBase elem = tr.GetComponent<NvUIBase>();
//		if ( elem != null )
//		{
//			elem.revertEnable();
//			return true;
//		}
//		
//		return false;
//	}
//	
//	public override void revertEnable()
//	{
//		base.revertEnable();
//		enumWidgets( revertEnableCallback );
//	}
	
	
	private bool setVisibleCallback( Transform tr, object args )
	{
		bool flag = (bool)args;
		
		NvUIBase elem = tr.GetComponent<NvUIBase>();
		bool ret = false;
		
		if ( elem != null )
		{
			elem.setVisible( flag );
			ret = true;
		}
		else
		{
			tr.gameObject.active = flag;
		}
		
		UIAnchor anchor = tr.GetComponent<UIAnchor>();
		if ( anchor != null )
		{
			anchor.enabled = flag;
		}
		
		Collider col = tr.GetComponent<Collider>();
		if ( col != null )
		{
			col.enabled = flag;
		}
		
		return ret;
	}
	
	public override void setVisible( bool flag )
	{
		base.setVisible( flag );
		Utility.enumWidgets( transform, setVisibleCallback, flag as object );
	}
	
//	private bool revertVisibleCallback( Transform tr, object args )
//	{
//		NvUIBase elem = tr.GetComponent<NvUIBase>();
//		if ( elem != null )
//		{
//			elem.revertVisible();
//			return true;
//		}
//		else
//		{
//			bool flag = revertVisibleFromTable( tr );
//			Debug.Log(">>>>>>>>>>>>>>>>>>>>>> revert visible (" + tr.name + ") = " + flag );
//			saveVisibleToTable( tr, flag );
//			tr.gameObject.active = flag;
//		}
//		
//		return false;
//	}
//	
//	public override void revertVisible()
//	{
//		base.revertVisible();
//		enumWidgets( revertVisibleCallback );
//	}
//	
//	private bool revertVisibleFromTable( Transform obj )
//	{
//		if ( m_visibleTable.ContainsKey( obj ) )
//		{
//			return m_visibleTable[obj];
//		}
//		return true;
//	}
//	
//	private void saveVisibleToTable( Transform obj, bool flag )
//	{
//		if ( obj != null )
//		{
//			m_visibleTable[ obj ] = flag;
//		}
//	}
	
	private void OpenOrCloseSceneCam( bool flag )
	{
		if ( m_closeSceneCamWhenOpened )
		{
			NvGameUIManager mgr = Singlton.getInstance("NvGameUIManager") as NvGameUIManager;
			if ( mgr.CachedMainCamera != null )
			{
				mgr.CachedMainCamera.enabled = flag;
			}
		}
		
		if ( m_CloseMenuWhenOpen )
		{
			Utility.SetMenuBarVisible( flag );
		}
	}
	
	private void OpenOrCloseUICamera( bool flag )
	{
		Camera[] cameras = NGUITools.FindActive<Camera>();
		foreach ( Camera cam in cameras )
		{
			UICamera uicam = cam.GetComponent<UICamera>();
			if ( uicam != null )
			{
				uicam.enabled = flag;
			}
		}
	}
	
	public void closeDialog()
	{
		if ( m_run == true )
		{
			OpenOrCloseSceneCam(true);
			OpenOrCloseUICamera(false);
			
			if ( m_dialogEffector != null )
			{
				m_effect = EDialogEffect.eDE_CloseEffect;
				OnDlgBeginEffect(EDialogEffect.eDE_CloseEffect);
				notify("OnDlgBeginEffect");
				
				m_dialogEffector.startCloseEffect();
				m_State = EState.E_STATE_CLOSING;
			}
			else
			{	
				OnDlgCloseInternal();
				
				setEnable ( false );
				setVisible( false );
				
				m_run = false;
				
				if ( m_modal )
				{
					
					m_modal = false;
					
					if ( eventOnEndModal != null )
					{
						eventOnEndModal( m_modalResult );
						// clean event chain when its done
						eventOnEndModal = null;
					}
				}
				
				OpenOrCloseUICamera(true);
				m_State = EState.E_STATE_CLOSED;
			}
		}
	}
	
	public void destroyDialog()
	{
		if ( LayerOwner != null )
		{
			LayerOwner.removeDialog( transform.name );
		}
		
		if ( DlgManagerOwner != null )
		{
			DlgManagerOwner.RemoveDialog( transform.name );
		}
		
		OnDlgDestroy();
	
		notify("OnDlgDestroy");
		
		GameObject.Destroy( gameObject );
	}
	
	public void showDialog( bool flag )
	{
		setVisible( flag );
	}
	
	public void doModal()
	{
		if ( m_modal == false )
		{
			openDialog();
			m_modal = true;
		}
	}
	
	public void endModal( int result )
	{
		if ( m_modal == true )
		{
			m_modalResult = result;
			closeDialog();
		}
	}
	
	public void registerNotifiactionListener( GameObject obj )
	{
		if ( obj != null )
		{
			if ( m_notificationListeners.Contains( obj ) == false )
			{
				m_notificationListeners.Add ( obj );
			}
		}
	}
	
	public void unregisterNotificationListener( GameObject obj )
	{
		if ( m_notificationListeners.Contains( obj ) )
		{
			m_notificationListeners.Remove( obj );
		}
	}
	
	public void notify( string funcName )
	{
		foreach ( GameObject obj in m_notificationListeners )
		{
			if ( obj == null )
			{
				// obj is already expired or damaged!
				m_doCleanup = true;
			}
			else 
			{
				obj.SendMessage( funcName, this as object, SendMessageOptions.DontRequireReceiver );
			}
		}
	}
	
	protected override void lazyUpdateImpl()
	{
		if ( enable )
		{
			if ( m_doCleanup )
			{
				m_doCleanup = false;
				
				List<int> idxs = new List<int>();
				
				for ( int ii = 0; ii < m_notificationListeners.Count; ++ii )
				{
					if ( m_notificationListeners[ii] == null )
					{
						idxs.Add( ii );
					}
				}
				
				foreach ( int idx in idxs )
				{
					m_notificationListeners.RemoveAt( idx );
				}
			}
			
			if ( m_run )
			{
				// update controls
				List<Transform> stack = new List<Transform>();
				for ( int ii = 0; ii < transform.childCount; ++ii )
				{
					stack.Add( transform.GetChild(ii) );
				}
				
				while ( stack.Count > 0 )
				{
					Transform tr = stack[0];
					stack.RemoveAt(0);
					
					NvUIBase elem = tr.GetComponent<NvUIBase>();
					if ( elem != null )
					{
						elem.lazyUpdate();
					}
					
					for ( int ii = 0; ii < tr.childCount; ++ii )
					{
						stack.Add( tr.GetChild(ii) );
					}
				}
			}
			
			// custom update
			OnUpdate();
		}
	}
	
	private void Update()
	{
		if ( standalone )
		{
			lazyUpdate();
		}
	}
	
	private void OnEffectDone()
	{
		OnDlgEndEffect(m_effect);
		notify("OnDlgEndEffect");
		
		OpenOrCloseUICamera(true);
		if ( m_effect == EDialogEffect.eDE_OpenEffect )
		{	
			m_run = true;
			OnDlgInit();
			notify("OnDlgInit");
			
			OpenOrCloseSceneCam(false);
			m_State = EState.E_STATE_OPENED;
			
		}
		else if ( m_effect == EDialogEffect.eDE_CloseEffect )
		{	
			OnDlgCloseInternal();
			
			setEnable ( false );
			setVisible( false );
			
			m_run = false;
			
			if ( m_modal )
			{
				m_modal = false;
				
				if ( eventOnEndModal != null )
				{
					eventOnEndModal( m_modalResult );
					// clean event chain when its done
					eventOnEndModal = null;
				}
			}
			m_State = EState.E_STATE_CLOSED;
		}
	}
	
	private void calcDepth()
	{
		List<Transform> stack = new List<Transform>();
		stack.Add( transform );

		while ( stack.Count > 0 )
		{
			Transform tr = stack[0];
			stack.RemoveAt(0);
			
			UIAnchor anchor = tr.GetComponent<UIAnchor>();
			if ( anchor != null )
			{
				anchor.depthOffset = anchor.depthOffset -  m_depth * 0.01f;
			}
			
			for ( int ii = 0; ii < tr.childCount; ++ii )
			{
				stack.Add( tr.GetChild(ii) );
			}
		}
	}
	
	public void enableAnchors( bool flag )
	{
		List<Transform> stack = new List<Transform>();
		stack.Add( transform );
		
		while ( stack.Count > 0 )
		{
			Transform tr = stack[0];
			stack.RemoveAt(0);
			
			UIAnchor anchor = tr.GetComponent<UIAnchor>();
			if ( anchor != null )
			{
				anchor.enabled = flag;
			}
			
			for ( int ii = 0; ii < tr.childCount; ++ii )
			{
				stack.Add( tr.GetChild(ii) );
			}
		}
	}
	
	public void applyAnchors()
	{
		List<Transform> stack = new List<Transform>();
		stack.Add( transform );
		
		while ( stack.Count > 0 )
		{
			Transform tr = stack[0];
			stack.RemoveAt(0);
			
			UIAnchor anchor = tr.GetComponent<UIAnchor>();
			if ( anchor != null )
			{
				anchor.SendMessage("OnEnable");
				anchor.SendMessage("Update");
			}
			
			for ( int ii = 0; ii < tr.childCount; ++ii )
			{
				stack.Add( tr.GetChild(ii) );
			}
		}
	}
}
