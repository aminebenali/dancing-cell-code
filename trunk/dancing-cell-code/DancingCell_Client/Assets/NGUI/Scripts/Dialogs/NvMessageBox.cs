using UnityEngine;
using System.Collections;

public class NvMessageBox : NvUIDialogBase {
	
	public enum EMessageBoxStyle
	{
		eStyle_OkayOnly = 0,
		eStyle_OkayCancel,
		eStyle_YesNo
	}
	
	public enum EResult
	{
		eResult_Nothing = 0,
		eResult_Okay,
		eResult_Cancel,
		eResult_Yes,
		eResult_No
	}
	
	private EMessageBoxStyle m_style = EMessageBoxStyle.eStyle_OkayOnly;
	
	[SerializeField]
	private NvUILabel m_title = null;
	
	[SerializeField]
	private NvUILabel m_description = null;
	
	[SerializeField]
	private NvUIButton m_btnOk = null;
	
	[SerializeField]
	private NvUIButton m_btnOkCentered = null;
	
	[SerializeField]
	private NvUIButton m_btnCancel = null;
	
	//private static Vector2 kOkButtonSide = new Vector2(-110.0f, -107.1f);
	
	public EMessageBoxStyle style
	{
		get { return m_style; }
		set {
		
			if ( m_style != value )
			{
				
				m_style = value;
//				applyStyle();
				
			}
			
		}
	}
	
	private string m_strDescription = "";
	private string m_strTitle = "";
	
	public string description
	{
		get { return m_strDescription; }
		set {
		
			if ( m_strDescription != value )
			{
				m_strDescription = value;
				
//				if ( m_description != null )
//				{
//					m_description.text = m_strDescription;
//				}
			}
		}
	}
	
	public string title
	{
		get { return m_strTitle; }
		set {
			
			if ( m_strTitle != value )
			{
				m_strTitle = value;
				
//				if ( m_title != null )
//				{
//					m_title.text = m_strTitle;
//				}
			}
		}
	}
	
	public EResult result
	{
		get { return (EResult)m_modalResult; }
	}
	
	protected override void OnDlgCreate()
	{
		
		//m_title = findChild("Main Panel/Anchor Buttons/Title") as NvUILabel;
		//m_description = findChild("Main Panel/Anchor Buttons/Description") as NvUILabel;
		//m_btnOk = findChild ("Main Panel/Anchor Buttons/BtnOkay") as NvUIButton;
		//m_btnCancel = findChild ("Main Panel/Anchor Buttons/BtnCancel") as NvUIButton;
		
		// add event handle
		m_btnOk.eventOnClick += OnButtonClick;
		m_btnCancel.eventOnClick += OnButtonClick;
		m_btnOkCentered.eventOnClick += OnButtonClick;

        m_btnOk.caption = "Ok";
        m_btnOkCentered.caption = "Ok";
        m_btnCancel.caption = "Cancel";
		// specifical
		//m_bg = transform.FindChild("Main Panel/Background");
		
	}
	
	protected override void OnDlgBeginEffect (EDialogEffect effect)
	{
		if ( effect == NvUIDialogBase.EDialogEffect.eDE_OpenEffect )
		{
			apply();
			m_modalResult = (int)EResult.eResult_Nothing;
		}
	}
	
	private void apply()
	{
		m_btnOk.setVisible(false);
		m_btnCancel.setVisible(false);
		m_btnOkCentered.setVisible(false);
		
		applyStyle();
		applyText();
	}
	
	private void applyStyle()
	{
		switch( m_style )
		{
		case EMessageBoxStyle.eStyle_OkayOnly:
			{	
				//Vector3 center = new Vector3(0.0f, kOkButtonSide.y, 0.0f);
				//m_btnOk.transform.localPosition = center;
			
				//m_btnOk.setVisible( true );
				//m_btnCancel.setVisible( false );
				m_btnOkCentered.setVisible( true );
			}
			break;
		case EMessageBoxStyle.eStyle_OkayCancel:
			{
				//m_btnOk.transform.localPosition = new Vector3( kOkButtonSide.x, kOkButtonSide.y, 0.0f );
				m_btnOk.setVisible( true );
				m_btnCancel.setVisible( true );
			}
			break;
		case EMessageBoxStyle.eStyle_YesNo:
			{
				//m_btnOk.transform.localPosition = new Vector3( kOkButtonSide.x, kOkButtonSide.y, 0.0f );
				//m_btnOk.caption = "Yes";
				//m_btnCancel.caption = "No";
			
				m_btnOk.setVisible( true );
				m_btnCancel.setVisible( true );
			}
			break;
		}
	}
	
	private void applyText()
	{
		if ( m_title != null )
		{
			m_title.text = m_strTitle;
		}
		
		if ( m_description != null )
		{
			m_description.text = m_strDescription;
		}
	}
	
	private void OnButtonClick( object sender, object context )
	{
		if ( (sender as NvUIButton) == m_btnOk || (sender as NvUIButton) == m_btnOkCentered )
		{
			if ( m_style == EMessageBoxStyle.eStyle_OkayCancel || m_style == EMessageBoxStyle.eStyle_OkayOnly )
			{
				//m_result = EResult.eResult_Okay;
				//Debug.Log ("NvMessage Callback: OnOkay");
				endModal( (int)EResult.eResult_Okay );
			}
			else if ( m_style == EMessageBoxStyle.eStyle_YesNo )
			{
				//Debug.Log ("NvMessage Callback: OnYes");
				endModal( (int)EResult.eResult_Yes );
			}
		}
		else if ( (sender as NvUIButton) == m_btnCancel )
		{
			if ( m_style == EMessageBoxStyle.eStyle_OkayCancel || m_style == EMessageBoxStyle.eStyle_OkayOnly )
			{
				//Debug.Log ("NvMessage Callback: OnCancel");
				endModal( (int)EResult.eResult_Cancel );
			}
			else if ( m_style == EMessageBoxStyle.eStyle_YesNo )
			{
				//Debug.Log ("NvMessage Callback: OnNo");
				endModal( (int)EResult.eResult_No );
			}
		}

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
	}
	
	protected override void OnDlgClose ()
	{		
		m_title.text = "";
		m_description.text = "";
	}
}
