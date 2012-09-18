using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using LitJson;

public enum CharModel
{
    eOne,
    eTwo
}

public enum CharSex
{
    eMan,
    eWoman
}

public enum CharForWhat
{
    eNull,
    eInterior,
    eChest,
    eDresser,
	
	eCreateCharacter,
	
    eWaitingRoomMain,
    eWaitingRoomLeft,
    eWaitingRoomRight,

    eDancingMain,
    eDancingLeft,
    eDancingRight
}

public enum PEType
{
	eBubbles,
	eFireworks,
    eWinFireworks
}

public class DcGlobalFunc : MonoBehaviour 
{
    static private string[,] strPrefabName = {
                                                {"Main_Char_Man_1","Main_Char_Man_2"},
                                                {"Main_Char_Woman_1","Main_Char_Woman_2"}
                                             };

    static private Vector3[] posCharPos = {
                                              new Vector3(-999f, -0.85f, -0.65f),
                                              new Vector3(0.4f, -0.85f, -0.65f),
                                              new Vector3(-1.22f, -0.76f, -0.65f),
                                              new Vector3(-1.23f, -2.63f, 0.92f),
		
											  new Vector3(0f, 0f, 0f),

                                              new Vector3(-0.06f, -0.8f, -0.8f),
                                              new Vector3(-1.08f, -0.47f, -0.8f),
                                              new Vector3(0.92f, -0.47f, -0.8f),
		
                                              new Vector3(204f, 5.841751f, 1230f),
                                              new Vector3(138.1621f, 3.841751f, 1230.258f),
                                              new Vector3(268.162f, 3.841751f, 1230.258f)
                                          };
    static private Vector3[] scaCharSca = {
                                              new Vector3(0.025f, 0.025f, 0.025f),
                                              new Vector3(0.025f, 0.025f, 0.025f),
                                              new Vector3(0.025f, 0.025f, 0.025f),
                                              new Vector3(0.055f, 0.055f, 0.055f),
		
											  new Vector3(0.025f, 0.025f, 0.025f),

                                              new Vector3(0.02f, 0.02f, 0.02f),
                                              new Vector3(0.02f, 0.02f, 0.02f),
                                              new Vector3(0.02f, 0.02f, 0.02f),
		
                                              new Vector3(1f, 1f, 1f),
                                              new Vector3(1f, 1f, 1f),
                                              new Vector3(1f, 1f, 1f)
                                          };

    static public string strCurMainCharGameObjName = "";
    static private int nNameSerial = 0;

    static public GameObject CreateChar(CharSex eSex = CharSex.eWoman,
                                        CharModel eModel = CharModel.eOne,
                                        CharForWhat eForWhat = CharForWhat.eInterior,
                                        bool bNeedCollider = false,
                                        bool bNeedLight = true)
    {
        GameObject cGO = Resources.Load("player/Char/"+strPrefabName[(int)eSex,(int)eModel]) as GameObject;
        GameObject cMainChar = Utility.Instantiate(cGO) as GameObject;
        cMainChar.name = cGO.name + (++nNameSerial).ToString();
        strCurMainCharGameObjName = cMainChar.name;

        ChangeCharTransform(strCurMainCharGameObjName,eForWhat);

        if(eForWhat == CharForWhat.eDancingLeft)
        {
            cMainChar.transform.Rotate(new Vector3(0, 160, 0));
        }
        else if(eForWhat == CharForWhat.eDancingRight)
        {
            cMainChar.transform.Rotate(new Vector3(0, -160, 0));
        }
        else
        {
            cMainChar.transform.Rotate(new Vector3(0, 180, 0));
        }

        if((int)eForWhat <= (int)CharForWhat.eWaitingRoomRight)
        {
            cMainChar.AddComponent<DcCharacterShadow>();
            DcCharAnim cCharAnim = cMainChar.GetComponent(typeof(DcCharAnim)) as DcCharAnim;
            if(cCharAnim)
            {
                //cCharAnim.bIsAutoPlaySpecialIdle = true;
            }
        }

        if(bNeedCollider)
        {
            cMainChar.layer = LayerMask.NameToLayer("GameUILayout");
            for(int i=0;i<cMainChar.transform.childCount;i++)
	        {
		        GameObject child = cMainChar.transform.GetChild(i).gameObject;
                child.layer = LayerMask.NameToLayer("GameUILayout");
	        }
            BoxCollider cBoxColl = cMainChar.AddComponent<BoxCollider>();
            //BoxCollider cBoxColl = gameObject.GetComponent<BoxCollider>();
            if(eSex == CharSex.eMan)
            {
                cBoxColl.center = new Vector3(0, 34.8f, 0);
                cBoxColl.size = new Vector3(20f, 72.1f, 15f);
            }
            else
            {
                cBoxColl.center = new Vector3(0, 30.7f, 0);
                cBoxColl.size = new Vector3(20f, 62.7f, 15f);
            }
        }
        else
        {
            cMainChar.layer = LayerMask.NameToLayer("Default");
            for(int i=0;i<cMainChar.transform.childCount;i++)
	        {
		        GameObject child = cMainChar.transform.GetChild(i).gameObject;
                child.layer = LayerMask.NameToLayer("Default");
	        }
        }

        if(bNeedLight)
        {
			GameObject cL = GameObject.Find("DirLight");
			if(!cL)
			{
           		cGO = Resources.Load("player/Light/DirLight") as GameObject;
            	GameObject cDirLight = Utility.Instantiate(cGO) as GameObject;
            	cDirLight.name = cGO.name;
			}
        }

        return cMainChar;
    }

