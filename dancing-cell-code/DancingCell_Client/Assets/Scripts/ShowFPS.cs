// 
//  ShowFPS.cs
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
using System.Collections;

public class ShowFPS : MonoBehaviour {
	
	public  float 	m_fUpdateInterval = 0.5f;	 
	private float 	m_fAccum   = 0; // FPS accumulated over the interval
	private int  	m_iFrames  = 0; // Frames drawn over the interval
	private float 	m_fTimeleft; // Left time for current interval
	public  string 	m_sFpsStr = null;  
	
	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		Debug.Log("Start ShowFPS.");
		m_fTimeleft = m_fUpdateInterval;
	}
	
	// Update is called once per frame
	void Update () {
	    m_fTimeleft -= Time.deltaTime;
	    m_fAccum += Time.timeScale/Time.deltaTime;
	    ++m_iFrames;
	    
	    // Interval ended - update GUI text and start new interval
	    if( m_fTimeleft <= 0.0 )
	    {
		    // display two fractional digits (f2 format)
		    float fps = m_fAccum/m_iFrames;
		    m_sFpsStr = System.String.Format("{0:F2} FPS",fps);
		
		    //  DebugConsole.Log(format,level);
	        m_fTimeleft = m_fUpdateInterval;
	        m_fAccum = 0.0f;
	        m_iFrames = 0;
	    }	
	}	
	
	void OnGUI()
	{
		GUI.contentColor = Color.green;
		GUI.Label(new Rect( 5,0, 100,30), m_sFpsStr );
		//GUI.Label(new Rect( 400,60, 100,80), NvConst.kDevelopMode.ToString() + "\n" +  NvConst.kVersionNum  );
		//GUI.Label(new Rect( 700,60, 100,80), Application.loadedLevelName);
	} 
}
