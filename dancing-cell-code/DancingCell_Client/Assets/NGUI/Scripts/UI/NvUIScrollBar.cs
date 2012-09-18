using UnityEngine;
using System.Collections;

public class NvUIScrollBar : NvUIStatic {

	private UIScrollBar m_scrollbar;
	
	private NvUIImage m_background = null;
	private NvUIImage m_foreground = null;
		
	[SerializeField]
	private UIScrollBar.Direction m_direction = UIScrollBar.Direction.Horizontal;
	 
	public UIScrollBar.Direction direction
	{
		get { return m_direction; }
		set {
		
			if ( m_direction != value )
			{
				m_direction = value;
				if ( m_scrollbar != null )
				{
					m_scrollbar.direction = value;
				}
			}
		}
	}	
	
	public NvUIImage background
	{
		get { return m_background; }
	}
	
	public NvUIImage foreground
	{
		get { return m_foreground; }
	}	
	
	//public event OnElementTriggered eventOnSliderChanged;
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl ();
		
		m_scrollbar = GetComponent<UIScrollBar>();
		m_background = findChild("Background") as NvUIImage;
		m_foreground = findChild ("Foreground") as NvUIImage;
	}
	
	
	public override void setVisible (bool flag)
	{
		base.setVisible (flag);
		
		if ( m_background != null )
		{
			m_background.setVisible(flag);
		}
		
		if ( m_foreground != null )
		{
			m_foreground.setVisible(flag);
		}
	}
		
//	public override void revertVisible ()
//	{
//		base.revertVisible ();
//		
//		if ( m_background != null )
//		{
//			m_background.revertVisible();
//		}
//		
//		if ( m_foreground != null )
//		{
//			m_foreground.revertVisible();
//		}
//		
//	}	
	
	public override void setEnable (bool flag)
	{
		base.setEnable (flag);
		
		if ( m_background != null )
		{
			m_background.setEnable(flag);
		}
		
		if ( m_foreground != null )
		{
			m_foreground.setEnable(flag);
		}
		
	}	
	
//	public override void revertEnable ()
//	{
//		base.revertEnable ();
//		
//		if ( m_background != null )
//		{
//			m_background.revertEnable();
//		}
//		
//		if ( m_foreground != null )
//		{
//			m_foreground.revertEnable();
//		}
//		
//	}	
}
