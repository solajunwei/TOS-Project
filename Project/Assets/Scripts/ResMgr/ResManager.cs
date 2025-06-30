using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// ��Դ����ģ��
public class ResManager : BaseManager<ResManager>
{
    //ͬ��������Դ
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //���������һ��GameObject���͵ģ��Ұ���ʵ�������ٷ��س�ȥֱ��ʹ�á�
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else //else���ʾ����TextAsset��AudioClip
            return res;
    }

    //�첽������Դ 
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        //�����첽���ص�Э��
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name, callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
        {
            //ʵ����һ���ٴ�������
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            //ֱ�Ӵ�������
            callback(r.asset as T);
        }
    }

}
