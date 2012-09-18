using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIAAADlg : NvUIDialogBase 
{
// 	private bool m_IsPlaying = false;
// 	
// 	public ManAnim[] m_Players = null;
// 	
// 	protected override void OnDlgCreate()
// 	{
//         //Object.FindObjectOfType(typeof(ManAnim));
// 
//         m_Players = Object.FindObjectsOfType(typeof(ManAnim)) as ManAnim[];
// 	}
// 	
// 	protected override void OnDlgInit ()
// 	{
//         showDialog(false);
// 	}
// 	
// 	private void WaitToClose(float timepass)
// 	{
// 		StartCoroutine(DoWaitToClose(timepass));
// 	}
// 	
// 
// 	IEnumerator DoWaitToClose(float timepass)
// 	{
// 		yield return new WaitForSeconds(timepass);
// 		
// 		closeDialog();
// 	}
// 	
// 	protected override void OnDlgBeginEffect( EDialogEffect effect ) 
// 	{
// 		if(effect == EDialogEffect.eDE_OpenEffect)//when open dlg
// 		{
// 			Apply();
// 		}
// 	}
// 	protected override void OnDlgEndEffect( EDialogEffect effect ) 
// 	{
// //		if (effect == NvUIDialogBase.EDialogEffect.eDE_CloseEffect)
// //		{
// //			EventOnCloseAnimEnd();
// //		}
// 	}
// 	
// 	private void Apply()
// 	{
// 
// 	}
// 	
// 	
// 	private void TestBtn()
// 	{
//         //Debug.Log("aaa");
// 
//         m_IsPlaying = !m_IsPlaying;
// 
//         if (m_Players != null)
//         {
//             for (int i = 0; i < m_Players.Length; i++)
//             {
//                 m_Players[i].PlayMotion(m_IsPlaying);
//             }
//             Debug.Log("m_Player!=null");
//         }
// 	}
}
