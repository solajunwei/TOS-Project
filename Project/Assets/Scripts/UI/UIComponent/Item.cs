using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ItemDate
{
    public int index;
    public object data;
    public UIList m_parent;
}

public class Item : MonoBehaviour
{

    public GameObject skin;

    [HideInInspector]
    public int index;

    [HideInInspector]
    public object data;

    [HideInInspector]
    public UIList m_parent;

    [HideInInspector]
    public Dictionary<string, Transform> m_childName;
    public void refData()
    {
        index = 0;
        data = null;
        m_parent = null;
        m_childName = new Dictionary<string, Transform>();
    }
}

