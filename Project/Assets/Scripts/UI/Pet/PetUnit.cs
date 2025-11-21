using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PetUnit : MonoBehaviour
{
    public Image _image;

    void Start()
    {
        
    }

    public void LoadPetInitialValue()
    {
        if(_image != null)
        {
            Sprite[] loadSprites = Resources.LoadAll<Sprite>("UI/Images/Unit/petagg");
            if(loadSprites != null && loadSprites.Length > 0)
            {
                // 默认去第一个元素
                Sprite spr = loadSprites.FirstOrDefault();
                if (spr != null)
                {
                    _image.sprite = spr;
                }
            }
        }
    }

    public void LoadAndReplaceImage(int PetId)
    {
        if(_image != null)
        {
            Sprite[] loadSprites = Resources.LoadAll<Sprite>("UI/Images/Unit/Characters");
            if(loadSprites != null && loadSprites.Length > 0)
            {
                // 找到id为petid的资源
                Sprite spr = loadSprites.FirstOrDefault(s => s.name == PetId.ToString());
                if (spr != null)
                {
                    _image.sprite = spr;
                }
            }
        }
    }
}
