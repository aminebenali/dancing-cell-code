using UnityEngine;
using System;

public enum eNumberType
{
    eGold,
    eSilver,
    eCombo
}

public class DcNumber
{
    private string[] strNumSprName_Gold = { "Word Gold 0", "Word Gold 1", "Word Gold 2", "Word Gold 3", "Word Gold 4", "Word Gold 5", 
                                            "Word Gold 6", "Word Gold 7", "Word Gold 8", "Word Gold 9" };
    private string[] strNumSprName_Silver = { "Word Silver 0", "Word Silver 1", "Word Silver 2", "Word Silver 3", "Word Silver 4", "Word Silver 5", 
                                              "Word Silver 6", "Word Silver 7", "Word Silver 8", "Word Silver 9" };
    private string[] strNumSprName_Combo = { "Combo 00", "Combo 01", "Combo 02", "Combo 03", "Combo 04", "Combo 05", "Combo 06", "Combo 07", "Combo 08", "Combo 09" };
    private eNumberType eNumType = 0;
    private int nInitDiv = 0;

    public void Init(ref UISprite[] SprNumber,eNumberType eType = eNumberType.eGold)
    {
        string strNum = "1";
        for (int j = 0; j < SprNumber.Length; j++)
        {
            SprNumber[j].enabled = false;

            if (j != 0)
                strNum += '0';
        }
        nInitDiv = Convert.ToInt32(strNum);
        eNumType = eType;
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
                if(eNumType == eNumberType.eGold)
                    SprNumber[i].spriteName = strNumSprName_Gold[nTemp];
                else if(eNumType == eNumberType.eSilver)
                    SprNumber[i].spriteName = strNumSprName_Silver[nTemp];
                else if(eNumType == eNumberType.eCombo)
                    SprNumber[i].spriteName = strNumSprName_Combo[nTemp];
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
