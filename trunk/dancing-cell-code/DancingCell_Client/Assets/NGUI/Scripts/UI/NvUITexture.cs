using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUITexture : NvUIStatic {
	
	
	private UITexture mTexture = null;
	
	private Material mMat = null;
	
	public string[] mPreloadTextureRes = null;
	
	[SerializeField]
	private string mTextureResName = "";
	
	[SerializeField]
	private Vector2 mSize = Vector2.zero;
	
	[SerializeField]
	private BoxCollider mBlock = null;
	
	public enum EMaterialType
	{
		eMT_Transparent = 0,
		eMT_TransparentColored,
		eMT_Texture
	}
	
	[SerializeField]
	private EMaterialType mMatType = EMaterialType.eMT_Texture;
	
	[SerializeField]
	private Color mColor = Color.white;
	
	public string TextureResName
	{
		get { return mTextureResName; }
		set 
		{
			if ( mTextureResName != value )
			{
				mTextureResName = value;
				Apply ();
			}
		}
	}
	
	public EMaterialType MatT
	{
		get { return mMatType; }
		set {
		
			if ( mMatType != value )
			{
				mMatType = value;
				ApplyMat ();
			}
			
		}
	}
	
	public Color MatColor
	{
		get { return mColor; }
		set {
			
			if ( mColor != value )
			{
				mColor = value;
				ApplyMat ();
			}
			
		}
	}
	
	public Vector2 Size
	{
		get { return mSize; }
		set {
		
			if ( mSize != value && value != Vector2.zero )
			{
				mSize = value;
				
			}
			
		}
	}
	
	//
	private static Dictionary<string, Texture> sTextureTable = new Dictionary<string, Texture>();
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl ();
		
		// preload
		foreach ( string res in mPreloadTextureRes )
		{
			Texture tex = Resources.Load( res ) as Texture;
			if ( tex != null && sTextureTable.ContainsKey(res) == false )
			{
				sTextureTable.Add( res, tex );
			}
		}
		
		ApplyMat ();
		Apply ();
	}
	
	
	public override void setVisible (bool flag)
	{
		base.setVisible (flag);
		
		if ( mTexture != null )
		{
			mTexture.enabled = flag;
		}
		
		if ( mBlock != null )
		{
			mBlock.enabled = flag;
		}
	}
	
	private void Apply()
	{
		if ( mTextureResName != "" )
		{
			Texture tex = null;
			if ( !sTextureTable.TryGetValue( mTextureResName, out tex ) )
			{
				tex = Resources.Load ( mTextureResName ) as Texture;
				if ( tex != null )
				{
					sTextureTable.Add( mTextureResName, tex );
				}
			}
			
			if ( tex != null )
			{
				if ( mMat == null )
				{
					ApplyMat();
				}
				
				mMat.mainTexture = tex;
				ApplySize();
			}
			
			if ( mTexture == null )
			{
				mTexture = gameObject.AddComponent<UITexture>();
			}
			
			mTexture.material = mMat;
		}
	}
	
	private void ApplyMat()
	{
		string shaderString = "Unlit/Texture";
		switch ( mMatType )
		{
		case EMaterialType.eMT_Texture:
			{
				shaderString = "Unlit/Texture";
			}
			break;
		case EMaterialType.eMT_Transparent:
			{
				shaderString = "Unlit/Transparent";
			}
			break;
		case EMaterialType.eMT_TransparentColored:
			{
				shaderString = "Unlit/Transparent Colored";
			}
			break;
		}
		
		if ( mMat == null )
		{
			mMat = new Material( Shader.Find( shaderString ) );
			mMat.hideFlags = HideFlags.HideAndDontSave;
		}
		else
		{
			mMat.shader = Shader.Find( shaderString );
		}
		
		if ( mTexture != null )
		{
			mTexture.color = mColor;
		}
	}
	
	private void ApplySize()
	{
		if ( mSize != Vector2.zero )
		{
			transform.localScale = new Vector3( mSize.x, mSize.y, 0.0f );
		}
		else
		{
			if ( mMat != null && mMat.mainTexture != null )
			{
				transform.localScale = new Vector3( mMat.mainTexture.width, mMat.mainTexture.height, 0.0f );
			}
		}
	}
}
