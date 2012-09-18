using UnityEngine;
using System.Collections;


public class NvUIImageLable : NvUIStatic
{
	[SerializeField]
	protected string m_value = "";
	
	[SerializeField]
	protected NvUIImage[] showSlots = null;
	
	[SerializeField]
	protected bool m_isFixedWidth = false;
	
	[SerializeField]
	protected int m_paddingWidth = 1;
	
	[SerializeField]
	protected int m_fixedWidth = 36;
	
	public delegate void funcOnAnimEnd();
	
	public event funcOnAnimEnd eventOnAnimEnd;
	
	public int paddingWidth
	{
		get { return m_paddingWidth; }
		set {
		
			if ( m_paddingWidth != value )
			{
				m_paddingWidth = value;
				
				apply();
				
			}
			
		}
	}
	
	public bool isFixedWidth
	{
		get { return m_isFixedWidth; }
		set {
		
			if ( m_isFixedWidth != value )
			{
				m_isFixedWidth = value;
				
				apply();
				
			}
			
		}
	}
	
	public enum EAnchor
	{
		eAnchor_Left = 0,
		eAnchor_Center,
		eAnchor_Right
	}
	
	[SerializeField]
	protected EAnchor m_anchor = EAnchor.eAnchor_Center;
	
	public EAnchor anchor
	{
		get { return m_anchor; }
		set {
			
			if ( m_anchor != value )
			{
				m_anchor = value;
				
				apply();
				
			}
		}
	}
	
	public int fixedWidth
	{
		get { return m_fixedWidth; }
		set {
		
			if ( m_fixedWidth != value )
			{
				m_fixedWidth = value;
				if ( m_isFixedWidth )
				{
					apply();
				}
			}
			
		}
	}
	
	protected Vector2 m_showLabelSize = Vector2.zero;
	
	protected void apply()
	{
		if(visible)
		{
			applyValue();
			applyAnchor();
		}
	}
	
	protected void DoAnimEnd()
	{
		if(eventOnAnimEnd!=null)
		{
			eventOnAnimEnd();
		}
	}
	
	protected void applyAnchor()
	{
		NvUIImage slotHead = showSlots[0];
		if ( slotHead != null )
		{
			float w = m_showLabelSize.x;
			float h = m_showLabelSize.y;
		
			if ( m_anchor == EAnchor.eAnchor_Center )
			{
				
				float offsetX = (w-slotHead.transform.localScale.x)/-2;
				
				//Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^m_value:  "+m_value+"  m_showLabelSize.x: "+m_showLabelSize.x+"offsetX: "+offsetX);
				
				foreach ( NvUIImage slot in showSlots )
				{
					if ( slot.visible )
					{
						Vector3 originPos = slot.transform.localPosition;
						slot.transform.localPosition = new Vector3( originPos.x + offsetX, originPos.y , originPos.z );
					}
				}
			}
		}
	}
	
	public override void setVisible (bool flag)
	{
		base.setVisible (flag);
		
		foreach( NvUIImage slot in showSlots )
		{
			slot.setVisible( flag );	
		}
		
		if ( flag == true )
		{
			apply();
		}
	}
	protected virtual void applyValue(){}
	
	protected virtual void OnAnimEnd(){}
		
	public override void setEnable (bool flag)
	{
		base.setEnable (flag);
		
		foreach( NvUIImage slot in showSlots )
		{
			slot.setEnable( flag );	
		}
	}
	
	public void PlayBBAnimation()
	{
		Animation anim = gameObject.GetComponent<Animation>();
		
		if(anim!=null && anim.clip!=null)
		{
			anim.Stop();
			anim.Play();
		}
	}
	
}