
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
/// <summary>
/// 目前这脚本控制视频播放为最新版。VideoManager废弃了
/// </summary>
public class VideoTest : MonoBehaviour
{
    #region 参数
    //定义参数获取video组件和RawImage组件
    public VideoPlayer videoPlayer;
    //public RawImage rawImage;

    public GameObject bgGo;
    public GameObject returnBtnGo;
    public string[] movieUrl;
    private int _playIndex;
    private int PlayClipNum = 100;//最大可播放的音频数

    private float _tempTime;//用于计算显示返回按钮的时间
    private bool _isTempTime;
    private float _timeVal;//计算器判断是否播放最后一个完成
    private bool _isPlay = false;
    public bool romoPlay;//远程控制播放
    public bool romoReturn;//远程控制返回
    public bool romoNext;//远程控制下个播放
    public bool IsPlay
    {
        get
        {
            return _isPlay;
        }
        set
        {
            _isPlay = value;
        }
    }
    //声音播放组件
    //private AudioSource thisAudioSource;
    //是否静音
    private bool isMute = false;

    #endregion
    private void Start()
    {
        BeginString();
    }

    private void Update()
    {
        PlayMovie();
        if (Input.GetMouseButtonDown(0) && IsPlay)
        {
            _tempTime = 0;
            _isTempTime = true;
            returnBtnGo.SetActive(true);
        }
        //if (isPlay)
        //{
        //    //BeginPlay();
        //    ToPlayThis0();
        //}
        if (romoPlay)//远程操控播放的方法
        {
            ToPlayThis0();
            romoPlay = false;
        }

        if (romoNext&&videoPlayer.isPlaying)
        {
            ToNextPlay();
            romoNext = false;
        }
        if (romoReturn)
        {
            StopPlay();
            romoReturn = false;
        }
        if (_isTempTime)
        {
            _tempTime += Time.deltaTime;
            if (_tempTime > 2.5f)
            {
                returnBtnGo.transform.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1f).
                    OnComplete(() =>
                    {
                        returnBtnGo.transform.GetComponent<Image>().color = Color.white;
                        returnBtnGo.SetActive(false);
                    });
                _isTempTime = false;
            }
        }
    }
    
    #region 方法
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void BeginString()
    {
        if (IsPlay)
        {
            return;
        }

        _playIndex = 0;
        movieUrl = new string[PlayClipNum];
        for (int i = 0; i < movieUrl.Length; i++)
        {
            //movieUrl[0] = Application.streamingAssetsPath + "/Video/1.mp4";
            //movieUrl[1] = Application.streamingAssetsPath + "/Video/2.mp4";
            movieUrl[i] = Application.streamingAssetsPath + "/Video/" + (i + 1).ToString() + ".mp4";
        }
        _isPlay = false;
    }

    /// <summary>
    /// 播放视频函数
    /// </summary>
    void PlayMovie()
    {
        //如果videoPlayer没有对应的视频texture，则返回
        if (videoPlayer.texture == null)
        {
            //print("没有对应的texture");
            return;
        }
        //print("IsPlay:"+IsPlay);
        //把VideoPlay的视频渲染到UGUI的RawImage上
        //rawImage.texture = videoPlayer.texture;
        //if (isPlay)
        //{
        //    timeVal += Time.deltaTime;
        //    //print("长度为:"+timeVal);
        //    if (timeVal>= videoPlayer.length)
        //    {
        //        Debug.Log("视频播放完毕动作！");
        //        StopPlay();
        //        timeVal = 0;
        //    }
        //}
        if (_isPlay)
        {
            //print("Frame:"+videoPlayer.frame);
            //print("FrameCount:"+videoPlayer.frameCount);
            if (videoPlayer.frame == (long)videoPlayer.frameCount-25)
            {
                Debug.Log("视频播放结束");
                ToNextPlay();
            }
        }
    }

    /// <summary>
    /// 播放视频0逻辑
    /// </summary>
    public void ToPlayThis0()
    {
        videoPlayer.url = movieUrl[_playIndex];
        BeginPlay();
    }

    public void ToNextPlay()
    {
        if (videoPlayer.isPlaying)
        {
            StopPlay();
        }

        if (_playIndex >=movieUrl.Length-1)
        {
            _playIndex = 0;
        }
        else
        {
            _playIndex++;
        }
        if (!File.Exists(movieUrl[_playIndex]))
        {
            print("not find");
            _playIndex = 0;
        }
        videoPlayer.url = movieUrl[_playIndex];
        BeginPlay();
    }
    /// <summary>
    /// 播放视频0的协程
    /// </summary>
    /// <returns></returns>
    //void ToPlay0()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    BeginPlay();
    //}

    /// <summary>
    /// 开始播放视频
    /// </summary>
    private void BeginPlay()
    {
        if (!IsPlay)
        {
            bgGo.SetActive(true);
            videoPlayer.Play();
            _isPlay = true;
        }
    }

    /// <summary>
    /// 停止播放视频
    /// </summary>
    public void StopPlay()
    {
            videoPlayer.Stop();
            bgGo.SetActive(false);
            _isPlay = false;
    }

    /// <summary>
    /// 暂停播放视频逻辑
    /// </summary>
    public void PauseThis()
    {
        if (_isPlay)
        {
            videoPlayer.Pause();
            _isPlay = false;
        }
        else
        {
            videoPlayer.Play();
            _isPlay = true;
        }
    }

    ///// <summary>
    ///// 增加音量逻辑
    ///// </summary>
    //public void AddVolume()
    //{
    //    thisAudioSource.volume += 0.1f;
    //}

    ///// <summary>
    ///// 减小音量逻辑
    ///// </summary>
    //public void DecreaseVolume()
    //{
    //    thisAudioSource.volume -= 0.1f;
    //}
    ///// <summary>
    ///// 静音功能
    ///// </summary>
    //public void MuteVolume()
    //{
    //    if (!isMute)
    //    {
    //        thisAudioSource.mute = true;
    //        isMute = true;
    //    }
    //    else
    //    {
    //        thisAudioSource.mute = false;
    //        isMute = false;
    //    }
    //}
        #endregion

}
