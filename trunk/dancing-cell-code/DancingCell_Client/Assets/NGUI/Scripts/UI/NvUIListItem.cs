using UnityEngine;
using System.Collections;

public class NvUIListItem : NvUIBase {
	
	private NvUIList owner = null;
	
	void Start () {
		Transform gridTr = gameObject.transform.parent;
		Transform panelTr = gridTr.transform.parent;
		Transform listTr = panelTr.transform.parent;
		owner = listTr.GetComponent<NvUIList>();
	}
	
	void OnPress (bool isPressed)
	{
		if ( isPressed )
		{
			//owner.setCurrentItem(gameObject);
		}
	}
}
