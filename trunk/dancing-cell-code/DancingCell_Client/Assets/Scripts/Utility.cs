using System;
using System.IO;
using System.IO.Compression;
using System.Text;
//using ICSharpCode.SharpZipLib.GZip;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public interface IPlayerDataListener
{
	//void OnPlayerDataOk (NvPlayer player, uint msgId, object msgObj, object userObj);

	//void OnPlayerDataFailed (NvPlayer player, uint msgId, string errMessage, object userObj);
}

// proxy base class
public class NvProxy : MonoBehaviour
{
	protected Dictionary< uint, List<IPlayerDataListener> > mListeners = new Dictionary< uint, List<IPlayerDataListener> > ();
	//protected NvPlayer mPlayer = null;
	
//	public NvPlayer Owner
//	{
//		get 
//		{ 
//			return mPlayer; 
//		}
//		
//		set 	
//		{
//			mPlayer = value;
//		}
//	}
	
	public void RegisterPlayerDataListener (uint id, IPlayerDataListener listener)
	{
		if (listener != null) {
			List<IPlayerDataListener> slots = null;
			if (mListeners.ContainsKey (id)) {
				slots = mListeners [id];
			} else {
				slots = new List<IPlayerDataListener> ();
				mListeners [id] = slots;
			}
			
			if (!slots.Contains (listener)) {
				slots.Add (listener);
			}
		}
	}
	
	public void UnregisterPlayerDataListener (uint id, IPlayerDataListener listener)
	{
		if (mListeners.ContainsKey (id)) {
			List<IPlayerDataListener> slots = mListeners [id];
			if (slots.Contains (listener)) {
				slots.Remove (listener);
			}
		}
	}
	
	public void ClearPlayerDataListeners ()
	{
		foreach (KeyValuePair< uint, List<IPlayerDataListener> > kv in mListeners) {
			kv.Value.Clear ();
		}
	}
	
//	protected void NotifySuccess (uint msgId, object msgObj, object userObj)
//	{
//		if (mListeners.ContainsKey (msgId)) {
//			List<int> tmp = new List<int> ();
//			List<IPlayerDataListener> slots = mListeners [msgId];
//			for (int ii = 0; ii < slots.Count; ++ii) {
//				if (slots [ii] == null) {
//					tmp.Add (ii);
//				} else {
//					slots [ii].OnPlayerDataOk (mPlayer, msgId, msgObj, userObj);
//				}
//			}
//			
//			foreach (int idx in tmp) {
//				slots.RemoveAt (idx);
//			}
//		}
//	}
	
//	protected void NotifyFailed (uint msgId, string errMessage, object userObj)
//	{
//		if (mListeners.ContainsKey (msgId)) {
//			List<int> tmp = new List<int> ();
//			List<IPlayerDataListener> slots = mListeners [msgId];
//			for (int ii = 0; ii < slots.Count; ++ii) {
//				if (slots [ii] == null) {
//					tmp.Add (ii);
//				} else {
//					slots [ii].OnPlayerDataFailed (mPlayer, msgId, errMessage, userObj);
//				}
//			}
//			
//			foreach (int idx in tmp) {
//				slots.RemoveAt (idx);
//			}
//		}
//	}
	
	public enum EProxyType
	{
		ePT_Building = 0,
		ePT_PlayerData,
		ePT_Inventory,
		ePT_PlayerDataXChange,
		ePT_CharaListData,
		ePT_CharaEquipedListData,
		ePT_CharaEquipListinBag,
		ePT_CharaEquipPutOn,
		ePT_CharaEquipTakeOff,
		ePT_CharaEquipSell,
		ePT_CharaEquipStrengthen,
		ePT_CharaCraftedListData,
		ePT_CharaCraftListinBag,
		ePT_CharaCraftPutOn,
		ePT_CharaCraftTakeOff,
		ePT_CharaCraftSell,
		ePT_CharaCraftStrengthen,
		ePT_CharaScrollList,
		ePT_CharaScrollUse,
		ePT_CharaExtendSlot,
		ePT_DungeonAvail,
		ePT_Shop,
		ePT_Forming,
		ePT_ChallengeInit,
		
		
		_Number_
	}
	
//	virtual public bool isValid {
//		//get { return mPlayer != null; }
//	}
	
	virtual protected void Init ()
	{
	}
}

public static class Utility
{
	static public Object Instantiate (Object original)
	{
		return Utility.Instantiate (original, Vector3.zero, Quaternion.identity);
	}
	
	static public Object Instantiate (Object original, Vector3 position, Quaternion rotation)
	{
		float t = Time.realtimeSinceStartup;
		Object obj = GameObject.Instantiate (original, position, rotation);
		t = Time.realtimeSinceStartup - t;
		
		//Debug.LogWarning(">>>>>>>>>>> Instantiate Object [" + original + "] costs " + t + " secs.");
		return obj;
	}
	
	static public Vector3 GetScreenRatio ()
	{
		return new Vector3 (Screen.width / NvConst.kBaseWidth, Screen.height / NvConst.kBaseHeight, 1.0f);
	}
	
