using UnityEngine;
using System.Collections;




public class NvUITextImageLable : NvUIImageLable 
{
	public string Value
	{
		get 
		{ 
			return m_value; 
		}
		set 
		{
			if ( m_value != value)
			{
				m_value = value;
				
				apply();
			}
		}
	}
	
	
	protected override void OnAnimEnd()
	{
		DoAnimEnd();
	}
		
	protected override void applyValue()
	{
//		string sNum = m_value;
//		NvUIImage slot = null;
//		
//		//NvSkillTextSprteController Controller = Singlton.getInstance("NvSkillTextSprteController")as NvSkillTextSprteController;
//		
//		m_showLabelSize = Vector2.zero;
//		
//		//Debug.Log("#######################################=====> sNum:  |"+sNum+"  sNum.Length: "+sNum.Length);
//		for ( int ii = 0; ii < showSlots.Length; ++ii )
//		{
//			if ( ii < sNum.Length )
//			{
//				string spritename = Controller.GetSpriteItem(sNum[ii].ToString());
//				
//				//Debug.Log("#######################################=====>spritename: "+spritename+"     showSlots[ii]: "+showSlots[ii].name);
//				
//				if(spritename=="")
//				{
//					showSlots[ii].setVisible(false);
//					
//					continue;
//				}
//				else
//				{
//					showSlots[ii].setVisible(true);
//					
//					showSlots[ii].spriteName = spritename;
//					
//				}
//				Vector3 scale = Vector3.zero;
//				
//				Vector3 pos = Vector3.zero;
//				
//				if ( slot != null )
//				{
//					scale = slot.transform.localScale;
//					pos = slot.transform.localPosition;	
//				}
//				
//				if ( m_isFixedWidth )
//				{
//					//Debug.Log("^^^^^^^^^m_isFixedWidth  showSlots: "+showSlots[ii].spriteName+"  m_fixedWidth: "+m_fixedWidth+" m_paddingWidth:"+m_paddingWidth );
//					
//					showSlots[ii].transform.localPosition = new Vector3( pos.x + (slot==null?0:m_fixedWidth) + (slot==null?0:m_paddingWidth), pos.y, pos.z );
//				}
//				else
//				{
//					if(slot!=null)
//					{
//						showSlots[ii].transform.localPosition = new Vector3( pos.x + scale.x/2+(showSlots[ii].transform.localScale.x/2) + (slot==null?0:m_paddingWidth), pos.y, pos.z );
//					
//						//Debug.Log("^^^^^^^^^showSlots: "+showSlots[ii].spriteName+" pos:"+pos+" scale: "+scale+" m_paddingWidth: "+m_paddingWidth +"  showSlots[ii].transform.localPosition: "+showSlots[ii].transform.localPosition);
//					}
//					else
//					{
//						showSlots[ii].transform.localPosition = new Vector3( 0, pos.y, pos.z );
//					}
//				}
//
//				if ( m_isFixedWidth )
//				{
//					scale = showSlots[ii].transform.localScale;
//					scale.x = m_fixedWidth;
//					showSlots[ii].transform.localScale = scale;
//				}
//				
//				showSlots[ii].setVisible( true );
//				slot = showSlots[ii];
//				
//				
//				m_showLabelSize.x = slot.transform.localPosition.x + slot.transform.localScale.x;
//				//Debug.Log("^^^^^^^^^^^^^^^^^^^^^slot: "+slot.spriteName+" m_numLabelSize.x: "+m_numLabelSize.x+"   slot.transform.localPosition.x: "+slot.transform.localPosition.x+"   slot.transform.localScale.x: "+ slot.transform.localScale.x);
//				m_showLabelSize.y = m_showLabelSize.y < slot.transform.localScale.y ? slot.transform.localScale.y : m_showLabelSize.y;
//			}
//			else 
//			{
//				showSlots[ii].setVisible( false );
//			}
//		}
	}
}
