using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DCGesture : MonoBehaviour 
{
	public bool m_IsPress = false; // is just press mouth or touch
	
	public bool m_IsUpPress = false;
	
	public bool m_IsHold = false;

	public delegate void DelegateGetGesture(string result);
	public delegate void NoHasPoints();
	public delegate void SamplingPoint(Vector3 posPos);
	
	public DelegateGetGesture FuncGetGesture;
	public NoHasPoints FuncNoHasPoints;
	public SamplingPoint FuncSamplingPoint;
	
	private bool m_IsEnabled = false;
	
	private bool m_IsCanDoCatchGesture = true;//if over ui,it will be false
	
	private List<Point2D> m_Point2dList = new List<Point2D>();
		
	private float m_TouchMoveLength = 0;

	private float m_SingleDirOffAngle = 20.0f;
	
	private GeometricRecognizer geometricrec = null;
	
	private bool m_IsSingleDir = false;
	
	// Use this for initialization
	void Start () 
	{
		geometricrec = new GeometricRecognizer();
		
		geometricrec.loadTemplates();
		
		//LogShow.getInstance().debug("=>Test!!  ");
			
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckPressState();
		
		if(!m_IsEnabled )
		{
			return;
		}
		if(!ProcessPick())
		{
			Debug.LogWarning("  ProcessPick: FALSE");
			return;
		}
		if(IsCanGetPos())
		{
			GetPoint();
		}
		ProcessDrop();
		
	}
	
	private bool ProcessPick()
	{
		if(m_IsPress && IsOnUI())
		{
			m_IsCanDoCatchGesture = false;
			
			ClearPoints();
			
			Debug.LogWarning(" ProcessPick OnUI ");
			
			return false;
		}
		else if(m_IsPress)
		{
			ClearPoints();
			
			OnPick();
			
			return true;
		}
		return true;
	}
	
	private bool ProcessDrop()
	{
		if(m_IsUpPress && IsOnUI())
		{
			m_IsCanDoCatchGesture = false;
			
			ClearPoints();
			
			Debug.LogWarning(" ProcessPick OnUI ");
			
			
			return false;
		}
		else if(m_IsUpPress)
		{
			if(!m_IsCanDoCatchGesture)
			{
				m_IsCanDoCatchGesture = true;
				
				return true;
			}
			else
			{
				OnDrop();
			}
			
			return true;
		}
		return false;
	}
	
	private void GetPoint()
	{
//		if(UICamera.currentTouch==null)
//		{
//			Debug.LogError("UICamera.currentTouch==null");
//			return;
//		}
//		Point2D tmppoint = new Point2D(UICamera.currentTouch.pos.x,UICamera.currentTouch.pos.y);
		
		Vector3 point = GetTouchPoint();
		
		Point2D tmppoint = new Point2D(point.x,point.y);
		
		if(FuncNoHasPoints!=null)
		{
			FuncNoHasPoints();
		}
		
		//Debug.LogWarning("======> mousePosition:  "+Input.mousePosition + " x: "+tmppoint.x+" y: "+tmppoint.y);
		if(m_Point2dList.Count>0)
		{
			float movelength = GetLength(m_Point2dList[m_Point2dList.Count-1],tmppoint);
			
			float dis_x = Mathf.Abs((float)(m_Point2dList[m_Point2dList.Count-1].x)-(float)(tmppoint.x));
			
			float dis_y = Mathf.Abs((float)(m_Point2dList[m_Point2dList.Count-1].y)-(float)(tmppoint.y));
			
			float maxdir = dis_x>dis_y?dis_x:dis_y;
			
			m_TouchMoveLength += movelength;
			
			if(movelength>15 || maxdir>8)
			{
				m_Point2dList.Add(tmppoint);
				
				if(FuncSamplingPoint!=null)
				{
					FuncSamplingPoint(new Vector3((float)tmppoint.x-400f, (float)tmppoint.y-240f, 0f));
				}
			}
		}
		else
		{
			m_Point2dList.Add(tmppoint);
			
			if(FuncSamplingPoint!=null)
			{
				FuncSamplingPoint(new Vector3((float)tmppoint.x-400f, (float)tmppoint.y-240f, 0f));
			}
		}
		
	}
	
	
	private float GetLength(Point2D pos1, Point2D pos2)
	{
		Vector2 p1 = new Vector2((float)pos1.x,(float)pos1.y);
		
		Vector2 p2 = new Vector2((float)pos2.x,(float)pos2.y);
		
		return (p1-p2).magnitude;
	}
	

	
	private void OnDrop()
	{
		if(m_Point2dList.Count<2)
		{
			ClearPoints();
			
			return;
		}
		
		//Debug.LogWarning("  m_Point2dList:  "+m_Point2dList.Count);
		
		string result = CatchGesture();
		
		//Debug.LogWarning("@@@@@@@@@@@@@@@@=========>  result:  "+result);
		
		if(FuncGetGesture!=null)
		{
			FuncGetGesture(result);
		}
		ClearPoints();
		
	}
	
	private string CatchGesture()
	{
		List<Point2D> tmplist = new List<Point2D>();
		
		foreach(Point2D point in m_Point2dList)
		{
			Point2D newpoint = new Point2D(point.x,point.y);
			
			tmplist.Add(newpoint);
		}
		if(m_IsSingleDir)
		{
			return JustGestureSingleDir(tmplist);
		}
		string retresult = TryToSingleDir(tmplist);
		
		if(retresult!="Unknow")
		{
			return retresult;
		}
		float timedis = 0;
		
		float curtime = Time.time;
		
		RecognitionResult result = geometricrec.recognize(tmplist);
		
		timedis = Time.time - curtime;
		
		Debug.LogWarning("++++++++++%%%%%%%%%%%%%%%%%%%    Timedis ; "+timedis);
		/*
		for(int i =0; i<m_Point2dList.Count;i++)
		{
			Debug.LogWarning(" Point: ["+i+"] :  "+ m_Point2dList[i].GetString());
		}
		*/
		result.Print();
		
		if(result.name.Contains("Rectangle"))
		{
			return "Rectangle";
		}
		else if(result.name.Contains("Triangle"))
		{
			return "Triangle";
		}
		else
		{
			return result.name;
		}
	}
	
	private string JustGestureSingleDir(List<Point2D> points)
	{
		if(m_Point2dList.Count<2)
		{
			if(FuncNoHasPoints!=null)
			{
				FuncNoHasPoints();
			}
			return "Unknow";
		}
		Point2D firstpos = points[0];
		
		Point2D endpos = points[points.Count-1];
		
		if(Mathf.Abs((float)(endpos.x - firstpos.x))>Mathf.Abs((float)(endpos.y - firstpos.y)))
		{
			if(endpos.x>firstpos.x)
			{
				return "Right";
			}
			else if(endpos.x<firstpos.x)
			{
				return "Left";
			}
			else
			{
				return "Unknow";
			}
		}
		else if(Mathf.Abs((float)(endpos.x - firstpos.x))<Mathf.Abs((float)(endpos.y - firstpos.y)))
		{
			if(endpos.y>firstpos.y)
			{
				return "Up";
			}
			else if(endpos.y<firstpos.y)
			{
				return "Down";
			}
			else
			{
				return "Unknow";
			}
		}
		else
		{
			return "Unknow";
		}
	}
	
	private void GetMaxMinSingleDirDis(List<Point2D> points,ref Vector2 max,ref Vector2 min)
	{
		for(int i=0;i<points.Count;i++)
		{
			if(max.x<points[i].x)
			{
				max.x = (float)points[i].x;
			}
			if(max.y<points[i].y)
			{
				max.y = (float)points[i].y;
			}
			if(min.x>points[i].x)
			{
				min.x = (float)points[i].x;
			}
			if(min.y>points[i].y)
			{
				min.y = (float)points[i].y;
			}
		}
	}
	
	private string TryToSingleDir(List<Point2D> points)
	{
		Vector2 max = new Vector2(float.MinValue,float.MinValue);
		
		Vector2 min = new Vector2(float.MaxValue,float.MaxValue);
		
		GetMaxMinSingleDirDis(points,ref max,ref min);
		//if(up or down)
		if((max.y-min.y) > (max.x - min.x))
		{
			if(((max.x-min.x)/(max.y-min.y))<Mathf.Tan((m_SingleDirOffAngle/180.0f)*Mathf.PI))
			{
				if(points[0].y<points[points.Count-1].y)
				{
					return "Up";
				}
				else
				{
					return "Down";
				}
			}
			else
			{
				return "Unknow";
			}
		}
		else if((max.y-min.y) < (max.x - min.x))
		{
			if(((max.y-min.y)/(max.x-min.x))<Mathf.Tan((15.0f/180.0f)*Mathf.PI))
			{
				if(points[0].x<points[points.Count-1].x)
				{
					return "Right";
				}
				else
				{
					return "Left";
				}
			}
			else
			{
				return "Unknow";
			}
		}
		else
		{
			return "Unknow";
		}
		
	}
	
	private void ClearPoints()
	{
		if(FuncNoHasPoints!=null)
		{
			FuncNoHasPoints();
		}
		m_Point2dList.Clear();
	}
	
	private void OnPick()
	{
	}
	
	public void SetGestureEnable(bool enable,bool issingledir = false)
	{
		if(!enable)
		{
			ClearPoints();
		}
		else
		{
			m_IsSingleDir = issingledir;
		}
		m_IsEnabled = enable;
	}
	
	private bool IsCanGetPos()
	{
		if(!m_IsCanDoCatchGesture)
		{
			return false;
		}
		if((Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer) && Input.GetMouseButton(0))
		{
			return true;
		}
		else if(Input.touchCount==1 && ( m_IsHold||m_IsPress ||m_IsUpPress) && ( Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
		{
			return true;
		}
		return false;
	}
	
	private Vector3 GetTouchPoint()
	{
		if(Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
		{
			return Input.mousePosition;
		}
		else if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Touch input = Input.GetTouch(0);
				
			Debug.Log("==========> GetTouchPoint: input.position:  "+input.position);
			return input.position;
		}
		else
		{
			return Vector3.zero;
		}
	}
	
	private void CheckPressState()
	{
		m_IsPress = false;
		
		m_IsUpPress = false;
		
		m_IsHold = false;
		
		if(Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
		{
			m_IsPress = Input.GetMouseButtonDown(0);
			
			m_IsUpPress = Input.GetMouseButtonUp(0);
			
			m_IsHold = Input.GetMouseButton(0);
		}
		else if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			
			if(Input.touchCount!=1)
			{
				return;
			}
			Touch input = Input.GetTouch(0);
			
			if(input.phase == TouchPhase.Began)
			{
				Debug.Log("=======> input.phase == TouchPhase.Began");
				m_IsPress = true;
			}
			if(input.phase == TouchPhase.Moved)
			{
				Debug.Log("=======> input.phase == TouchPhase.Moved");
				m_IsHold = true;
			}
			if(input.phase == TouchPhase.Ended)
			{
				Debug.Log("=======> input.phase == TouchPhase.Ended");
				m_IsUpPress = true;
			}
		}
	}
	
	private bool IsOnUI()
	{
		NvGameUISceneManager mgr = Singlton.getInstance("NvGameUISceneManager") as NvGameUISceneManager;
		
		if(mgr.IsOverUI())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void SetSingleDirOffAngle(float angle)
	{
		m_SingleDirOffAngle = angle;
	}
}