	static public DateTime ConvertFromUnixTimestamp (double timestamp)
	{
		DateTime origin = new DateTime (1970, 1, 1, 0, 0, 0, 0);
		return origin.AddSeconds (timestamp).ToLocalTime ();
	}
	
//	static public double ConvertToUnixTimestamp (DateTime date)
//	{
//		DateTime origin = new DateTime (1970, 1, 1, 0, 0, 0, 0).ToLocalTime ();
//		TimeSpan diff = date.ToLocalTime () - origin;
//		double clientStamp =  Math.Floor (diff.TotalSeconds);
//		return clientStamp - NvConst.kServerClientTimeOffset;
//	}
	
	static public string GetQuestDetail (string desc)
	{
		string val = "";
		char[] splitor = new char[]{ '|','F','|' };
		string[] after = desc.Split (splitor);
		val = after [0];		
		return val;
	}
	
	static public string GetQuestFinDetail (string desc)
	{
		string val = "";
		char[] splitor = new char[]{ '|','F','|' };
		string[] after = desc.Split (splitor);
		val = after [after.Length - 1];		
		return val;
	}
	
	static public void SetLayer (GameObject target, int layerMask, bool recursively = true)
	{
		List< Transform > stack = new List<Transform> ();
		stack.Add (target.transform);
		
		while (stack.Count > 0) {
			Transform tr = stack [0];
			stack.RemoveAt (0);
			tr.gameObject.layer = layerMask;
			
			if (recursively) {
				for (int ii = 0; ii < tr.childCount; ++ii) {
					stack.Add (tr.GetChild (ii));
				}
			}
		}
	}
	
	public delegate bool widgetCallback (Transform tr,object args);
	
	static public void enumWidgets (Transform root, widgetCallback cb, object args = null)
	{
		List<Transform> stack = new List<Transform> ();
		for (int ii = 0; ii < root.childCount; ++ii) {
			stack.Add (root.GetChild (ii));
		}
		
		while (stack.Count > 0) {
			Transform tr = stack [0];
			stack.RemoveAt (0);
			
			if (!cb (tr, args)) {
				for (int ii = 0; ii < tr.childCount; ++ii) {
					stack.Add (tr.GetChild (ii));
				}
			}
		}
	}
	
	static public string School2SpriteName (int school)
	{
		switch (school) {
			case 1:
				return "crow_small";
			case 2:
				return "shark_small";
			case 3:
				return "lizard_small";
			case 4:
				return "fox_small";
			case 5:
				return "butterfly_small";
			case 6:
				return "bear_small";
		}
		
		// default
		return "crow_small";
	}
	
	static public string School2LocKey (int school)
	{
		switch (school) {
			case 1:
				return "CROW";
			case 2:
				return "SHARK";
			case 3:
				return "LIZARD";
			case 4:
				return "FOX";
			case 5:
				return "BUTTERFLY";
			case 6:
				return "BEAR";
		}
		
		return "CROW";
	}
	
	static public string Rank2LocKey (int rank)
	{
		string ret = "RANK_LOWER";
		switch (rank) {
			case 0:
				ret = "RANK_LOWER";
				break;		
			case 1:
				ret = "RANK_MIDDLE";
				break;		
			case 2:
				ret = "RANK_HIGH";
				break;		
			case 3:
				ret = "RANK_LAND";
				break;		
			case 4:
				ret = "RANK_SKY";
				break;	
		}
		
		return ret;
	}
	
	static public string Rank2SpriteName( int rank )
	{
		string ret = "lower";
		switch ( rank )
		{
		case 0:
			ret = "lower";
			break;
		case 1:
			ret = "middle";
			break;
		case 2:
			ret = "up";
			break;
		case 3:
			ret = "land";
			break;
		case 4:
			ret = "sky";
			break;
		}
		
		return ret;
	}
	
//	static public Vector3 ConvertPosition (Vector3 worldPos, Camera srcCam, Camera desCam)
//	{
//		if (srcCam != null && desCam != null) {
//			Vector3 screenPoint = srcCam.WorldToScreenPoint (worldPos);
//			return desCam.ScreenToWorldPoint (worldPos);
//		}
//		return Vector3.zero;
//	}
	
	static public void SetAnchorCamera (GameObject obj)
	{
		int layer = obj.layer;
		Camera cam = NGUITools.FindCameraForLayer (layer);
		
		if (cam != null) {
			Utility.enumWidgets (obj.transform, SetAnchorCameraCallback, cam as object);	
		}
	}
	
	static public Vector3 CalcuateWorldPosition (Transform tr)
	{
		Transform parent = tr;
		
		Vector3 pos = Vector3.zero;
		while (parent != null) {
			pos += parent.localPosition;
			parent = parent.parent;
		}
		
		return pos;
	}
	
	static private bool SetAnchorCameraCallback (Transform tr, object args)
	{
		Camera cam = args as Camera;
		UIAnchor anchor = tr.GetComponent<UIAnchor> ();
		if (anchor != null) {
			anchor.uiCamera = cam;
		}
		return false;
	}
	
