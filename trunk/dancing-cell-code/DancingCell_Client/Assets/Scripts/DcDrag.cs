using UnityEngine;
using System.Collections;

public class DcDrag : MonoBehaviour
{
    public Vector3 m_GestureDir = Vector3.one;

    public GameObject m_TargetObj = null;

    public BoxCollider m_BoxCollider = null;

    public float m_Precision = 20.0f;

    public string FuncDrop = "";

    private bool m_IsPress = false; // is just press mouth or touch

    private bool m_IsUpPress = false;

    private bool m_IsHold = false;

    private Vector3 m_StartPos = Vector3.zero;

    private bool m_IsNeedToDisEnableBox = false;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        m_BoxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsNeedToDisEnableBox)
        {
            m_IsNeedToDisEnableBox = false;

            m_BoxCollider.enabled = false;
        }
        CheckPressState();

        if (m_IsPress)
        {
            _OnPick();
        }
        if (m_IsHold)
        {
            _OnHold();
        }
        if (m_IsUpPress)
        {
            _OnDrop();
        }
    }

    private void _OnPick()
    {
        m_StartPos = GetMousePos();

        //Debug.LogWarning("StartPos:  " + m_StartPos);
    }

    private void _OnHold()
    {
        Vector3 curpos = GetMousePos();

        if ((curpos - m_StartPos).magnitude > m_Precision && !m_IsUpPress)
        {
            m_BoxCollider.enabled = true;
        }
    }

    private void _OnDrop()
    {
        Vector3 dir = GetMoveDir();

        //Debug.LogWarning("dir: " + dir + "m_TargetObj: " + m_TargetObj.name);

        if (dir != Vector3.zero && m_TargetObj != null)
        {
            //Debug.LogWarning(" m_TargetObj.SendMessage(FuncDrop, dir " + " FuncDrop: " + FuncDrop);

            m_TargetObj.SendMessage(FuncDrop, dir);
        }
        Vector3 curpos = GetMousePos();

        //Debug.LogWarning("dir: " + dir + "m_TargetObj: " + m_TargetObj.name + " ========> curpos: " + curpos + "  m_StartPos: " + m_StartPos + "  dis: " + (curpos - m_StartPos).magnitude);

        if (m_BoxCollider.enabled == true && (curpos - m_StartPos).magnitude > m_Precision)
        {
            //Debug.LogWarning(" here 1");
            m_BoxCollider.enabled = true;

            m_IsNeedToDisEnableBox = true;
        }
        else
        {
            //Debug.LogWarning(" here 2");
            m_BoxCollider.enabled = false;
        }
        
    }

    private void CheckPressState()
    {
        m_IsPress = false;

        m_IsUpPress = false;

        m_IsHold = false;

        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            m_IsPress = Input.GetMouseButtonDown(0);

            m_IsUpPress = Input.GetMouseButtonUp(0);

            m_IsHold = Input.GetMouseButton(0);
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {

            if (Input.touchCount != 1)
            {
                return;
            }
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Began)
            {
                //Debug.Log("=======> input.phase == TouchPhase.Began");
                m_IsPress = true;
            }
            if (input.phase == TouchPhase.Moved)
            {
                //Debug.Log("=======> input.phase == TouchPhase.Moved");
                m_IsHold = true;
            }
            if (input.phase == TouchPhase.Ended)
            {
                //Debug.Log("=======> input.phase == TouchPhase.Ended");
                m_IsUpPress = true;
            }
        }
    }


    private Vector3 GetMoveDir()
    {
        Vector3 curpos = GetMousePos();

        //Debug.LogWarning("StartPos:  " + m_StartPos + " curpos: " + curpos);

        if ((curpos - m_StartPos).magnitude < m_Precision)
        {
            m_StartPos = Vector3.zero;

            return Vector3.zero;
        }
        Vector3 dir = curpos - m_StartPos;

        dir.Normalize();

        Vector3 ret = new Vector3(dir.x * m_GestureDir.x, dir.y * m_GestureDir.y, dir.z * m_GestureDir.z);

        //Debug.LogWarning("GetMoveDir:  " + ret);

        return ret;
    }


    private Vector3 GetMousePos()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            return new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Touch input = Input.GetTouch(0);

            //Debug.Log("==========> GetTouchPoint: input.position:  " + input.position);
            return new Vector3(input.position.x, input.position.y, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
