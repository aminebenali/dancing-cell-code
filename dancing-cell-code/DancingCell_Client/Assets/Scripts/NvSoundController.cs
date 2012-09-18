using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;




public class NvSoundController : MonoBehaviour
{	
	public List<AudioSource>   m_TrackBgm = new List<AudioSource>();
	public List<AudioSource>   m_TrackSEList = new List<AudioSource> (); 
	public List<AudioSource>   m_FixTrackSEList = new List<AudioSource> ();
		
	private bool m_IsInit = false;
	public int m_ActiveBgmTrack = 0;
	private int m_CurrentVol = 100;
	private int m_CurrentBgmVol = 100;
	private int m_CurrentSeVol = 100;
	private string m_CurrentBgmName = "";
	private float m_FadeInTime = 0;
	private bool m_StoppingFadeFlg = false;
	
	public bool m_CrossFade = false;
	public AudioSource m_LastActiveBgmTrack;
	
	public bool m_isMute = false;
	
	public enum E_SND_STAT
	{
		STAT_SE_FADE_BGM = 0,
		
		STAT_NULL
	}
	
	private E_SND_STAT m_Status = E_SND_STAT.STAT_NULL;
	private int m_FadeBgmSeTrk = 0;
	
	
	public bool IsInit ()
	{
		return m_IsInit;
	}
	
	// Use this for initialization
	void Awake ()
	{
		for (int i=0; i<NvConst.kMaxSeTrack; i++) 
		{
			AudioSource audiosrc = gameObject.AddComponent<AudioSource> ();
			audiosrc.volume = (float)m_CurrentSeVol / 100.0f;
			audiosrc.loop = false;
			audiosrc.playOnAwake = false;
			m_TrackSEList.Add (audiosrc);
		}
		
		for ( int i = 0; i < NvConst.kMaxFixedSeTrack; ++i)
		{
			AudioSource audiosrc = gameObject.AddComponent<AudioSource> ();
			audiosrc.volume = (float)m_CurrentSeVol / 100.0f;
			audiosrc.loop = false;
			audiosrc.playOnAwake = false;
			m_FixTrackSEList.Add ( audiosrc );
		}
		
		for (int i = 0; i<2; ++i)
		{
			GameObject o = new GameObject("BgmTrack"+i);
			o.transform.parent = transform;
			AudioSource src = o.AddComponent(typeof(AudioSource)) as AudioSource;
			src.loop = true;
			src.playOnAwake = false;
			m_TrackBgm.Add(src);
		}
		
		if (PlayerPrefs.HasKey("NV_BGM_VOL"))
		{
			m_CurrentBgmVol = PlayerPrefs.GetInt("NV_BGM_VOL");
		}
		
		if (PlayerPrefs.HasKey("NV_SE_VOL"))
		{
			m_CurrentSeVol = PlayerPrefs.GetInt("NV_SE_VOL");
		}
		
		if (PlayerPrefs.HasKey("NV_MUTE"))
		{
			m_isMute = ( PlayerPrefs.GetInt("NV_MUTE") == 1 )? true: false;
		}
		
		SetMute(m_isMute);
		m_IsInit = true;
	}
	
	void SetMute( bool b )
	{
		m_isMute = b;
		int val = (( b == true ) ?1 : 0) ;
		PlayerPrefs.SetInt(  "NV_MUTE",  val ) ;	
		
		foreach (AudioSource src in m_TrackBgm )
		{
			src.mute = m_isMute;
		}
		foreach (AudioSource src in m_TrackSEList )
		{
			src.mute = m_isMute;
		}
		foreach(AudioSource src in m_FixTrackSEList )
		{
			src.mute = m_isMute;
		}
	}
	
	void SetVol (int vol)
	{
		m_CurrentVol = vol;
		m_CurrentBgmVol = vol;
		m_CurrentSeVol = vol;		
		
		PlayerPrefs.SetInt("NV_BGM_VOL", m_CurrentBgmVol);
		PlayerPrefs.SetInt("NV_SE_VOL", m_CurrentSeVol );
	}
	
