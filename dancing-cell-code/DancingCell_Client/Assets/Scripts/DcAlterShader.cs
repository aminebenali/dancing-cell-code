using UnityEngine;
using System.Collections;

public class DcAlterShader : MonoBehaviour 
{
	public SkinnedMeshRenderer[] m_smr;
	
	void Start () 
	{
        m_smr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        //foreach (SkinnedMeshRenderer smr in m_smr) 
		for(int i = 0; i < m_smr.Length; i++)
		{
			//头发 和 眼镜 带透明
			if(m_smr[i].name == "hair" || m_smr[i].name == "glasses")
			{
				m_smr[i].material.shader = Shader.Find("Unlit/Transparent Cutout");
			}
			//替他身体部分不透明
			else
			{
            	m_smr[i].material.shader = Shader.Find("Diffuse");//Unlit/Texture
			}
		}
    }
	
	void Update () 
	{
	
	}
}
