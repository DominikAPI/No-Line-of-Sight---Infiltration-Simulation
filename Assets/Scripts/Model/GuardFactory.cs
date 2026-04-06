
public static class GuardFactory
{
    public static GuardEntity CreateStandardGuard(float speed = 0) => new GuardEntity(5f, 1.5f, 40f, speed);

    public static GuardEntity CreateEliteGuard(float speed = 0) => new GuardEntity(10f, 0.75f, 60f, speed);
}
