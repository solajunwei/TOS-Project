using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addlist : MonoBehaviour
{
   public UIList list;
   // public GameObject skin;
   public Button btn;
   // Start is called before the first frame update
   void Start()
   {
       // list.setSkin = skin.GetComponent<RectTransform>();
       List<dataa> dic = new List<dataa>();
       for (int i = 0; i < 6; i++)
       {
           dataa date = new dataa();
           date.name = "����" + i;
           date.index = i;
           dic.Add(date);
       }
       list.OnUpdateItem += change;
       list.m_left = 0;
       list.m_top = 20;
       list.m_down = 30;
       list.setData(dic);
       List<dataa> dica = new List<dataa>();
       btn.onClick.AddListener(delegate ()
       {

           Debug.Log(" AddListener === ");
           dica.Clear();
           for (int i = 0; i < 10; i++)
           {
               dataa date = new dataa();
               date.name = "�Ѿ��ı�";
               date.index = i;
               dica.Add(date);
           }
           list.setData(dica);
       });
   }
   void change(Item item)
   {
       Debug.Log(" change === ");
    //    Text text = item.m_childName["Text"].GetComponent<Text>();
    //    Button on_btn = item.m_childName["Button"].GetComponent<Button>();
    //    text.text = "xx" + (item.data as dataa).index + "--" + (item.data as dataa).name;
    //    //��ť�����Ϣ�¼�
    //    on_btn.onClick.AddListener(() =>
    //    {
    //        Debug.Log((item.data as dataa).index);
    //    });
   }
   void Update()
   {
        
   }
}

