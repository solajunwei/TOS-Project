using UnityEngine;

public class StartScript : MonoBehaviour
{

    void Start()
    {
        GameConfig.Instance.getTables();
        var mono = MonoMgr.Instance;
        UIManager.Instance.ShowPanel<Login>("Perfabs/Login/Login");
        UIManager.Instance.ShowPanel<Setting>("Perfabs/Main/Setting", E_UI_Layer.Top);
    }
}
