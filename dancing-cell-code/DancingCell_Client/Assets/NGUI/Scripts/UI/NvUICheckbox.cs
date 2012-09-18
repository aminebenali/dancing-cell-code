using UnityEngine;
using System.Collections;

public class NvUICheckbox : NvUIButton {
	
	public enum EChkboxState
	{
		eChkboxState_Unchecked = 0,
		eChkboxState_Checked
	}
	
	[SerializeField]
	private EChkboxState m_state = EChkboxState.eChkboxState_Unchecked;
	
	public EChkboxState state
	{
		get { return m_state; }
	}
	
	protected override void OnClickImpl ()
	{
		if ( m_state == EChkboxState.eChkboxState_Unchecked )
		{
			m_state = EChkboxState.eChkboxState_Checked;
		}
		else
		{
			m_state = EChkboxState.eChkboxState_Unchecked;
		}
		
		apply();
		
		base.OnClickImpl();
	}
	
	protected override void AwakeImpl ()
	{
		base.AwakeImpl();
		
		if ( enable )
		{
			apply ();
		}
	}
	
	protected override void OnHoverImpl( bool hover ) {}
	protected override void OnPressImpl( bool pressed ) {}
	
	private void apply()
	{
		if ( m_state == EChkboxState.eChkboxState_Checked )
		{
			SwitchStyle( m_skin.pressedStyle, m_skin.pressedColor );
		}
		else if ( m_state == EChkboxState.eChkboxState_Unchecked )
		{
			SwitchStyle( m_skin.normalStyle, m_skin.normalColor );
		}
	}
	
	public void setChecked( bool flag )
	{
		if ( enable )
		{
			m_state = flag ? EChkboxState.eChkboxState_Checked : EChkboxState.eChkboxState_Unchecked;
			apply();
		}
	}
}
