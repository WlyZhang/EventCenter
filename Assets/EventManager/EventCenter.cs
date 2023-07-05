///--------------------------------------------------------
///------------全局事件处理中心--------------------------------------------
///--------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EventCenter  
{
    //成员变量
    static private EventDispatcher _dispacher = new EventDispatcher();
    /// <summary>
    /// 初始化
    /// </summary>
    static public void Init() { }
    /// <summary>
    /// 销毁
    /// </summary>
    static public void Destroy()
    {
        _dispacher.Clear();
    }
    /// <summary>
    /// 注册事件监听器
    /// </summary>
    /// <param name="evtid"></param>
    /// <param name="listener"></param>
    static public void AddEventListener(int evtid, EventCallBack listener)
    {
        _dispacher.RegisterEventListener(evtid, listener);
    }
    /// <summary>
    ///反 注册事件监听器
    /// </summary>
    static public void RemoveEventListener(int evtid, EventCallBack listener)
    {
        _dispacher.UnRegisterEventListener(evtid, listener);
    }

    /// <summary>
    /// 注册 事件枚举的事件监听器
    /// </summary>
    /// <param name="evtid"></param>
    /// <param name="listener"></param>
    static public void AddEventListener(EEvent evtid, EventCallBack listener)
    {
        _dispacher.RegisterEventListener((int)evtid, listener);
    }
    /// <summary>
    /// 反注册 事件枚举的事件监听器
    /// </summary>
    static public void RemoveEventListener(EEvent evtid, EventCallBack listener)
    {
        _dispacher.UnRegisterEventListener((int)evtid, listener);
    }


    /// <summary>
    /// 注册有监听对象 的事件 监听器
    /// </summary>
    /// <param name="evtid"></param>
    /// <param name="listener"></param>
    /// <param name="callback"></param>
    static public void AddEventListener(EEvent evtid,IEventListener listener,EventCallBack callback)
    {
        _dispacher.RegisterEventListener((int)evtid, listener, callback);
    }
    /// <summary>
    ///  反注册有监听对象 的事件 监听器
    /// </summary>
    static public void RemoveEventListener(EEvent evtid, IEventListener listener, EventCallBack callback)
    {
        _dispacher.UnRegisterEventListener((int)evtid, listener, callback);
    }


    /// <summary>
    /// 分发 有参数 事件
    /// </summary>
    /// <param name="evt"></param>
    static public void SendEvent(Events evt)
    {
        _dispacher.DispatchEvent(evt);
    }
    /// <summary>
    /// 分发 无参数事件
    /// </summary>
    /// <param name="evtid"></param>
    static public void SendEvent(EEvent evtid)
    {
        Events evt = new Events((int)evtid);
        _dispacher.DispatchEvent(evt);
    }
}
