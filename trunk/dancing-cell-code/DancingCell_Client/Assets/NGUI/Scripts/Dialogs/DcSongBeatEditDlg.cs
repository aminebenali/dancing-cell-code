using UnityEngine;
using System;
using System.Collections;

public class DcSongBeatEditDlg : NvUIDialogBase
{
    //UI�ؼ�
    public GameObject[] SongSel;                            //�����б� ��̬��ֵ
    private UILabel[]   LabSongName     = new UILabel[20];  //�����б��еĸ�����
    private UILabel[]   LabSinger       = new UILabel[20];  //�����б��еĸ�����
    private UISprite[]  SprSongListBg   = new UISprite[20]; //�����б��еı�ѡ��ʱ��ʾ�ı���ͼ

    //�߼�����
    private object  objLastSelSongBtnSender = null; //��¼�����б�����һ�α�����Ŀؼ�����
    private int     nCurSelEditSongIdx      = -1;   //��ǰ�༭�ĸ������

    protected override void OnDlgInit()
    {
        for (int i = 0; i < SongSel.Length; i++)
        {
            SongSel[i].SetActiveRecursively(false);
            for (int j = 0; j < SongSel[i].transform.childCount; j++)
            {
                GameObject child = SongSel[i].transform.GetChild(j).gameObject;
                if(child.name == "LabName")
                {
                    LabSongName[i] = child.GetComponent(typeof(UILabel)) as UILabel;
                }
                else if(child.name == "LabSinger")
                {
                    LabSinger[i] = child.GetComponent(typeof(UILabel)) as UILabel;
                }
                else if(child.name == "SongListBg")
                {
                    SprSongListBg[i] = child.GetComponent(typeof(UISprite)) as UISprite;
                    SprSongListBg[i].enabled = false;
                }
            }
        }

        StartCoroutine(_ListedSongName());
    }

    IEnumerator _ListedSongName()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        while (mgr.cShareData == null)
        {
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i < mgr.cShareData.lstSongData.Count; i++)
        {
            SongSel[i].SetActiveRecursively(true);
            LabSongName[i].text = mgr.cShareData.lstSongData[i].strSongName;
            LabSinger[i].text = mgr.cShareData.lstSongData[i].strSinger;
        }
    }

    private void SelSong(object sender)
    {
        if (objLastSelSongBtnSender == sender)
            return;
        objLastSelSongBtnSender = sender;

        NvSoundController soundctr = Singlton.getInstance("NvSoundController") as NvSoundController;
        soundctr.PlaySe("ui_touch");
		
		GameObject obj = sender as GameObject;
        char[] splt = { '_' };
        string lastnum = DcGlobalFunc.GetLastString(obj.transform.name, splt);
        //Debug.Log("1111111111" + lastnum);
        int nSelSong = Convert.ToInt32(lastnum) - 1;
		
		SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        for (int i = 0; i < mgr.cShareData.lstSongData.Count; i++)
        {
            SprSongListBg[i].enabled = false;
        }
        SprSongListBg[nSelSong].enabled = true;
        soundctr.PlaySong(mgr.cShareData.lstSongData[nSelSong].strOfficial);
    }

    private void SaveCueEditSongInfo()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        DcGlobalFunc.SaveJson(mgr.cShareData.lstSongData, "Assets/Resources/TXT/Table/Song.txt");
    }
}
