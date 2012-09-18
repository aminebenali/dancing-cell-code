using UnityEngine;
using System.Collections;

public class DcCharacterShadow : MonoBehaviour 
{
	private GameObject  shadowobj   = null;
	private bool        m_IsVisable = true;
	private float       m_alpha     = 1.0f;

	void Awake()
	{
		shadowobj = Instantiate(Resources.Load("player/Shadow/shadowperfab",typeof(GameObject)))as GameObject;
		shadowobj.transform.parent = gameObject.transform;

        shadowobj.transform.localPosition = new Vector3(-2, 0.1f, 0);
        shadowobj.transform.localScale = new Vector3(6f, 6f, 6f);
        shadowobj.transform.Rotate(new Vector3(13f, 0f, 0f));
		
		shadowobj.renderer.enabled = m_IsVisable;
        SetAlpha(0.7f);
	}
	
	public void SetVisable(bool IsVisable)
	{
		m_IsVisable = IsVisable;
		shadowobj.renderer.enabled = m_IsVisable;
	}

    public void SetAlpha(float newalpha)
    {
        m_alpha = newalpha;
        
        if (shadowobj.renderer.material != null)
        {
            Color maincolor = shadowobj.renderer.material.color;
            Color newcolor = new Color(maincolor.r, maincolor.g, maincolor.b, newalpha);
            shadowobj.renderer.material.color = newcolor;
        }
    }
}
