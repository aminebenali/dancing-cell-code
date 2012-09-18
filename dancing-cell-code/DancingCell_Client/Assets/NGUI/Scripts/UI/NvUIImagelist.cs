using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUIImagelist : NvUIStatic {
	
	[System.Serializable]
	public class SpriteContext
	{
		public UIAtlas atlas = null;
		public string spriteName;
		
		public SpriteContext( UIAtlas _atlas, string _spriteName )
		{
			atlas = _atlas;
			spriteName = _spriteName;
		}
	}
	
	public override int depth
	{
		get 
		{ 
			return base.depth;
		}
		set {
			
			base.depth = value;
		
			int ii = 0;
			foreach ( NvUIImage image in m_Imagelist )
			{
				Vector3 pos = image.transform.localPosition;
				pos.z += transform.localPosition.z + (-1f) * ii;
				++ii;
				image.transform.localPosition = pos;
			}
			
		}
	}
	
	public float deepestDepthZ
	{
		get {
		
			if ( m_Imagelist.Count > 0 )
			{
				return m_Imagelist[m_Imagelist.Count-1].transform.localPosition.z;
			}
			
			return transform.localPosition.z;
		}
	}
	
	[SerializeField]
	private List<SpriteContext> m_spriteContextTable = new List<SpriteContext>();
	
	[SerializeField]
	private Color m_spriteColor = Color.white;
	
	//
	private List<NvUIImage> m_Imagelist = new List<NvUIImage>();
	
	public int Length
	{
		get { return m_Imagelist.Count; }
	}
	
	public Color spriteColor
	{
		get 
		{
			return m_spriteColor;
		}
		
		set 
		{
			if ( m_spriteColor != value )
			{
				m_spriteColor = value;
				foreach ( NvUIImage image in m_Imagelist )
				{
					image.spriteColor = m_spriteColor;
				}
			}
		}
	}
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl();
		
		int ii = 0;
		foreach ( SpriteContext ctx in m_spriteContextTable )
		{
			NvUIImage image = buildAnImage( ctx.atlas, ctx.spriteName );
			if ( image != null )
			{
				//image.depth = ii++;
				Vector3 pos = image.transform.localPosition;
				pos.z += transform.localPosition.z + (-1f) * ii;
				++ii;
				
				image.transform.localPosition = pos;
				image.spriteColor = m_spriteColor;
				m_Imagelist.Add( image );
			}
		}
	}
	
	private NvUIImage buildAnImage( UIAtlas atlas, string spriteName )
	{
		if ( atlas != null )
		{
			UIAtlas.Sprite sprite = atlas.GetSprite( spriteName );
			
			if ( sprite != null )
			{
				
				Rect rc = sprite.outer;
				
				GameObject obj = new GameObject("ImageSlot (" + spriteName + ")");
				obj.transform.parent = transform;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localRotation = Quaternion.identity;
				obj.transform.localScale = new Vector3( rc.width, rc.height, 1.0f );
				obj.layer = gameObject.layer;
				
				UISprite spr = obj.AddComponent<UISprite>();
				spr.atlas = atlas;

				NvUIImage image = obj.AddComponent<NvUIImage>();
				image.spriteName = spriteName;
				return image;
			}
		}
		return null;
	}
	
	public NvUIImage addImage( UIAtlas atlas, string spriteName )
	{
		if ( spriteName != "" && atlas != null )
		{
			NvUIImage image = buildAnImage( atlas, spriteName );
			if ( image != null )
			{
				image.depth = m_Imagelist.Count;
				image.spriteColor = m_spriteColor;
				
				m_Imagelist.Add( image );
				
				return image;
			}
		}
		return null;
	}
	
	public void removeImage( int index )
	{
		if ( index >= 0 && index < m_Imagelist.Count )
		{
			GameObject.Destroy( m_Imagelist[index].gameObject );
			m_Imagelist.RemoveAt(index);
		}
	}
	
	public NvUIImage getImage( int index )
	{
		if ( index >= 0 && index < m_Imagelist.Count )
		{
			return m_Imagelist[index];
		}
		return null;
	}
	
	public void clear()
	{
		foreach ( NvUIImage image in m_Imagelist )
		{
			GameObject.Destroy( image.gameObject );
		}
		
		m_Imagelist.Clear();
		m_spriteContextTable.Clear();
	}
	
	public override void setEnable (bool flag)
	{
		base.setEnable (flag);
		
		foreach ( NvUIImage image in m_Imagelist )
		{
			image.setEnable( flag );
		}
	}
	
	public override void setVisible( bool flag )
	{
		base.setVisible(flag);
		
		foreach ( NvUIImage image in m_Imagelist )
		{
			image.setVisible( flag );
		}
	}
	
//	public override void revertEnable ()
//	{
//		base.revertEnable ();
//		
//		foreach ( NvUIImage image in m_Imagelist )
//		{
//			image.revertEnable();
//		}
//	}
//	
//	public override void revertVisible ()
//	{
//		base.revertVisible ();
//		
//		foreach ( NvUIImage image in m_Imagelist )
//		{
//			image.revertVisible();
//		}
//	}
}
