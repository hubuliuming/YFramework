/****************************************************
    文件：AudioManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/17 14:25:39
    功能：mono类管理
*****************************************************/

using UnityEngine;

namespace YFramework.Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private static AudioSource mSource;
        private  AudioSource mBGMSource;
        
        public void PlaySound(string clipName)
        {
            var clip = Resources.Load<AudioClip>(clipName);
            mSource.clip = clip;
            mSource.Play();
        }
        public void PlayBGM(string clipName,bool loop = true)
        {
            if (mBGMSource == null)
            {
                mBGMSource = gameObject.AddComponent<AudioSource>();
            }
            var clip = Resources.Load<AudioClip>(clipName);
            mBGMSource.clip = clip;
            mBGMSource.Play();
            mBGMSource.loop = loop;
        }

        public void StopAll()
        {
            SoundStop();
            BGMStop();
        }
        public void MuteAll()
        {
            SoundMute();
            BGMMute();
        }
        
        public void SoundPause()
        {
            mSource.Pause();
        }
        public void SoundStop()
        {
            mSource.Stop();
        }
        public void SoundMute()
        {
            mSource.mute = true;
        }

        public void BGMPause()
        {
            mBGMSource.Pause();
        }
        public void BGMStop()
        {
            mBGMSource.Stop();
        }
        public void BGMMute()
        {
            mBGMSource.mute = true;
        }
    }
}
