using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvGameUIManager : MonoBehaviour {
	
	protected List<GameObject> mSceneList = new List<GameObject>();
	
	protected Transform mGameUILayoutCameraLinkTr = null;
	protected Camera mGameUILayoutCamera = null;
	
	protected Transform mBillboardLayoutOrthoCameraLinkTr = null;
	protected Camera mBillboardLayoutOrthoCamera = null;
	
	private bool m_inited = false;
	private NvGameUISceneManager mGameUISceneManager = null;
	
	// panel for billboards
	//private Transform mOrthoBillboardLinkTr = null;
	//private Transform mPerspBillboardLinkTr = null;
	
	// billboard chain for created billboard
	private List<GameObject> mBillboardEntityChainOrtho = new List<GameObject>();
	private List<GameObject> mBillboardEntityChainPersp = new List<GameObject>();
	
	private Camera mMainCamera = null;
	private Transform mPrespectiveBillboardCameraTr = null;
	
	public Camera CachedMainCamera
	{
		get { return mMainCamera; }
	}

	public Transform GameUILayoutCameraLink
	{
		get { return mGameUILayoutCameraLinkTr; }
	}
	
	public Camera GameUICamera
	{
		get { return mGameUILayoutCamera; }
	}
	
	void Awake()
	{
		if (m_inited == false)
		{
			m_inited = true;
			launchCamera();
		}
	}
	
	void Start()
	{
		// create GaemUISceneManager
		mGameUISceneManager = Singlton.getInstance("NvGameUISceneManager") as NvGameUISceneManager;
	}
	
	private void launchCamera() 
	{		
		GameObject CameraPrefab = Resources.Load( "UI Prefabs/Prefabs/GameUICamera" ) as GameObject;
		GameObject CameraObj = Utility.Instantiate( CameraPrefab ) as GameObject;
		
		CameraObj.name = CameraPrefab.name;
		CameraObj.transform.localPosition = Vector3.zero;
		CameraObj.transform.localRotation = Quaternion.identity;
		CameraObj.transform.localScale = Vector3.one;
		
		mGameUILayoutCameraLinkTr = CameraObj.transform.FindChild("GameUILayout Camera/Link");
		//mBillboardLayoutCameraLinkTr = CameraObj.transform.FindChild("BillboardLayout Camera/Link");
		mGameUILayoutCamera = mGameUILayoutCameraLinkTr.parent.GetComponent<Camera>();
		//mBillboardLayoutCamera = mBillboardLayoutCameraLinkTr.parent.GetComponent<Camera>();
		
		mGameUILayoutCameraLinkTr.localScale = Utility.GetScreenRatio();
		//mBillboardLayoutCameraLinkTr.localScale = Utility.GetScreenRatio();
		
		GameObject BillboardCameraPrefab = Resources.Load ("UI Prefabs/Prefabs/BillboardUICamera") as GameObject;
		GameObject BillboardRootObj = Utility.Instantiate( BillboardCameraPrefab ) as GameObject;
		
		BillboardRootObj.name = BillboardCameraPrefab.name;
		BillboardRootObj.transform.localPosition = Vector3.zero;
		BillboardRootObj.transform.localRotation = Quaternion.identity;
		BillboardRootObj.transform.localScale = Vector3.one;
		mBillboardLayoutOrthoCameraLinkTr = BillboardRootObj.transform.FindChild("BillboardUILayout Camera ( Orthographic )/Link/Anchor/Panel");
		mBillboardLayoutOrthoCamera = BillboardRootObj.transform.FindChild("BillboardUILayout Camera ( Orthographic )").GetComponent<Camera>();
		
		// create billboard link
		//mOrthoBillboardLinkTr = createBillboardLink("Orthographics");
		//mPerspBillboardLinkTr = createBillboardLink("Perspective");
		mBillboardLayoutOrthoCamera.enabled = false;
		
		GameObject.DontDestroyOnLoad ( CameraObj );
		GameObject.DontDestroyOnLoad ( BillboardRootObj );
	}
	
	public void Reset()
	{
		mMainCamera = null;
		mPrespectiveBillboardCameraTr = null;
		mGameUISceneManager.Dispose();
	}
	
	public GameObject AddBillboardEntity( GameObject prefab, bool isOrtho = true, GameObject source = null )
	{
		GameObject obj = Utility.Instantiate( prefab ) as GameObject;
		obj.name = prefab.name;
		if ( isOrtho )
		{
			obj.transform.parent = mBillboardLayoutOrthoCameraLinkTr;
		}
		else
		{
			GameObject panel = new GameObject("Panel");
			panel.transform.parent = source.transform;
			panel.transform.localPosition = Vector3.zero;
			panel.transform.localRotation = Quaternion.identity;
			panel.transform.localScale = Vector3.one;
			panel.AddComponent<UIPanel>();
			panel.layer = LayerMask.NameToLayer("BillboardLayout3D");
			obj.transform.parent = panel.transform;
		}
		
		obj.transform.localScale = Vector3.one;
		obj.transform.name = prefab.name;
		
		Utility.SetLayer( obj, isOrtho ? LayerMask.NameToLayer("BillboardLayout") : LayerMask.NameToLayer("BillboardLayout3D") );
		
		if ( obj.transform.GetComponent<UIPanel>() != null )
		{
			GameObject.DestroyImmediate( obj.transform.GetComponent<UIPanel>() );
		}
		
		if ( isOrtho )
		{
			if ( mBillboardLayoutOrthoCamera.enabled == false )
			{
				mBillboardLayoutOrthoCamera.enabled = true;
			}
			mBillboardEntityChainOrtho.Add(obj);
		}
		else
		{
			mBillboardEntityChainPersp.Add( obj );
		}
		
		return obj;
	}
	
	public void RemoveBillboardEntity( GameObject entity, bool isOrtho = true )
	{
		List<GameObject> chain = isOrtho ? mBillboardEntityChainOrtho : mBillboardEntityChainPersp;
		if ( chain.Contains( entity ) )
		{
			GameObject.Destroy( entity );
			chain.Remove( entity );
			
			if ( chain.Count <= 0 )
			{
				if ( isOrtho )
				{
					mBillboardLayoutOrthoCamera.enabled = false;
				}
			}
		}
	}
	
	public void setMainCamera( Camera cam )
	{
		if ( mMainCamera != cam && cam != null )
		{
			mMainCamera = cam;
		}
	}
	
	private void Update()
	{
		if ( mMainCamera == null )
		{
			GameObject obj = GameObject.FindGameObjectWithTag("MainCamera");
			if ( obj != null )
			{
				mMainCamera = obj.GetComponent<Camera>();
				
				if ( mMainCamera != null && mPrespectiveBillboardCameraTr == null )
				{
					
					mMainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("BillboardLayout3D"));
					
					//
					GameObject camObj = new GameObject("Prespective Billboard Camera");
					camObj.transform.parent = mMainCamera.transform;
					camObj.transform.localPosition = Vector3.zero;
					camObj.transform.localRotation = Quaternion.identity;
					camObj.transform.localScale = Vector3.one;
					
					Camera cam = camObj.AddComponent<Camera>();
					cam.cullingMask = 1 << LayerMask.NameToLayer("BillboardLayout3D");
					cam.isOrthoGraphic = false;
					cam.near = mMainCamera.near;
					cam.far = mMainCamera.far;
					cam.depth = 0;
					cam.fov = mMainCamera.fov;
					cam.clearFlags = CameraClearFlags.Depth;
					
					UICamera hitChecker = camObj.AddComponent<UICamera>();
					hitChecker.eventReceiverMask = cam.cullingMask;
					
					mPrespectiveBillboardCameraTr = camObj.transform;
				}
				
			}
		}
	}
}
