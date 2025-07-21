namespace Assets.Version2.StatusEffectSystem
{
    public enum StatusEffectStack
    {
        None = 0,

        //Only one(StatusEffectData not StatusEffectType) is allowed on the same unit
        Unique = 1,

        //Only one is allowed on the same unit, and the time can be refreshed when it is applyed again
        UniqueRefresh = 2,

        Stack = 3,
        StackRefresh = 4
    }
}