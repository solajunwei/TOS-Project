using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : BaseManagerMomoBehaviour<UIComponent>
{
    //让子类重写（覆盖）此方法，来实现UI的隐藏与出现

    public virtual void ShowMe()
    {

    }
    
    public virtual void HideMe() {
        Destroy(gameObject);
    }
}
