using UnityEngine;
using System.Collections;

public class DcAddChar : MonoBehaviour 
{
	public string strCharSceneName = "";
	
	void Start () 
	{
		if(gameObject.animation["Take 001"] != null)
		{
			gameObject.animation["Take 001"].wrapMode = WrapMode.Loop;
		}

        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.AddScene(strCharSceneName, AddCharSceneEnd);
	}
	
    void AddCharSceneEnd(string str)
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.AddScene("UI_DancingMain", AddUIDancingMainEnd);
	}

    void AddUIDancingMainEnd(string str)
    {
        GameObject obj = GameObject.Find("DancingMain");
        if (obj)
        {
            NvUIDialogManager m_DialogMgr = obj.GetComponent(typeof(NvUIDialogManager)) as NvUIDialogManager;
            DcDancingMainDlg dlgDM = m_DialogMgr.FindDialog("DancingMainDlg") as DcDancingMainDlg;
            obj = GameObject.Find("CharManager");
            dlgDM.InitCharMgr(obj.GetComponent<DcCharMgr>());
            dlgDM.StartDancing();
        }
    }

    void Update()
    {

    }
}
