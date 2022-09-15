/****************************************************
    文件：IController.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace YFramework
{
    public interface IController :ICanGetModel,ICanSendCommand,ICanRegisterEvent,ICanGetSystem
    {
       
    }

    // public abstract class AbstractController : IController ,ICanGetModel,ICanGetSystem , ICanSendCommand
    // {
    //     private IArchitecture _architecture; 
    //     IArchitecture IBelongToArchitecture.GetArchitecture()
    //     {
    //         return _architecture;
    //     }
    // }
}