using UnityEngine;
using System.Collections;

public class CommonUITexture : MonoBehaviour
{
	private UITexture m_Texture;
	
	// Use this for initialization
	void Awake ()
	{
		if ( m_Texture == null ){
			m_Texture = gameObject.GetComponent<UITexture>();
		}
		if (m_Texture == null ){
			Debug.LogError("No Texture Found!!");
		}
	}
	
	void Start()
	{		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public Material ChangePortraitTexture( string name )
	{				
		Texture t = Resources.Load("UI Prefabs/PortraitMaterial/"+name) as Texture;
		if ( t == null ){
			Debug.LogError("No Such Portrait "+ name);
		}		
		m_Texture.material = new Material(Shader.Find("Unlit/Transparent"));
		m_Texture.material.mainTexture = t;
		
		return m_Texture.material;
	}
	
	public void ChangeTutorialTexture( string name )
	{
		Texture t = Resources.Load("UI Prefabs/TutorialGuide/"+name) as Texture;
		if ( t == null ){
			Debug.LogError("No Such TutorialGuide "+ name);
		}		
		m_Texture.material = new Material(Shader.Find("Unlit/Transparent"));
		m_Texture.material.mainTexture = t;		
	}
	
	public void ChangeStoryTexture( string name )
	{
		Texture t = Resources.Load("UI Prefabs/StoryBoard/" + name ) as Texture;
		if ( t == null ){
			Debug.LogError("No Such Story "  + name);
		}
		
		m_Texture.material = new Material(Shader.Find("Unlit/Transparent Colored"));					
		m_Texture.material.mainTexture = t;
		m_Texture.alpha = 0.01f;
		
	}
	
	public void SetStroryTextureAlpha( float val )
	{
		if (m_Texture != null ){
			m_Texture.alpha = val;
		}else{
			Debug.LogError("Texture is NULL!!!");
		}
	}
	
	
			
}

