using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ui element callback
public delegate void OnElementTriggered( object sender, object context );


public class NvUIDialogLayerHandler
{
	
	private NvUIDialogLayer m_owner = null;
	
	public bool isValid
	{
		get { return m_owner != null; }
	}
	
	public int depth
	{
		get {
			return isValid ? m_owner.depth : 0;
		}
		
		set {
			if ( isValid )
			{
				m_owner.depth = value;
			}
		}
	}
	
	public NvUIDialogLayerHandler( NvUIDialogLayer layer )
	{
		m_owner = layer;
	}
	
	public void openDialog( string dlgName )
	{
		NvUIDialogBase dlg = m_owner.findDialog( dlgName );
		if ( dlg != null )
		{
			dlg.openDialog();
		}
	}
	
	public void closeDialog( string dlgName )
	{
		NvUIDialogBase dlg = m_owner.findDialog( dlgName );
		if ( dlg != null )
		{
			dlg.closeDialog();
		}
	}
	
	public void hideDialog( string dlgName )
	{
		NvUIDialogBase dlg = m_owner.findDialog( dlgName );
		if ( dlg != null )
		{
			dlg.showDialog( false );
		}
	}
	
	public bool destroyDialog( string dlgName )
	{
		return m_owner.destoryDialog( dlgName );
	}
	
	public bool removeDialog( string dlgName )
	{
		return m_owner.removeDialog( dlgName );
	}
	
	public NvUIDialogBase findDialog( string dlgName )
	{
		return m_owner.findDialog( dlgName );
	}
	
	public NvUIDialogBase createDialog( string dlgName, string dlgPrefab )
	{
		return m_owner.createDialog( dlgName, dlgPrefab );
	}
	
	public NvUIDialogBase createDialog( string dlgName, GameObject dlgPrefab )
	{
		return m_owner.createDialog( dlgName, dlgPrefab );
	}
	
	public void destroyAllDialogs()
	{
		m_owner.destroyAllDialogs();
	}
}


public class NvUIDialogLayer
{
	
	// the map stores dialogs by their names
	private Dictionary< string, NvUIDialogBase > m_dialogChain = new Dictionary<string, NvUIDialogBase>();
	private NvUIDialogLayerHandler m_handler = null;
	
	private int m_depth = 0;
	
	public int depth
	{
		get { return m_depth; }
		set {
			
			if ( m_depth != value )
			{
				m_depth = value;
				
				// recalculate all dialog depth stored in NvUIDialogBase
				recalcDepth();
			}
		}
	}
	
	// dialog will be spawned on this root!!!
	protected Transform m_spawnRoot = null;
	
	private bool m_enable = true;
	
	public bool enable
	{
		get { return m_enable; }
		set {
			
			if ( m_enable != value )
			{
				m_enable = value;
			}
		}
	}
	
	public NvUIDialogLayer( Transform spawnRoot )
	{
		m_spawnRoot = spawnRoot;
	}
	
	public NvUIDialogLayerHandler getHandler() 
	{
		if ( m_handler == null )
		{
			m_handler = new NvUIDialogLayerHandler( this );
		}
		
		return m_handler;
	}
	
	public NvUIDialogBase createDialog( string dlgName, string dlgPrefab )
	{
		if ( m_dialogChain.ContainsKey( dlgName ) )
		{
			return m_dialogChain[dlgName];
		}
		else
		{
			GameObject dlgPrefabObj = Resources.Load ( dlgPrefab ) as GameObject;
			return createDialog( dlgName, dlgPrefabObj );
		}
	}
	
	public NvUIDialogBase createDialog( string dlgName, GameObject dlgPrefabObj )
	{
		if ( !m_dialogChain.ContainsKey( dlgName ) && dlgPrefabObj != null )
		{
			GameObject dlgObj = Utility.Instantiate ( dlgPrefabObj ) as GameObject;
			dlgObj.name = dlgName;
			dlgObj.transform.parent = m_spawnRoot;
			dlgObj.transform.localPosition = Vector3.zero;
			dlgObj.transform.localRotation = Quaternion.identity;
			dlgObj.transform.localScale = Vector3.one;
			
			NvUIDialogBase dlgBase = dlgObj.GetComponent<NvUIDialogBase>();
			if ( dlgBase != null )
			{
				dlgBase.LayerOwner = this;
				dlgBase.depth = dlgBase.depth + m_depth;
				dlgBase.OnCreated();
				dlgBase.setVisible(false);
				dlgBase.setEnable(false);
				m_dialogChain.Add( dlgName, dlgBase );
			}
			else 
			{
				GameObject.DestroyImmediate( dlgObj );
			}
			
			return dlgBase;
		}
		
		return null;
	}
	
	public NvUIDialogBase findDialog( string dlgName )
	{
		if ( m_dialogChain.ContainsKey( dlgName ) )
		{
			return m_dialogChain[dlgName];
		}
		else
		{
			return null;
		}
	}
	
	public bool destoryDialog( string dlgName )
	{
		NvUIDialogBase dlg = findDialog( dlgName );
		if ( dlg != null )
		{
			dlg.destroyDialog();
			//m_dialogChain.Remove( dlgName );
			return true;
		}
		return false;
	}
	
	public bool removeDialog( string dlgName )
	{
		if ( m_dialogChain.ContainsKey( dlgName ) )
		{
			m_dialogChain.Remove( dlgName );
			return true;
		}
		return false;
	}
	
	public void destroyAllDialogs()
	{
		List<NvUIDialogBase> tmp = new List<NvUIDialogBase>();
		foreach ( KeyValuePair<string, NvUIDialogBase> kv in m_dialogChain )
		{
			tmp.Add( kv.Value );
		}
		
		foreach ( NvUIDialogBase dlg in tmp )
		{
			dlg.destroyDialog();
		}
		
		m_dialogChain.Clear();
	}
	
	private void recalcDepth()
	{
		foreach ( KeyValuePair<string, NvUIDialogBase> kv in m_dialogChain )
		{
			kv.Value.depth = kv.Value.depth + m_depth;
		}
	}
	
	public void update()
	{
		if ( m_enable )
		{
			// call update for all dialogs
			foreach ( KeyValuePair<string, NvUIDialogBase> kv in m_dialogChain )
			{
				NvUIDialogBase dlg = kv.Value;
				if ( dlg.enable && dlg.standalone == false )
				{
					dlg.lazyUpdate(); 
				}
			}
		}
	}
	
	public bool IsSceneCheckEnable()
	{
		foreach(NvUIDialogBase dlg in m_dialogChain.Values)
		{
			if(!dlg.IsSceneCheckEnable() && dlg.isOpened)
			{
				return false;
			}
		}
		return true;
	}
}