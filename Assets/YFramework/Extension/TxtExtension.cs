/****************************************************
    文件：TxtExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace YFramework.Extension
{
    public static class TxtExtension
    {
        public static void StartTypewrite(this Text text, float interval = 0.1f ,Action onFinish = null)
        {
            var typewriter = text.gameObject.AddComponent<TypewriterEffect>();
            typewriter.InitText(text,interval);
            typewriter.StartWrite();
            if(!typewriter.Active) onFinish?.Invoke();
        }
        
        public static bool IsEmail(this string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            bool isValid = Regex.IsMatch(email, pattern);
            return isValid;
        }
    }

    public class TypewriterEffect : MonoBehaviour
    {
        private enum TextType
        {
            UIText,
            TextMesh,
        }

        private string _word;

        public string Word
        {
            get => _word;
        }

        private float _timer;
        private int _curWordIndex;
        private float _writeInterval = 0.1f;
        private string _originalWord;
        private bool _init;
        //是否正在打字
        public bool Active { get; private set; }


        private TextType _type;
        private Text _uiText;
        private TextMesh _textMesh;


        private void Update()
        {
            if (Active)
            {
                SetWords();
            }
        }

        public void InitWords(string word)
        {
            this._word = word;
        }

        public void StartWrite()
        {
            if (!_init)
            {
                Debug.LogWarning("启动打字前，请先使用SetText设置一下参数");
                return;
            }
            switch (_type)
            {
                case TextType.UIText:
                    _uiText.text = "";
                    break;
                case TextType.TextMesh:
                    _textMesh.text = "";
                    break;
            }

            Active = true;
        }
        public void InitText(Text text,float interval)
        {
            this._uiText = text;
            this._originalWord = text.text;
            this._writeInterval = interval;
            this._type = TextType.UIText;
            this._init = true;
        }
        public void InitText(TextMesh text,float interval)
        {
            this._textMesh = text;
            this._originalWord = text.text;
            this._writeInterval = interval;
            this._type = TextType.TextMesh;
            this._init = true;
        }
        

        private void SetWords()
        {
            _timer += Time.deltaTime;
            if (_timer >= _writeInterval)
            {
                _curWordIndex++;
                if (_originalWord.Length > 0 && _curWordIndex <= _originalWord.Length -1)
                {
                    switch (_type)
                    {
                        case TextType.UIText:
                            _uiText.text = _originalWord.Substring(0, _curWordIndex);
                            break;
                        case TextType.TextMesh:
                            _textMesh.text = _originalWord.Substring(0, _curWordIndex);
                            break;
                    }
                   
                }

                if (_curWordIndex >= _originalWord.Length)
                {
                    Active = false;
                    _curWordIndex = 0;
                    _timer = 0;
                    switch (_type)
                    {
                        case TextType.UIText:
                            _uiText.text = _originalWord;
                            break;
                        case TextType.TextMesh:
                            _textMesh.text = _originalWord;
                            break;
                    }
                }
                
                _timer = 0;
            }
        }
    }
}