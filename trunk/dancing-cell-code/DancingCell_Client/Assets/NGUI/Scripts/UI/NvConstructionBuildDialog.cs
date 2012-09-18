using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvConstructionBuildDialog : NvUIDialogBase {
	
	private bool m_started = false;
	
	private GameObject obj = null;
	//private NvUIList mList = null;
	
	private Transform mPanelTr = null;
	private Transform mSlideTr = null;
	private NvUIList mList = null;
	
	private GameObject _obj = null;
	
	//public NvUIBase.ObjectClickFunction buildFun;
	
	void Awake()
	{
		GameObject dialogPrefab = Resources.Load( "UI Prefabs/Prefabs/UIConstructionBuildDialog" ) as GameObject;
		obj = Utility.Instantiate( dialogPrefab ) as GameObject;
		obj.name = dialogPrefab.name;
		obj.transform.parent = gameObject.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		
		mPanelTr = obj.transform.FindChild("Panel");
		mSlideTr = mPanelTr.transform.FindChild("SlideBuilding");
		
		//add ui list
		_obj = new GameObject("UIList");
		mList = _obj.AddComponent<NvUIList>();
		mList.transform.parent = mPanelTr;
		_obj.transform.localPosition = Vector3.zero;
		_obj.transform.localRotation = Quaternion.identity;
		_obj.transform.localScale = Vector3.one;	
		
		/*
		//add ensure button
		__obj = new GameObject("EnsureButton");
		mButton = __obj.AddComponent<NvUIButton>();
		mButton.transform.parent = mPanelTr;
		__obj.transform.localPosition = Vector3.zero;
		__obj.transform.localRotation = Quaternion.identity;
		__obj.transform.localScale = Vector3.one;	
		*/
		
	}
	
	// Use this for initialization
	void Start () {
		
		//set ui list (PnS)
		mList.transform.localPosition = mSlideTr.transform.localPosition;
		Vector2 displayRange = new Vector2(mSlideTr.transform.localScale.x, mSlideTr.transform.localScale.y);
		//mList.displayRange = displayRange;
		/*
		//set ui button
		mButton.buttonName = "Ensure";
		Vector3 tmpPos = new Vector3 (mEnsureButtonPnS.x, mEnsureButtonPnS.y, -1.0f);
		mButton.panelTr.localPosition = tmpPos;
		Vector2 tmpSize = new Vector2 (mEnsureButtonPnS.width, mEnsureButtonPnS.height);
		mButton.buttonSize = tmpSize;
		//mButton.buttonFunWithParam = buildFun;
		
		mButton.OnClickEvent += new ObjectClickFunction(buildFun);
		mButton.clickParam = null;
		*/
		
		m_started = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( m_started == false )
		{
			return;
		}
	}
	
	public void setList(List<NvUIListItemBase> _items) 
	{
		mList.setList(_items);
	}
	
	public void addItem(NvUIListItemBase _item)
	{
		mList.addItem(_item);
	}
	
	public void removeItem(int index)
	{
		mList.removeItem(index);
	}
	/*
	public NvUIListItemBase getSeletedItem()
	{
		return mList.selectedItem;
	}
	*/	
	public void clearList()
	{
		mList.clearList();
	}
	
	private void addItems()
	{
		
	}
	
}
