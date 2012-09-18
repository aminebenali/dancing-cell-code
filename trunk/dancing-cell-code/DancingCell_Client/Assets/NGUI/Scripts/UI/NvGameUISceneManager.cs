using UnityEngine;
using System.Collections;

public class NvGameUISceneManager : NvUIBaseSceneManager {
	
	private NvUIDialogLayerHandler m_defaultLayer = null;
	
	public NvUIDialogLayerHandler defaultLayer
	{
		get { return m_defaultLayer; }
	}
	
	protected override void AwakeImpl ()
	{		
		m_defaultLayer = createDlgLayer("Default Layer");
		m_defaultLayer.depth = 0;
	}
}
