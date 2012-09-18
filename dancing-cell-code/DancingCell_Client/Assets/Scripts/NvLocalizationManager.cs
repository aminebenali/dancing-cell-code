using UnityEngine;
using System.Collections;

public class NvLocalizationManager : MonoBehaviour
{
	public Localization m_Localization;
	
	private bool m_IsInit = false;
	
	
	
	// Use this for initialization
	void Start ()
	{
		m_Localization = DcGame.getInstance ().transform.FindChild ("Localization(Clone)").GetComponent (typeof(Localization)) as Localization;
		if (m_Localization == null){
			Debug.LogError("CAN'T FIND LOCALIZATION FILE");
		}
		//m_Localization.currentLanguage = "cn";
		
		m_IsInit = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public bool IsInit()
	{
		return m_IsInit;
	}
	
	public void ChangeLanguage(string lan)
	{
		m_Localization.currentLanguage = lan;
	}
	
	public string GetValue(string key)
	{
		return m_Localization.Get(key);
	}
}

