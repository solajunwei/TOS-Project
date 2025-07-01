using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTxt : MonoBehaviour
{
    public void AnimCallback()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
