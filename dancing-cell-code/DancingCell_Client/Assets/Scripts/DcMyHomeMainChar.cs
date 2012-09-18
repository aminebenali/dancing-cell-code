
using UnityEngine;
using System.Collections;

public class DcMyHomeMainChar : MonoBehaviour 
{	
	public Animation m_Animation = null;
	
	void Awake()
	{
		m_Animation = gameObject.animation;
	}
	void Start()
    {
		m_Animation.CrossFade("idle");
    }
}