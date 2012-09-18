using UnityEngine;

/// <summary>
/// Selectable sprite that follows the mouse or the touch.
/// </summary>

public class NvUICursor : MonoBehaviour
{
	public object userData = null;
	public int index = -1;
	public NvUIDialogBase dialogController;
	
	// Camera used to draw this cursor
	public Camera uiCamera;
	
	private Transform mTrans;
	//get only one UISprite Object, later it will be replaced of one Class Object
	public UISprite mSprite;
	public UIAtlas mAtlas;
	public string mSpriteName;
	
	public bool isMouseButtonUp = false;
	
	void Awake()
	{
		mTrans = transform;
		mSprite = GetComponentInChildren<UISprite>();
		mAtlas = mSprite.atlas;
		mSpriteName = mSprite.spriteName;
		if ( uiCamera == null ) uiCamera = (Singlton.getInstance( "NvCommonUIManager" ) as NvCommonUIManager).SystemUICamera;
	}
	
	
	
	void Update()
	{
		
		//if (mSprite.atlas != null)
		{
			//Vector3 pos = Input.mousePosition;
			Vector3 pos = Vector3.zero;
			
			if (uiCamera != null)
			{
				if ( Input.touchCount == 1 )
				{
					// we've got only a single touch here, handle what it comes
					Touch touch = Input.GetTouch(0);
					pos = touch.position;
				}
				else
				{
					pos = Input.mousePosition;
					/*
					if ( Input.touchCount == 0 && mSprite.atlas != null )
					{
						Debug.Log ( "###################     mouse down : " + Input.GetMouseButtonDown( 0 ) );
						Debug.Log ( "###################     mouse up : " + Input.GetMouseButtonUp( 0 ) );
					}
					*/
					if ( (Input.GetMouseButtonUp( 0 ) && mSprite.atlas != null ) )
					{
						if ( dialogController != null )
						{
							mSprite.atlas = null;
							dialogController.SendMessage( "OnCursorUpError", this, SendMessageOptions.DontRequireReceiver );
						}
					}
				}
				
				
				// Since the screen can be of different than expected size, we want to convert
				// mouse or touch coordinates to view space, then convert that to world position.
				pos.x = Mathf.Clamp01(pos.x / Screen.width);
				pos.y = Mathf.Clamp01(pos.y / Screen.height);
				mTrans.position = uiCamera.ViewportToWorldPoint(pos);
				
				//Debug.Log( "##########################################     position: " + mTrans.position );
				
				
				// For pixel-perfect results
				if (uiCamera.isOrthoGraphic)
				{
					mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(mTrans.localPosition, mTrans.localScale);
				}
			}
			else
			{
				// Simple calculation that assumes that the camera is of fixed size
				pos.x -= Screen.width * 0.5f;
				pos.y -= Screen.height * 0.5f;
				mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(pos, mTrans.localScale);		
			}	
		}
	}
	
	
	public void Clear ()
	{
		Set( mAtlas, mSpriteName, userData, index );
	}
	
	public void Set ( UIAtlas atlas, string sprite, object data, int _index )
	{
		mSprite.atlas = atlas;
		mSprite.spriteName = sprite;
		mSprite.MakePixelPerfect();
		
		userData = data;
		index = _index;
		
		Update();
	}
}