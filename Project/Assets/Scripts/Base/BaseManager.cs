using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T> where T:new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
}
