namespace CCG.MVVM.MainMenu
{
    public class MainMenuPayloadUi : IMainMenuPayloadUi
    {
        public IMainMenu3dController Controller { get; }

        public MainMenuPayloadUi(IMainMenu3dController controller)
        {
            Controller = controller;
        }
        
    }
}