	void SetBgmVol (int vol)
	{
		m_CurrentBgmVol = vol;
	}
	
	void SetSeVol (int vol)
	{
		m_CurrentSeVol = vol;
	}
	
	float GetPlayTime (AudioSource sur)
	{		
		return sur.time;
	}
	
	public float GetBgmPlayTime ()
	{
		return GetPlayTime ( m_TrackBgm[m_ActiveBgmTrack] );
	}
	
	public float GetSePlayeTime (string seName)
	{
		float time = 0;
		foreach (AudioSource src in m_TrackSEList)
		{
			if (src.clip.name.Equals (seName))
			{
				time = src.time;
				break;
			}
		}		
		return time;
	}
	
	public bool IsBgmPlaying ()
	{
		return m_TrackBgm[m_ActiveBgmTrack].isPlaying;
	}
	
	public bool IsBgmLooping ()
	{
		return m_TrackBgm[m_ActiveBgmTrack].loop;
	}
	
	public bool IsSeLooping (string seName)
	{
		bool loop = false;
		
		foreach (AudioSource src in m_TrackSEList) 
		{
			if (src.clip.name.Equals (seName)) 
			{
				loop = src.loop;
				break;
			}
		}
		return loop;		
	}
	
	public int GetVol ()
	{
		return m_CurrentVol;
	}
	
	public int GetBgmVol ()
	{
		return m_CurrentBgmVol;
	}
	
	public int GetSeVol ()
	{
		return m_CurrentSeVol;
	}
	
	public string GetCurrentBgmName()
	{
		return m_CurrentBgmName;
	}
	
	public void PlayBgm(string bgmName, float fadeInTime = 0.2f, float fadeOutTime = 0.5f)
	{		
		//m_TrackBgm[0]
		if ( bgmName.Equals(m_CurrentBgmName))
		{
			if ( !m_StoppingFadeFlg  && m_TrackBgm[m_ActiveBgmTrack].isPlaying)
			{
				return;
			}
			else
			{
				PlayBgmClip( fadeInTime );
				return;
			}
		}

		if ( m_TrackBgm[m_ActiveBgmTrack].clip == null )
		{
			PlayBgmClip(bgmName, fadeInTime);
		}
		else
		{
			if ( m_TrackBgm[m_ActiveBgmTrack].isPlaying )
			{
				m_FadeInTime = fadeInTime;
				m_CurrentBgmName = bgmName;
				TweenVolume tv = TweenVolume.Begin( m_TrackBgm[m_ActiveBgmTrack].gameObject, fadeOutTime, 0 );
				tv.eventReceiver = gameObject;	
				tv.callWhenFinished = "LastBgmFadeEnd";
				m_LastActiveBgmTrack = m_TrackBgm[m_ActiveBgmTrack];

				if ( m_CrossFade )
				{
					//m_TrackBgm[m_ActiveBgmTrack].Stop();
					m_ActiveBgmTrack = (m_ActiveBgmTrack + 1)%2;
					PlayBgmClip(m_CurrentBgmName, fadeInTime);
				}
			}
			else
			{
				PlayBgmClip(bgmName, fadeInTime);			
			}
		}		
	}
	
	public void PlayBgMusic(string bgmName)
	{
		StopBgmImmediately();
		PlayBgm(bgmName, 0, 0);
	}
	
