using UnityEngine;
using System.Collections;

public class NvUIProgressBar : NvUIStatic {
	
	[SerializeField]
	private int m_maxValue = 100;
	
	[SerializeField]
	private float m_value = 0.0f;
	
	private UISlider m_slider = null;
	
	[SerializeField]
	private bool m_showValue = false;
	
	[SerializeField] 
	private UISlider.Direction m_direction = UISlider.Direction.Horizontal;
	
	[SerializeField]
	private NvUILabel m_lblValue = null;
	
	[SerializeField]
	private NvUIImage m_background = null;
	
	[SerializeField]
	private NvUIImage m_foreground = null;
	
	// progress-bar event
	public event OnElementTriggered eventOnProgress;
	
	public UISlider.Direction direction
	{
		get { return m_direction; }
		set {
		
			if ( m_direction != value )
			{
			
				m_direction = value;
				
				if ( m_slider != null )
				{
					m_slider.direction = m_direction;
				}
				
			}
			
		}
	}
	
	public bool showValue
	{
		get { return m_showValue; }
		set {
		
			if ( m_showValue != value )
			{
				m_showValue = value;
				if ( m_lblValue != null )
				{
					m_lblValue.setVisible( m_showValue );
				}
			}
		}
	}
	
	public float Value
	{
		get { return m_value; }
		set {
		
			if ( m_value != value )
			{
				m_value = value;
				if ( m_value > m_maxValue )
				{
					m_value = m_maxValue;
				}
				
				applyValue ();
			}
		}
	}
	
	public int MaxValue
	{
		get { return m_maxValue; }
		set {
		
			if ( m_maxValue != value )
			{
				
				m_maxValue = value;
				
				if ( m_maxValue < m_value )
				{
					Value = (float)m_maxValue;
				}
				else 
				{
					applyValue();
				}
			}
			
		}
	}
	
	public Color valueColor
	{
		get { 
		
			if ( m_lblValue != null )
			{
				return m_lblValue.fontColor;
			}
			return Color.gray;
		}
		
		set {
		
			if ( m_lblValue != null )
			{
				m_lblValue.fontColor = value;
			}
			
		}
	}
	
	public string valueText
	{
		get {
		
			if ( m_lblValue != null )
			{
				return m_lblValue.text;
			}
			return (m_value.ToString()) + " %";
		}
		
		set {
		
			if ( m_lblValue != null )
			{
				m_lblValue.text = value;
			}
			
		}
	}
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl();
		
		m_slider = gameObject.GetComponent<UISlider>();
		
		if ( m_slider != null )
		{
			m_slider.direction = m_direction;
			m_slider.eventReceiver = gameObject;
			m_slider.functionName = "OnProgress";
			m_slider.thumb = null;
			applyValue();
		}
		
		if ( m_lblValue == null )
		{
			m_lblValue = findChild("Value") as NvUILabel;
			if ( m_lblValue != null )
			{
				m_lblValue.setVisible( m_showValue );
			}
		}
		else
		{
			m_lblValue.setVisible( m_showValue );
		}
		
		if ( m_background == null )
		{
			m_background = findChild("Background") as NvUIImage;
			if ( m_background != null )
			{
				m_background.setVisible( m_showValue );
			}
		}
		else
		{
			m_background.setVisible( m_showValue );
		}
		
		if ( m_foreground == null )
		{
			m_foreground = findChild("Foreground") as NvUIImage;
			if ( m_foreground != null )
			{
				m_foreground.setVisible( m_showValue );
			}
		}
		else
		{
			m_foreground.setVisible( m_showValue );
		}
	}
	
	private void applyValue()
	{
		if ( m_slider != null )
		{
			m_slider.sliderValue = m_maxValue > 0 ? Mathf.Clamp01(m_value / m_maxValue) : m_value;
		}
		
		OnProgress();
	}
	
	private void OnProgress()
	{
		if ( eventOnProgress != null )
		{
			eventOnProgress( this, m_value as object );
		}
	}
	
	public override void setVisible (bool flag)
	{
		base.setVisible (flag);
		
		if ( m_background != null )
		{
			m_background.setVisible( flag );
		}
		
		if ( m_foreground != null )
		{
			m_foreground.setVisible( flag );
		}
		
		if ( m_lblValue != null )
		{
			m_lblValue.setVisible( flag );
		}
	}
	
//	public override void revertVisible()
//	{
//		base.revertVisible();
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
//		if ( m_lblValue != null )
//		{
//			m_lblValue.revertVisible();
//		}
//	}
	
	public override void setEnable (bool flag)
	{
		base.setEnable (flag);
		
		if ( m_lblValue != null )
		{
			m_lblValue.setEnable( flag );
		}
		
		if ( m_background != null )
		{
			m_background.setEnable( flag );
		}
		
		if ( m_foreground != null )
		{
			m_foreground.setEnable( flag );
		}
	}
	
//	public override void revertEnable ()
//	{
//		base.revertEnable ();
//		
//		if ( m_lblValue != null )
//		{
//			m_lblValue.revertEnable();
//		}
//		
//		if ( m_foreground != null )
//		{
//			m_foreground.revertEnable();
//		}
//		
//		if ( m_lblValue != null )
//		{
//			m_lblValue.revertEnable();
//		}
//	}
}
