using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DcCreateCharacterDlg : NvUIDialogBase 
{
    public UIInput InpUserName;
	public UICheckbox[] ChbSexSel;
    public BoxCollider BoxOK;
	public UISprite SprOKBtn;
    public UISprite SprOKText;

    private bool bIsShowOK = false;
    private int nCurSelSex = 1;//Default Woman

    public GameObject[] cTempChar;
    private GameObject cManObj;
    private GameObject cWomanObj;
	
    private string[] strManName =	{
										"Robert","Carl","Scott","Tom","Eddy","Kris","Peter",
										"Johnson","Bruce","Richard","Charles","Bill","Joseph","John"
                                 	};
    private string[] strWomanName =	{
                                    	"Shelly","Mary","Dolly","Nancy","Jane","Barbara",
										"Shirley","Emily","Sophia","Vivian","Lillian","Joy"
                                  	};

    protected override void OnDlgCreate()
    {
        PlayerPrefs.SetInt("UserSex", 0);
	}

	protected override void OnDlgInit ()
	{
        BoxOK.size = new Vector3(0,0,0);
		SprOKBtn.spriteName = "GrayButton";
        SprOKText.color = new Color(0.66f, 0.66f, 0.66f);

        cTempChar[0] = GameObject.Find("GameObj_Man");
        cTempChar[1] = GameObject.Find("GameObj_Woman");

        cManObj = DcGlobalFunc.CreateChar(CharSex.eMan, CharModel.eOne, CharForWhat.eCreateCharacter, true);
        cManObj.transform.parent = cTempChar[0].transform;
        cManObj.transform.localPosition = new Vector3(0f, 0f, 0f);
        cManObj.transform.Rotate(new Vector3(0, 180f, 0));

        cWomanObj = DcGlobalFunc.CreateChar(CharSex.eWoman, CharModel.eOne, CharForWhat.eCreateCharacter,true);
        cWomanObj.transform.parent = cTempChar[1].transform;
        cWomanObj.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

    protected override void lazyUpdateImpl()
    {
        //Debug.LogError(InpUserName.text + "  " + bIsShowOK);

        if (!string.IsNullOrEmpty(InpUserName.text) && !bIsShowOK)
        {
            bIsShowOK = true;
            BoxOK.size = new Vector3(1,1,1);
			SprOKBtn.spriteName = "GreenButton";
            SprOKText.color = new Color(1, 1, 1);
        }

        if (string.IsNullOrEmpty(InpUserName.text) && bIsShowOK)
        {
            bIsShowOK = false;
            BoxOK.size = new Vector3(0,0,0);
			SprOKBtn.spriteName = "GrayButton";
            SprOKText.color = new Color(0.66f, 0.66f, 0.66f);
        }

        ChangeCharColor(cManObj);
        ChangeCharColor(cWomanObj);
    }

    void ChangeCharColor(GameObject cObj)
    {
        float fColor = (cObj.transform.position.z + 0.25f) * -1.65f;
        fColor = fColor > 1f ? 1f : fColor;
        SkinnedMeshRenderer[] cSMR;
        cSMR = cObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        float fTemp = fColor * 0.588f;
        for (int i = 0; i < cSMR.Length; i++)
        {
            cSMR[i].material.color = new Color(fTemp, fTemp, fTemp, 1f);

            if(cSMR[i].name == "hair" || cSMR[i].name == "glasses")
            {
                if(fTemp < 0.5f)
                {
                    cSMR[i].material.shader = Shader.Find("Transparent/Diffuse");
                }
                else
                {
                    cSMR[i].material.shader = Shader.Find("Unlit/Transparent Cutout");
                }
            }
        }
    }

    private void OK()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;

        PlayerPrefs.SetString("UserName", InpUserName.text);
		PlayerPrefs.SetInt("UserSex", nCurSelSex);
		mgr.cShareData.strUserName = InpUserName.text;
        mgr.cShareData.eUserSex = nCurSelSex == 0 ? CharSex.eMan : CharSex.eWoman;

        showDialog(false);

        mgr.ChangeScene("UI_Map");

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_enter");
    }

    private void Random()
    {
		if(nCurSelSex == 0)
		{
        	InpUserName.text = strManName[(int)(UnityEngine.Random.value * 100f) % strManName.Length];
		}
		else
		{
			InpUserName.text = strWomanName[(int)(UnityEngine.Random.value * 100f) % strWomanName.Length];
		}
		
        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
    }

    private bool bIsFirst = true;
	private void SelSex(bool bIsCheck)
	{
		if(bIsCheck)
		{
			InpUserName.text = "";
			
			nCurSelSex = ChbSexSel[0].isChecked ? 0 : 1;
			
            if (bIsFirst)
            {
                bIsFirst = false;
                return;
            }

            if (nCurSelSex == 0)
            {
                cTempChar[0].gameObject.animation.CrossFade("CharToFront");
                cTempChar[1].gameObject.animation.CrossFade("CharToAfter");
            }
            else
            {
                cTempChar[0].gameObject.animation.CrossFade("CharToAfter");
                cTempChar[1].gameObject.animation.CrossFade("CharToFront");
            }

            NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
            soundctr.PlaySe("ui_check");
		}
	}
}