	static public string SetItemQualityColor (int rank)
	{
		switch (rank) {
			case 1:
				return "[00ff00]";//green
			case 2:
				return "[0000ff]";//blue
			case 3:
				return "[800080]";//purple
			case 4:
				return "[ffa500]";//orange
			default :
				return "[ffffff]";//white
		}		
	}
	
//	static public string UnzipString (byte[] data )
//	{		
//		GZipInputStream gzi = new GZipInputStream(new MemoryStream(data));
//		MemoryStream re = new MemoryStream();
//		int count=0;
//		
//		byte[] buffer = new byte[ 4096 ];
//		
//		while ((count = gzi.Read(buffer, 0, 4096)) != 0)
//		{            
//			re.Write(buffer,0,count);
//		}
//		byte[] depress = re.ToArray();
//		gzi.Close();
//		re.Close();
//		
//		return Encoding.UTF8.GetString( depress );
//	}
	
//	static public double GatherLeftSeconds (int lastGatherTime, int coolDownTime)
//	{
//		System.DateTime dNow = System.DateTime.Now;
//		double now = Utility.ConvertToUnixTimestamp ( dNow );
//		double leftSeconds = (lastGatherTime + coolDownTime) - now;
//		
//		if (leftSeconds <= 0)
//			leftSeconds = 0;
//		
//		return leftSeconds;
//	}
	
	static public void SetMenuBarVisible(bool bShow = true)
	{
#if UNITY_ANDROID || UNITY_IPHONE		
		//MobageCNService.setMenubarVisibility(bShow ? "TRUE" : "FALSE");
#endif		
	}
	
//	static public string GetPlayerIconSpriteName()
//	{
//		//int school = NvPlayerManager.getInstance().CurrentPlayer.school;
//		//int gender = NvPlayerManager.getInstance().CurrentPlayer.gender;
//		string spriteName = "";
//		if(school == 1)
//		{
//			spriteName = "crow";
//		}
//		else if(school == 2)
//		{
//			spriteName = "shark";
//		}
//		else if(school == 3)
//		{
//			spriteName = "lizard";
//		}
//		else
//		{
//			Debug.LogError("Invalid School Value. "+ school);
//		}
//		
//		spriteName += ((gender == 0) ? "_m" : "_f");
//		Debug.Log("GetPlayerIconSpriteName: "+spriteName);
//
//		return spriteName;
//	}
}
 
[System.Serializable]

public struct Zipper
{
	
//	public static string ZipString (string sBuffer)
//	{
//		MemoryStream m_msBZip2 = null;
//		GZipOutputStream m_osBZip2 = null;
//		string result;		
//		{			
//			m_msBZip2 = new MemoryStream ();
//			Int32 size = sBuffer.Length;
//			// Prepend the compressed data with the length of the uncompressed data (firs 4 bytes)
//			//
//			using (BinaryWriter writer = new BinaryWriter(m_msBZip2, System.Text.Encoding.ASCII)) {
//				writer.Write (size);                
//				m_osBZip2 = new GZipOutputStream (m_msBZip2);
//				m_osBZip2.Write (Encoding.ASCII.GetBytes (sBuffer), 0, sBuffer.Length);                
//				m_osBZip2.Close ();
//				result = Convert.ToBase64String (m_msBZip2.ToArray ());
//				m_msBZip2.Close ();               
//				writer.Close ();
//			}
//		} 		
//		{
//			if (m_osBZip2 != null) {
//				m_osBZip2.Dispose ();
//			}
//			
//			if (m_msBZip2 != null) {
//				m_msBZip2.Dispose ();
//			}
//		}
//		return result;
//	}

//	public static string UnzipString (byte[] compbytes )
//	{
//		string result;
//		MemoryStream m_msBZip2 = null;
//		//BZip2InputStream m_isBZip2 = null;		
//		 GZipInputStream m_isBZip2 = null;		
//		{	
//			//compbytes = compbytes.Trim('\0');
//			//Debug.Log("compBytes is "+ compbytes);
//			//m_msBZip2 = new MemoryStream (Convert.FromBase64String (compbytes));
//			m_msBZip2 = new MemoryStream ( compbytes );
//			// read final uncompressed string size stored in first 4 bytes
//			//
//			using (BinaryReader reader = new BinaryReader(m_msBZip2)) 
//			{
//				Int32 size = reader.ReadInt32 ();                
//				m_isBZip2 = new GZipInputStream (m_msBZip2);
//				byte[] bytesUncompressed = new byte[size];
//				m_isBZip2.Read (bytesUncompressed, 0, bytesUncompressed.Length);
//				m_isBZip2.Close ();
//				m_msBZip2.Close ();
//				result = Encoding.ASCII.GetString (bytesUncompressed);
//				reader.Close ();
//			}
//		} 		
//		{
//			if (m_isBZip2 != null) {
//				m_isBZip2.Dispose ();
//			}
//
//			if (m_msBZip2 != null) {
//				m_msBZip2.Dispose ();
//			}
//		}
//		return result;
//	} 
	
}