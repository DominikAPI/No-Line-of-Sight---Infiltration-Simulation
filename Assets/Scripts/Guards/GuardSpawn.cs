using UnityEngine;

public class GuardSpawn : MonoBehaviour
{
    [SerializeField] private bool isPatrolling;
    [SerializeField] private bool isElite;
    [SerializeField] private float speed;

    public bool IsPatrolling { get => isPatrolling; }

    public bool IsElite { get => isElite; }

    public GuardEntity CreateEntity()
    {
        float currentSpeed = IsPatrolling ? speed : 0.0f;
        return IsElite ? GuardFactory.CreateEliteGuard(currentSpeed) : GuardFactory.CreateStandardGuard(currentSpeed);
    }

    public Vector3 Position { get => transform.position; }

    public Quaternion Rotation { get => transform.rotation; }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Vector3 dir = transform.up;

        Gizmos.color = isElite ? Color.red : Color.green;

        Gizmos.DrawSphere(pos, 0.2f);

        Gizmos.DrawLine(pos, pos + dir);

        Vector3 right = Quaternion.Euler(0, 0, 30) * -dir * 0.3f;
        Vector3 left = Quaternion.Euler(0, 0, -30) * -dir * 0.3f;

        Gizmos.DrawLine(pos + dir, pos + dir + right);
        Gizmos.DrawLine(pos + dir, pos + dir + left);

        if (isPatrolling)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pos, 0.4f);
        }
    }
}
