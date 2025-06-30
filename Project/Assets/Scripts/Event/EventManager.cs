using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{
    //����һ���սӿ�
}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}


public class EventManager : BaseManager<EventManager>
{

    //�ֵ��У�key��Ӧ���¼������֣�
    //value��Ӧ���Ǽ�������¼���Ӧ��ί�з�����
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    //����¼�����
    //��һ���������¼�������
    //�ڶ��������������¼��ķ���
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDic.ContainsKey(name))
        {
            EventInfo<T> eventInfo = eventDic[name] as EventInfo<T>;
            if (null != eventInfo)
            {
                eventInfo.actions += action;
            }
        }
        //û�е����
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }
    //���ڲ���Ҫ��������������ط���
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            EventInfo eventInfo = eventDic[name] as EventInfo;
            if(null != eventInfo)
            {
                eventInfo.actions += action;
            }
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }


    //ͨ���¼����ֽ����¼�����
    public void EventTrigger<T>(string name, T info)
    {
        //��û�ж�Ӧ���¼�����
        //�е���������˹�������¼���
        if (eventDic.ContainsKey(name))
        {
            //����ί�У�����ִ��ί���еķ�����
            //����һ��C#�ļ򻯲���,���ڣ���ֱ�ӵ���ί��
            EventInfo<T> eventInfo = eventDic[name] as EventInfo<T>;
            if(null != eventInfo)
            {
                eventInfo.actions?.Invoke(info);
            }
        }
    }

    //���ڲ���Ҫ��������������ط���
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions?.Invoke();
        }
    }

    //�Ƴ���Ӧ���¼�����
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            //�Ƴ����ί��
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }

    //���ڲ���Ҫ��������������ط���
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }

    //��������¼�����(��Ҫ�����л�����ʱ)
    public void Clear()
    {
        eventDic.Clear();
    }
}
