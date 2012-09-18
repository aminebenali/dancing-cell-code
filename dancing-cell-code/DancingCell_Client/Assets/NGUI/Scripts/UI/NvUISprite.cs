using UnityEngine;
using System.Collections;

public class NvUISprite : NvUIBase {
	
	private GameObject go = null;
	protected Transform goTr = null;

	public void createNumber( Transform tr, UIAtlas atlas, string status, string num )
	{
		go = new GameObject("number");
		go.transform.parent = tr;
		go.transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);
		go.transform.localScale = Vector3.one;		
		goTr = go.transform;
		go.AddComponent<UIPanel>();
		go.layer = LayerMask.NameToLayer("GameUILayer");
		char[] cc = num.ToCharArray();
		UISprite[] spArrray = new UISprite[cc.Length];
		float offsetX = 0;
		for (int i = 0; i < cc.Length; i ++)
		{
			spArrray[i] = NGUITools.AddSprite( go, atlas, status + cc[i].ToString() );
			if ( i > 0 )
			{
				//advice: padding conf  all set zero.
				offsetX += spArrray[ i - 1 ].transform.localScale.x + 1;
			}
			spArrray[i].transform.localPosition = new Vector3( go.transform.localPosition.x + offsetX, go.transform.localPosition.y, go.transform.localPosition.z);
			spArrray[i].MakePixelPerfect();
		}
	}
	
	public Transform spriteTr 
	{
		get { return goTr;}
		set 
		{ 
			if (goTr != value) 
			{
				goTr = value;
			}
		} 
	}
}
