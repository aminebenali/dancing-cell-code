using UnityEngine;
using System.Collections;

//创建BeginGameDlg
public class DcCreateDlg : MonoBehaviour 
{
	public bool bIsPlayBgSound = false;
	public string strBgSoundName = "";
	public string strUIRoot = "";
	public string strDlgName = "";
	
	void Start () 
	{
		//Debug.Log("DcCreateDlg Start");
		
		StartCoroutine(_CreateDlg());
		
		if(bIsPlayBgSound)
		{
			NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
			soundctr.PlayBgMusic(strBgSoundName);
		}
	}
	
	IEnumerator _CreateDlg()
    {
        GameObject obj = GameObject.Find(strUIRoot);
		while(!obj)
		{
			yield return new WaitForSeconds(0.1f);
			obj = GameObject.Find(strUIRoot);
		}
		
        NvUIDialogManager m_DialogMgr = obj.GetComponent(typeof(NvUIDialogManager)) as NvUIDialogManager;
		m_DialogMgr.OpenDialog(strDlgName);
    }
}
