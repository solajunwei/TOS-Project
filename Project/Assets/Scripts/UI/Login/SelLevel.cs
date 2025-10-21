
public class SelLevel : UIComponent
{
    public void onClickLand(int tag)
    {
        UIManager.Instance.HidePanel("Perfabs/Main/Setting");
        UIManager.Instance.ShowPanel<SaveFile>("Perfabs/Login/SaveFile");
    }

    public void onClickBack()
    {
        UIManager.Instance.HidePanel("Perfabs/Login/SelLevel");
    }

    public void onClickPet()
    {
        //UIManager.Instance.ShowPanel<SaveFile>("Perfabs/Login/SaveFile");
    }

    public void onClickUp()
    {

    }
}
