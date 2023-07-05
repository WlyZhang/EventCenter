///------------------------------------------------------------------------------------------------------------
///事件派发器
///------------------------------------------------------------------------------------------------------------
///------------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;



public struct Events
{
    private int _id;
    private object _data;
    public int id{get{return _id;}}
    public object Data{get{return _data;}}

    /// <summary>
    /// 双参构造
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public Events(int id,object data)
    {
        _id=id;
        _data=data;
    }
    /// <summary>
    /// 单参数 构造
    /// </summary>
    /// <param name="id"></param>
    public Events(int id)
    {
        _id=id;
        _data=null;
    }
    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public Events(EEvent id, object data = null)
    {
        _id=(int )id;
        _data=data;
    }
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetData<T>()
    {
        return (T)_data;
    }
}

/// <summary>
/// 回调函数
/// </summary>
public delegate void EventCallBack(Events args);

/// <summary>
/// 监听对象接口
/// </summary>
public interface IEventListener
{
    bool isValidEventListener { get; }
}
/// <summary>
/// 监听对象
/// </summary>
public struct EventListener
{
    public IEventListener listener;
    public EventCallBack callBack;

    /// <summary>
    /// 构造
    /// </summary>
    public EventListener(IEventListener l, EventCallBack c)
    {
        listener = l;
        callBack = c;
    }
}
/// <summary>
/// 事件派发器
/// </summary>
public class EventDispatcher  
{
    // 成员变量
    private readonly Dictionary<int, EventCallBack> _dicCallBack = new Dictionary<int, EventCallBack>();
    private readonly Dictionary<int, List<EventListener>> _dicListener = new Dictionary<int, List<EventListener>>();
   
    /// <summary>
    /// 注册事件监听器
    /// </summary>
    public void RegisterEventListener(int evtId, EventCallBack callback)
    {
        if (null == callback)
        {
            Debug.LogError("Null Callback - RegisterEventListener ");
            return;
        }
        if (_dicCallBack.ContainsKey(evtId))
        {
            _dicCallBack[evtId] += callback;
        }
        else
        {
            _dicCallBack[evtId] = callback;
        }
    }

    /// <summary>
    /// 反注册事件监听器
    /// </summary>
    /// <param name="evtId"></param>
    /// <param name="callback"></param>
    public void UnRegisterEventListener(int evtId, EventCallBack callback)
    {
        if (null == callback)
        {
            Debug.LogError("Null callBack - UnRegisterEventListener");
            return;
        }
        if (_dicCallBack.ContainsKey(evtId))
        {
            _dicCallBack[evtId] -= callback;
            if (null == _dicCallBack[evtId])
            {
                _dicCallBack.Remove(evtId);
            }
        }
    }

    /// <summary>
    /// 注册事件监听器
    /// </summary>
    /// <param name="evtId"></param>
    /// <param name="listener"></param>
    /// <param name="callback"></param>
    public void RegisterEventListener(int evtId,IEventListener listener,EventCallBack callback)
    {
        if (null == callback)
        {
            Debug.LogError("Null lsitener - RegisterEventListener");
            return;
        }
        
        //没有 添加
        List<EventListener> listeners;
        if (_dicListener.TryGetValue(evtId, out listeners))
        {
            //避免重复添加
            for(int i=0,count=listeners.Count;i<count;++i)
            {
                EventListener l = listeners[i];
                if (l.listener == listener && l.callBack == callback)
                {
                    Debug.LogError("Add event {0} Listener repeat - RegisterEventListener");
                    return;
                }
            }
        }
        else
        {
            listeners = new List<EventListener>();
            _dicListener.Add(evtId, listeners);
        }
        //加入列表
        listeners.Add(new EventListener(listener, callback));
    }
    /// <summary>
    /// 反注册 事件 监听器
    /// </summary>
    /// <param name="evtId"></param>
    /// <param name="listener"></param>
    /// <param name="callback"></param>
    public void UnRegisterEventListener(int evtId, IEventListener listener, EventCallBack callback)
    {
        if (null == callback)
        {
            Debug.LogError("Null listener - UnRegisterEventListener");
            return;
        }
        //没找到
        List<EventListener> listeners;
        if (!_dicListener.TryGetValue(evtId, out listeners))
        {
            return;
        }
        //删除
        for (int i = 0, count = listeners.Count; i < count; ++i)
        {
            EventListener l = listeners[i];
            if (l.listener == listener && l.callBack == callback)
            {
                listeners.RemoveAt(i);
                break;
            }
        }
        //列表为空 删除
        if (listeners.Count < 1)
        {
            _dicListener.Remove(evtId);
        }
    }
    /// <summary>
    /// 分发 事件
    /// </summary>
    /// <param name="evt"></param>
    public void DispatchEvent(Events evt)
    {
        //回调函数
        if (_dicCallBack.ContainsKey(evt.id))
        {
            EventCallBack listener = _dicCallBack[evt.id];
            listener(evt);
        }
        //回调对象
        List<EventListener> listeners;
        if (_dicListener.TryGetValue(evt.id, out listeners))
        {
            for (int i = 0; i < listeners.Count; ++i)
            {
                EventListener l = listeners[i];
                if (l.listener.isValidEventListener)
                {
                    l.callBack(evt);
                }
            }
        }
    }

    public void Clear()
    {
        _dicCallBack.Clear();
        _dicListener.Clear();
    }
}
