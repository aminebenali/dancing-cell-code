using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NvUIDialogTabPage : NvUIDialogBase
{
	public NvUIDialogBase TabOwner
	{
		get {
		
			if ( mTab != null )
			{
				return mTab.dialogOwner;
			}
			
			return null;
		}
	}
	
	public void LockTab( bool flag )
	{
		NvUIDialogBase dlg = TabOwner;
		if ( dlg != null )
		{
			if ( flag )
			{
				dlg.Lock();
			}
			else
			{
				dlg.Unlock();
			}
		}
	}
	
	protected override void OnDlgInit ()
	{
		if ( mTab != null )
		{
			StartCoroutine("CloseOtherPagesAsync");
		}
	}
	
	private IEnumerator CloseOtherPagesAsync()
	{
		yield return new WaitForEndOfFrame();
		mTab.SendMessage("OnPageOpened");
	}
}


[System.Serializable]
public class NvUITabPage
{
	private NvUITab m_tabControl = null;
	
	public GameObject m_dialogPrefab = null;
	public int m_dialogDepth = 0;
	
	public NvUIDialogBase m_dialogOwner = null;
	
	// dialog script grab from dialog object( m_dialog )
	private NvUIDialogBase m_dlgScript = null;
	
	public bool isCreatable
	{
		get { return m_dialogPrefab != null; }
	}
	
	public bool isOpened
	{
		get {
			
			if ( m_dlgScript != null )
			{
				return m_dlgScript.isOpened;
			}
			return false;
		}
	}
	
	public NvUIDialogBase Dialog
	{
		get { return m_dlgScript; }
	}
	
	public NvUITabPage( NvUITab tab )
	{
		m_tabControl = tab;
	}
	
	public void open()
	{
		if ( m_dlgScript == null )
		{
			if ( m_dialogPrefab != null && m_tabControl != null )
			{
				GameObject dlg = Utility.Instantiate( m_dialogPrefab ) as GameObject;
				dlg.name = m_dialogPrefab.name;
				dlg.transform.parent = m_tabControl.spawnTarget;
				dlg.transform.localPosition = Vector3.zero;
				dlg.transform.localRotation = Quaternion.identity;
				dlg.transform.localScale = Vector3.one;
				
				m_dlgScript = dlg.GetComponent<NvUIDialogBase>();
				int depthFixed = m_dialogOwner != null ? m_dialogOwner.depth : 0;
				if ( m_dlgScript != null )
				{
					m_dlgScript.depth = m_dialogDepth + depthFixed;
					m_dlgScript.OnCreated();
					m_dlgScript.setVisible( false );
					m_dlgScript.setEnable( false );
				}
				
				m_dlgScript.mTab = m_tabControl;
				
				// assign camera to anchor
				List<Transform> stack = new List<Transform>();
				stack.Add( dlg.transform );
				while ( stack.Count > 0 )
				{
					Transform tr = stack[0];
					stack.RemoveAt(0);
					UIAnchor ac = tr.GetComponent<UIAnchor>();
					if ( ac != null )
					{
						//Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>> anchor = " + ac.name);
						ac.uiCamera = (Singlton.getInstance("NvGameUIManager") as NvGameUIManager).GameUICamera;
					}
					
					for ( int ii = 0 ; ii < tr.childCount; ++ii )
					{
						stack.Add( tr.GetChild(ii) );
					}
				}
			}	
		}
			
		if ( m_dlgScript != null && m_dlgScript.isOpened == false )
		{
			m_dlgScript.openDialog();
		}
	}
	
	public void close()
	{
		if ( m_dlgScript != null && m_dlgScript.isOpened )
		{
			m_dlgScript.closeDialog();
		}
	}
	
	public void lazyUpdate()
	{
		if ( m_dlgScript != null )
		{
			m_dlgScript.lazyUpdate();
		}
	}
	
}

public class NvUITab : NvUIStatic 
{
	// related dialog will be spwaned and attach on this transform
	public Transform spawnTarget = null;
	public NvUIDialogBase dialogOwner = null;
	
	[System.Serializable]
	public class TabContext
	{
		public NvUIButton tabButton = null;
		public GameObject dialogPrefab = null;	
		public int dialogDepth = 0;
	}
	
	public TabContext[] m_tabContexts = null;
	
	// tab page will be generated from context table
	private NvUITabPage[] m_tabPages = null;
	
	// active default tab page by index
	public int m_defaultIndex = 0;
	
	private List<NvUITabPage> mTabsNeedToBeClosed = new List<NvUITabPage>();
	
	//private NvTutorial mTutorial;
	
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
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl();
		
		if ( dialogOwner != null )
		{
			dialogOwner.registerNotifiactionListener(gameObject);
		}
		
		//enumWidgets(buildVisibleTableCallback);
		
		if ( m_tabContexts != null && m_tabContexts.Length > 0 )	
		{
			m_tabPages = new NvUITabPage[ m_tabContexts.Length ];
			int ii = 0;
			foreach ( TabContext ctx in m_tabContexts )
			{
				m_tabPages[ii] = new NvUITabPage( this );
				m_tabPages[ii].m_dialogPrefab = ctx.dialogPrefab;
				m_tabPages[ii].m_dialogDepth = ctx.dialogDepth;
				m_tabPages[ii].m_dialogOwner = dialogOwner;
				if ( ctx.tabButton != null )
				{
					ctx.tabButton.index = ii;
					ctx.tabButton.eventOnClick += OnTabClicked;
				}
				++ii;
			}
		}
		
