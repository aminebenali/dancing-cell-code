using UnityEngine;
using System;
using System.Collections;

public class DcSongBeatEditDlg : NvUIDialogBase
{
    //UI控件
    public GameObject[] SongSel;                            //歌曲列表 静态赋值
    private UILabel[]   LabSongName     = new UILabel[20];  //歌曲列表中的歌曲名
    private UILabel[]   LabSinger       = new UILabel[20];  //歌曲列表中的歌手名
    private UISprite[]  SprSongListBg   = new UISprite[20]; //歌曲列表中的被选择时显示的背景图

    //逻辑变量
    private object  objLastSelSongBtnSender = null; //记录歌曲列表中上一次被点击的控件对象
    private int     nCurSelEditSongIdx      = -1;   //当前编辑的歌曲序号

    //提示框
    private NvCommonUIManager mCommonUIManager = null;

    //初始化
    protected override void OnDlgInit()
    {
        mCommonUIManager = Singlton.getInstance("NvCommonUIManager") as NvCommonUIManager;

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

        StartCoroutine(_ListedSong());
    }

    //初始化-歌曲列表
    IEnumerator _ListedSong()
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

    //单击歌曲列表中某首歌时的事件响应
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

    //检索歌曲表ID
    private void RetrievalSongListID()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        for (int i = 0; i < mgr.cShareData.lstSongData.Count; i++)
        {
            if (mgr.cShareData.lstSongData[i].nSongID != i)
            {

            }
        }
        mCommonUIManager.showMessageBox("歌曲表ID异常，工具默认以序号0开始递增1的标号规则修复歌曲ID", "",
                                        NvMessageBox.EMessageBoxStyle.eStyle_OkayOnly, OnEndModal, true);
    }

    //保存当前编辑的歌曲信息
    private void SaveCueEditSongInfo()
    {
        SceneManager mgr = Singlton.getInstance("SceneManager") as SceneManager;
        DcGlobalFunc.SaveJson(mgr.cShareData.lstSongData, "Assets/Resources/TXT/Table/Song.txt");
    }
}
