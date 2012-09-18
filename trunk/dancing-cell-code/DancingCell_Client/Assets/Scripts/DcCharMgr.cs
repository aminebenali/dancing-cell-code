using UnityEngine;
using System.Collections;

public class DcCharMgr : MonoBehaviour
{
    public DcCharAnim cMainChar = null;
    private DcCharAnim[] cCostar = new DcCharAnim[2];

    int nCurPlayAnim = 0;//0:idle 1:dancing

    int nMainCharState = 0;
    Vector3 posCurMainCharPos;

    void Start()
    {
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        GameObject cCharObj = DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex,CharModel.eOne,CharForWhat.eDancingMain,false);
        cMainChar = cCharObj.GetComponent(typeof(DcCharAnim)) as DcCharAnim;

        if(mgr.cShareData.nDancingEntranceType == 2)
        {
			if(mgr.cShareData.cCurRoomData.litPeopleSex.Count >= 1)
			{
            	cCharObj = DcGlobalFunc.CreateChar(mgr.cShareData.cCurRoomData.litPeopleSex[0] == 0 ? CharSex.eMan : CharSex.eWoman,
                                                	mgr.cShareData.cCurRoomData.litPeopleSex[0] == 0 ? CharModel.eOne : CharModel.eTwo,
                                                	CharForWhat.eDancingLeft,false,false);
			}
        }
        else
        {
            cCharObj = DcGlobalFunc.CreateChar(mgr.cShareData.eUserSex == CharSex.eMan ? CharSex.eWoman : CharSex.eMan,
											CharModel.eOne,CharForWhat.eDancingLeft,false,false);
        }
        cCostar[0] = cCharObj.GetComponent(typeof(DcCharAnim)) as DcCharAnim;

        if(mgr.cShareData.nDancingEntranceType == 2)
        {
			if(mgr.cShareData.cCurRoomData.litPeopleSex.Count >= 2)
			{
            	cCharObj = DcGlobalFunc.CreateChar(mgr.cShareData.cCurRoomData.litPeopleSex[1] == 0 ? CharSex.eMan : CharSex.eWoman,
                                                	mgr.cShareData.cCurRoomData.litPeopleSex[1] == 0 ? CharModel.eOne : CharModel.eTwo,
                                                	CharForWhat.eDancingRight,false,false);
			}
        }
        else
        {
            cCharObj = DcGlobalFunc.CreateChar(CharSex.eWoman,CharModel.eTwo,CharForWhat.eDancingRight,false,false);
        }
        cCostar[1] = cCharObj.GetComponent(typeof(DcCharAnim)) as DcCharAnim;

        posCurMainCharPos = cMainChar.transform.localPosition;
    }

    void Update()
    {
        if (nMainCharState != 0)
        {
            switch (nMainCharState)
            {
                case 1:
                    posCurMainCharPos.z--;
                    if (posCurMainCharPos.z <= 1206f)
                    {
                        posCurMainCharPos.z = 1206f;
                        nMainCharState = 0;
                    }
                    break;
                case -1:
                    posCurMainCharPos.z++;
                    if (posCurMainCharPos.z >= 1254f)
                    {
                        posCurMainCharPos.z = 1254f;
                        nMainCharState = 0;
                    }
                    break;
                case 10:
                    if(posCurMainCharPos.z > 1232)
                    {
                        posCurMainCharPos.z--;
                    }
                    else if(posCurMainCharPos.z < 1228)
                    {
                        posCurMainCharPos.z++;
                    }
                    else
                    {
                        nMainCharState = 0;
                    }
                    break;
            }

            cMainChar.transform.localPosition = posCurMainCharPos;
        }
    }

    public void MainCharDancing(bool bIsInplace = false)
    {
        if(!bIsInplace)
        {
            nMainCharState = 1;
        }
        cMainChar.PlayDancing();
        nCurPlayAnim = 1;
    }

    public void ForceMainCharIdleForDancing()
    {
        cMainChar.PlayIdle(IdleForWhat.eForDancing);
    }

    public void MainCharIdleForDancing(bool bIsReturnInplace = false)
    {
        if(nCurPlayAnim == 0)
            return;

        nMainCharState = bIsReturnInplace ? 10 : -1;
        //Debug.LogError("CCCCCCCCCCCCCC:"+nMainCharState);
        if(nMainCharState == -1)
        {
            cMainChar.PlayBackoff();
        }
        else
        {
            cMainChar.PlayIdle(IdleForWhat.eForDancing);
        }
        nCurPlayAnim = 0;
    }

    public void MainCharCheer()
    {
        cMainChar.PlayCheer();
        for (int i = 0; i < cCostar.Length; i++)
        {
            //cCostar[i].PlayClaphand();
        }
    }

    //////////////////////////////////////////////////////

    public void CosterDancing()
    {
        for (int i = 0; i < cCostar.Length; i++)
        {
            cCostar[i].PlayDancing();
        }
    }

    public void CosterIdleForDancing()
    {
        for (int i = 0; i < cCostar.Length; i++)
        {
            cCostar[i].PlayIdle(IdleForWhat.eForDancing);
        }
    }
}
