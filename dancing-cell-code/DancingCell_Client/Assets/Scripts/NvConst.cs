using UnityEngine;
using System.Collections;

public enum RunType
{
	Game,
	AvatarEditor,
	Other
}

public enum EDevelopMode
{
	eDM_Develop = 0,
	eDM_Sandbox,
	
	
	eDM_Release,
	eDM_Reservered1
}


//this class is created for defined some const value, and some features to use as macro.
 public static class NvConst  
{
	private static RunType  g_RunType = RunType.Game; 
	
	public static void SetRunType(RunType mrun)
	{
		Debug.Log("===========>   Change Run Type to "+mrun+"  <=========== ");
		
		g_RunType = mrun;
	}
	
	public static RunType GetRunType()
	{
	
		return g_RunType;
	}
	
	public static int kServerClientTimeOffset = 0;
	
	public static int kGameTipsMaxNum = 12;
	
	public const bool kOffline = false;
	
	public static bool kIsDebugBuild 
	{
		get { return (kDevelopMode == EDevelopMode.eDM_Develop) || ( kDevelopMode == EDevelopMode.eDM_Sandbox) ; }
	}
	
	public const int kMaxSeTrack = 10;
	
	public const int kMaxFixedSeTrack = 3;
	
	public const float kBaseHeight = 480.0f;
	
	public const float kBaseWidth = 800.0f;
	
	public const float kBaseHeightMinThreshold = 540.0f;
	
	public const float kBaseHeightMaxThreshold = 720.0f;
	
	public const int kUserId = 2005;

	public const int kConstructionDetailQuestNum = 3;

	public const string kResPath_BuildingTemplate = "TXT/ConstructionDetail/BuildingTemplate";
	public const string kResPath_OfflineBuildingList = "TXT/UserData/Offline_Buildinglist";
	public const string kResPath_OfflineUserStatus = "TXT/UserData/Offline_UserStatus";
	public const string kResPath_OfflineOverlappableInventory = "TXT/UserData/Offline_OverlappableInventory";
	
	public const string kVersionNum = "Rev_0.1.0";
	
	public const EDevelopMode kDevelopMode = EDevelopMode.eDM_Develop;
	public const string kUrlDevelop = "http://ninja.mbga.mbgadev.cn";
	public const string kUrlSandbox = "http://ninjatx.staging.mbga.mbgadev.cn";
	public const string kUrlRelease = "http://ninjatx.sp-app.mobage.cn";
	
	public const int kEnterTowerLv = 8;
	public const int kEnterArenaLv = 15;
}
