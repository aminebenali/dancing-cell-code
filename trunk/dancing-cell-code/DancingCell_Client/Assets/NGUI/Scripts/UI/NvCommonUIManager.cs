using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NvCommonUIManager : MonoBehaviour {
	
	public string[] loadingSprite = 
	{
		"qu",
		"yu",
		"hou"		
	};
	

	private List< NvUIBaseSceneManager> m_UISceneMgrs = new List<NvUIBaseSceneManager>();
	//private Dictionary<string, NvUIDialogBase> m_dialogChain = new Dictionary<string, NvUIDialogBase>();
	private NvUIDialogLayer m_CommonLayer = null;
	
	// the modal dialog which is working on the current stage
	private NvUIDialogBase m_workingDialog = null;
	
	// baffle
	private BoxCollider m_baffle = null;
	
	// loading indicator
	private Camera m_LoadingCam = null;
	private UISprite m_loadingIndicatorSprite = null;
	
	private Camera m_CLRCam = null;
	//private Camera m_LoadingPgCam = null;
	private UISprite m_LoadingSprite = null;
	private UISprite m_LoadingPgSprite = null;
	private UISlider m_LoadingProgressBar = null;
	private TweenTransform m_LoadingPgRotator = null;
	private UILabel m_Tips = null;
	
	private BoxCollider m_LoadingCollider = null;
	
	// indicator rotator
	private TweenTransform m_loadingIndicatorRotator = null;
	
	private bool m_WillShowIndicator = false;
	private float m_ShowIndicatorThreshold = 0.5f;
	private float m_ShowIndicatorAccl = 0.0f;
	
	private bool m_PgWillShowIndicator = false;
	private float m_PgShowIndicatorThreshold = 0.5f;
	private float m_PgShowIndicatorAccl = 0.0f;	
	
	//private List< MsgBase > m_QueuePopupFailedMessage = new List<MsgBase>();
	//private List< MsgBase > m_QueuePopupInfoMessage = new List<MsgBase>();
	class MessageDescriptor
	{
		public string title;
		public string details;
	   // public NvUIDialogBase.OnEndModal cb;
	}
	
	private List< MessageDescriptor > m_QueuePopupFailedMessage = new List<MessageDescriptor>();
	private List< MessageDescriptor > m_QueuePopupInfoMessage = new List<MessageDescriptor>();
	
	public void registerSceneManagr( NvUIBaseSceneManager mgr )
	{
		if ( mgr != null )
		{
			if ( m_UISceneMgrs.Contains( mgr ) == false )
			{
				m_UISceneMgrs.Add( mgr );
			}
		}
	}
	
	public void unregisterSceneManager( NvUIBaseSceneManager mgr )
	{
		if ( m_UISceneMgrs.Contains( mgr ) )
		{
			m_UISceneMgrs.Remove( mgr );
		}
	}
	
//	public void pushFailedMessage( MsgBase msg )
//	{
//		if ( msg != null )
//		{
//			MsgError errMsg = msg as MsgError;
//			
//			if ( errMsg != null )
//			{
//				MessageDescriptor ds = new MessageDescriptor();
//				ds.title = errMsg.err.message.title;
//				ds.details = errMsg.err.message.message;
//				ds.cb = null;
//				m_QueuePopupFailedMessage.Add( ds );
//				
//				NvGame.gIsGamePaused = true;
//			}
//		}
//	}
	
//	public void pushInfoMessage( MsgBase msg )
//	{
//		if ( msg != null )
//		{
//			if ( msg.msg.Count > 0 )
//			{
//				foreach ( MsgItem item in msg.msg )
//				{
//					MessageDescriptor ds = new MessageDescriptor();
//					ds.title = item.title;
//					ds.details = item.message;
//					ds.cb = null;
//					m_QueuePopupInfoMessage.Add( ds );
//				}
//				NvGame.gIsGamePaused = true;
//			}
//		}
//	}

//	public void pushQuestCompMessage( MsgBase msg )
//	{
//		if ( msg != null )
//		{
//			if (msg.quest_comp != null && msg.quest_comp.Count > 0 )
//			{
//				foreach ( string  str in msg.quest_comp.Keys )
//				{
//					MessageDescriptor ds = new MessageDescriptor();
//					NvLocalizationManager lm = ( Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager );
//					ds.title = lm.GetValue("QuestCompInfoTitle1");
//					ds.details = lm.GetValue("QuestCompInfoBody");
//					ds.cb = RefreshQuest;					
//					m_QueuePopupInfoMessage.Add( ds );
//				}
//				NvGame.gIsGamePaused = true;
//			}
//		}
//	}
	
	public void RefreshQuest( int code )
	{
//		if ( NvGlobalFunc.GetNvConstructionController() != null ){
//			NvConstructionView m_View = NvGlobalFunc.GetNvConstructionController().findView("NvConstructionView")as NvConstructionView;
//			m_View.RequestQuest();
//		}
	}
	
	public void pushFailedMessage( string title, string details )
	{
		if ( details != "" )
		{
			MessageDescriptor ds = new MessageDescriptor();
			ds.title = title;
			ds.details = details;
			m_QueuePopupFailedMessage.Add( ds );
			//NvGame.gIsGamePaused = true;
		}
	}
	
	private class MsgBoxContext
	{
		public NvMessageBox msgbox = null;
		public string title;
		public string description;
		public NvMessageBox.EMessageBoxStyle style = NvMessageBox.EMessageBoxStyle.eStyle_OkayCancel;
		public NvMessageBox.OnEndModal callback = null;
	}
	
	public NvUIDialogBase showMessageBox( string msg, string title = "",
		NvMessageBox.EMessageBoxStyle style = NvMessageBox.EMessageBoxStyle.eStyle_OkayOnly, 
		NvUIDialogBase.OnEndModal callback = null, bool closeIfOpened = false )
	{
		
		bool pendingMessagebox = false;
		
		if ( m_workingDialog != null && m_workingDialog.isOpened )
		{
			if ( !closeIfOpened )
			{
				return null;
			}
			else
			{
				pendingMessagebox = true;
				m_workingDialog.endModal(-1);
			}
		}

		NvUIDialogBase msgbox = findDialog( "MessageBox" );
		if ( msgbox == null )
		{
			msgbox = createDialog( "MessageBox", "UI Prefabs/Prefabs/Dialogs/MessageBox" );
		}
		
		if ( msgbox != null )
		{
			NvMessageBox messagebox = msgbox as NvMessageBox;
			
			if ( pendingMessagebox )
			{
				MsgBoxContext ctx = new MsgBoxContext();
				ctx.msgbox = messagebox;
				ctx.title = title;
				ctx.description = msg;
				ctx.style = style;
				ctx.callback = callback;
				
				StartCoroutine("OpenMessageBox", ctx);
			}
			else
			{
				messagebox.eventOnEndModal += HandleDlgeventOnEndModal;
				if ( callback != null )
				{
					messagebox.eventOnEndModal += callback;
				}
				
				messagebox.description = msg;
				messagebox.title = title;
				messagebox.style = style;
				messagebox.doModal();
				
				m_workingDialog = messagebox;
				m_baffle.enabled = true;
			}
		}
		
		return msgbox;
	}
	
	IEnumerator OpenMessageBox( MsgBoxContext ctx )
	{
		while ( m_workingDialog != null && m_workingDialog.isOpened == true )
		{
			yield return null;
		}
		
		NvMessageBox msgbox = ctx.msgbox;
		msgbox.title = ctx.title;
		msgbox.description = ctx.description;
		msgbox.style = ctx.style;
		msgbox.eventOnEndModal += HandleDlgeventOnEndModal;
		if ( ctx.callback != null )
		{
			msgbox.eventOnEndModal += ctx.callback;
		}
		msgbox.doModal();
		m_baffle.enabled = true;
		
		m_workingDialog = msgbox;
	}
	
//	public NvUIDialogBase showEquipmentStrengthenEnsureDlg( NvEquipmentStrengthenInfo info, NvUIDialogBase.OnEndModal callback)		
//	{
//		
//		if ( m_workingDialog != null )
//		{
//			if ( m_workingDialog is NvEquipmentStrengthenEnsureDialog )
//			{
//				return m_workingDialog;	
//			}
//			return null;
//		}		
//		
//		NvUIDialogBase EquipStrengEnsureDlg = findDialog( "EquipmentStrengthenEnsureDialog" );
//		//Debug.Log( "#################################    EquipStrengEnsureDlg : " + EquipStrengEnsureDlg );
//		if ( EquipStrengEnsureDlg == null )
//		{
//			EquipStrengEnsureDlg = createDialog( "EquipStrengthenEnsureDialog", 
//				"UI Prefabs/Prefabs/Dialogs/EquipmentUI/EquipStrengthenEnsureDialog" );
//		}		
//		//Debug.Log( "#################################    EquipStrengEnsureDlg : " + EquipStrengEnsureDlg );
//		if ( EquipStrengEnsureDlg != null )
//		{
//			NvEquipmentStrengthenEnsureDialog equipStreEnsureDlg = EquipStrengEnsureDlg as NvEquipmentStrengthenEnsureDialog;
//			equipStreEnsureDlg.mLblTitle.text = info.equipName;
//			equipStreEnsureDlg.mImgIcon.spriteName = info.iconName;
//			equipStreEnsureDlg.mImgIcon.transform.localScale = info.iconScale;
//			if ( info.curLv < 10 )
//			{
//				equipStreEnsureDlg.mImgCurLv[1].transform.localScale = Vector3.zero;
//				equipStreEnsureDlg.mImgCurLv[0].setVisible ( true );
//				equipStreEnsureDlg.mImgCurLv[0].spriteName = "" + info.curLv;
//				equipStreEnsureDlg.mImgCurLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}
//			else if ( info.curLv < 20 )
//			{
//				equipStreEnsureDlg.mImgCurLv[1].setVisible ( true );
//				equipStreEnsureDlg.mImgCurLv[1].spriteName = "1";
//				equipStreEnsureDlg.mImgCurLv[1].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//				equipStreEnsureDlg.mImgCurLv[0].setVisible ( true );
//				equipStreEnsureDlg.mImgCurLv[0].spriteName = "" +  ( info.curLv - 10 ) ;
//				equipStreEnsureDlg.mImgCurLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}
//			if ( info.newLv < 10 )
//			{
//				equipStreEnsureDlg.mImgNewLv[1].transform.localScale = Vector3.zero;
//				equipStreEnsureDlg.mImgNewLv[0].setVisible ( true );
//				equipStreEnsureDlg.mImgNewLv[0].spriteName = "" + info.newLv;
//				equipStreEnsureDlg.mImgNewLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}
//			else if ( info.newLv < 20 )
//			{
//				equipStreEnsureDlg.mImgNewLv[1].setVisible ( true );
//				equipStreEnsureDlg.mImgNewLv[1].spriteName = "1";
//				equipStreEnsureDlg.mImgNewLv[1].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//				equipStreEnsureDlg.mImgNewLv[0].setVisible ( true );
//				equipStreEnsureDlg.mImgNewLv[0].spriteName = "" + ( info.newLv - 10 ) ;
//				equipStreEnsureDlg.mImgNewLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}			
//			
//			for ( int i = 0; i < 4; i ++ )
//			{
//				equipStreEnsureDlg.mLblAttrName[i].text = "";
//				equipStreEnsureDlg.mLblCurAttrValue[i].text = "";
//				equipStreEnsureDlg.mLblNewAttrValue[i].text = "";
//				equipStreEnsureDlg.mImgSmallArrow[i].transform.localScale = Vector3.one;
//			}
//			
//			for ( int i = 0; i < 4; i ++ )
//			{
//				if ( info.curEquipAttr.Count / 2 > i )
//				{
//					equipStreEnsureDlg.mLblAttrName[i].text = info.curEquipAttr[i*2];
//					equipStreEnsureDlg.mLblCurAttrValue[i].text = info.curEquipAttr[i*2+1];
//					equipStreEnsureDlg.mLblNewAttrValue[i].text = info.newEquipAttr[i*2+1];
//					equipStreEnsureDlg.mImgSmallArrow[i].transform.localScale = new Vector3 ( 21.0f, 11.0f, 0.0f );
//				}
//			}
//			
//			equipStreEnsureDlg.mLblCoinName.text = info.coinName;
//			equipStreEnsureDlg.mLblCoinNum.text = "" + info.coinNum;
//			
//			equipStreEnsureDlg.eventOnEndModal += HandleDlgeventOnEndModal;
//			equipStreEnsureDlg.doModal();
//			m_baffle.enabled = true;
//			
//			if ( m_workingDialog != null )
//			{
//				m_workingDialog.closeDialog();
//			}
//			else
//			{
//				// disable dialog scene manager
//				foreach ( NvUIBaseSceneManager UISceneMgr in m_UISceneMgrs )
//				{
//					UISceneMgr.activate = false;
//				}
//			}
//			
//			m_workingDialog = equipStreEnsureDlg;
//		}
//		
//		if ( EquipStrengEnsureDlg != null && callback != null )
//		{
//			EquipStrengEnsureDlg.eventOnEndModal += callback;
//		}
//		
//		return EquipStrengEnsureDlg;
//	}
	
//	public NvUIDialogBase showNinjutsuStrengthenEnsureDlg( NvNinjutsuStrengthenInfo info, NvUIDialogBase.OnEndModal callback)		
//	{
//		if ( m_workingDialog != null )
//		{
//			if ( m_workingDialog is NvNinjutsuStrengthenEnsureDialog )
//			{
//				return m_workingDialog;	
//			}
//			return null;
//		}		
//		
//		NvUIDialogBase NinjutsuStrengEnsureDlg = findDialog( "NinjutsuStrengthenEnsureDialog" );
//		
//		if ( NinjutsuStrengEnsureDlg == null )
//		{
//			NinjutsuStrengEnsureDlg = createDialog( "NinjutsuStrengthenEnsureDialog", 
//				"UI Prefabs/Prefabs/Dialogs/NinjutsuUI/NinjutsuStrengthenEnsureDialog" );
//		}		
//		
//		if ( NinjutsuStrengEnsureDlg != null )
//		{
//			NvNinjutsuStrengthenEnsureDialog ninjutsuStreEnsureDlg = NinjutsuStrengEnsureDlg as NvNinjutsuStrengthenEnsureDialog;
//			ninjutsuStreEnsureDlg.mLblTitle.text = info.craftName;			
//			ninjutsuStreEnsureDlg.mImgIcon.spriteName = info.iconName;
//			ninjutsuStreEnsureDlg.mImgIcon.transform.localScale = info.iconScale;
//			if ( info.curLv < 10 )
//			{
//				ninjutsuStreEnsureDlg.mImgCurLv[1].transform.localScale = Vector3.zero;
//				ninjutsuStreEnsureDlg.mImgCurLv[0].setVisible ( true );
//				ninjutsuStreEnsureDlg.mImgCurLv[0].spriteName = "" + info.curLv;
//				ninjutsuStreEnsureDlg.mImgCurLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}
//			else if ( info.curLv < 20 )
//			{
//				ninjutsuStreEnsureDlg.mImgCurLv[1].setVisible ( true );
//				ninjutsuStreEnsureDlg.mImgCurLv[1].spriteName = "1";
//				ninjutsuStreEnsureDlg.mImgCurLv[1].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//				ninjutsuStreEnsureDlg.mImgCurLv[0].setVisible ( true );
//				ninjutsuStreEnsureDlg.mImgCurLv[0].spriteName = "" +  ( info.curLv - 10 ) ;
//				ninjutsuStreEnsureDlg.mImgCurLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}
//			if ( info.newLv < 10 )
//			{
//				ninjutsuStreEnsureDlg.mImgNewLv[1].transform.localScale = Vector3.zero;
//				ninjutsuStreEnsureDlg.mImgNewLv[0].setVisible ( true );
//				ninjutsuStreEnsureDlg.mImgNewLv[0].spriteName = "" + info.newLv;
//				ninjutsuStreEnsureDlg.mImgNewLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}
//			else if ( info.newLv < 20 )
//			{
//				ninjutsuStreEnsureDlg.mImgNewLv[1].setVisible ( true );
//				ninjutsuStreEnsureDlg.mImgNewLv[1].spriteName = "1";
//				ninjutsuStreEnsureDlg.mImgNewLv[1].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//				ninjutsuStreEnsureDlg.mImgNewLv[0].setVisible ( true );
//				ninjutsuStreEnsureDlg.mImgNewLv[0].spriteName = "" + ( info.newLv - 10 ) ;
//				ninjutsuStreEnsureDlg.mImgNewLv[0].transform.localScale = new Vector3 ( 22.0f, 35.0f, 0.0f );
//			}
//			ninjutsuStreEnsureDlg.mLblDesc.text = info.description;
//			ninjutsuStreEnsureDlg.mLblCoinName.text = info.coinName;
//			ninjutsuStreEnsureDlg.mLblCoinNum.text = info.coinNum;
//			
//			ninjutsuStreEnsureDlg.eventOnEndModal += HandleDlgeventOnEndModal;
//			ninjutsuStreEnsureDlg.doModal();
//			m_baffle.enabled = true;			
//			
//			if ( m_workingDialog != null )
//			{
//				m_workingDialog.closeDialog();
//			}
//			else
//			{
//				// disable dialog scene manager
//				foreach ( NvUIBaseSceneManager UISceneMgr in m_UISceneMgrs )
//				{
//					UISceneMgr.activate = false;
//				}
//			}
//			
//			m_workingDialog = ninjutsuStreEnsureDlg;			
//		}
//		
//		if ( NinjutsuStrengEnsureDlg != null && callback != null )
//		{
//			NinjutsuStrengEnsureDlg.eventOnEndModal += callback;
//		}
//		
//		return NinjutsuStrengEnsureDlg;		
//	}
	
//	public NvUIDialogBase showItemSellDlg ( NvItemInfo info, NvUIDialogBase.OnEndModal callback )
//	{
//		if ( m_workingDialog != null )
//		{
//			if ( m_workingDialog is NvItemSellDialog)
//			{
//				return m_workingDialog;	
//			}
//			return null;
//		}		
//		
//		NvUIDialogBase ItemSellDlg = findDialog( "ItemSellDialog" );
//		
//		if ( ItemSellDlg == null )
//		{
//			ItemSellDlg = createDialog( "ItemSellDialog", "UI Prefabs/Prefabs/Dialogs/ItemUI/ItemSellDialog" );
//		}				
//		
//		if ( ItemSellDlg != null )
//		{
//			NvItemSellDialog itemSellDlg = ItemSellDlg as NvItemSellDialog;
//			
//			itemSellDlg.mLblTitle.text = info.itemName;
//			itemSellDlg.mImgIcon.spriteName = info.iconName;
//			itemSellDlg.mImgIcon.transform.localScale = info.iconScale;
//			itemSellDlg.mItemMaxNum = info.itemMaxNum;
//			itemSellDlg.mItemPrice = info.itemPrice;
//			itemSellDlg.mLblCoinNum.text = "" + info.itemPrice;
//			itemSellDlg.mLblCoinName.text = info.coinName;
//			
//			itemSellDlg.eventOnEndModal += HandleDlgeventOnEndModal;
//			itemSellDlg.doModal();
//			m_baffle.enabled = true;					
//			
//			if ( m_workingDialog != null )
//			{
//				m_workingDialog.closeDialog();
//			}
//			else
//			{
//				// disable dialog scene manager
//				foreach ( NvUIBaseSceneManager UISceneMgr in m_UISceneMgrs )
//				{
//					UISceneMgr.activate = false;
//				}
//			}			
//			
//			m_workingDialog = itemSellDlg;			
//		}
//		
//		if ( ItemSellDlg != null && callback != null )
//		{
//			ItemSellDlg.eventOnEndModal += callback;
//		}
//		
//		return ItemSellDlg;				
//	}
	
//	public NvUIDialogBase showEquipSellDlg ( NvItemInfo info, NvUIDialogBase.OnEndModal callback )
//	{
//		if ( m_workingDialog != null )
//		{
//			if ( m_workingDialog is NvEquipSellDialog )
//			{
//				return m_workingDialog;	
//			}
//			return null;
//		}		
//		
//		NvUIDialogBase EquipSellDlg = findDialog( "EquipSellDialog" );
//
//		if ( EquipSellDlg == null )
//		{
//			EquipSellDlg = createDialog( "EquipSellDialog", "UI Prefabs/Prefabs/Dialogs/EquipmentUI/EquipSellDialog" );
//		}					
//		
//		if ( EquipSellDlg != null )
//		{
//			NvEquipSellDialog equipSellDlg = EquipSellDlg as NvEquipSellDialog;
//			
//			equipSellDlg.mLblTitle.text = info.itemName;
//			equipSellDlg.mImgIcon.spriteName = info.iconName;
//			equipSellDlg.mImgIcon.transform.localScale = info.iconScale;
//			equipSellDlg.mItemPrice = info.itemPrice;
//			equipSellDlg.mLblCoinNum.text = "" + info.itemPrice;
//			equipSellDlg.mLblCoinName.text = info.coinName;			
//			
//			equipSellDlg.eventOnEndModal += HandleDlgeventOnEndModal;
//			equipSellDlg.doModal();
//			m_baffle.enabled = true;					
//			
//			if ( m_workingDialog != null )
//			{
//				m_workingDialog.closeDialog();
//			}
//			else
//			{
//				// disable dialog scene manager
//				foreach ( NvUIBaseSceneManager UISceneMgr in m_UISceneMgrs )
//				{
//					UISceneMgr.activate = false;
//				}
//			}			
//			
//			m_workingDialog = equipSellDlg;				
//		}
//		
//		if ( EquipSellDlg != null && callback != null )
//		{
//			EquipSellDlg.eventOnEndModal += callback;
//		}
//		
//		return EquipSellDlg;			
//	}
	
	public NvUIDialogBase showEquipSmithResultDlg( string equipIconRes, 
		List<string> equipBaseInfo,  List<string> equipAttrInfo, NvUIDialogBase.OnEndModal callback)
	{
		if ( m_workingDialog != null )
		{
//			if ( m_workingDialog is NvEquipSmithResultDialog )
//			{
//				return m_workingDialog;	
//			}
			return null;
		}		
		
		NvUIDialogBase EquipSmithResultDlg = findDialog( "EquipSmithResultDlg" );
		
		if ( EquipSmithResultDlg == null )
		{
			EquipSmithResultDlg = createDialog( "EquipSmithResultDlg", 
				"UI Prefabs/Prefabs/Dialogs/EquipmentUI/EquipSmithResultDialog" );
		}				
		
//		if ( EquipSmithResultDlg != null )
//		{
//			NvEquipSmithResultDialog equipSmithResultDlg = EquipSmithResultDlg as NvEquipSmithResultDialog;
//			
//			equipSmithResultDlg.mItemIcon.spriteName = equipIconRes;
//			equipSmithResultDlg.mItemIcon.transform.localScale = new Vector3 ( 64.0f, 64.0f, 0.0f );
//			
//			for( int i = 0; i < equipSmithResultDlg.mItemBaseInfo.Count; i ++ )
//			{
//				equipSmithResultDlg.mItemBaseInfo[i].text = equipBaseInfo[i];
//			}
//			for( int i = 0; i < equipSmithResultDlg.mItemAttrInfo.Count; i ++ )
//			{
//				if ( i < equipAttrInfo.Count )
//				{
//					equipSmithResultDlg.mItemAttrInfo[i].text = equipAttrInfo[i];
//				}
//				else
//				{
//					equipSmithResultDlg.mItemAttrInfo[i].text = "";
//				}
//			}			
//			
//			equipSmithResultDlg.eventOnEndModal += HandleDlgeventOnEndModal;
//			equipSmithResultDlg.doModal();
//			m_baffle.enabled = true;
//			
//			if ( m_workingDialog != null )
//			{
//				m_workingDialog.closeDialog();
//			}
//			else
//			{
//				// disable dialog scene manager
//				foreach ( NvUIBaseSceneManager UISceneMgr in m_UISceneMgrs )
//				{
//					UISceneMgr.activate = false;
//				}
//			}
//			
//			m_workingDialog = equipSmithResultDlg;			
//		}
		
		if ( EquipSmithResultDlg != null && callback != null )
		{
			EquipSmithResultDlg.eventOnEndModal += callback;
		}
		
		return EquipSmithResultDlg;		
		
	}
	
//	public NvUIDialogBase showCraftCompoundResultDlg( string craftIconRes, Vector3 scale,
//		List<string> craftBaseInfo, NvUIDialogBase.OnEndModal callback)
//	{
//		if ( m_workingDialog != null )
//		{
//			if ( m_workingDialog is NvCraftCompoundResultDialog )
//			{
//				return m_workingDialog;	
//			}
//			return null;
//		}	
//		
//		NvUIDialogBase CraftCompoundResultDlg = findDialog( "CraftCompoundResultDlg" );
//		
//		if ( CraftCompoundResultDlg == null )
//		{
//			CraftCompoundResultDlg = createDialog( "CraftCompoundResultDlg", 
//				"UI Prefabs/Prefabs/Dialogs/NinjutsuUI/CraftCompoundResultDialog" );
//		}				
//		
//		if ( CraftCompoundResultDlg != null )
//		{
//			//NvCraftCompoundResultDialog craftCompoundResultDlg = CraftCompoundResultDlg as NvCraftCompoundResultDialog;
//			
//			craftCompoundResultDlg.mItemIcon.spriteName = craftIconRes;
//			craftCompoundResultDlg.mItemIcon.transform.localScale = scale;
//			
//			for( int i = 0; i < craftCompoundResultDlg.mItemBaseInfo.Count; i ++ )
////			{
////				craftCompoundResultDlg.mItemBaseInfo[i].text = craftBaseInfo[i];
////			}			
////			
////			craftCompoundResultDlg.eventOnEndModal += HandleDlgeventOnEndModal;
////			craftCompoundResultDlg.doModal();
////			m_baffle.enabled = true;
////			
////			if ( m_workingDialog != null )
////			{
////				m_workingDialog.closeDialog();
////			}
////			else
////			{
////				// disable dialog scene manager
////				foreach ( NvUIBaseSceneManager UISceneMgr in m_UISceneMgrs )
////				{
////					UISceneMgr.activate = false;
////				}
////			}
////			
////			m_workingDialog = craftCompoundResultDlg;						
//	//	}
//		
//		if ( CraftCompoundResultDlg != null && callback != null )
//		{
//			CraftCompoundResultDlg.eventOnEndModal += callback;
//		}
//		
//		return CraftCompoundResultDlg;			
//		
//	}
	
	public NvUIDialogBase showModalDialog( string dlgName )
	{
		if ( m_workingDialog != null )
		{
			// close modal dialog abnormally.
			m_workingDialog.endModal(-1);
			m_workingDialog = null;
		}
		
		NvUIDialogBase dlg = findDialog( dlgName );
		if ( dlg != null )
		{
			dlg.eventOnEndModal += HandleDlgeventOnEndModal;
			dlg.doModal();
			m_workingDialog = dlg;
			m_baffle.enabled = true;
		}
		
		return dlg;
	}

	void HandleDlgeventOnEndModal (int result)
	{
		m_baffle.enabled = false;
		m_workingDialog = null;
	}
	
	public NvUIDialogBase findDialog( string dlgName )
	{
		if ( m_CommonLayer != null )
		{
			return m_CommonLayer.findDialog( dlgName );
		}
		return null;
	}
	
	public NvUIDialogBase createDialog( string dlgName, string dlgPrefab )
	{
		if ( m_CommonLayer != null )
		{
			NvUIDialogBase dlg = m_CommonLayer.createDialog( dlgName, dlgPrefab );
			if ( dlg != null )
			{
				dlg.registerNotifiactionListener( gameObject );
			}
			return dlg;
		}
		return null;
	}
	
	private void Awake()
	{
		initialize();
	}
	
	private void Update()
	{
		if ( m_QueuePopupFailedMessage.Count > 0 || m_QueuePopupInfoMessage.Count > 0 )
		{
			if ( m_workingDialog == null )
			{
				List<MessageDescriptor> dsList = m_QueuePopupFailedMessage.Count > 0 ? m_QueuePopupFailedMessage : m_QueuePopupInfoMessage;
				if ( dsList.Count > 0 )
				{
					//MessageDescriptor ds = dsList[0];
					dsList.RemoveAt(0);
					//showMessageBox( ds.details, ds.title, NvMessageBox.EMessageBoxStyle.eStyle_OkayOnly, ds.cb );
				}
			}
		}
		else 
		{
			//if ( NvGame.gIsGamePaused )
			{
				//NvGame.gIsGamePaused = false;
			}
		}
		
		if ( m_WillShowIndicator )
		{
			m_ShowIndicatorAccl += Time.deltaTime;
			if ( m_ShowIndicatorAccl > m_ShowIndicatorThreshold )
			{
				m_WillShowIndicator = false;
				m_loadingIndicatorSprite.enabled = true;
				m_loadingIndicatorRotator.enabled = true;
			}
		}
		
		if ( m_PgWillShowIndicator )
		{
			m_PgShowIndicatorAccl += Time.deltaTime;
			if ( m_PgShowIndicatorAccl > m_PgShowIndicatorThreshold )
			{
				m_PgWillShowIndicator = false;
				m_LoadingPgSprite.enabled = true;
				m_LoadingPgRotator.enabled = true;
			}
		}
		
		
		if ( m_CommonLayer != null )
		{
			m_CommonLayer.update();
		}
		
		Vector3 ratio = Utility.GetScreenRatio();
		if ( mNGUI2DDialogCameraTr.localScale != ratio )
		{
			mNGUI2DDialogCameraTr.localScale = ratio;
		}
	}
	
	protected Transform mNGUI2DDialogCameraTr = null;
	protected GameObject mNGUI2DCamera = null;
	protected Camera m_uiCamera = null;
	protected UIFont m_commonFont = null;
	
	//private bool m_started = false;
	private bool m_inited = false;
	
	public Transform SystemUICameraTransform
	{
		get { return mNGUI2DDialogCameraTr; }
	}
	
	public GameObject SystemUICameraObj
	{
		get { return mNGUI2DCamera; }
	}
	
	public Camera SystemUICamera
	{
		get { return m_uiCamera;}
	}
	
	private void initialize() 
	{
		if ( !m_inited )
		{
			//Debug.Log ("======> init common ui manager");
			m_inited = true;
			launchCameras();
			launchCommonFont();
			
			m_CommonLayer = new NvUIDialogLayer( SystemUICameraTransform );
		}
	}
	
	private void launchCameras()
	{	
		launchDialogCamera();
	}
	
	private void launchDialogCamera() 
	{
		GameObject CameraPrefab = Resources.Load( "UI Prefabs/Prefabs/SystemUICamera" ) as GameObject;
        //Debug.Log (">>>>>> launch Camera1111111111(" + CameraPrefab + ")");
		mNGUI2DCamera = Utility.Instantiate( CameraPrefab ) as GameObject;
        //Debug.Log (">>>>>> launch Camera2222222222");
		mNGUI2DCamera.name = CameraPrefab.name;
		Transform cameraTr = mNGUI2DCamera.transform.FindChild("Camera");
		m_uiCamera = cameraTr.GetComponent<Camera>();
		mNGUI2DDialogCameraTr = cameraTr.transform.FindChild("Link");
		mNGUI2DDialogCameraTr.localPosition = Vector3.zero;
		mNGUI2DDialogCameraTr.localRotation = Quaternion.identity;
		mNGUI2DDialogCameraTr.localScale = Utility.GetScreenRatio();
				
		m_baffle = mNGUI2DDialogCameraTr.FindChild("Baffle").GetComponent<BoxCollider>();
		m_baffle.enabled = false;
		m_baffle.transform.localScale = new Vector3( Screen.width, Screen.height, 0.0f );
		
		
		m_CLRCam = mNGUI2DCamera.transform.FindChild("CLRCam").GetComponent<Camera>();
		m_CLRCam.enabled = false;
		//m_LoadingPgCam = mNGUI2DCamera.transform.FindChild("LoadingCam").GetComponent<Camera>();
		m_LoadingProgressBar = mNGUI2DCamera.transform.FindChild("LoadingCam/LoadingIndicator Panel/LoadingProgressBar").GetComponent<UISlider>();
		m_LoadingProgressBar.gameObject.SetActiveRecursively(false);
		m_LoadingPgSprite = mNGUI2DCamera.transform.FindChild("LoadingCam/LoadingIndicator Panel/MovingObj/LoadingIndicator").GetComponent<UISprite>();
		m_LoadingPgSprite.enabled = false;
		m_LoadingSprite = mNGUI2DCamera.transform.FindChild("LoadingCam/LoadingIndicator Panel/MovingObj/LoadSprite").GetComponent<UISprite>();
		m_LoadingSprite.enabled = false;
		m_LoadingPgRotator = mNGUI2DCamera.transform.FindChild("LoadingCam/LoadingIndicator Panel/MovingObj/LoadingIndicator").GetComponent<TweenTransform>();
		m_LoadingPgRotator.enabled = false;
		m_LoadingCollider = mNGUI2DCamera.transform.FindChild("LoadingCam").GetComponent<BoxCollider>();
		m_LoadingCollider.enabled = false;
		m_Tips = mNGUI2DCamera.transform.FindChild("LoadingCam/LoadingIndicator Panel/Tips").GetComponent<UILabel>();
		m_Tips.enabled = false;
		
		
		m_LoadingCam = mNGUI2DCamera.transform.FindChild("IndicateCam").GetComponent<Camera>();
		m_loadingIndicatorRotator = mNGUI2DCamera.transform.FindChild("IndicateCam/LoadingIndicator Panel/LoadingIndicator").GetComponent<TweenTransform>();
		m_loadingIndicatorRotator.enabled = false;
		m_loadingIndicatorSprite = mNGUI2DCamera.transform.FindChild("IndicateCam/LoadingIndicator Panel/LoadingIndicator").GetComponent<UISprite>();
		m_loadingIndicatorSprite.enabled = false;
		//Debug.Log (">>>>>> launch Camera");
		
		DontDestroyOnLoad(mNGUI2DCamera);
		DontDestroyOnLoad(mNGUI2DDialogCameraTr);
	}
	
	public void clearSystemUICamera()
	{
		//mNGUI2DDialogCameraTr.DetachChildren();
		if ( m_workingDialog != null )
		{
			m_workingDialog.endModal(-1);
			m_workingDialog = null;
		}	
	}
	
	private void launchCommonFont()
	{
		// TODO: implement later
	}
	
	
	public void ShowLoadingIndicator( bool flag )
	{
		m_LoadingCam.enabled = flag;
		if ( flag )
		{
			m_ShowIndicatorAccl = 0.0f;
			m_WillShowIndicator = true;
		}
		else
		{
			m_WillShowIndicator = false;
			m_loadingIndicatorSprite.enabled = flag;
			m_loadingIndicatorRotator.enabled = flag;
		}
	}
	
	public void ShowLoadingIndicatorWithProgress( bool flag )
	{
//		m_LoadingCollider.enabled = flag;
//		m_CLRCam.enabled = flag;
//		m_LoadingSprite.enabled = flag;
//		m_Tips.enabled = flag;
//		if ( flag )
//		{
//			m_Tips.text = ( Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager ).GetValue("tips_"+UnityEngine.Random.Range(0, NvConst.kGameTipsMaxNum));
//			m_LoadingSprite.spriteName  = loadingSprite[ UnityEngine.Random.Range(0,3) ];
//			m_ShowIndicatorAccl = 0.0f;
//			m_PgWillShowIndicator = true;
//			m_LoadingPgCam.enabled = true;
//			m_LoadingProgressBar.gameObject.SetActiveRecursively(flag);
//		}
//		else
//		{
//			m_LoadingPgCam.enabled = false;
//			m_PgWillShowIndicator = false;
//			m_LoadingPgSprite.enabled = flag;
//			m_loadingIndicatorRotator.enabled = flag;
//			m_LoadingProgressBar.gameObject.SetActiveRecursively(flag);
//		}
	}
	
	public void SetLoadingProgressAlpha( float v)
	{
		//m_LoadingPgSprite.alpha = v;
		//m_LoadingSprite.alpha = v;
		//m_Tips.alpha = v;
		
//		UIWidget[] widgts = m_LoadingProgressBar.GetComponentsInChildren<UIWidget>();		
//		foreach ( UIWidget wd in widgts){
//			wd.alpha = v;
//		}
	}
	
	public void SetLoadingProgress( float v )
	{
		m_LoadingProgressBar.sliderValue = v;		
	}
	
	public void EnableCLRCam( bool flag )
	{
		m_CLRCam.enabled = flag;
	}
}