		//mTutorial = NvTutorialManager.getInstance().CurrentTutorial;
	}
	
	public void activeTab( int index )
	{
		if ( index >= 0 && index < m_tabPages.Length )
		{
			if ( m_tabPages[index].isOpened == false )
			{
				NvUICheckbox chkbox = m_tabContexts[index].tabButton as NvUICheckbox;
				if ( chkbox != null )
				{
					chkbox.setChecked(true);
					chkbox.SendMessage("OnClickWithoutSoundEffect");
				}
			}
		}
	}
	
	protected override void lazyUpdateImpl ()
	{	
		foreach ( NvUITabPage page in m_tabPages )
		{
			page.lazyUpdate();
		}
	}
	
	private void OnTabClicked( object sender, object context )
	{
		int index = (int)context;
		bool bTabOpened = false;
		
		for( int ii = 0; ii < m_tabPages.Length; ++ii )
		{
			NvUITabPage page = m_tabPages[ii];
			if ( ii == index )
			{
				if ( page.isOpened == false && page.isCreatable )
				{
					//page.open();
					bTabOpened = true;
				}
				
				if ( m_tabContexts[ii].tabButton is NvUICheckbox )
				{
					NvUICheckbox chkbox = m_tabContexts[ii].tabButton as NvUICheckbox;
					chkbox.setChecked( true );
				}
				
				break;
			}
		}
		
		for ( int ii = 0; ii < m_tabPages.Length; ++ii )
		{
			NvUITabPage page = m_tabPages[ii];
			if ( ii != index )
			{
				if ( page.isOpened && bTabOpened )
				{
					mTabsNeedToBeClosed.Add( page );
				}
				
				if ( m_tabContexts[ii].tabButton is NvUICheckbox )
				{
					NvUICheckbox chkbox = m_tabContexts[ii].tabButton as NvUICheckbox;
					chkbox.setChecked( false );
				}
			}
		}
		
		if ( index >= 0 )
		{
			m_tabPages[index].open();
		}
		
		//mTutorial.TutorialChkTabButton ( index );
	}
	
	private delegate bool widgetCallBack( Transform tr, object args );
	
	private void enumWidgets( widgetCallBack cb, object args = null )
	{
		List<Transform> stack = new List<Transform>();
		for ( int ii = 0; ii < transform.childCount; ++ii )
		{
			stack.Add( transform.GetChild(ii) );
		}
		
		while ( stack.Count > 0 )
		{
			Transform tr = stack[0];
			stack.RemoveAt(0);
			
			if ( !cb( tr, args ) )
			{
				for ( int ii = 0; ii < tr.childCount; ++ii )
				{
					stack.Add( tr.GetChild(ii) );
				}
			}
		}
	}
	
	private bool setEnableCallback( Transform tr, object args )
	{
		bool flag = (bool)args;
		
		NvUIBase elem = tr.GetComponent<NvUIBase>();
		if ( elem != null && !(elem is NvUIDialogBase) )
		{
			elem.setEnable( flag );
			return true;
		}
		else if ( elem is NvUIDialogBase )
		{
			return true;
		}
		
		return false;
	}
	
	public override void setEnable( bool flag )
	{
		base.setEnable( flag );
		Utility.enumWidgets( transform, setEnableCallback, flag as object );
	}
	
	private bool setVisibleCallback( Transform tr, object args )
	{
		bool flag = (bool)args;
		
		NvUIBase elem = tr.GetComponent<NvUIBase>();
		
		if ( elem != null  && !(elem is NvUIDialogBase) )
		{
			elem.setVisible( flag );
			return true;
		}
		else if ( !(elem is NvUIDialogBase ) )
		{
			tr.gameObject.active = flag;
		}
		else
		{
			return true;
		}
		
		return false;
	}
	
	public override void setVisible( bool flag )
	{
		base.setVisible( flag );
		Utility.enumWidgets( transform, setVisibleCallback, flag as object );
	}
	

	private bool mActiveDefaultTab = true;
	
	private void OnDlgInit()
	{
		if ( mActiveDefaultTab )
		{
			activeTab(m_defaultIndex);
			mActiveDefaultTab = false;
		}
	}
	
	private void OnDlgBeginEffect( NvUIDialogBase dlg )
	{
		if ( dlg.dialogEffect == NvUIDialogBase.EDialogEffect.eDE_OpenEffect )
		{
			activeTab(m_defaultIndex);
			mActiveDefaultTab = false;
		}
	}
	
	private void OnDlgClose()
	{
		mActiveDefaultTab = true;
		// keep it clear for spawning new page
		transform.localPosition = Vector3.zero;
		
		foreach ( NvUITabPage page in m_tabPages )
		{
			if ( page.isOpened )
			{
				page.Dialog.closeDialog();
			}
		}
	}
	
	private void OnPageOpened()
	{
		if ( mTabsNeedToBeClosed.Count > 0 )
		{
			
			foreach ( NvUITabPage page in mTabsNeedToBeClosed )
			{
				page.close();
			}
			
			mTabsNeedToBeClosed.Clear();
			
		}
	}
}
