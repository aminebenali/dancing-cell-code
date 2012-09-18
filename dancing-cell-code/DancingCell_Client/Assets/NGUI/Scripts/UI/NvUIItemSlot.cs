using UnityEngine;
using System.Collections;

public class NvUIItemSlot : NvUIStatic {
	
	public NvUICursor uiCursor;
	public UISprite slotSprite;
	public NvUIDialogBase dialogController;
	
	public int index = -1;
	public int aimID = -1;
	public object itemData = null;
	
	/// <summary>
	/// if the slot has own default show, like weapon icon, this funciton will clear it before item display.
	/// </summary>
	protected virtual void DropImpl( GameObject go ) {}
	/// <summary>
	/// if the slot has own default show, like weapon icon, this funciton will display the default show after slot item get out.
	/// </summary>
	protected virtual void DragImpl() {}
	/// <summary>
	/// if the slot is lock, it's the reason
	/// </summary>
	protected virtual bool LockCheck() { return false; }
	/// <summary>
	/// the function after picked up or dropped he item
	/// </summary>
	protected virtual void PostCursor(bool isDrop) {}
	
	void OnClick()
	{
		//Debug.Log( "####################    OnClick" );
		//if ( LockCheck() )
		//{
		//	return;
		//}
		//
		////empty slot
		//if ( slotSprite.atlas == null )
		//{
		//	OnDrop(null);
		//}
		////stored slot
		//else if ( slotSprite.atlas != null )
		//{
		//	UpdateCursor();
		//	slotSprite.atlas = null;
		//	DragImpl();
		//}		
		//
		//if ( dialogController != null )
		//{
		//	dialogController.SendMessage( "OnSlotSelected", this, SendMessageOptions.DontRequireReceiver );
		//}		
	}	
	
	void OnSelect ()
	{
		//Debug.Log( "###########################     on Select ~~~~~" );
		if ( dialogController != null )
		{
			dialogController.SendMessage( "OnSlotSelected", this, SendMessageOptions.DontRequireReceiver );
		}		
	}
	
	void OnPress ()
	{
		//Debug.Log( "###########################     on Press ~~~~~" );
	}
	
	void OnDrop ( GameObject go )
	{
		//Debug.Log( "####################    OnDrop" );
		if ( LockCheck() )
		{
			return;
		}
		
		if ( /*uiCursor != null && */uiCursor.mSprite != null )
		{
			DropImpl( go );
			slotSprite.atlas = uiCursor.mSprite.atlas;
			slotSprite.spriteName = uiCursor.mSprite.spriteName;
			//slotSprite.MakePixelPerfect();
			uiCursor.userData = null;
			uiCursor.Clear();
			PostCursor(true);
		}		
	}
	
	void OnDrag ( Vector2 delta )
	{
		//Debug.Log( "####################    OnDrag" );
		if ( LockCheck() )
		{
			return;
		}
		
		if ( slotSprite.atlas != null )
		{
			UpdateCursor();
			slotSprite.atlas = null;
			DragImpl();
		}

	}
	
	void UpdateCursor ()
	{
		
		if ( slotSprite.atlas != null )
		{
			uiCursor.Set( slotSprite.atlas, slotSprite.spriteName, itemData, index );
			PostCursor(false);
		}
		else if ( slotSprite.atlas == null )
		{
			uiCursor.Clear();
			PostCursor(true);
		}
	}
	
}