	private void PlayBgmClip( float fadeInTime )
	{
		m_ActiveBgmTrack = (m_ActiveBgmTrack+1)%2;
//		Debug.Log("BGM is "+ m_CurrentBgmName);
//		Debug.Log("play bgm now Active channel is " + m_ActiveBgmTrack );
		m_TrackBgm[m_ActiveBgmTrack].enabled = true;
		
		if (m_TrackBgm[m_ActiveBgmTrack].clip == null || !m_TrackBgm[m_ActiveBgmTrack].clip.name.Equals(m_CurrentBgmName))
		{
			//Debug.Log("reload!!!!!");
			AudioClip clip = Resources.Load("Sound/Bgm/"+m_CurrentBgmName) as AudioClip;
			m_TrackBgm[m_ActiveBgmTrack].clip = clip;
		}
		m_TrackBgm[m_ActiveBgmTrack].volume = 0.011f;
		m_TrackBgm[m_ActiveBgmTrack].Play();
		TweenVolume tv = TweenVolume.Begin( m_TrackBgm[m_ActiveBgmTrack].gameObject, fadeInTime, (float)m_CurrentBgmVol/100.0f );	
		tv.eventReceiver = gameObject;
		tv.callWhenFinished = "PlayBgmFadeEnd";
	}
	
	private void PlayBgmClip(string bgmName, float fadeInTime)
	{
		m_ActiveBgmTrack = (m_ActiveBgmTrack + 1)%2;
///		Debug.Log("play bgm AAAAAAAAAAA now Active channel is " + m_ActiveBgmTrack );
		AudioClip clip = Resources.Load("Sound/Bgm/"+bgmName) as AudioClip;
		m_CurrentBgmName = bgmName;
		m_TrackBgm[m_ActiveBgmTrack].enabled = true;
		m_TrackBgm[m_ActiveBgmTrack].clip = clip;
		m_TrackBgm[m_ActiveBgmTrack].volume = 0.011f;
		m_TrackBgm[m_ActiveBgmTrack].Play();
		TweenVolume tv = TweenVolume.Begin( m_TrackBgm[m_ActiveBgmTrack].gameObject, fadeInTime, (float)m_CurrentBgmVol/100.0f );
		tv.eventReceiver = gameObject;
		tv.callWhenFinished = "PlayBgmFadeEnd";
	}
	
	void PlayBgmFadeEnd( UITweener uit )
	{
//		Debug.Log("play bgm fade end");
		GameObject.Destroy(uit);
	}
	
	void LastBgmFadeEnd( UITweener uit )
	{		
		GameObject.Destroy(uit);
		m_LastActiveBgmTrack.enabled = true;
		m_LastActiveBgmTrack.Stop();
		m_LastActiveBgmTrack.volume = (float)m_CurrentVol/100.0f;
		if ( !m_CrossFade )
		{			
			PlayBgmClip(m_CurrentBgmName, m_FadeInTime );
		}
	}
	
	public void PlaySe( string seName , bool loop = false )
	{
		AudioClip clip = Resources.Load("Sound/Se/"+seName) as AudioClip;
		int track = ChooseFreeSeTrack();
		m_TrackSEList[track].clip = clip;
		m_TrackSEList[track].loop = loop;
		m_TrackSEList[track].Play();
	}
	
	public void PlaySeAndFadeBgm( string seName )
	{
		AudioClip clip = Resources.Load("Sound/Se/"+seName) as AudioClip;
		
		int track = ChooseFreeSeTrack();
		m_TrackSEList[track].clip = clip;
		m_TrackSEList[track].loop = false;
		m_TrackSEList[track].Play();
		
		m_FadeBgmSeTrk = track;
		
		if ( clip.length < 0.5f )
		{
			//no fade bgm, cause se is too short
			if ( IsBgmPlaying() )
			{
				m_TrackBgm[m_ActiveBgmTrack].volume = (float)( m_CurrentBgmVol / 300.0f );
			}
		}
		else
		{
			if ( IsBgmPlaying() )
			{
				TweenVolume.Begin( m_TrackBgm[m_ActiveBgmTrack].gameObject, 0.2f, (float)m_CurrentBgmVol/300.0f );
			}
		}		
		m_Status = E_SND_STAT.STAT_SE_FADE_BGM;
	}
	
