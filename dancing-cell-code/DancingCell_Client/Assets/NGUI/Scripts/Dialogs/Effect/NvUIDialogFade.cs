using UnityEngine;
using System.Collections;

public class NvUIDialogFade : NvUIDialogEffectBase 
{
	public Color fadeFrom = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	public Color fadeTo = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
	public float duration = 1.0f;
	
	public TweenColor.Method fadeMethod = UITweener.Method.EaseInOut;
	
	
	protected override UITweener startOpenEffectInternal (GameObject widget)
	{
		if ( widget.GetComponent<UIWidget>() != null )
		{
			TweenColor tc = TweenColor.Begin( widget, duration, fadeTo );
			tc.style = UITweener.Style.Once;
			tc.from = fadeFrom;
			
			return tc as UITweener;
		}
		
		return null;
	}
	
	protected override UITweener startCloseEffectInternal (GameObject widget)
	{
		if ( widget.GetComponent<UIWidget>() != null )
		{
			TweenColor tc = TweenColor.Begin( widget, duration, fadeFrom );
			tc.style = UITweener.Style.Once;
			tc.from = fadeTo;
			
			return tc as UITweener;
		}
		return null;
	}
}
