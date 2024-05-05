namespace Poedle.Enums
{
    public static class ItemClassesEnum
    {
        public enum ItemClasses
        {
            NONE,
            ABYSS_JEWEL,
            AMULET,
            BELT,
            BODY_ARMOUR,
            BOOTS,
            BOW,
            CLAW,
            CONTRACT,
            DAGGER,
            FISHING_ROD,
            GLOVES,
            HELMET,
            HYBRID_FLASK,
            ITEM_PIECE,
            JEWEL,
            LIFE_FLASK,
            MANA_FLASK,
            MAP,
            ONE_HANDED_AXE,
            ONE_HANDED_MACE,
            ONE_HANDED_SWORD,
            QUIVER,
            RELIC,
            RING,
            RUNE_DAGGER,
            SCEPTRE,
            SHIELD,
            STAFF,
            THRUSTING_ONE_HANDED_SWORD,
            TWO_HANDED_AXE,
            TWO_HANDED_MACE,
            TWO_HANDED_SWORD,
            UTILITY_FLASK,
            WAND,
            WARSTAFF
        }

        private readonly static Dictionary<ItemClasses, string> _enumToStrExceptions = new()
        {
            { ItemClasses.ABYSS_JEWEL, "Abyss Jewel" },
            { ItemClasses.BODY_ARMOUR, "Body Armour" },
            { ItemClasses.FISHING_ROD, "Fishing Rod" },
            { ItemClasses.HYBRID_FLASK, "Hybrid Flask" },
            { ItemClasses.ITEM_PIECE, "Item Piece" },
            { ItemClasses.LIFE_FLASK, "Life Flask" },
            { ItemClasses.MANA_FLASK, "Mana Flask" },
            { ItemClasses.ONE_HANDED_AXE, "One-Handed Axe" },
            { ItemClasses.ONE_HANDED_MACE, "One-Handed Mace" },
            { ItemClasses.ONE_HANDED_SWORD, "One-Handed Sword" },
            { ItemClasses.RUNE_DAGGER, "Rune Dagger" },
            { ItemClasses.THRUSTING_ONE_HANDED_SWORD, "Thrusting One-Handed Sword" },
            { ItemClasses.TWO_HANDED_AXE, "Two-Handed Axe" },
            { ItemClasses.TWO_HANDED_MACE, "Two-Handed Mace" },
            { ItemClasses.TWO_HANDED_SWORD, "Two-Handed Sword" },
            { ItemClasses.UTILITY_FLASK, "Utility Flask" }
        };

        private readonly static Dictionary<string, ItemClasses> _strToEnumExceptions = new()
        {
            { "Abyss Jewel", ItemClasses.ABYSS_JEWEL },
            { "Body Armour", ItemClasses.BODY_ARMOUR },
            { "Fishing Rod", ItemClasses.FISHING_ROD },
            { "Hybrid Flask", ItemClasses.HYBRID_FLASK },
            { "Item Piece", ItemClasses.ITEM_PIECE },
            { "Life Flask", ItemClasses.LIFE_FLASK },
            { "Mana Flask", ItemClasses.MANA_FLASK },
            { "One-Handed Axe", ItemClasses.ONE_HANDED_AXE },
            { "One-Handed Mace", ItemClasses.ONE_HANDED_MACE },
            { "One-Handed Sword", ItemClasses.ONE_HANDED_SWORD  },
            { "Rune Dagger", ItemClasses.RUNE_DAGGER },
            { "Thrusting One-Handed Sword", ItemClasses.THRUSTING_ONE_HANDED_SWORD },
            { "Two-Handed Axe", ItemClasses.TWO_HANDED_AXE },
            { "Two-Handed Mace", ItemClasses.TWO_HANDED_MACE },
            { "Two-Handed Sword", ItemClasses.TWO_HANDED_SWORD  },
            { "Utility Flask", ItemClasses.UTILITY_FLASK }
        };

        public static string EnumToStr(ItemClasses pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }

        public static ItemClasses StrToEnum(string pEnum)
        {
            return EnumUtil.GetEnumByName(pEnum, _strToEnumExceptions);
        }
    }
}
