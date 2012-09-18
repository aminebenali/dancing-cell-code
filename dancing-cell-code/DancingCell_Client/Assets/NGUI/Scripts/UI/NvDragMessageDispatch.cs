using UnityEngine;
using System.Collections;

public class NvDragMessageDispatch : MonoBehaviour {

	public GameObject targetDragCamera = null;
	public GameObject targetSlot = null;
	
	
	void OnPress( bool pressed )
	{
		if ( targetDragCamera != null )
		{
			targetDragCamera.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
		
		if ( targetSlot != null )
		{
			targetSlot.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	
	void OnDrag (Vector2 delta)	
	{
		if ( targetDragCamera != null )
		{
			targetDragCamera.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnScroll (float delta)
	{
		if ( targetDragCamera != null )
		{
			targetDragCamera.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
		}
	}
}
