//-----------------------------------------------------------------
//  Copyright 2011 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEngine;
using System.Collections;


// Interface for animatable sprite-based objects.
public interface ISpriteAnimatable 
{
	GameObject gameObject
	{
		get;
	}

	TextureAnim[] States
	{
		get;
		set;
	}
}
