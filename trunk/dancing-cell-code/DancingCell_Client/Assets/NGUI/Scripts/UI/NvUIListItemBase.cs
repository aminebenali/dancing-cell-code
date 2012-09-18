using UnityEngine;
using System.Collections;

public class NvUIListItemBase : NvUIBase {
	
	public int index = -1;
	public object itemData = null;
	public NvUIList listController = null;
	
	private UISprite slotSprite;
	
	/// <summary>
	/// Raises the click event. when add this component to the list item object, you must add the "UIButtonMessage" component to this too for "OnClick" function
	/// </summary>
	private void OnClick()
	{
		if ( listController != null )
		{
			listController.SendMessage("OnItemSelected", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnDrag ( Vector2 delta )
	{
		/*
		if ( slotSprite == null )
		{
			slotSprite = gameObject.transform.FindChild("item").GetComponent<UISprite>();
		}
		
		if ( slotSprite != null )
		{
			slotSprite.atlas = null;
		}	
		*/	
	}
}
