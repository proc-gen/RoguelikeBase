
using RoguelikeBase.UI;

namespace RoguelikeBase.Scenes
{
    internal class RootScreen : ScreenObject
    {
        MainMenuScreen mainMenuScreen;

        public RootScreen()
        {
            mainMenuScreen = new MainMenuScreen();
        }

        public override void Update(TimeSpan delta)
        {
            mainMenuScreen.Update(delta);
            base.Update(delta);
        }

        public override void Render(TimeSpan delta)
        {
            mainMenuScreen.Render(delta);
            base.Render(delta);
        }
    }
}
