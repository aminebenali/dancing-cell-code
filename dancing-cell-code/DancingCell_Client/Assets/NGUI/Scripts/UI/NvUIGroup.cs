using UnityEngine;
using System.Collections;

public class NvUIGroup : NvUIBase {
	
	private bool setVisibleCallback( Transform tr, object args )
	{
		bool flag = (bool)args;
		NvUIBase elem = tr.GetComponent<NvUIBase>();
		
		bool bRet = false;
		
		if ( elem != null )
		{
			elem.setVisible( flag );
			bRet = true;
		}
		else 
		{
			elem.gameObject.active = flag;
		}
		
		if ( tr.collider != null )
		{
			tr.collider.enabled = flag;
		}
		
		return bRet;
	}
	
	private bool setEnableCallback( Transform tr, object args )
	{
		NvUIBase elem = tr.GetComponent<NvUIBase>();
		if ( elem != null )
		{
			elem.setEnable( (bool)args );
			return true;
		}
		
		return false;
	}
	
	public override void setVisible (bool flag)
	{
		base.setVisible( flag );
		Utility.enumWidgets( transform, setVisibleCallback, flag as object );
	}
	
	public override void setEnable ( bool flag )
	{
		base.setEnable( flag );
		Utility.enumWidgets( transform, setEnableCallback, flag as object );	
	}
	
}
