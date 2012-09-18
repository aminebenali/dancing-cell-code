using UnityEngine;
using System.Collections;

public class NvListItemBuilder : NvUIBase {

	private static NvListItemBuilder s_instance = null;
	
	private Vector3 Slide_Horizontal = new Vector3(1.0f, 0.0f, 0.0f);
	private Vector3 Slide_Vertical = new Vector3(0.0f, 1.0f, 0.0f);
	
	void Awake()
	{
		initialize();
	}
	
	void initialize()
	{
		
	}
	
	public static NvListItemBuilder getInstance()
	{
		if (s_instance == null)
		{
			s_instance = Singlton.getInstance("NvListItemBuilder") as NvListItemBuilder;
			s_instance.initialize();
		}
		return s_instance;
	}	
	
	public GameObject addDragComponent(GameObject item, UIGrid mGrid, Vector3 itemSize)
	{
		GameObject _item = item;
		//spawn in grid
		//Debug.Log("====================>    bjy:    _item:"+_item+"|"+"mGrid:"+mGrid);
		_item.transform.parent = mGrid.transform;
		//_item.transform.localScale = Vector3.one;
		_item.transform.localScale = itemSize;
		
		//add drag object
		_item.AddComponent<UIDragObject>();
		_item.GetComponent<UIDragObject>().target = mGrid.transform;
		_item.GetComponent<UIDragObject>().restrictWithinPanel = true;
		if ( mGrid.arrangement == UIGrid.Arrangement.Horizontal )
		{
			_item.GetComponent<UIDragObject>().scale = Slide_Horizontal;
		}
		else if ( mGrid.arrangement == UIGrid.Arrangement.Vertical )
		{
			_item.GetComponent<UIDragObject>().scale = Slide_Vertical;
		}
		//add box collider
		_item.AddComponent<BoxCollider>();
		_item.GetComponent<BoxCollider>().size = itemSize;
		//add button scale
		_item.AddComponent<NvUIListItem>();
		
		return _item;
	}
}