    static public void ChangeCharTransform(string strName,CharForWhat eForWhat = CharForWhat.eInterior)
    {
        GameObject cMainChar = GameObject.Find(strName);
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        Vector3 posTemp = posCharPos[(int)eForWhat];
		if(eForWhat == CharForWhat.eDresser && mgr.cShareData.eUserSex == CharSex.eMan)
		{
			posTemp.y -= 0.25f;
		}
        
        cMainChar.transform.localPosition = posTemp;
        cMainChar.transform.localScale = scaCharSca[(int)eForWhat];

        DcCharAnim cCharAnim = cMainChar.GetComponent(typeof(DcCharAnim)) as DcCharAnim;
        if(cCharAnim)
        {
            if(eForWhat == CharForWhat.eDresser)
            {
                //cCharAnim.PlayIdle();
                //cCharAnim.bIsAutoPlaySpecialIdle = false;
            }
            else
            {
                //cCharAnim.bIsAutoPlaySpecialIdle = true;
                //cCharAnim.PlaySpecialIdle();
            }
        }
    }

    static public ParticleEmitter CreateParticle(PEType eType)
    {
		string strName = "";
		switch(eType)
		{
		case PEType.eBubbles:
			strName = "player/SceneElements/Bubbles";
			break;
		case PEType.eFireworks:
			strName = "player/SceneElements/Fireworks";
			break;
		}
        GameObject cPEObj = Resources.Load(strName) as GameObject;
        GameObject cPEObjInt = Utility.Instantiate(cPEObj) as GameObject;
        cPEObjInt.name = cPEObj.name;
        ParticleEmitter cPE = cPEObjInt.GetComponent(typeof(ParticleEmitter)) as ParticleEmitter;
		ChangePEPosition(eType);
		cPE.emit = false;
        return cPE;
    }
	
	static public void SetPEEmitState(PEType eType,bool bIsEmit)
	{
		string strName = "";
		switch(eType)
		{
		case PEType.eBubbles:
			strName = "Bubbles";
			break;
		case PEType.eFireworks:
			strName = "Fireworks";
			break;
        case PEType.eWinFireworks:
			strName = "WinFireworks";
			break;
		}
		
        GameObject cPEObj = GameObject.Find(strName);
		if(!cPEObj)
			return;
		ParticleEmitter cPE = cPEObj.GetComponent(typeof(ParticleEmitter)) as ParticleEmitter;
		cPE.emit = bIsEmit;
	}
	
	static public void ChangePEPosition(PEType eType)
    {
		string strName = "";
		switch(eType)
		{
		case PEType.eBubbles:
			strName = "Bubbles";
			break;
		case PEType.eFireworks:
			strName = "Fireworks";
			break;
		}
		
        GameObject cPEObj = GameObject.Find(strName);
		if(!cPEObj)
			return;
		
		switch(eType)
		{
		case PEType.eBubbles:
        	cPEObj.transform.localPosition = new Vector3(206f, 0, 1230f);
			break;
		case PEType.eFireworks:
			switch((int)UnityEngine.Random.Range(0,4))
			{
			case 0:
				cPEObj.transform.localPosition = new Vector3(112f, 90f, 1402f);
				break;
			case 1:
				cPEObj.transform.localPosition = new Vector3(270f, 91f, 1402f);
				break;
			case 2:
				cPEObj.transform.localPosition = new Vector3(308f, 98f, 1402f);
				break;
			case 3:
				cPEObj.transform.localPosition = new Vector3(182f, 143f, 1402f);
				break;
			}
			break;
		}
    }

	static public void  SaveJson( object obj,string filepathandname)
	{
		//Debug.LogWarning("========> SaveJson:   "+filepathandname);
		string levelnameLower = Application.loadedLevelName.ToLower();
		
		if(Application.isEditor)
		{
			string file = "./" + filepathandname;
			if (File.Exists(file) )
			{
				File.Delete (file);
			}
	
			System.IO.TextWriter writer = new System.IO.StreamWriter(file, false);
			LitJson.JsonWriter jw = new JsonWriter( writer as System.IO.TextWriter );
			jw.PrettyPrint = true;
			LitJson.JsonMapper.ToJson( obj, jw );
			writer.Close();
		}
	}
	
    static public string GetLastString(string res,char[] m_spiltwords)
    {
        string words = res;

        string[] split = words.Split(m_spiltwords);

        if (split.Length > 0)
        {
            return split[split.Length - 1];
        }
        return "";

    }
	
	static public string GetFirstString(string res,char[] m_spiltwords)
    {
        string words = res;

        string[] split = words.Split(m_spiltwords);

        if (split.Length > 0)
        {
            return split[0];
        }
        return "";

    }
	
	static public void PrintPassTime(string str,DateTime starttime)
	{
		DateTime   dt1=starttime; 
		DateTime   dt2=DateTime.Now; 
		
		string T= ((dt2 - dt1).TotalMilliseconds).ToString();
	
		Debug.Log("=======> "+str+"   Pass Time(mill)L:  "+T);
	}
}
