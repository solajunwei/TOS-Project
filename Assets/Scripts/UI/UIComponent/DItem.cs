using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ItemDate
{
    public int index;
    public object data;
    public DListView m_parent;
}

public class DItem : MonoBehaviour
{
    public int index;
    public object data;
    public DListView m_parent;
    public Dictionary<string, Transform> m_childName;
    public void refData()
    {
        index = 0;
        data = null;
        m_parent = null;
        m_childName = new Dictionary<string, Transform>();
    }
}
