using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManagerMomoBehaviour<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance != null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
