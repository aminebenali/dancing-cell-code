using UnityEngine;
using System.Collections;
using System.Collections.Generic;


enum CharAttr
{
	ePhysical,
	eEndurance,
	eCharm,
	eTechnique
}

public class DcPersonalDlg : NvUIDialogBase 
{	
	public UILabel LabName;
	public UILabel LabLv;
	
	public UILabel LabDancingCnt;
	public UILabel LabCombo;
	public UILabel LabMaxScores;
	public UILabel LabHasSongs;
	
	public UISlider ProPhysical;
	public UISlider ProEndurance;
	public UISlider ProCharm;
	public UISlider ProTechnique;
	public UILabel LabPhysical;
	public UILabel LabEndurance;
	public UILabel LabCharm;
	public UILabel LabTechnique;
	
	private int nPhysicalUpperLimit		= 100;
	private int nEnduranceUpperLimit	= 100;
	private int nCharmUpperLimit		= 100;
	private int nTechniqueUpperLimit	= 100;
    
	public UISprite[] SprMoneyNumber;
    private DcNumber cMoneyNumber = new DcNumber();
    public UISprite[] SprExpNumber;
    private DcNumber cExpNumber = new DcNumber();
	
	protected override void OnDlgCreate()
	{
		
	}
	
	protected override void OnDlgInit ()
	{
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
		DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex, CharModel.eOne, CharForWhat.eChest,true);
		
		LabName.text = mgr.cShareData.strUserName;
		LabLv.text = "6";
		LabDancingCnt.text = "8";
		LabCombo.text = "36";
		LabMaxScores.text = "18000";
		LabHasSongs.text = "9";
		
		SetAttribute(CharAttr.ePhysical,30);
		SetAttribute(CharAttr.eEndurance,60);
		SetAttribute(CharAttr.eCharm,50);
		SetAttribute(CharAttr.eTechnique,80);

        cMoneyNumber.Init(ref SprMoneyNumber, eNumberType.eSilver);
        cExpNumber.Init(ref SprExpNumber);
		
		//test
		cMoneyNumber.ShowNumber(ref SprMoneyNumber, 12345);
        cExpNumber.ShowNumber(ref SprExpNumber, 67890);
	}
	
	void SetAttribute(CharAttr eAttr,int nVal)
	{
		switch(eAttr)
		{
		case CharAttr.ePhysical:
			ProPhysical.sliderValue = nVal/(float)nPhysicalUpperLimit;
			LabPhysical.text = nVal.ToString() + "/" + nPhysicalUpperLimit.ToString();
			break;
		case CharAttr.eEndurance:
			ProEndurance.sliderValue = nVal/(float)nEnduranceUpperLimit;
			LabEndurance.text = nVal.ToString() + "/" + nEnduranceUpperLimit.ToString();
			break;
		case CharAttr.eCharm:
			ProCharm.sliderValue = nVal/(float)nCharmUpperLimit;
			LabCharm.text = nVal.ToString() + "/" + nCharmUpperLimit.ToString();
			break;
		case CharAttr.eTechnique:
			ProTechnique.sliderValue = nVal/(float)nTechniqueUpperLimit;
			LabTechnique.text = nVal.ToString() + "/" + nTechniqueUpperLimit.ToString();
			break;
		}
	}
	
    private void ReturnInterior()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        mgr.ChangeScene("UI_MyHome_Interior");
		
		NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
		soundctr.PlaySe("ui_touch");
    }
}
