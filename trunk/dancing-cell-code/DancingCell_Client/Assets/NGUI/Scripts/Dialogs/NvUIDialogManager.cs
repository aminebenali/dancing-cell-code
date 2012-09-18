using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUIDialogManager : MonoBehaviour {
	
	private NvGameUISceneManager mGameSceneMgr = null;
	
	public enum EDialogLayer
	{
		eDlgLayer_Default = 0,
		eDlgLayer_HUD,
		eDlgLayer_InGameDialog,
		eDlgLayer_TopmostDialog,
		// add your layer here!
		
		_eDlgLayer_Number_
	}
	
	//
	private Dictionary< string, NvUIDialogBase > mDialogTable = new Dictionary<string, NvUIDialogBase>();
	
	[System.Serializable]
	public class DialogContext
	{
		public string dialogName;
		public GameObject dialogPrefab;
		public int depth = 0;
		public EDialogLayer layer = EDialogLayer.eDlgLayer_Default;
	}
	
	[SerializeField]
	private DialogContext[] mContextTable = null;
	
	void Awake()
	{
		mGameSceneMgr = Singlton.getInstance("NvGameUISceneManager") as NvGameUISceneManager;
		mGameSceneMgr.registerUIDialogManager( this );
	}
	
	public NvUIDialogBase OpenDialog( string dlgName, int tabIndex = 0 )
	{
		if ( dlgName == "" ) return null;
		
		if ( mDialogTable.ContainsKey( dlgName ) )
		{
			NvUIDialogBase dlg = mDialogTable[ dlgName ];
			if ( dlg.isOpened == false )
			{
				dlg.openDialog( tabIndex );
				return dlg;
			}
		}
		else 
		{
			NvUIDialogBase dlg = CreateDialog( dlgName );
			if ( dlg != null )
			{
				dlg.openDialog( tabIndex );
			}
			
			return dlg;
		}
		return null;
	}
	
	public NvUIDialogBase FindDialog( string dlgName )
	{
		if ( mDialogTable.ContainsKey(dlgName) )
		{
			return mDialogTable[ dlgName ];
		}
		return null;
	}
	
	public void DestroyDialog( string dlgName )
	{
		NvUIDialogBase dlg = FindDialog( dlgName );
		DestroyDialog( dlg ) ;
	}
	
	public void DestroyDialog( NvUIDialogBase dlg )
	{
		if ( dlg != null )
		{
			foreach ( KeyValuePair< string, NvUIDialogBase > kv in mDialogTable )
			{
				if ( kv.Value == dlg )
				{
					dlg.destroyDialog();
					mDialogTable.Remove( kv.Key );
					break;
				}
			}
		}
	}
	
	public void RemoveDialog( string dlgName )
	{
		if ( mDialogTable.ContainsKey( dlgName ) )
		{
			mDialogTable.Remove( dlgName );
		}
	}
	
	private bool findTabCallback( Transform tr, object args )
	{
		NvUIDialogBase dlg = args as NvUIDialogBase;
		
		if ( dlg.HasTabControl )
		{
			return true;
		}
		else
		{
			NvUITab tab = tr.GetComponent<NvUITab>();
			if ( tab != null )
			{
				dlg.mTab = tab;
				return true;
			}
		}
		
		return false;
	}
	
	public NvUIDialogBase CreateDialog( string dlgName )
	{
		if ( mGameSceneMgr == null )
		{
			Debug.Log ("NvUIDialogManager: Wait mGameSceneMgr before using this function!");
			return null;
		}
		
		foreach ( DialogContext ctx in mContextTable )
		{
			if ( ctx.dialogPrefab == null ) continue;
			
			if ( ( ctx.dialogName != "" && ctx.dialogName == dlgName ) || ( dlgName == ctx.dialogPrefab.name ) )
			{
				// spawn dialog
				NvUIDialogLayerHandler handler = mGameSceneMgr.findDlgLayer( ctx.layer.ToString(), true );
				
				if ( handler.depth != (int)ctx.layer )
				{
					handler.depth = (int)ctx.layer;
				}
				//Debug.Log("=======>   dlgName: "+dlgName+ "   ctx.dialogPrefab:  "+ctx.dialogPrefab);
				NvUIDialogBase dlg = handler.createDialog( dlgName, ctx.dialogPrefab );
				dlg.depth = ctx.depth + (int)ctx.layer;
				dlg.DlgManagerOwner = this;
				
				// find tab
				Utility.enumWidgets( dlg.transform, findTabCallback, dlg as object );
				
				mDialogTable.Add( dlgName, dlg );
				
				return dlg;
			}
		}
		return null;
	}

	void OnDestroy()
	{
	//	foreach ( KeyValuePair< string, NvUIDialogBase > kv in mDialogTable )
	//	{
	//		NvUIDialogBase dlg = kv.Value;
	//		dlg.destroyDialog();
	//	}
		mDialogTable.Clear();
	}
}
