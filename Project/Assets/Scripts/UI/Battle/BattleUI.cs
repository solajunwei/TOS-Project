using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class DrawPetData
{
    public int petId;

}

public class BattleUI : MonoBehaviour
{

    public Image[] _iamges;
    public GameObject _prefab;
    //private Dictionary<int, >
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ³é¿¨
    public void Draw()
    {
        Image image = _iamges[0];
        GameObject obj = Instantiate(_prefab, image.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;   

        PetUnit unit = obj.GetComponent<PetUnit>();
        unit.LoadAndReplaceImage(11001);
    }

    public void OnClickSkill(int skillId)
    {

    }
}
