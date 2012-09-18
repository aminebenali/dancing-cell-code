using UnityEngine;
using System.Collections;

public class NvUIStatic : NvUIBase {
	
	[HideInInspector][SerializeField]
	protected Transform m_widget = null;
	
	protected Rect m_rect;
	
	[HideInInspector][SerializeField]
	protected UIAtlas m_atlas = null;
	
	protected float m_zOffset = 0.0f;
	
	public Rect rect
	{
		get {
			
			return m_rect;
		}
		
		set {
			
			if ( m_rect != value )
			{
				m_rect = value;
				
				if ( m_widget != null )
				{
					Vector3 originPos = m_widget.localPosition;
					Vector3 originScale = m_widget.localScale;
					
					m_widget.localPosition = new Vector3( m_rect.x, m_rect.y, originPos.z );
					m_widget.localScale = new Vector3( m_rect.width, m_rect.height, originScale.z );
					
					calcRect ();
				}
			}
		}
	}
	
	// the width and height of widget will be mutipled with screenRatio.
	public Rect absoluteRect
	{
		get {
			
			if ( m_widget != null )
			{
				Vector3 screenRatio = Utility.GetScreenRatio();
				Rect rc = new Rect( m_widget.localPosition.x, m_widget.localPosition.y, m_rect.width * screenRatio.x, m_rect.height * screenRatio.y );
				return rc;
			}
			return new Rect(0,0,0,0);
		}
	}
	
	
	public float width
	{
		get { return m_rect.width; }
		set {
			
			if ( m_rect.width != value && value > 0.0f )
			{
				Rect rc = m_rect;
				rc.width = value;
				
				rect = rc;
			}
		}
	}
	
	public float height
	{
		get { return m_rect.height; }
		set {
			
			if ( m_rect.height != value && value > 0.0f )
			{
				Rect rc = m_rect;
				rc.height = value;
				
				rect = rc;
			}
			
		}
	}
	
	virtual public UIAtlas atlas
	{
		get {
			
			return m_atlas;
		}
		
		set {
			
			if ( m_atlas != value /*&& value != null */)
			{
				m_atlas = value;
				if ( m_widget != null )
				{
					UISprite sprite = m_widget.GetComponent<UISprite>();
					
					if ( sprite != null )
					{
						sprite.atlas = value;
					}
				}
			}
		}
	}
	
	virtual protected void calcRect() {}
	
	protected override void AwakeImpl()
	{
		if ( m_widget == null )
		{
			m_widget = transform;
		}
		
		if ( m_widget != null )
		{
			if ( m_atlas != null )
			{
				UISprite sprite = m_widget.GetComponent<UISprite>();
				if ( sprite != null )
				{
					sprite.atlas = m_atlas;
				}
			}
			else
			{
				UISprite sprite = m_widget.GetComponent<UISprite>();
				if ( sprite != null )
				{
					m_atlas = sprite.atlas;
				}
			}
		}
	}
	
	public override void setVisible (bool flag)
	{
		base.setVisible( flag );
		
		if ( m_widget != null )
		{
			UIWidget elem = m_widget.GetComponent<UIWidget>();
			if ( elem != null )
			{
				elem.enabled = flag;
			}
		}
	}
	
//	public override void revertVisible()
//	{
//		base.revertVisible();
//		
//		if ( m_widget != null )
//		{
//			UIWidget elem = m_widget.GetComponent<UIWidget>();
//			if ( elem != null )
//			{
//				elem.enabled = visible;
//			}
//		}
//	}
}
