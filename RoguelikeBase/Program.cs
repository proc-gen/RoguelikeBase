﻿using SadConsole.Configuration;

Settings.WindowTitle = "Roguelike Base";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<RoguelikeBase.Scenes.RootScreen>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts(true)
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();