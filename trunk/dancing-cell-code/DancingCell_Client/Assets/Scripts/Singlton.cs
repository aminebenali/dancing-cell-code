// 
//  Singlton.cs
//  
//  Author:
//    saviosun 
// 
//  Copyright (c) 6, 3, 2012
// 
//  All rights reserved.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Singlton : MonoBehaviour
{
	private static GameObject m_cContainer = null;
	private static string m_cName = "Singlton";
	private static Dictionary<string, object> m_SingltonMap = new Dictionary<string, object>();

	public static object getInstance (string sName)
	{
		if(m_cContainer == null)
		{
			Debug.Log("Create Singlton.");
			m_cContainer = new GameObject ();
			m_cContainer.name = m_cName;	
			m_cContainer.AddComponent (typeof(Singlton));
		}
		if (!m_SingltonMap.ContainsKey(sName)) {
			if(System.Type.GetType(sName) != null)
			{
				m_SingltonMap.Add(sName, m_cContainer.AddComponent (System.Type.GetType(sName)));
			}
			else
			{
				Debug.LogWarning("Singlton Type ERROR!!! (" + sName + ")");
			}
			//postInitialize();
		}
		return m_SingltonMap[sName];
	}	
	
	public void RemoveInstance(string sName)
	{
		if (m_cContainer != null && m_SingltonMap.ContainsKey(sName))
		{
			UnityEngine.Object.Destroy((UnityEngine.Object)(m_SingltonMap[sName]));
			m_SingltonMap.Remove(sName);
		}
	}

	void Awake ()
	{
		/*Try To Find Where the Singlton is create.
		if(m_cContainer != null)
		{
			Debug.LogError("You are try to create 2nd singlton, It is Dangerous!!");
		}
		*/
		Debug.Log("Awake Singlton.");
		DontDestroyOnLoad (gameObject);
	}
	
	void Start()
	{
		Debug.Log("Start Singlton.");
	}	
	
	void Update()
	{
	}
	
	void OnApplicationQuit()
	{
		//Debug.Log("Destroy Singlton");
		if(m_cContainer != null)
			GameObject.Destroy(m_cContainer);
		if(gameObject)
			GameObject.Destroy(gameObject);				
	}
	
}
