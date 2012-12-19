using UnityEngine;
using System;

public class DcSongBeatEditDlg : NvUIDialogBase 
{
    public GameObject[] SongSel;
    public UILabel[] LabSongName = new UILabel[20];

    protected override void OnDlgInit()
    {
        for (int i = 0; i < SongSel.Length; i++)
        {
            SongSel[i].SetActiveRecursively(false);
            for (int j = 0; j < SongSel[i].transform.childCount; j++)
            {
                GameObject child = SongSel[i].transform.GetChild(j).gameObject;
                if (child.name == "LabName")
                {
                    LabSongName[i] = child.GetComponent(typeof(UILabel)) as UILabel;
                }
            }
        }
    }
}
