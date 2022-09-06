/****************************************************
    文件：CounterViewController.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using YFramework;
using YFramework.Examples;
using UnityEngine;
using UnityEngine.UI;

namespace CounterApp
{
    public class CounterViewController : MonoBehaviour ,IController
    {
        private ICounterModel _counterModel;

        private void Start()
        {
            _counterModel = this.GetModel<ICounterModel>();

            _counterModel.Count.RegisterOnValueChange(OnCounterChange);
            OnCounterChange(0);
            transform.Find("BtnAdd").GetComponent<Button>().onClick.AddListener(()=>
            {
               this.SendCommand<AddCountCommand>();
            });
            transform.Find("BtnSub").GetComponent<Button>().onClick.AddListener(()=>
            {
                this.SendCommand(new SubCounterCommand());
            });
            
            
        }

        private void OnCounterChange(int newCount)
        {
            transform.Find("CountText").GetComponent<Text>().text = newCount.ToString();
        }

        private void OnDestroy()
        {
            _counterModel.Count.UnRegisterOnValueChange(OnCounterChange);
        }


        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return CounterApp.Interface;
        }
    }

    public interface ICounterModel : IModel
    {
       
    }
    public class CounterModel : AbstractModel,ICounterModel
    {
        public  BindableProperty<int> Count { get; } = new BindableProperty<int>()
        {
            Value = 0
        };
        

        protected override void OnInit()
        {
           
        }
        
    }
    
}