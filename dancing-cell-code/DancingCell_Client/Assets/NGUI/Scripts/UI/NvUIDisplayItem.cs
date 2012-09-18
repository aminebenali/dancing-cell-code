using UnityEngine;
using System.Collections;

public class NvUIDisplayItem : NvUIStatic {

	public object itemData = null;
	public int index = -1;
	public NvUIDialogBase dialogController;
	
	private void OnClick()
	{
		if ( dialogController != null )
		{
			dialogController.SendMessage("OnDisplayItemSelected", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
}
