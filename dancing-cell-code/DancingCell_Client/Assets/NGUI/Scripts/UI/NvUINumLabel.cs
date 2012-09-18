using UnityEngine;
using System.Collections;

public class NvUINumLabel : NvUIImageLable
{
	public string[] spriteTable = new string[12] { "red_0", "red_1", "red_2", "red_3", "red_4", "red_5", "red_6", "red_7", "red_8", "red_9", "red_minus", "red_plus" };
	
	public bool ShowSign = false;
	
	public int Value
	{
		get { 
			
			if(m_value == "")
			{
				return 0;
			}
			else 
			{
				return int.Parse(m_value); 
			}
			
		}
		set {
			
			if(value > 0)
			{
				if ( ShowSign )
				{
					if(m_value!="+"+value.ToString())
					{
						m_value = "+"+value.ToString();
					
						apply();
					}	
				}
				else
				{
					if ( m_value != value.ToString() )
					{
						m_value = value.ToString();
						apply ();
					}
				}
			}
			else
			{
				if(m_value!=value.ToString())
				{
					m_value = value.ToString();
				
					apply();
				}
			}
		}
	}
	
#if UNITY_EDITOR
	[SerializeField]
	private Color m_NumColor = Color.white;
#else
	private Color m_NumColor = Color.white;
#endif
	
	private Color m_NumColorPrevious = Color.white;
	
	public Color NumColor
	{
		get { return m_NumColor; }
		set {
		
			m_NumColorPrevious = m_NumColor;
			m_NumColor = value;
			foreach ( NvUIImage slot in showSlots )
			{
				if ( slot != null )
				{
					slot.spriteColor = value;
				}
			}
			
		}
	}

	protected override void OnAnimEnd()
	{
		DoAnimEnd();
	}
	
	protected override void applyValue()
	{
		string sNum = m_value;
		NvUIImage slot = null;

		m_showLabelSize = Vector2.zero;
		
		for ( int ii = 0; ii < showSlots.Length; ++ii )
		{
			if ( ii < sNum.Length )
			{
				
				if ( sNum[ii] == '-' )
				{
					showSlots[ii].spriteName = spriteTable[10];
				}
				else if ( sNum[ii] == '+' )
				{
					showSlots[ii].spriteName = spriteTable[11];
				}
				else if ( sNum[ii] >= '0' && sNum[ii] <= '9' )
				{
					showSlots[ii].spriteName = spriteTable[ (int)(sNum[ii] - '0') ];
				}
				
				Vector3 scale = Vector3.zero;
				Vector3 pos = Vector3.zero;
				
				if ( slot != null )
				{
					scale = slot.transform.localScale;
					pos = slot.transform.localPosition;	
				}
				
				if ( m_isFixedWidth )
				{
					//Debug.Log("^^^^^^^^^m_isFixedWidth  showSlots: "+showSlots[ii].spriteName+"  m_fixedWidth: "+m_fixedWidth+" m_paddingWidth:"+m_paddingWidth );
					
					showSlots[ii].transform.localPosition = new Vector3( pos.x + (slot==null?0:m_fixedWidth) + m_paddingWidth, pos.y, pos.z );
				}
				else
				{
					//Debug.Log("^^^^^^^^^showSlots: "+showSlots[ii].spriteName+"  showSlots[ii].transform.localPosition: "+showSlots[ii].transform.localPosition+" pos:"+pos+" scale: "+scale+" m_paddingWidth: "+m_paddingWidth );
					showSlots[ii].transform.localPosition = new Vector3( pos.x + scale.x + m_paddingWidth, pos.y, pos.z );
				}

				if ( m_isFixedWidth )
				{
					scale = showSlots[ii].transform.localScale;
					scale.x = m_fixedWidth;
					showSlots[ii].transform.localScale = scale;
				}
				
				showSlots[ii].setVisible( true );
				slot = showSlots[ii];
				
				
				m_showLabelSize.x = slot.transform.localPosition.x + slot.transform.localScale.x;
				//Debug.Log("^^^^^^^^^^^^^^^^^^^^^slot: "+slot.spriteName+" m_numLabelSize.x: "+m_numLabelSize.x+"   slot.transform.localPosition.x: "+slot.transform.localPosition.x+"   slot.transform.localScale.x: "+ slot.transform.localScale.x);
				m_showLabelSize.y = m_showLabelSize.y < slot.transform.localScale.y ? slot.transform.localScale.y : m_showLabelSize.y;
			}
			else 
			{
				showSlots[ii].setVisible( false );
			}
		}
	}
	
#if UNITY_EDITOR
	void Update ()
	{
		base.lazyUpdateImpl ();
		if ( m_NumColor != m_NumColorPrevious )
		{
			NumColor = m_NumColor;
		}
	}
#endif
	
}
