using UnityEngine;
using System.Collections;

public class NvUIImage : NvUIStatic {
	
	enum ESetMask
	{
		eSM_Depth = 1,
		eSM_SpriteName = 1 << 1,
		eSM_Color = 1 << 2
	}
	
	private int m_settingMsk = 0;
	
	[SerializeField]
	private string m_spriteName;
	
	private UISprite sprite = null;
	
	public UISprite uiSprite
	{
		get { return sprite; }
	}
	
	public override int depth
	{
		get { return base.depth; }
		set {
		
			base.depth = value;
			
			if ( sprite != null )
			{
				sprite.depth = value;
			}
			else
			{
				m_settingMsk |= (int)ESetMask.eSM_Depth;
			}
		}
	}
	
	[SerializeField]
	private bool m_AutoSize = true;
	
	[SerializeField]
	private Color m_spriteColor = Color.white;
	
	// previous color
	private Color m_spritePrevColor = Color.white;
	
	public string spriteName
	{
		get {
			
			if ( sprite != null && sprite.spriteName != m_spriteName )
			{
				m_spriteName = sprite.spriteName;
			}
			
			return m_spriteName;
		}
		
		set {
			if ( m_spriteName != value )
			{
				m_spriteName = value;
				
				if ( sprite != null )
				{
					sprite.spriteName = m_spriteName;
					if ( m_AutoSize )
					{
						sprite.MakePixelPerfect();
					}
				}
				else
				{
					m_settingMsk |= (int)ESetMask.eSM_SpriteName;
				}
			}
			
		}
	}
	
	protected override void lazyUpdateImpl ()
	{
		base.lazyUpdateImpl ();
		
		if ( (m_settingMsk & (int)ESetMask.eSM_Color) != 0 )
		{
			if ( sprite != null )
			{
				m_spritePrevColor = sprite.color;
				sprite.color = m_spriteColor;
				m_settingMsk &= ~((int)ESetMask.eSM_Color);
			}
		}
		else
		{
			if ( m_spritePrevColor != m_spriteColor )
			{
				m_spritePrevColor = m_spriteColor;
				sprite.color = m_spriteColor;
			}
		}
		
		if ( (m_settingMsk & (int)ESetMask.eSM_Depth) != 0 )
		{
			if ( sprite != null )
			{
				sprite.depth = depth;
				m_settingMsk &= ~((int)ESetMask.eSM_Depth);
			}
		}
		
		if ( (m_settingMsk & (int)ESetMask.eSM_SpriteName) != 0 )
		{
			if ( sprite != null )
			{
				sprite.spriteName = m_spriteName;
				if ( m_AutoSize )
				{
					sprite.MakePixelPerfect();
				}
				m_settingMsk &= ~((int)ESetMask.eSM_SpriteName);
			}
		}
	}
	
	public Color spriteColor
	{
		get {
		
			return m_spriteColor;
			
		}
		
		set {
		
			if ( m_spriteColor != value )
			{
				m_spritePrevColor = m_spriteColor;
				m_spriteColor = value;
				if ( sprite != null )
				{
					sprite.color = m_spriteColor;
				}
			}
			else
			{
				m_settingMsk |= (int)ESetMask.eSM_Color;
			}
		}
	}
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl();
		
		if ( m_widget != null )
		{
			sprite = m_widget.GetComponent<UISprite>();
			
			if ( sprite != null )
			{
				sprite.spriteName = m_spriteName;
				sprite.color = m_spriteColor;
				
				if ( m_AutoSize )
				{
					sprite.MakePixelPerfect();
				}
			}
		}
	}
	
}
