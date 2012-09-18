using UnityEngine;
using System.Collections;

public class NvUIButton : NvUIStatic {
	
	[SerializeField]
	protected NvUILabel buttonCaption = null;

	[SerializeField]
	protected BoxCollider buttonCollider = null;
	
	
	[SerializeField]
	protected NvUIImagelist buttonBackground = null;
	
	
	[System.Serializable]
	public class ButtonStyle
	{
		public UIAtlas atlas = null;
		public string spriteName;
		public Vector3 Offset = Vector3.zero;
	}
	
	[System.Serializable]
	public class ButtonSkin
	{
		public ButtonStyle[] normalStyle = null;
		public ButtonStyle[] hoverStyle = null;
		public ButtonStyle[] pressedStyle = null;
		public ButtonStyle[] disableStyle = null;
		
		public Color normalColor = Color.white;
		public Color hoverColor = Color.white;
		public Color pressedColor = Color.white;
		public Color disableColor = Color.white;
	}
	
	[SerializeField]
	protected ButtonSkin m_skin = new ButtonSkin();
	
	public NvUIImagelist Background
	{
		get { return buttonBackground; }
	}
	
	[SerializeField]
	protected string m_caption;
	
	// click event
	public event OnElementTriggered eventOnClick;
	
	public event OnElementTriggered eventOnPressed;
	public event OnElementTriggered eventOnHover;
	
	// whether pressed or not
	private bool m_isPressed = false;
	private bool m_isHover = false;
	
	
	[SerializeField]
	private int m_index = -1;
	
	public int index
	{
		get { return m_index; }
		set {
			if ( m_index != value )
			{
				m_index = value;
			}
		}
	}
	
	// get or set button caption
	public string caption
	{
		get
		{
			return m_caption;
		}
		
		set 
		{
			if ( m_caption != value )
			{
				m_caption = value;
				if ( buttonCaption != null )
				{
					buttonCaption.text = m_caption;
				}
			}
		}
	}
	
	protected override void AwakeImpl()
	{
		base.AwakeImpl();
	
		Transform tr = null;
		if ( buttonCaption == null )
		{
			tr = transform.FindChild("Caption");
			if ( tr != null )
			{
				buttonCaption = tr.GetComponent<NvUILabel>();
			}
		}
		
		if ( buttonCaption != null )
		{
			buttonCaption.text = m_caption;
		}
		
		if ( buttonBackground == null )
		{
			tr = transform.FindChild("Background");
			if ( tr != null )
			{
				buttonBackground = tr.GetComponent<NvUIImagelist>();
			}
		}
		
		if ( buttonBackground != null )
		{
			foreach ( ButtonStyle style in m_skin.normalStyle )
			{
				NvUIImage image = buttonBackground.addImage( style.atlas, style.spriteName );
				image.transform.localPosition = style.Offset;
				
				if ( image != null )
				{	
					image.spriteColor = m_skin.normalColor;
				}
			}
		}
		
		if ( buttonCollider != null && buttonBackground != null )
		{
			NvUIImage image = buttonBackground.getImage(0);
			if ( image == null )
			{
				Debug.LogWarning("NvUIButton (" + name + "): the button has no properly configured skin, check it!");
			}
			else
			{
				buttonCollider.transform.localScale = image.transform.localScale;
			}
		}
	}
	
	protected void SwitchStyle( ButtonStyle[] styles, Color styleColor )
	{
		if ( buttonBackground != null && styles != null && styles.Length > 0 && styles.Length == buttonBackground.Length )
		{
			int ii = 0;
			foreach( ButtonStyle style in styles )
			{
				NvUIImage image = buttonBackground.getImage(ii++);
				if ( image == null )
				{
					Debug.LogWarning("NvButton(" + name + "): Out of range");
					break;
				}
				else 
				{	
					image.atlas = style.atlas;
					image.spriteName = style.spriteName;
					image.spriteColor = styleColor;
					image.transform.localPosition = style.Offset;
					
					if ( ii - 1 == 0 && buttonCollider != null )
					{
						buttonCollider.transform.localScale = image.transform.localScale;
					}
				}
			}
			
			if ( buttonCaption != null )
			{
				Vector3 pos = buttonCaption.transform.localPosition;
				pos.z = buttonBackground.deepestDepthZ + (-0.5f);
				buttonCaption.transform.localPosition = pos;
			}
		}
	}
	
