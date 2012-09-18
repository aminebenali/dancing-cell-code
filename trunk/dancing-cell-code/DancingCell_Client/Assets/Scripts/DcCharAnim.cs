
using UnityEngine;
using System.Collections;

public enum IdleForWhat
{
    eForUI,
    eForDancing
}

public class DcCharAnim : MonoBehaviour 
{	
	public Animation m_Animation = null;
	
    public bool bIsAutoPlaySpecialIdle = false;

	void Awake()
	{
		m_Animation = gameObject.animation;
	}

    void Start()
    {
        m_Animation["idle01"].wrapMode = WrapMode.Loop;
        m_Animation["idle02"].wrapMode = WrapMode.Loop;
        m_Animation["wudao"].wrapMode = WrapMode.Loop;

        PlayIdle(IdleForWhat.eForDancing);

        if(bIsAutoPlaySpecialIdle)
        {
            PlaySpecialIdle();
        }
    }

	void Update () 
	{
       
	}

    public void PlayIdle(IdleForWhat eFor = IdleForWhat.eForUI)
    {
        m_Animation.Stop();
        switch(eFor)
        {
            case IdleForWhat.eForUI:
                m_Animation.CrossFade("idle02");
                break;
            case IdleForWhat.eForDancing:
                m_Animation.CrossFade("idle01");
                break;
        }
    }

    public void PlaySpecialIdle()
    {
        if(!bIsAutoPlaySpecialIdle)
            return;

        m_Animation.Stop();
        string strAnimName = "specialidle0" + (int)UnityEngine.Random.Range(1, 3);
        m_Animation.CrossFade(strAnimName);
        m_Animation.CrossFadeQueued("idle02", 0.3F, QueueMode.CompleteOthers);

        StartCoroutine(DelayPlaySpecialIdle(20f+UnityEngine.Random.Range(0,10)));
    }

    IEnumerator DelayPlaySpecialIdle(float timevalue)
	{
		yield return new WaitForSeconds(timevalue);
		PlaySpecialIdle();
	}

    public void PlayBackoff()
    {
        m_Animation.Stop();
        m_Animation.CrossFade("Backoff");
        m_Animation.CrossFadeQueued("idle01", 0.3F, QueueMode.CompleteOthers);
    }

    public void PlayCheer()
    {
        m_Animation.Stop();
        m_Animation.CrossFade("cheer");
        m_Animation.CrossFadeQueued("idle01", 0.3F, QueueMode.CompleteOthers);
    }

    public void PlayClaphand()
    {
//        m_Animation.Stop();
//        m_Animation.CrossFade("claphand");
//        m_Animation.CrossFadeQueued("idle01", 0.3F, QueueMode.CompleteOthers);
    }

    public void PlayDancing()
    {
        //string animname = m_Animation.clip.name;
        m_Animation.Stop();
        m_Animation.CrossFade("wudao");
    }
}