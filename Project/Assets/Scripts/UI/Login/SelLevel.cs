
public class SelLevel : UIView
{
    public void onClickLand(int tag)
    {
        UIManager.Instance.GoBack(E_UI_Layer.Top);
        UIManager.Instance.OpenView<SaveFile>("Perfabs/Login/SaveFile");
    }

    public void onClickBack()
    {
        UIManager.Instance.GoBack();
    }

    public void onClickPet()
    {
        //UIManager.Instance.ShowPanel<SaveFile>("Perfabs/Login/SaveFile");
    }

    public void onClickUp()
    {

    }
}
