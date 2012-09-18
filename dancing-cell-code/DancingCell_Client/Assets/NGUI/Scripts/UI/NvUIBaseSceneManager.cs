using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUIBaseSceneManager : MonoBehaviour {
	
	protected Dictionary<string, NvUIDialogLayer> m_layers = new Dictionary<string, NvUIDialogLayer>();
	protected Transform m_sceneRoot = null;
	
	protected NvGameUIManager m_gameUIManager = null;
	
	virtual protected void AwakeImpl() {}
	
	private bool m_active = true;
	private bool m_destroyOnChangeScene = false;
	
	protected Dictionary<string, NvUIDialogManager> m_dlgManagerChain = new Dictionary<string, NvUIDialogManager>();
	
	public bool activate
	{
		get { return m_active; }
		set {
			
			if ( m_active != value )
			{
				m_active = value;
			}
			
		}
	}
	
	public bool destroyOnChangeScene
	{
		get { return m_destroyOnChangeScene; }
		set {
		
			if ( m_destroyOnChangeScene != value )
			{
				m_destroyOnChangeScene = value;
			}
			
		}
	}
	
	private void Awake()
	{
		m_gameUIManager = Singlton.getInstance("NvGameUIManager") as NvGameUIManager;
		m_sceneRoot = m_gameUIManager.GameUILayoutCameraLink;
		
		AwakeImpl();
	}
	
	public NvUIDialogLayerHandler createDlgLayer( string layerName )
	{
		if ( m_layers.ContainsKey( layerName ) )
		{
			return m_layers[layerName].getHandler();
		}
		else 
		{
			NvUIDialogLayer layer = new NvUIDialogLayer( m_sceneRoot );
			m_layers.Add( layerName, layer );
			return m_layers[ layerName ].getHandler();
		}
	}
	
	public NvUIDialogLayerHandler findDlgLayer( string layerName, bool createIfNotExisted = false )
	{
		if ( m_layers.ContainsKey( layerName ) )
		{
			return m_layers[ layerName ].getHandler();
		}
		else
		{
			
			if ( createIfNotExisted )
			{
				return createDlgLayer( layerName );
			}
			
			return null;
		}
	}
	
	public bool destroyDlgLayer( string layerName )
	{
		if ( m_layers.ContainsKey( layerName ) )
		{
			NvUIDialogLayer layer = m_layers[ layerName ];
			layer.destroyAllDialogs();
			m_layers.Remove( layerName );
			return true;
		}
		else
		{
			return false;
		}
	}
	
	virtual protected void UpdateImpl() {}
	
	private void Update()
	{
		if ( activate )
		{
			foreach ( KeyValuePair<string, NvUIDialogLayer> kv in m_layers )
			{
				NvUIDialogLayer layer = kv.Value;
				if ( layer.enable )
				{
					layer.update();
				}
			}
			
			UpdateImpl();
		}
	}
	
	public void Dispose()
	{
		// remove all dialogs
		foreach ( KeyValuePair<string, NvUIDialogLayer> kv in m_layers )
		{
			kv.Value.destroyAllDialogs();	
		}

		DisposeImpl();
		
		m_dlgManagerChain.Clear();
		
		if ( destroyOnChangeScene )
		{
			GameObject.Destroy( gameObject );
		}
	}
	
	// do user dispose job in this function
	virtual protected void DisposeImpl() {}
	
	
	public void registerUIDialogManager( NvUIDialogManager mgr )
	{
		if ( mgr != null && mgr.gameObject.name != "" )
		{
			string mgrName = mgr.gameObject.name;
			if ( m_dlgManagerChain.ContainsKey( mgrName ) == false )
			{
				m_dlgManagerChain.Add ( mgrName, mgr );
			}
		}
	}
	
	public void unregisterUIDialogManager( NvUIDialogManager mgr )
	{
		if ( mgr != null )
		{
			if ( m_dlgManagerChain.ContainsKey( mgr.gameObject.name ) )
			{
				m_dlgManagerChain.Remove( mgr.gameObject.name );
			}
		}
	}
	
	public NvUIDialogManager findUIDialogManager( string mgrName )
	{
		if ( m_dlgManagerChain.ContainsKey( mgrName ) )
		{
			return m_dlgManagerChain[ mgrName ];
		}
		return null;
	}
	
	virtual public bool IsOverUI()
	{
	
		if(UICamera.hoveredObject !=null)
		{
			Debug.Log("UICamera.hoveredObject !=null:  "+UICamera.hoveredObject.name);
			return true;
		}
		
// 		if(UICamera.selectedObject!=null)
// 		{
// 			Debug.Log("UICamera.selectedObject!=null:   "+UICamera.selectedObject.transform.parent.name+"/"+UICamera.selectedObject.name);
// 
//             Debug.Break();
// 			return true;
// 		}
		
		int layerindex = LayerMask.NameToLayer("GameUILayout");
			
		UICamera cam = UICamera.FindCameraForLayer(layerindex);
		
		for (int i = 0; i < Input.touchCount; ++i)
		{
			Touch input = Input.GetTouch(i);
			
			RaycastHit lastHit;
			
			Vector3 pos = cam.camera.ScreenToViewportPoint(input.position);
			
			if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f) continue;
			
			Camera currentCamera = cam.cachedCamera;
			
			Ray ray = currentCamera.ScreenPointToRay(input.position);

			// Raycast into the screen
			int mask = currentCamera.cullingMask & (int)cam.eventReceiverMask;
			
			float dist = (cam.rangeDistance > 0f) ? cam.rangeDistance : currentCamera.farClipPlane - currentCamera.nearClipPlane;
			
			if (Physics.Raycast(ray, out lastHit, dist, mask)) 
			{
				Debug.Log("HIt!!!");
				return true;
			}
		}
		return false;
	}
	
	virtual public bool IsSceneCheckEnable()
	{
//		if(IsOverUI())
//		{
//			return false;
//		}
		foreach(NvUIDialogLayer layer in m_layers.Values)
		{
			if(!layer.IsSceneCheckEnable())
			{
				return false;
			}
		}
		
		
		return true;
	}
}
