using UnityEngine;
using System.Collections;

public class NvUIDialogTransition : NvUIDialogEffectBase {
	
	public Transform fromTr = null;
	public Transform toTr = null;
	public float duration = 1.0f;
	
	public GameObject trRoot = null;
	
	protected override UITweener startOpenEffectInternal (GameObject widget)
	{
		if ( widget == trRoot && fromTr != null && toTr != null )
		{
			TweenTransform tt = TweenTransform.Begin( widget, duration, fromTr, toTr );
			tt.style = UITweener.Style.Once;
			
			return tt as UITweener;
		}
		return null;
	}
	
	protected override UITweener startCloseEffectInternal (GameObject widget)
	{
		if ( widget == trRoot )
		{
			TweenTransform tt = TweenTransform.Begin( widget, duration, toTr, fromTr );
			tt.style = UITweener.Style.Once;
			
			return tt as UITweener;
		}
		return null;
	}
	
}
