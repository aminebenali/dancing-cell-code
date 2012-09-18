using UnityEngine;
using System.Collections;

public class NvUISlider : NvUIStatic {
	
	private UISlider m_slider = null;
	
	private NvUIImage m_background = null;
	private NvUIImage m_foreground = null;
	private NvUIImage m_thumb = null;
	
	[SerializeField]
	private float m_value = 0.0f;
	
	[SerializeField]
	private int m_maxValue = 0;
	
	public event OnElementTriggered eventOnSliderChanged;
	
	public float Value
	{
		get { return m_value; }
		set {
		
			if ( m_value - value > 1e-6 )	
			{
				m_value = value;
				if ( m_slider != null )
				{
					m_slider.sliderValue = m_maxValue > 0 ? Mathf.Clamp01(m_value / m_maxValue) : m_value;
				}
				
				OnSliderChanged();
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
	
	public NvUIImage thumb
	{
		get { return m_thumb; }
	}
	
	public int MaxValue
	{
		get { return m_maxValue; }
		set {
			if ( m_maxValue != value )
			{
				m_maxValue = value > 0 ? value : 1;
				if ( m_slider != null )
				{
					if ( m_value > m_maxValue )
					{
						Value = m_maxValue;
					}
					else 
					{
						m_slider.sliderValue = Mathf.Clamp01(m_value / m_maxValue);
						OnSliderChanged();
					}
				}
			}
		}
	}
	
	[SerializeField]
	private UISlider.Direction m_direction = UISlider.Direction.Horizontal;
	 
	public UISlider.Direction direction
	{
		get { return m_direction; }
		set {
		
			if ( m_direction != value )
			{
				m_direction = value;
				if ( m_slider != null )
				{
					m_slider.direction = value;
				}
			}
		}
	}
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl();
		
		m_slider = GetComponent<UISlider>();
		m_background = findChild("Background") as NvUIImage;
		m_foreground = findChild ("Foreground") as NvUIImage;
		m_thumb = findChild("Thumb") as NvUIImage;		
	}
	
	virtual protected void Start()
	{
		if ( m_slider != null )
		{
			m_slider.direction = m_direction;
			m_slider.sliderValue = m_maxValue > 0 ? Mathf.Clamp01(m_value / m_maxValue) : m_value;
			m_slider.functionName = "OnSliderChanged";
		}
	}
	
	private void OnSliderChanged()
	{
		if ( eventOnSliderChanged != null )
		{
			eventOnSliderChanged( this, m_value );
		}
		
		m_value = m_maxValue > 0 ? m_slider.sliderValue * m_maxValue : m_slider.sliderValue;
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
		
		if ( m_thumb != null )
		{
			m_thumb.setVisible(flag);
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
//		if ( m_thumb != null )
//		{
//			m_thumb.revertVisible();
//		}
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
		
		if ( m_thumb != null )
		{
			m_thumb.setEnable(flag);
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
//		if ( m_thumb != null )
//		{
//			m_thumb.revertEnable();
//		}
//	}
}
