using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class NvUIBattleResurrection : NvUIDialogBase 
{
	public UILabel                 m_ShowText = null;

	public delegate void        ButHandler();   
	
	public                             ButHandler EventOnCancel;
	
	public                             ButHandler EventOnContinue;
	
	private bool                    m_IsCanPress  = true;
	
	public UISprite   M_TEST = null;
	
	protected override void OnDlgCreate()
	{
		//m_TimeLab = findChild("Main Panel/center/Panel/Panel Common Atlas/textvalue/TimeValue") as NvUILabel;
	}
		
	
	private void OnCancel()
	{
		if(isOpened && m_IsCanPress)
		{
			closeDialog();
			
			EventOnCancel();
			
			m_IsCanPress = false;
		}
	}
	
	private void OnContinue()
	{
		if(isOpened && m_IsCanPress)
		{
			if(IsCanContinue())
			{
				closeDialog();
			
				EventOnContinue();
			}
			else
			{
				BuyMcoin();
			}
		}
	}
	
//	private void ShowNotEnoughMoney()
//	{
//		NvCommonUIManager mCommonUIManager = Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager;
//		
//		string desc =  (Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager).GetValue("ResurrectionValue03");
//					
//		mCommonUIManager.showMessageBox(desc, "", NvMessageBox.EMessageBoxStyle.eStyle_YesNo, OnDoBuyMcoin);
		
// }
	
//	private void OnDoBuyMcoin( int result )
//	{
//		if ( result == (int)NvMessageBox.EResult.eResult_Yes )
//		{
//			BuyMcoin();
//		}
//	}
	
	private bool IsCanContinue()
	{
		int curcoinnum = 0;
		
		int needcoinnum = 10;
		
		GetCoinNum(ref curcoinnum,ref needcoinnum);
		
		if(curcoinnum<needcoinnum)
		{
			return false;
		}
		return true;
	}
	
	private void BuyMcoin()
	{
//		SceneManager sm = Singlton.getInstance("SceneManager") as SceneManager;
//		
//		ChargeSceneController mChargeSceneController = GameObject.FindObjectOfType(typeof(ChargeSceneController)) as ChargeSceneController;
//		
//		if(mChargeSceneController == null)
//		{
//			sm.AddScene("ChargeScene", onLoadChargeScene);
//		}
//		else
//		{
//			onLoadChargeScene("");
//		}
	}
	
	private void onLoadChargeScene(string name)
	{
		//ChargeSceneController mChargeSceneController = GameObject.FindObjectOfType(typeof(ChargeSceneController)) as ChargeSceneController;
		
		//mChargeSceneController.ShowWarning(MSG_ERR_CODE.E_ERR_INSUFFICIENT_NCOIN);
		
		//mChargeSceneController.InsuffientNCoin();
		
		//mChargeSceneController.BuyNCoin();
		
		//mChargeSceneController.ShowWarning(MSG_ERR_CODE.E_ERR_INSUFFICIENT_NCOIN);
		
		m_IsCanPress = true;
	}
	
	protected override void OnDlgBeginEffect( EDialogEffect effect ) 
	{
		if(effect == EDialogEffect.eDE_OpenEffect)//when open dlg
		{
			//m_ShowText.text = /*textleft+*/needcoinnum.ToString()/*+textright*/;
			
			Apply();
		}
	}
	
	private void Apply()
	{
		int needcoinnum = 10;
		
		int curcoin = 0;
		
		GetCoinNum(ref curcoin,ref needcoinnum);
		
		m_ShowText.text = needcoinnum.ToString();
	}
	
	private void GetCoinNum(ref int curcoinnum,ref int needcoinnum)
	{
		//NvPlayerManager player = Singlton.getInstance("NvPlayerManager")as NvPlayerManager;
		
//			if(player.CurrentPlayer!=null &&  player.CurrentPlayer.CostSettings!=null)
//			{
//				CostConst costconst = player.CurrentPlayer.CostSettings.Clone();
//				
//				if(costconst!=null)
//				{
//					needcoinnum = costconst.mcoin_continue;
//				}
//			    curcoinnum = player.CurrentPlayer.mCoin;
//			}
//			else
//			{
//				curcoinnum = 0;
//				
//				needcoinnum = 10;
//			}
	}
	
	protected override void OnDlgInit ()
	{
		//WaitToClose();
		//m_TimeRemain = 15;
		
		//ShowTime();
		Apply();
		
		m_IsCanPress = true;
	}
	
//	private void ShowTime()
//	{
//		if(m_TimeRemain<=0)
//		{
//			OnCancel();
//		}
//		NvLocalizationManager localMgr = Singlton.getInstance("NvLocalizationManager") as NvLocalizationManager;
//		
//		string secremain = localMgr.GetValue("secremain");
//		
//		m_TimeLab.text = "[FFFE00]" + m_TimeRemain + secremain;
//		
//		StartCoroutine(DoWaitReduceTime());
//		
//	}
	
//	IEnumerator DoWaitReduceTime()
//	{
//		yield return new WaitForSeconds(1.0f);
//		
//		m_TimeRemain--;
//		
//		ShowTime();
//	}
	
	protected override void OnDlgEndEffect( EDialogEffect effect ) 
	{
		if (effect == NvUIDialogBase.EDialogEffect.eDE_CloseEffect)
		{
			//EventOnCloseAnimEnd();
		}
	}
	

}
