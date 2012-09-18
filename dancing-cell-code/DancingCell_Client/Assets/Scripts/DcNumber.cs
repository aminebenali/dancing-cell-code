using UnityEngine;
using System;

public class DcNumber
{
    private string[] strNumSprName = { "crti0", "crti1", "crti2", "crti3", "crti4", "crti5", "crti6", "crti7", "crti8", "crti9" };
    private string[] strNumSprName_Silver = { "Silver0", "Silver1", "Silver2", "Silver3", "Silver4", "Silver5", "Silver6", "Silver7", "Silver8", "Silver9" };
    private int nNumType = 0;
    private int nInitDiv = 0;

    public void Init(ref UISprite[] SprNumber,bool bIsSilver = false)
    {
        string strNum = "1";
        for (int j = 0; j < SprNumber.Length; j++)
        {
            SprNumber[j].enabled = false;

            if (j != 0)
                strNum += '0';
        }
        nInitDiv = Convert.ToInt32(strNum);
        nNumType = Convert.ToInt32(bIsSilver);
    }

    public void Hide(ref UISprite[] SprNumber)
    {
        for (int j = 0; j < SprNumber.Length; j++)
        {
            SprNumber[j].enabled = false;
        }
    }

    public int ShowNumber(ref UISprite[] SprNumber, int nNum)
    {
        int nDividend = nInitDiv;
        int nTemp = nNum / nDividend;

        int i = 0;
        int nCnt = 0;
        bool bIsFirst = false;
        while (i < SprNumber.Length)
        {
            //Debug.LogError("nDividend:" + nDividend + "nTemp:" + nTemp);
            if (nTemp != 0 || (nDividend != 0 && bIsFirst))
            {
                SprNumber[i].enabled = true;
                if(nNumType == 0)
                    SprNumber[i].spriteName = strNumSprName[nTemp];
                else
                    SprNumber[i].spriteName = strNumSprName_Silver[nTemp];
                i++;
                nNum %= nDividend;
                if (nTemp != 0)
                {
                    bIsFirst = true;
                }
            }

            nDividend /= 10;
            if (nDividend == 0)
            {
                return i;//return Digit
            }
            nTemp = nNum / nDividend;

            if (nCnt++ > 10)
                return 0;
        }

        return 0;
    }
}
