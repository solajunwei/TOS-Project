using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadConfigManager : BaseManager<ReadConfigManager>
{
    public List<Dictionary<string, string>> ReadConfig(string readname)
    {

        // �� txt �е����ݼ��ؽ�txt�ı���
        TextAsset txt = Resources.Load(readname) as TextAsset;

        // ������ı�������
        Debug.Log(txt);

        // �Ի��з���Ϊ�ָ�㣬�����ı��ָ���������ַ����������������ʽ������ÿ���ַ���������
        string[] str = txt.text.Split('\n');

        // �����ı��е��ַ������
        Debug.Log("str[0]= " + str[0]);
        Debug.Log("str[1]= " + str[1]);

        List<string> TitleList = new List<string>();
        string[] firstName = str[0].Split('\t');
        foreach(string name in firstName)
        {
            TitleList.Add(name);
        }

        List<Dictionary<string, string>> RetData = new List<Dictionary<string, string>>();
        // ��ÿ���ַ����������Զ�����Ϊ�ָ�㣬����ÿ�����ŷָ����ַ������ݱ������
        // foreach (string strs in str)
        for (int i = 1; i < str.Length; i++)   
        {
            string[] ss = str[i].Split('\t');
            
            Dictionary<string, string> value = new Dictionary<string, string>();
            for (int j = 0; j < ss.Length; j++)
            {
                value.Add(TitleList[j], ss[j]);
            }
            RetData.Add(value);
        }

        return RetData;
    }
}
