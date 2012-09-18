using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvUIBase : MonoBehaviour {
	
	[SerializeField]
	private bool m_enable = true;
	
	[SerializeField]
	protected int m_depth = 0;
	
	virtual public int depth
	{
		get { return m_depth; }
		set {
		
			if ( m_depth != value )
			{
				m_depth = value;
				Vector3 pos = transform.localPosition;
				pos.z += m_depth * (-0.5f);
				transform.localPosition = pos;
			}
		}
	}
	
	public bool enable
	{
		get { return m_enable; }
	}
	
	[SerializeField]
	private bool m_visible = true;
	
	[SerializeField]
	private bool DisableGameObjectWhenInvisible = true;
	
	//private bool m_preVisible = true;
	//private bool m_preEnable = true;
	
#if false
	[HideInInspector][SerializeField]
	protected NvUIBase elem_parent = null;
	
	[HideInInspector][SerializeField]
	protected List<NvUIBase> elem_children = new List<NvUIBase>();
#endif
	
	public bool visible
	{
		get { return m_visible; }
	}
	
	private void Awake()
	{
		Vector3 pos = transform.localPosition;
		pos.z += m_depth * (-0.5f);
		transform.localPosition = pos;
		
#if false
		Transform tr = transform.parent;
		if ( tr != null )
		{
			elem_parent = tr.GetComponent<NvUIBase>();
		}
		
		for ( int ii = 0; ii < transform.childCount; ++ii )
		{
			tr = transform.GetChild(ii);
			NvUIBase obj = tr.GetComponent<NvUIBase>();
			if ( obj != null )
			{
				elem_children.Add(obj);
			}
		}
#endif	
		AwakeImpl();
	}
	
	virtual protected void AwakeImpl()
	{
		setEnable( m_enable );
		setVisible( m_visible );
	}
	
	public NvUIBase findChildByName( string name )
	{
		List<Transform> stack = new List<Transform>();
		stack.Add( transform );
		
		while ( stack.Count > 0 )
		{
			Transform tr = stack[0];
			stack.RemoveAt( 0 );
			
			int count = tr.GetChildCount();
			for ( int ii = 0; ii < count; ++ii )
			{
				Transform child = tr.GetChild(ii);
				if ( child.name == name )
				{
					return child.GetComponent<NvUIBase>();
				}
				
				if ( child.childCount > 0 )
				{
					stack.Add( child );
				}
			}
		}
		
		return null;
	}
	
	public NvUIBase findChild( string path )
	{
		Transform tr = transform.FindChild( path );
		if ( tr != null )
		{
			return tr.GetComponent<NvUIBase>();
		}
		return null;
	}
	
	public void addChild( NvUIBase child )
	{
		if ( child )
		{
			Transform tr = child.transform;
			tr.parent = transform;
		}
	}
	
	public void removeChildByName( string name )
	{
		NvUIBase child = findChildByName(name);
		
		if ( child != null )
			
		{
			GameObject.Destroy( child.gameObject );
		}
	}
	
	public void removeChild( string path )
	{
		NvUIBase child = findChild(path);
		
		if ( child != null )
		{
			GameObject.Destroy( child.gameObject );
		}
	}
	
	// call update by lazyUpdate in manager instead of Update function called by monobehaviour
	// it can not only make calling sequence in one way, but also can keep running update even the gameobject is disabled.
	public void lazyUpdate()
	{
		if ( m_enable )
		{
			lazyUpdateImpl();
		}
	}

	virtual public void setEnable( bool flag )
	{
		if ( m_enable != flag )
		{
			//m_preEnable = m_enable;
			m_enable = flag;
			
			//gameObject.active = flag;
		}
	}
	
//	virtual public void revertEnable()
//	{
//		bool flag = m_preEnable;
//		m_preEnable = m_enable;
//		m_enable = flag;
//		
//		//gameObject.active = flag;
//	}
	
	virtual public void setVisible( bool flag )
	{
		if ( m_visible != flag )
		{
			//m_preVisible = m_visible;
			m_visible = flag;
			
			if ( DisableGameObjectWhenInvisible )
			{
				// in order to disable visual effect, disable gameObject to make ngui remove merged mesh for that object.
				gameObject.active = flag;
			}
		}
	}
	
//	virtual public void revertVisible()
//	{
//		bool flag = m_preVisible;
//		m_preVisible = m_visible;
//		m_visible = flag;
//		
//		//gameObject.active = flag;
//	}
	
	virtual protected void lazyUpdateImpl() {}
	
}
