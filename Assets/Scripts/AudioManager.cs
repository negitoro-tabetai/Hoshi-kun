using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    const string BGMPath = "Audio/BGM";
    const string SEPath = "Audio/SE";

    const string BGMVolumeKey = "BGMVolume";
    const string SEVolumeKey = "SEVolume";

    const float BGMVolumeDefault = 1.0f;
    const float SEVolumeDefault = 1.0f;


    public float BGMVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(BGMVolumeKey, BGMVolumeDefault);
        }
    }
    public float SEVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(SEVolumeKey, SEVolumeDefault);
        }
    }

    //BGM用、SE用のオーディオソース
    [SerializeField] AudioSource _BGMSource, _SESource;
    //全Audioを保持
    Dictionary<string, AudioClip> _BGMDictionaly, _SEDictionaly;

    new void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);

        _BGMDictionaly = new Dictionary<string, AudioClip>();
        _SEDictionaly = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll(BGMPath);
        object[] seList = Resources.LoadAll(SEPath);


        foreach (AudioClip bgm in bgmList)
        {
            _BGMDictionaly[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            _SEDictionaly[se.name] = se;
        }

    }
    void Start()
    {
        _BGMSource.volume = BGMVolume;
        _SESource.volume = SEVolume;
        _BGMSource.volume = 0.3f;
        _SESource.volume = 0.3f;
    }

    /// <summary>
    /// 指定したファイル名のSEを流す。
    /// </summary>
    public void PlaySE(string seName)
    {
        if (!_SEDictionaly.ContainsKey(seName))
        {
            Debug.Log(seName + "という名前のSEがありません");
            return;

        }
        _SESource.PlayOneShot(_SEDictionaly[seName] as AudioClip);
    }

    /// <summary>
    /// 指定したファイル名のBGMを流す。
    /// </summary>
    public void PlayBGM(string bgmName)
    {
        if (!_BGMDictionaly.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "という名前のBGMがありません");
            return;
        }
        if (_BGMSource.clip != _BGMDictionaly[bgmName] as AudioClip)
        {
            _BGMSource.clip = _BGMDictionaly[bgmName] as AudioClip;
            _BGMSource.Play();
        }
    }
    /// <summary>
    /// BGMを止める
    /// </summary>
    public void StopBGM()
    {
        _BGMSource.Stop();
    }

    /// <summary>
	/// BGMとSEのボリュームを別々に変更&保存
	/// </summary>
	public void ChangeVolumeSE(float SEVolume)
    {
        _SESource.volume = SEVolume;
        PlayerPrefs.SetFloat(SEVolumeKey, SEVolume);
    }
    public void ChangeVolumeBGM(float BGMVolume)
    {
        _BGMSource.volume = BGMVolume;
        PlayerPrefs.SetFloat(BGMVolumeKey, BGMVolume);
    }
}

