using UnityEngine;
using System.Collections;

public class NvDialogSpawner : MonoBehaviour {
	
	public GameObject[] DialogPrefabs = null;
	public string layerName = "Default Layer";
	public string startDlgName = "";
	
	void Start()
	{
		NvGameUISceneManager mgr = Singlton.getInstance("NvGameUISceneManager") as NvGameUISceneManager;
		NvUIDialogLayerHandler layer = mgr.findDlgLayer( layerName );
		
		if ( layer != null )
		{
			if ( DialogPrefabs != null )
			{
				foreach ( GameObject obj in DialogPrefabs )
				{
					if ( obj != null )
					{
						NvUIDialogBase dlg = layer.createDialog( obj.name, obj );
						if ( dlg != null && dlg.name == startDlgName )
						{
							dlg.openDialog();
						}
					}
				}
			}
		}
		
		GameObject.Destroy( gameObject );
	}
	
}
