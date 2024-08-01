using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadConfigManager : BaseManager<ReadConfigManager>
{
    public List<Dictionary<string, string>> ReadConfig(string readname)
    {

        // 将 txt 中的内容加载进txt文本中
        TextAsset txt = Resources.Load(readname) as TextAsset;

        // 输出该文本的内容
        Debug.Log(txt);

        // 以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
        string[] str = txt.text.Split('\n');

        // 将该文本中的字符串输出
        Debug.Log("str[0]= " + str[0]);
        Debug.Log("str[1]= " + str[1]);

        List<string> TitleList = new List<string>();
        string[] firstName = str[0].Split('\t');
        foreach(string name in firstName)
        {
            TitleList.Add(name);
        }

        List<Dictionary<string, string>> RetData = new List<Dictionary<string, string>>();
        // 将每行字符串的内容以逗号作为分割点，并将每个逗号分隔的字符串内容遍历输出
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
