
public class SaveFile : UIView
{
    public void onBack()
    {
        UIManager.Instance.GoBack();
    }

    // ¼òµ¥
    public void onOpenSaveEase()
    {
        UIManager.Instance.OpenView<SelectLevel>("Perfabs/Login/SelectLevel");
    }

    // À§ÄÑ
    public void onOpenSaveDifficulty()
    {
        UIManager.Instance.OpenView<SelectLevel>("Perfabs/Login/SelectLevel");
    }

    public void onOpenNewGame()
    {
        UIManager.Instance.OpenView<SelectLevel>("Perfabs/Login/SelectLevel");
    }    
}
