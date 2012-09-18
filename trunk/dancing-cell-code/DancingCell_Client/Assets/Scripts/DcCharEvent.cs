using UnityEngine;
using System.Collections;

public class DcCharEvent : MonoBehaviour 
{

    void OnClick()
    {
        GameObject obj = GameObject.Find("Interior");
        if (obj)
        {
            NvUIDialogManager m_DialogMgr = obj.GetComponent(typeof(NvUIDialogManager)) as NvUIDialogManager;
            DcInteriorDlg dlgI = m_DialogMgr.FindDialog("InteriorDlg") as DcInteriorDlg;
            if (dlgI.gameObject.active)
            {
                dlgI.GotoPersonal();
            }
        }
		
		obj = GameObject.Find("CreateCharacter");
        if (obj)
        {
            
        }
    }
}
