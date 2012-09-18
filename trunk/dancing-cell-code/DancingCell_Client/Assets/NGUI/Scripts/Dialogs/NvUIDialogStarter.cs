using UnityEngine;
using System.Collections;

public class NvUIDialogStarter : MonoBehaviour {

	[SerializeField]
	private NvUIDialogManager mDialogMgr = null;
	
	[SerializeField]
	private string mStartDialogName = "";
	
	void Start()
	{
		if ( mDialogMgr != null )
		{
			mDialogMgr.OpenDialog( mStartDialogName );
		}
		
		if ( mDialogMgr != null && mDialogMgr.gameObject != gameObject )
		{
			GameObject.Destroy( gameObject );
		}
		else 
		{
			// remove component only
			GameObject.Destroy( this );
		}
	}
	
}
