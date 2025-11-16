using UnityEngine;

public class StartScript : MonoBehaviour
{

    void Start()
    {
        GameConfig.Instance.getTables();
        var mono = MonoMgr.Instance;

        UIManager.Instance.OpenView<Login>("Perfabs/Login/Login");
        UIManager.Instance.OpenView<Setting>("Perfabs/Main/Setting", E_UI_Layer.Top);
    }
}
