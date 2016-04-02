using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        InGamePause,
        CharacterSelection
    }


    public enum ItemQuality
    {
        Rare,
        Legendary,
        Set
    }

    public enum ItemType
    {
        [Description("Axe")]
        Axe1H,
        [Description("Two-Handed Axe")]
        Axe2H,
        [Description("Amulet")]
        Amulet,
        [Description("Belt")]
        Belt,
        [Description("Boots")]
        Boots,
        [Description("Bow")]
        Bow,
        [Description("Bracers")]
        Bracers,
        [Description("Ceremonial Knife")]
        CeremonialKnife,
        [Description("Chest Armor")]
        Chest,
        [Description("Cloak")]
        Cloak,
        [Description("Hand Crossbow")]
        Crossbow1H,
        [Description("Crossbow")]
        Crossbow2H,
        [Description("Crusader Shield")]
        CrusaderShield,
        [Description("Dagger")]
        Dagger,
        [Description("Daibo")]
        Daibo,
        [Description("Fist Weapon")]
        Fist,
        [Description("Flail")]
        Flail1H,
        [Description("Two-Handed Flail")]
        Flail2H,
        [Description("Gloves")]
        Gloves,
        [Description("Helm")]
        Helm,
        [Description("Mace")]
        Mace1H,
        [Description("Two-Handed Mace")]
        Mace2H,
        [Description("Mighty Belt")]
        MightyBelt,
        [Description("Mighty Weapon")]
        MightyWeapon1H,
        [Description("Two-Handed Mighty Weapon")]
        MightyWeapon2H,
        [Description("Mojo")]
        Mojo,
        [Description("Source")]
        Orb,
        [Description("Pants")]
        Pants,
        [Description("Polearm")]
        Polearm,
        [Description("Quiver")]
        Quiver,
        [Description("Ring")]
        Ring,
        [Description("Shield")]
        Shield,
        [Description("Shoulders")]
        Shoulders,
        [Description("Spear")]
        Spear,
        [Description("Spirit Stone")]
        SpiritStone,
        [Description("Staff")]
        Staff,
        [Description("Sword")]
        Sword1H,
        [Description("Two-Handed Sword")]
        Sword2H,
        [Description("Wand")]
        Wand,
        [Description("Wizard Hat")]
        WizardHat,
        [Description("Voodoo Mask")]
        VoodooMask,
        SwingBlade,
        SwingBlunt,
        Axe,
        Flail,
        HandCrossbow,
        Mace,
        MightyWeapon,
        Sword
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
    public enum CurrentTab
    {
        Core,
        Offense,
        Defense,
        Utility,
        Bot,
        Character,
        Inventory
    }

    public enum CharacterStat
    {
        Block,
        Vitality,
        BotDamage,
        CritChance,
        CritDamage,
        ResistAll,
        Life,
        Regen,
        LifePerHit,
        Sockets,
        DamageAgainstElites,
        DamageReduction,
        ExtraGold,
        ExtraExperience,
        Thorns,
        BotAttackSpeed,
        Intelligence,
        Strength,
        Dexterity,
        MainStat,
        Damage,
        Armor,
    }

    [Serializable]
    public struct Point
    {
        public int x, y;
    }
}