	public void PlaySe(int ch, string seName, bool loop = false)
	{
		if ( ch >= NvConst.kMaxFixedSeTrack || ch < 0)
		{
			Debug.LogError("CHANNEL OVERFLOW");
		}
		
		AudioClip clip = Resources.Load("Sound/Se/"+seName ) as AudioClip;
		if (m_FixTrackSEList[ch].isPlaying)
		{
			m_FixTrackSEList[ch].Stop();
		}
		m_FixTrackSEList[ch].clip = clip;
		m_FixTrackSEList[ch].loop = loop;
		m_FixTrackSEList[ch].Play();
	}

	public void StopBgm( float fadeOutTime = 0.5f )
	{
		if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
		{
			m_StoppingFadeFlg = true;
			TweenVolume tv = TweenVolume.Begin( m_TrackBgm[m_ActiveBgmTrack].gameObject, fadeOutTime, 0 );
			tv.eventReceiver = gameObject;				
			tv.callWhenFinished = "StopBgmFadeEnd";
			m_LastActiveBgmTrack = m_TrackBgm[m_ActiveBgmTrack];
			//Debug.Log("Set Last Active channel is " + m_ActiveBgmTrack );
		}
	}
	
	public void StopBgmImmediately()
	{
		if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
		{
			m_TrackBgm[m_ActiveBgmTrack].Stop();
		}		
	}
	
	public void StopSe(string seName)
	{
		foreach(AudioSource src in m_TrackSEList )
		{
			if (src.clip.name.Equals(seName))
			{
				if(src.isPlaying)
				{
					src.Stop();
				}
			}
		}
		
		foreach(AudioSource src in m_FixTrackSEList )
		{
			if (src.clip.name.Equals(seName))
			{
				if(src.isPlaying)
				{
					src.Stop();
				}
			}
		}
	}
	
	public void StopAllSe()
	{		
		foreach(AudioSource src in m_TrackSEList )
		{
			if(src.isPlaying)
			{
				src.Stop();
			}			
		}		
		foreach(AudioSource src in m_FixTrackSEList )
		{
			if(src.isPlaying)
			{
				src.Stop();
			}			
		}		
	}
	
	public void StopAllSnd()
	{
		StopBgm();
		StopAllSe();
	}
	
	public void StopAllSndImmediately()
	{
		StopBgmImmediately();
		StopAllSe();
	}
	
	void StopBgmFadeEnd(UITweener uit)
	{	
		//Debug.Log("Call back is here !! "+m_LastActiveBgmTrack.clip.name);
		//Debug.Log("Active channel is " + m_ActiveBgmTrack );
		m_StoppingFadeFlg = false;
		m_LastActiveBgmTrack.enabled = true;
		m_LastActiveBgmTrack.volume = (float)m_CurrentBgmVol/100.0f;
		m_LastActiveBgmTrack.Stop();
		GameObject.Destroy(uit);
	}
		
	// Update is called once per frame
	void Update ()
	{
		switch(m_Status)
		{
		case E_SND_STAT.STAT_NULL:
			break;
		case E_SND_STAT.STAT_SE_FADE_BGM:
			if ( !m_TrackSEList[m_FadeBgmSeTrk].isPlaying )
			{
				m_Status = E_SND_STAT.STAT_NULL;
				TweenVolume.Begin( m_TrackBgm[m_ActiveBgmTrack].gameObject, 0.2f, (float)m_CurrentBgmVol/100.0f );
				m_FadeBgmSeTrk = 0;
			}
			break;
		}
		
	}
	
	private int  ChooseFreeSeTrack ()
	{
		for(int i=0;i< m_TrackSEList.Count; i++)
		{
			if(!m_TrackSEList[i].isPlaying)
			{
				return i;
			}
		}
		int val = Random.Range(0, NvConst.kMaxSeTrack);
		Debug.Log("RANDOM Track is " + val);
		return val;
	}
	
	public int GetFreetSeTrack()
	{
		int cnt = 0;
		foreach(AudioSource src in m_TrackSEList)
		{
			if (!src.isPlaying)
			{
				cnt++;
			}
		}
		return cnt;
	}
	
	public float GetRealBGMVol()
	{
		return m_TrackBgm[m_ActiveBgmTrack].volume;
	}

}
