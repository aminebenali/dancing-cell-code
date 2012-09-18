using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DcResultDlg_Anim : MonoBehaviour 
{
    public GameObject PanBgShake;
    public GameObject[] cWinFlicker;
    public UISprite SprBlackLine = null;
	
	void BgShakeOver()
	{
		gameObject.animation.Stop();
        gameObject.animation.CrossFade("ShopBgColorChange");
	}
	
	void WinAnimOver()
	{
        if (!PanBgShake.animation.isPlaying)
        {
            PanBgShake.animation.CrossFade("ResultShake");

            StartCoroutine(DelayShowWinFlicker(0,0.0f));
			StartCoroutine(DelayShowWinFlicker(1,0.1f));
			StartCoroutine(DelayShowWinFlicker(2,0.2f));

            DcGlobalFunc.SetPEEmitState(PEType.eWinFireworks, true);
            StartCoroutine(StopWinFireworks(1.5f));
        }
	}
	
	float fTime = 0f;
	bool[] bIsPositive = {true,false,true};
	float fLimits = 3.1415926f/2f;
	Vector3[] vecRot = {new Vector3(0,0,0),new Vector3(0,0,0),new Vector3(0,0,0)};
	void Update()
	{
		if(cWinFlicker.Length == 0)
			return;
		//Debug.LogError("sssssssssssssssss"+fTime);
		fTime += Time.deltaTime;
		if(fTime >= 0.1f)
		{
			fTime = 0f;
			//Debug.LogError("aaaaa");
			for (int i = 0; i < cWinFlicker.Length; i++)
            {
                if(cWinFlicker[i].active)
				{
					vecRot[i].y += bIsPositive[i] ? Time.deltaTime : -Time.deltaTime;
					if(vecRot[i].y > fLimits || vecRot[i].y < -fLimits)
					{
						//if(i == 0)
						//	Debug.LogError("BBBBBBBBBBBBBBBBBBB: "+vecRot[0].y);
						vecRot[i].y = 0f;
						bIsPositive[i] = !bIsPositive[i];
					}
					cWinFlicker[i].transform.Rotate(vecRot[i]);
				}
            }
			//Debug.LogError("VVVVVV: "+vecRot[0].y);
		}
	}

    void LoseAnimOver()
    {
        if (SprBlackLine)
        {
            SprBlackLine.enabled = true;
            SprBlackLine.animation.Play();
        }
    }

    void BlackLineAnimOver()
    {
        gameObject.animation.Stop();
        gameObject.animation.CrossFade("BlackLine_1");
    }
	
	IEnumerator DelayShowWinFlicker(int i,float timevalue)
    {
        yield return new WaitForSeconds(timevalue);
        cWinFlicker[i].SetActiveRecursively(true);
    }
	
    IEnumerator StopWinFireworks(float timevalue)
    {
        yield return new WaitForSeconds(timevalue);
        DcGlobalFunc.SetPEEmitState(PEType.eWinFireworks, false);
    }
}
