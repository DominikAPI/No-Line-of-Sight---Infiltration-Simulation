
public static class GuardFactory
{
    public static GuardEntity CreateStandardGuard() => new GuardEntity(5f, 1.5f, 40f);

    public static GuardEntity CreateEliteGuard() => new GuardEntity(10f, 0.5f);
}
