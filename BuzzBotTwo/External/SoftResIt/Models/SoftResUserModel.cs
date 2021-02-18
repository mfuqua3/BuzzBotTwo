using System;
using System.Collections.Generic;

namespace BuzzBotTwo.External.SoftResIt.Models
{
    public class SoftResUserModel
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public SoftResSpec? Spec { get; set; }
        public List<int> Items { get; set; }
        public string Note { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }

    public enum SoftResSpec : Int32
    {
        [Class(Class.Druid)]
        [Role(Role.Healer)]
        RestoDruid = 10,
        [Class(Class.Druid)]
        [Role(Role.Tank)]
        BearDruid = 11,
        [Class(Class.Druid)]
        [Role(Role.Melee)]
        CatDruid = 12,
        [Class(Class.Druid)]
        [Role(Role.Caster)]
        BalanceDruid = 13,
        [Class(Class.Hunter)]
        [Role(Role.Ranged)]
        MarksmanshipHunter = 20,
        [Class(Class.Hunter)]
        [Role(Role.Ranged)]
        BeastMasteryHunter = 21,
        [Class(Class.Hunter)]
        [Role(Role.Ranged)]
        SurvivalHunter = 22,
        [Class(Class.Mage)]
        [Role(Role.Caster)]
        FrostMage = 30,
        [Class(Class.Mage)]
        [Role(Role.Caster)]
        FireMage = 31,
        [Class(Class.Mage)]
        [Role(Role.Caster)]
        ArcaneMage = 32,
        [Class(Class.Priest)]
        [Role(Role.Healer)]
        HolyPriest = 40,
        [Class(Class.Priest)]
        [Role(Role.Healer)]
        DisciplinePriest = 41,
        [Class(Class.Priest)]
        [Role(Role.Caster)]
        ShadowPriest = 42,
        [Class(Class.Paladin)]
        [Role(Role.Healer)]
        HolyPaladin = 50,
        [Class(Class.Paladin)]
        [Role(Role.Tank)]
        ProtPaladin = 51,
        [Class(Class.Paladin)]
        [Role(Role.Melee)]
        RetPaladin = 52,
        [Class(Class.Rogue)]
        [Role(Role.Melee)]
        SwordRogue = 60,
        [Class(Class.Rogue)]
        [Role(Role.Melee)]
        DaggerRogue = 61,
        [Class(Class.Rogue)]
        [Role(Role.Melee)]
        MaceRogue = 62,
        [Class(Class.Shaman)]
        [Role(Role.Healer)]
        RestoShaman = 70,
        [Class(Class.Shaman)]
        [Role(Role.Melee)]
        EnhanceShaman = 71,
        [Class(Class.Shaman)]
        [Role(Role.Caster)]
        ElementalShaman = 72,
        [Class(Class.Warlock)]
        [Role(Role.Caster)]
        AfflictionWarlock = 80,
        [Class(Class.Warlock)]
        [Role(Role.Caster)]
        DemonologyWarlock = 81,
        [Class(Class.Warlock)]
        [Role(Role.Caster)]
        DestructionWarlock = 82,
        [Class(Class.Warlock)]
        [Role(Role.Caster)]
        HybridWarlock = 83,
        [Class(Class.Warlock)]
        [Role(Role.Caster)]
        SmRuinWarlock = 84,
        [Class(Class.Warlock)]
        [Role(Role.Caster)]
        DsRuinWarlock = 85,
        [Class(Class.Warrior)]
        [Role(Role.Melee)]
        FuryWarrior = 90,
        [Class(Class.Warrior)]
        [Role(Role.Tank)]
        ProtWarrior = 91,
        [Class(Class.Warrior)]
        [Role(Role.Melee)]
        ArmsWarrior = 92,
    }
    public enum Role
    {
        Tank,
        Healer,
        Caster,
        Melee,
        Ranged,
        Bench
    }
    public enum Class
    {
        Warrior,
        Paladin,
        Hunter,
        Shaman,
        Rogue,
        Druid,
        Mage,
        Warlock,
        Priest,
        Undefined
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class RoleAttribute : Attribute
    {
        public RoleAttribute(Role role)
        {
            Role = role;
        }

        public Role Role { get; }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ClassAttribute : Attribute
    {
        public Class WowClass { get; }

        public ClassAttribute(Class wowClass)
        {
            WowClass = wowClass;
        }
    }
}