	// delegates for UI event trigger
	private void OnClick()
	{
		if ( enable )
		{
			//(Singlton.getInstance("NvSoundController") as NvSoundController).PlaySe("se_click");
			OnClickImpl();
		}
		else
		{
			// TODO: later
		}
	}
	
	private void OnClickWithoutSoundEffect()
	{
		if ( enable )
		{
			OnClickImpl();
		}
	}
	
	private void OnPress( bool pressed )
	{
		if ( enable )
		{
			OnPressImpl( pressed );
		}
	}
	
	private void OnHover( bool hover )
	{
		if ( enable )
		{
			OnHoverImpl( hover );
		}
	}
	
	virtual protected void OnClickImpl() 
	{
		if ( eventOnClick != null )
		{
			eventOnClick( this, m_index );
		}
	}
	
	virtual protected void OnPressImpl( bool pressed )
	{
		if ( m_isHover )
		{
			m_isHover = false;
		}
		
		m_isPressed = pressed;
		if ( pressed )
		{
			SwitchStyle( m_skin.pressedStyle, m_skin.pressedColor );
			if ( eventOnPressed != null )
			{
				eventOnPressed( this, m_index );
			}
		}
		else 
		{
			SwitchStyle( m_skin.normalStyle, m_skin.normalColor );
		}
	}
	
	virtual protected void OnHoverImpl( bool hover )
	{
		if ( m_isPressed == false )
		{
			if ( !m_isHover && hover )
			{
				m_isHover = true;
				SwitchStyle( m_skin.hoverStyle, m_skin.hoverColor );
				
				if ( eventOnHover != null )
				{
					eventOnHover( this, m_index );
				}
			}
			else if ( m_isHover && !hover )
			{
				m_isHover = false;
				SwitchStyle( m_skin.normalStyle, m_skin.normalColor );
			}
		}
	}
	
	public override void setVisible( bool flag )
	{
		base.setVisible( flag );
		
		if ( buttonCaption != null )
		{
			buttonCaption.setVisible( flag );
		}
		
		if ( buttonBackground != null )
		{
			buttonBackground.setVisible( flag );
		}
		
		if ( buttonCollider != null )
		{
			buttonCollider.gameObject.SetActiveRecursively( flag );
		}
	}
	
//	public override void revertVisible()
//	{
//		base.revertVisible();
//		
//		if ( buttonCaption != null )
//		{
//			buttonCaption.revertVisible();
//		}
//		
//		if ( buttonBackground != null )
//		{
//			buttonBackground.revertVisible();
//		}
//	}
	
	public override void setEnable (bool flag)
	{
		base.setEnable (flag);
		
		if ( buttonCaption != null )
		{
			buttonCaption.setEnable( flag );
		}
		
		if ( buttonCollider != null )
		{
			buttonCollider.enabled = flag;
		}
		
		if ( buttonBackground != null )
		{
			buttonBackground.setEnable( flag );
		}
		
		if ( !flag )
		{
			m_isPressed = false;
			m_isHover = false;
			
			SwitchStyle( m_skin.disableStyle, m_skin.disableColor );
		}
		else 
		{
			SwitchStyle( m_skin.normalStyle, m_skin.normalColor );
		}
	}
	
	[SerializeField]
	private bool mEnableColliderGizmos = false;
	
	void OnDrawGizmos () 
	{
		if ( mEnableColliderGizmos && Application.isEditor )
		{
			if ( buttonCollider != null )
			{
				Gizmos.DrawWireCube( buttonCollider.center, buttonCollider.size );
			}
		}
	}
	
	
//	public override void revertEnable()
//	{
//		base.revertEnable();
//		
//		if ( buttonCaption != null )
//		{
//			buttonCaption.revertEnable();
//		}
//		
//		if ( buttonCollider != null )
//		{
//			buttonCollider.enabled = enable;
//		}
//		
//		if ( buttonBackground != null )
//		{
//			buttonBackground.revertEnable();
//		}
//		
//		if ( !enable )
//		{
//			m_isPressed = false;
//			m_isHover = false;
//			
//			SwitchStyle( m_skin.disableStyle, m_skin.disableColor );
//		}
//		else 
//		{
//			SwitchStyle( m_skin.normalStyle, m_skin.normalColor );
//		}
//	}
}
