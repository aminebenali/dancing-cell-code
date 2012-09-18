using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUIList : NvUIStatic {
	
	private Transform m_ViewSpriteTr = null;
	private Vector3 m_ViewScale = Vector3.one;
	private Transform m_TopLeft = null;
	private Transform m_BottomRight = null;
	//private UIDragCamera m_DragCamera = null;
	private Camera m_UICamera = null;
	
	private GameObject m_ListPrefab = null;
	private GameObject m_ListView = null;
	private Transform m_ListViewLink = null;
	private UIViewport m_ListViewViewport = null;
	private UIAnchor m_listViewAnchor = null;
	
	private List<NvUIListItemBase> itemList = new List<NvUIListItemBase>();
	private NvUIListItemBase m_selectedItem = null;
	private NvUIListItemBase m_lastSelectedItem = null;
	
	//protected UIDraggableCamera m_ListViewCamera = null;
	//protected List<NvUIListItemBase> _itemlist = new List<NvUIListItemBase>();
	protected UITable m_ListViewTable = null;
	protected string prefabName = null;
	
	// on item selected event
	public event OnElementTriggered eventOnItemSelected;
	
	public enum EListStyle 
	{
		eStyle_Horizontal = 0,
		eStyle_Vertical,
	}
	
	public NvUIListItemBase[] ItemList
	{
		get {
			
			return itemList.ToArray();
			
		}
	}
	
	
	public int ItemCount
	{
		get { return itemList.Count; }
	}
	
	public string listName = "ListView";
	public NvUIImage listBaseImage;
	public Vector2 padding = new Vector2( 8.0f, 8.0f );
	public int columns;
	public EListStyle style;
	public int dragCameraDistance = 5000;
	public Vector3 scale = new Vector3( 400.0f, 240.0f, 1.0f );
	public NvUIScrollBar horizontalScrollBar;
	public NvUIScrollBar verticalScrollBar;
	public int emptyItem = 0;
	public Vector2 itemScale = Vector2.zero;
	public List<GameObject> itemPrefabs;
	public List<int> storeEmptyList = new List<int>();
	public float mListCameraOffsetX = 0.0f;
	public float mListCameraOffsetY = 0.0f;
	
	protected int row;
	protected List<GameObject> itemTree = new List<GameObject>();
	
	//private Vector3 m_dragCameraPos0 = Vector3.zero;
	//private UIDraggablePanel m_DragPanel = null;
	private UIScrollBar m_horizontalScrollBar = null;
	private UIScrollBar m_verticalScrollBar = null;
	private Vector2 m_dragCameraScale = Vector2.one;
	private float m_listDistance = 0.0f;
	
	private Bounds mBounds;
	//private bool mShouldMove = false;
	//private bool mCalculatedBounds = false;
	
	/*
	public Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
			}
			return mBounds;
		}
	}
	*/
	public GameObject ListView
	{
		get {return m_ListView;}
	}
	
	protected override void AwakeImpl()
	{
		base.AwakeImpl();
		
		m_ViewSpriteTr = transform.FindChild("ListBaseSprite");
		m_ViewScale = m_ViewSpriteTr.localScale;
		m_TopLeft = transform.FindChild("ListBaseSprite/TopLeft");
		m_BottomRight = transform.FindChild("ListBaseSprite/BottomRight");
		//m_DragCamera = transform.FindChild("ListBaseSprite").GetComponent<UIDragCamera>();
		NvGameUIManager mgr = Singlton.getInstance("NvGameUIManager") as NvGameUIManager;
		m_UICamera = mgr.GameUICamera;
		
		m_ListPrefab = Resources.Load("UI Prefabs/Prefabs/Controls/UIListView") as GameObject;
		m_ListView = GameObject.Instantiate( m_ListPrefab ) as GameObject;
		//rename list view
		m_ListView.name = listName;
		Transform listviewCamera = m_ListView.transform.FindChild("UIListViewCamera");
		//Transform listviewCamera = m_ListView.transform.FindChild("UIListViewCamera" );
		//m_ListViewCamera = listviewCamera.GetComponent<UIDraggableCamera>();
		//rename list view draggable camera
		//m_ListViewCamera.name = listName + "Camera";
		//set draggable camera to drag camera
		//m_DragCamera.draggableCamera = m_ListViewCamera;
		m_ListViewViewport = listviewCamera.GetComponent<UIViewport>();
		//set viewport camera
		m_ListViewViewport.sourceCamera = m_UICamera;
		m_ListViewLink = m_ListView.transform.FindChild("Link");
		m_ListViewLink.localScale = Utility.GetScreenRatio();
		Transform listOffset = m_ListView.transform.FindChild("Link/Anchor/ListOffset");
		listOffset.transform.localPosition = new Vector3 ( dragCameraDistance, dragCameraDistance, listOffset.transform.localPosition.z );
		m_ListViewTable = listOffset.GetComponent<UITable>();
		m_listViewAnchor = m_ListView.transform.FindChild("Link/Anchor").GetComponent<UIAnchor>();
		//m_DragPanel = m_ListView.transform.FindChild("Link/Anchor/ListOffset").GetComponent<UIDraggablePanel>();
		//set ui layer camera to list view anchor
		m_listViewAnchor.uiCamera = m_UICamera;
		//set slider bar
		if ( horizontalScrollBar != null )
		{
			m_horizontalScrollBar = horizontalScrollBar.GetComponent<UIScrollBar>();
			//m_DragPanel.scale = new Vector3( 1.0f, 0.0f, 0.0f );
			//m_DragPanel.horizontalScrollBar = m_horizontalScrollBar;
		}
		if ( verticalScrollBar != null )	
		{
			m_verticalScrollBar = verticalScrollBar.GetComponent<UIScrollBar>();
			//m_DragPanel.scale = new Vector3( 0.0f, 1.0f, 0.0f );
			//m_DragPanel.verticalScrollBar = m_verticalScrollBar;
		}
	}
	
	private void Start()
	{
		InitApply ();
	}
	
	void InitApply()
	{
		inputItemsFromEditor();
		scrollBarSet();
		applyStyle();
	}
	
	protected override void lazyUpdateImpl()
	{
		if ( horizontalScrollBar != null )
		{
			//float barValue = (m_ListViewCamera.transform.localPosition.x - m_dragCameraPos0.x ) / ( m_listDistance - m_dragCameraScale.x - itemScale.x);
			//m_horizontalScrollBar.scrollValue = barValue;
		}
	}
	
	protected virtual void inputItemsFromEditor() 
	{
		if ( itemPrefabs != null && itemPrefabs.Count > 0 )
		{
			//_itemlist.Clear();
			List<NvUIListItemBase> _itemlist = new List<NvUIListItemBase>();
			//int i = 0;
			int ii = 0;
			foreach( GameObject prefab in itemPrefabs )
			{
				if ( prefab != null && itemScale != Vector2.zero)
				{
					GameObject item = GameObject.Instantiate( prefab ) as GameObject;
					item.name = prefab.name + ii;
					prefabName = prefab.name;
					item.transform.parent = m_ListViewTable.transform;
					item.transform.localPosition = Vector3.zero;
					item.transform.localScale = Vector3.one;
					//item.transform.localScale = new Vector3( itemScale.x, itemScale.y, 0.0f );
					item.GetComponent<BoxCollider>().size = new Vector3( itemScale.x, itemScale.y, 0.0f );
					item.transform.FindChild("itemBase").transform.localScale = new Vector3( itemScale.x, itemScale.y, 0.0f );
					item.GetComponent<NvUIListItemBase>().listController = this;
					
					itemTree.Add( item );
					
					//UIDragCamera _camera = item.GetComponent<UIDragCamera>();
					//_camera.draggableCamera = m_ListViewCamera;
					NvUIListItemBase _item = item.GetComponent<NvUIListItemBase>();
					_item.index = ii;
					
					_itemlist.Add( _item );
					++ ii;
				}
			}
			if ( _itemlist.Count > 0 )
			{
				setList( _itemlist );
				checkEmptyItems();
			}
			
			row = itemPrefabs.Count / columns;
		}
		else
		{
			row = 1;
		}
			
	}
	
	private void scrollBarSet()
	{

		Vector3 _diffValue = m_TopLeft.transform.localPosition - m_BottomRight.transform.localPosition;
		m_dragCameraScale = new Vector2( Mathf.Abs( _diffValue.x * m_ViewScale.x ), Mathf.Abs( _diffValue.y * m_ViewScale.y) );
		//Debug.Log( "####################    m_dragCameraScale:" + m_dragCameraScale);
		
		if ( style == EListStyle.eStyle_Horizontal )
		{
			m_listDistance = itemScale.x * columns + padding.x * ( columns - 1 );
		}
		else if ( style == EListStyle.eStyle_Vertical )
		{	
			m_listDistance = itemScale.y * row + padding.y * ( row - 1 );
		}
		//Debug.Log( "####################    m_listDistance:" + m_listDistance);
		
		if ( horizontalScrollBar != null )
		{
			m_horizontalScrollBar.barSize = m_dragCameraScale.x / m_listDistance;
		}
		if ( verticalScrollBar != null )
		{
			m_verticalScrollBar.barSize = m_dragCameraScale.y / m_listDistance;
		}
		/*
		//copy from UIDraggablePanel
		if ( horizontalScrollBar != null )
		{
			m_horizontalScrollBar.onChange += OnHorizontalBar;
		}
		
		if ( verticalScrollBar != null )
		{
			m_verticalScrollBar.onChange += OnVerticalBar;
		}
		*/		
	}
	/*
	public void checkEmptyItems()
	{
		storeEmptyList.Clear();
		for ( int j = 0; j < _itemlist.Count; j ++ )
		{
			if ( _itemlist[j].status == NvUIListItemBase.ITEM_STATUS.EMPTY )
			{
				storeEmptyList.Add(j);
			}
		}
	}
	*/
	protected virtual void checkEmptyItems()	{}
	
	private void applyStyle()
	{
		m_ViewScale = scale;
		//set list view viewport TOP_LEFT Transform
		m_ListViewViewport.topLeft = m_TopLeft;
		//set list view viewport BOTTOM_RIGHT Transform
		m_ListViewViewport.bottomRight = m_BottomRight;
		//set list view padding (x, y)
		m_ListViewTable.padding = padding;
		/*set list view columns
			if columns == 0 then Horizontal
			if columns == 1 then Vertical
			if columns > 1 then mutli-lines*/
		m_ListViewTable.columns = columns;
		
		m_ViewSpriteTr.localScale = m_ViewScale;
		
//		if ( style == EListStyle.eStyle_Horizontal )
//		{
//			m_ListViewCamera.scale = new Vector2(1.0f, 0.0f );
//			//m_dragCameraPos0 = new Vector3( m_ViewScale.x / 2 /*+ itemScale.x / 2 */+ dragCameraDistance, dragCameraDistance - (itemScale.y / 2) * row, -100.0f );
//			float offsetx = dragCameraDistance * m_ListViewLink.localScale.x - m_TopLeft.transform.localPosition.x * m_ListViewLink.localScale.x * m_ViewScale.x - padding.x * m_ListViewLink.localScale.x;
//			float offsety = dragCameraDistance * m_ListViewLink.localScale.y - m_TopLeft.transform.localPosition.y * m_ListViewLink.localScale.y * m_ViewScale.y - padding.y * m_ListViewLink.localScale.y * ( - row * 2 + 1) + 2.0f;//(itemScale.y / 2) * row - padding.y;
//			offsetx += mListCameraOffsetX;
//			offsety += mListCameraOffsetY;
//			m_dragCameraPos0 = new Vector3 ( offsetx, offsety, -100.0f );
//			m_ListViewCamera.transform.localPosition = m_dragCameraPos0;
//		}
//		else if ( style == EListStyle.eStyle_Vertical )
//		{	
//			m_ListViewCamera.scale = new Vector2(0.0f, 1.0f );
//			float offsetx = (dragCameraDistance + (itemScale.x / 2) * row) * m_ListViewLink.localScale.x;
//			float offsety = (dragCameraDistance - m_TopLeft.transform.localPosition.y * m_ViewScale.y) * m_ListViewLink.localScale.y;//dragCameraDistance - m_ViewScale.y / 2 - itemScale.y / 2;
//			m_dragCameraPos0 = new Vector3( offsetx, offsety, -100.0f );
//			m_ListViewCamera.transform.localPosition = m_dragCameraPos0;
//		}
		
	}
	/*
	public void setItem( int index, NvUIListItemBase.ITEM_STATUS status, UIAtlas srcAtlas,  string spriteName, Vector3 scale)
	{
		itemList[ index ].status = status;
		m_ListViewTable.transform.FindChild(""+prefabName+index+"/item").GetComponent<NvUIImage>().atlas = srcAtlas;
		if ( srcAtlas != null )
		{
			m_ListViewTable.transform.FindChild(""+prefabName+index+"/item").GetComponent<NvUIImage>().spriteName = spriteName;
			m_ListViewTable.transform.FindChild(""+prefabName+index+"/item").localScale = scale;
		}
	}
	*/
	
	/// <summary>
	/// launch a list of items
	/// </summary>
	public void setList(List<NvUIListItemBase> _items)
	{
		if ( _items != null && _items.Count > 0 )
		{
			itemList.Clear();
			foreach ( NvUIListItemBase item in _items )
			{
				itemList.Add ( item );
			}
		}
	}
	
	/// <summary>
	/// Add a simple item to list
	/// </summary>
	public void addItem(NvUIListItemBase _item)
	{
		if ( _item != null )
		{
			if (_item.transform.parent == m_ListViewTable.transform)
			{
				return;
			}
			if ( !itemList.Contains(_item) )
			{
				_item.gameObject.transform.parent = m_ListViewTable.transform;
				_item.transform.localPosition = Vector3.zero;
				_item.transform.localScale = Vector3.one;
				_item.GetComponent<BoxCollider>().size = new Vector3( itemScale.x, itemScale.y, 0.0f );
				//_item.transform.FindChild("itemBase").transform.localScale = new Vector3( itemScale.x, itemScale.y, 0.0f );
				_item.listController = this;
				//_item.GetComponent<UIDragCamera>().draggableCamera = m_ListViewCamera;
				
				itemList.Add( _item );
				_item.index = itemList.Count - 1;
				_item.listController = this;
				
				m_ListViewTable.repositionNow = true;
			}
		}
	}
	
	/// <summary>
	/// Add a simple slot item to list
	/// </summary>	
	/// 
	public void addSlotItem ( NvUIListItemBase _item )
	{
		if ( _item != null )
		{
			if (_item.transform.parent == m_ListViewTable.transform)
			{
				return;
			}
			if ( !itemList.Contains(_item) )
			{
				_item.gameObject.transform.parent = m_ListViewTable.transform;
				_item.transform.localPosition = Vector3.zero;
				_item.transform.localScale = Vector3.one;
				//_item.GetComponent<BoxCollider>().size = new Vector3( itemScale.x, itemScale.y, 0.0f );
				if ( _item.transform.FindChild ( "Slot/HitBox" ) != null )
				{
					Transform boxTr = _item.transform.FindChild ( "Slot/HitBox" );
					boxTr.GetComponent<BoxCollider>().size = new Vector3( itemScale.x, itemScale.y, 0.0f );
				}
				_item.listController = this;
				//_item.GetComponent<UIDragCamera>().draggableCamera = m_ListViewCamera;
				
				itemList.Add( _item );
				_item.index = itemList.Count - 1;
				_item.listController = this;
				
				m_ListViewTable.repositionNow = true;
			}
		}		
	}
	
	public NvUIListItemBase createItem( GameObject prefab )
	{
		if ( prefab != null )
		{
			GameObject obj = GameObject.Instantiate( prefab ) as GameObject;
			obj.name = prefab.name;
			NvUIListItemBase item = obj.GetComponent<NvUIListItemBase>();
			
			addItem( item );
			
			
			if ( obj.GetComponent<UIPanel>() != null )
			{
				// shouldn't generate panel at this point
				//Debug.Log ("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! panel has been generated!");
				GameObject.Destroy( obj.GetComponent<UIPanel>() );
			}
			
			return item;
			
		}
		
		return null;
	}
	
	/// <summary>
	/// Remove a item from the list.
	/// </summary>
	public void removeItem(int index)
	{
		if ( index >= 0 && index < itemList.Count )
		{
			GameObject.DestroyImmediate( m_ListViewTable.transform.GetChild(index) );
			m_ListViewTable.repositionNow = true;
			
			itemList.RemoveAt( index );
			
			for ( int ii = index; ii < itemList.Count; ++ii )
			{
				NvUIListItemBase item = itemList[ii];
				item.index = ii;
			}
		}
	}	
	
	/// <summary>
	/// Insert an item to the list
	/// </summary>
	public void insertItem(int index, NvUIListItemBase item )
	{
		if ( item != null )
		{
			if ( index > itemList.Count )
			{
				addItem ( item );
			}
			else if ( index >= 0 && index < itemList.Count )
			{
				item.listController = this;
				itemList.Insert( index, item );
				for ( int ii = index; ii < itemList.Count; ++ii )
				{
					NvUIListItemBase _item = itemList[ii];
					_item.index = ii;
				}
			}
		}
	}	
	
	/// <summary>
	/// select a item
	/// </summary>
	public void selectItem( int index )
	{
		if ( index >= 0 && index < itemList.Count )
		{
			OnItemSelectedWithoutSE( itemList[ index ] );
		}
	}
	
	/// <summary>
	/// selected item.
	/// </summary>
	public NvUIListItemBase selectedItem
	{
		get {
			return m_selectedItem;	
		}
	}
	
	public NvUIListItemBase lastSelectedItem
	{
		get {
		
			return m_lastSelectedItem;
			
		}
	}
	
	public int selectedItemIdx
	{
		get {
			return m_selectedItem != null ? m_selectedItem.index : -1;
		}	
	}
	
	/// <summary>
	/// clear the list.
	/// </summary>
	public void clearList()
	{
		foreach ( NvUIListItemBase item in itemList )
		{
			GameObject.Destroy( item.gameObject );
		}
		itemList.Clear();
		
		//m_ListViewTable.repositionNow = true;
	}
	
	public NvUIListItemBase FindItem( int index )
	{
		if ( index >= 0 && index < itemList.Count )
		{
			foreach( NvUIListItemBase _item in itemList )
			{
				if ( _item.index == index )
				{
					return _item;
				}
			}
			return null;
		}
		else
		{
			return null;
		}
		
	}
	
	private void OnItemSelected( NvUIListItemBase item )
	{
		if ( enable )
		{
			//(Singlton.getInstance("NvSoundController") as NvSoundController).PlaySe("se_click");
			
			m_lastSelectedItem = m_selectedItem;
			m_selectedItem = item;
			
			if ( eventOnItemSelected != null )
			{
				eventOnItemSelected( this, item );
			}
		}
	}
	
	private void OnItemSelectedWithoutSE( NvUIListItemBase item )
	{
		if ( enable )
		{
			m_lastSelectedItem = m_selectedItem;
			m_selectedItem = item;
			
			if ( eventOnItemSelected != null )
			{
				eventOnItemSelected( this, item );
			}
		}
	}
	
	public void DraggableCameraEnable( bool flag )
	{
//		if ( flag == true )
//		{
//			if ( style == EListStyle.eStyle_Horizontal )
//			{
//				m_ListViewCamera.scale = new Vector2(1.0f, 0.0f );
//			}
//			else if ( style == EListStyle.eStyle_Vertical )
//			{
//				m_ListViewCamera.scale = new Vector2(0.0f, 1.0f );
//			}
//		}
//		else if ( flag == false )
//		{
//			m_ListViewCamera.scale = Vector2.zero;
//		}
	}
	
	void OnDestroy()
	{
		DestroyImmediate( m_ListView );
	}
	
	public override void setEnable (bool flag)
	{
		base.setEnable (flag);
		
		//m_ListView.GetComponent<NvUIBase>().setEnable( flag );
		
		if ( listBaseImage != null )
		{
			listBaseImage.setEnable( flag );
		}
		
		foreach ( NvUIListItemBase item in itemList )
		{
			//GameObject.Destroy( item.gameObject );
			UIDragCamera obj = item.transform.GetComponent<UIDragCamera>();
			if ( obj != null )
			{
				obj.enabled = flag;
			}
		}
	}
	
	private bool setVisibleCallback( Transform tr, object args )
	{
		bool flag = (bool)args;
		NvUIBase elem = tr.GetComponent<NvUIBase>();
		if ( elem != null )
		{
			elem.setVisible( flag );
			return true;
		}
		else
		{
			tr.gameObject.active = flag;
			if ( tr.collider != null )
			{
				tr.collider.enabled = flag;
			}
		}
		return false;
	}
	
	public override void setVisible (bool flag)
	{
		base.setVisible (flag);
	
		Utility.enumWidgets( transform, setVisibleCallback, flag as object );
		
		//if ( listBaseImage != null )
		//{
		//	listBaseImage.setVisible( flag );
		//}
		
		//m_ListView.GetComponent<NvUIBase>().setVisible( flag );
		m_ListView.SetActiveRecursively( flag );
	}
	
	public void resetListPosition()
	{
		applyStyle();
	}
	
	public void repositionNow()
	{
		m_ListViewTable.repositionNow = true;
//		m_ListViewCamera.ConstrainToBounds ( true );
	}
}
