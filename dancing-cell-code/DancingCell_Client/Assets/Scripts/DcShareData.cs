using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class DcBuildingData
{
    public int nBuildingID;
    public string strBuildingName;
    public string strMasterName;
    public string str3DSceneName;
    public List<int> litSongIDs = new List<int>();//MAX = 5
    public List<string> litDiffLvs = new List<string>();//mapping litSongIDs
    public List<string> litSongMaster = new List<string>();//mapping litSongIDs
    public List<int> litMasterSex = new List<int>();//mapping litSongIDs
    public List<string> litMode = new List<string>();//mapping litSongIDs
	public List<int> litHighScores = new List<int>();//mapping litSongIDs
}

public class DcOpeInfo
{
    public List<int> litOpeInfo = new List<int>();
}

public class DcOperatorData
{
    public int nShowTime;//multiply 100
    public Dictionary<string, DcOpeInfo> mapOpeCode = new Dictionary<string, DcOpeInfo>();
}

public class DcBeatData
{
    public int nID;
    public List<DcOperatorData> litOpeData = new List<DcOperatorData>();
}

public class DcSongData
{
    public int nSongID;
    public int nWhereScene;
    public string strSongName;
    public string strAtlas;
    public string strSpriteName;
    public string strSinger;
    public string strAudition;
    public string strOfficial;
    public int nRewardMoney;
    public int nRewardExp;
    public int nShowReadyTime;
    public int nShowGoTime;
    public int nBeatTime;//multiply 1000
    public int nLength;
    public int litBeatDataIdx;
}

public class DcImitateRoomData
{
	public int nRoomID;
	public string strRoomName;
    public string strMode;
    public string strDiff;
	public int nSongID;
	public int nNumOfPeople;
	public List<string> litPeopleName = new List<string>();//MAX = 3
    public List<int> litPeopleSex = new List<int>();
}

public class DcImitateFriendData
{
	public int nFriendID;
	public string strFriendName;
	public int nFriendSex;
	public int nFriendLv;
	public int nHasMusic;
	public int nDancingCnt;
}

public class DcImitateTheLatestSongData
{
    public int nSongID;
    public string strSongName;
    public string strOwner;
    public int nIsMySelf;
}

public class DcImitateTaskData
{
    public int nTaskID;
    public string strTaskName;
    public string strTaskDescribe;
    public int nState;//0,unreceived 1,accepted 2,Achievable 3,Completed
    public int nRewardMoney;
    public int nRewardExp;
}

public class DcShareData
{
	public int nCurSelBuildingIdx = 0;
	public int nCurSelSongIdx4Building = 0;
	
	public bool bIsWin = false;
	public int nBasicScore = 0;
	public int nComboScore = 0;
	
    public int nDancingEntranceType = 0;//0,ChallengeList 1,Disk 2,Room
	public int nShopEntranceType = 0;//0,Map 1,Outside 2,Interior 3,Chest 4,Dresser
	public int nDiskEntranceType = 0;//0,Interior 1,Lobby
	
	public string strUserName = "";
	public CharSex eUserSex = CharSex.eMan;

    public List<DcBuildingData> lstBuildingData = new List<DcBuildingData>();
    public List<DcSongData> lstSongData = new List<DcSongData>();
    public List<DcBeatData> lstBeatData = new List<DcBeatData>();
    public DcSongData cCurSongData = null;
    public string strDiffLv = "";
    public string strMode = "";//Default,Drumbeat,Dynamic,Notation
	public int nHighScores = 0;
	public bool bIsChallengeOthers = false;
	
	public List<DcImitateRoomData> lstRoomsData = new List<DcImitateRoomData>();//MAX = 8 (TEMP)
    public DcImitateRoomData cCurRoomData = null;
	
	public List<DcImitateFriendData> lstFriendsData = new List<DcImitateFriendData>();//MAX = 5 (TEMP)
	public DcImitateFriendData cCurFriendData = null;

    public List<DcImitateTheLatestSongData> lstTheLatestSongsData = new List<DcImitateTheLatestSongData>();//MAX = 8 (TEMP)
    public DcImitateTheLatestSongData cCurTheLatestSongData = null;

    public List<DcImitateTaskData> lstTasksData = new List<DcImitateTaskData>();//MAX = 3 (TEMP)
    //public DcImitateTaskData cCuTaskData = null;
	
	//Generalized Data Structure 
    public string[] strManCharIcon = { "Head portrait man 01", "Head portrait man 02", "Head portrait man 03" };
    public string[] strWomanCharIcon = { "Head portrait girl 01", "Head portrait girl 02", "Head portrait girl 03" };
	
	public string[] strDancingMode = { "Default", "Drumbeat", "Dynamic", "Notation" };
	
	public bool bIsInBuildNewRoom = false;
	public string strCurRoomName = "";//Create new room process to select songs temporary record the current room name

	
    public DcShareData()
    {
        UnityEngine.TextAsset s = (UnityEngine.TextAsset)Resources.Load("TXT/Table/Building", typeof(UnityEngine.TextAsset));
        lstBuildingData = JsonMapper.ToObject<List<DcBuildingData>>(s.text);

        s = (UnityEngine.TextAsset)Resources.Load("TXT/Table/Song", typeof(UnityEngine.TextAsset));
        lstSongData = JsonMapper.ToObject<List<DcSongData>>(s.text);

        s = (UnityEngine.TextAsset)Resources.Load("TXT/Table/BeatData", typeof(UnityEngine.TextAsset));
        lstBeatData = JsonMapper.ToObject<List<DcBeatData>>(s.text);
		
		s = (UnityEngine.TextAsset)Resources.Load("TXT/Table/ImitateRooms", typeof(UnityEngine.TextAsset));
        lstRoomsData = JsonMapper.ToObject<List<DcImitateRoomData>>(s.text);
		
		s = (UnityEngine.TextAsset)Resources.Load("TXT/Table/ImitateFriends", typeof(UnityEngine.TextAsset));
        lstFriendsData = JsonMapper.ToObject<List<DcImitateFriendData>>(s.text);

        s = (UnityEngine.TextAsset)Resources.Load("TXT/Table/ImitateTheLatestSongs", typeof(UnityEngine.TextAsset));
        lstTheLatestSongsData = JsonMapper.ToObject<List<DcImitateTheLatestSongData>>(s.text);

        s = (UnityEngine.TextAsset)Resources.Load("TXT/Table/ImitateTasks", typeof(UnityEngine.TextAsset));
        lstTasksData = JsonMapper.ToObject<List<DcImitateTaskData>>(s.text);
    }
	
}
