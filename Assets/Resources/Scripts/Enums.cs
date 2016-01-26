using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Resources.Scripts
{
    public enum GameClasses
    {
        Barbarian,
        Crusader,
        DemonHunter,
        Monk,
        WitchDoctor,
        Wizard
    }

    public enum GameState
    {
        Login,
        MainMenu,
        CharacterCreation,
        InGame,
        InGamePause
    }


    public enum ItemQuality
    {
        Grey,
        White,
        Blue,
        Yellow,
        Brown,
        Green
    }

    public enum QuestState
    {
        NotEligible,
        NotInQuestBook,
        InProccess,

    }

    public enum HeroState
    {
        NotCreated,
        Normal,
        Dead
    }

    public enum FontType
    {
        DiabloFont,
        StandartFont
    }

    public enum ButtonState
    {
        Up,
        Down,
        Over,
        Disabled,
        Selected
    }
}
