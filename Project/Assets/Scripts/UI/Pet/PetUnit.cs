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

    public void LoadAndReplaceImage(int PetId)
    {
        if(_image != null)
        {
            Sprite[] loadSprites = Resources.LoadAll<Sprite>("UI/Images/Unit/Characters");
            if(loadSprites != null && loadSprites.Length > 0)
            {
                Sprite spr = loadSprites.FirstOrDefault(s => s.name == PetId.ToString());
                if (spr != null)
                {
                    _image.sprite = spr;
                }
            }
        }
    }
}
