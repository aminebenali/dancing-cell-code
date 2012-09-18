using UnityEngine;
using System.Collections;

public class NvUILabel : NvUIStatic {
	
	private UILabel m_label = null;
	
	[SerializeField]
	private UILabel.Effect m_effectStyle = UILabel.Effect.None;
	
	[SerializeField]
	private Color m_effectColor = Color.black;
	
	
	[SerializeField]
	private bool m_multiLine = true;
	
	[SerializeField]
	private int m_lineWidth = 0;
	
	[SerializeField]
	private string m_text;

	public int lineWidth	
	{
		get { return m_lineWidth; }
		set {
		
			if ( m_lineWidth != value && value > 0 )
			{
				m_lineWidth = value;
				if ( m_label != null )
				{
					m_label.lineWidth = m_lineWidth;
				}
			}
			
		}
	}
	
	public bool multiLine
	{
		get { return m_multiLine; }
		
		set {
		
			if ( m_multiLine != value )
			{
				m_multiLine = value;
				if ( m_label != null )
				{
					m_label.multiLine = m_multiLine;
				}
			}
			
		}
	}
	
	public string text
	{
		get {
			
			return m_text;
			
		}
		
		set {
			
			if ( m_text != value )
			{
				m_text = value;
				if ( m_label != null )
				{
					m_label.text = m_text;
				}
			}
		}
	}
	
	public Vector2 fontSize
	{
		get {
			
			if ( m_label != null )
			{
				Vector3 scale = m_label.transform.localScale;
				return new Vector2( scale.x, scale.y );
			}
			
			return Vector2.zero;
		}
		
		set {
			
			if ( m_label != null )
			{
				Vector2 scale = value;
				m_label.transform.localScale = new Vector3( scale.x, scale.y, 0.0f );
			}

		}
	}
	
	public Color fontColor
	{
		
		get { 
			
			if ( m_label != null )
			{
				return m_label.color;
			}
			
			return Color.gray;
		}
		
		set {
			
			if ( m_label != null )
			{
				m_label.color = value;
			}
			
		}
	}
	
	public UILabel.Effect effectStyle
	{
		get { return m_effectStyle; }
		set {
		
			if ( m_effectStyle != value )
			{
				m_effectStyle = value;
				if ( m_label != null )
				{
					m_label.effectStyle = value;
				}
			}
		}
	}
	
	public Color effectColor
	{
		get { return m_effectColor; }
		set {
		
			if ( m_effectColor != value )
			{
				m_effectColor = value;
				if ( m_label != null )
				{
					m_label.effectColor = value;
				}
			}
			
		}
	}
	
	public UIWidget.Pivot alignment
	{
		get {
			
			if ( m_label != null )
			{
				return m_label.pivot;
			}
			
			return UIWidget.Pivot.Center;
		}
		
		set {
			
			if ( m_label != null )
			{
				m_label.pivot = value;
			}
		}
	}
	
	public bool isPassword
	{
		get {
			
			if ( m_label != null )
			{
				return m_label.password;
			}
			
			return false;
		}
		
		set {
			
			if ( m_label != null )
			{
				m_label.password = value;
			}
		}
	}
	
	public UIFont font
	{
		get {
			
			if ( m_label != null )
			{
				return m_label.font;
			}
			return null;
			
		}
		
		set {
		
			if ( m_label != null )
			{
				m_label.font = font;
			}
			
		}
	}
	
	protected override void AwakeImpl()
	{
		base.AwakeImpl();
		
		m_label = GetComponent<UILabel>();
		
		if ( m_label != null )
		{
			m_widget = m_label.transform;
			m_label.lineWidth = m_lineWidth;
			m_label.multiLine = m_multiLine;
			m_label.text = m_text;
			m_label.effectStyle = m_effectStyle;
			m_label.effectColor = m_effectColor;
		}	
	}
}
