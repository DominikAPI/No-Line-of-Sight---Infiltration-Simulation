using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TurningPointController : MonoBehaviour
{
    [SerializeField] private float turningAngle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPatrol patrol = collision.GetComponent<IPatrol>();
        patrol?.Turn(turningAngle);
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.offset, col.size);

        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.yellow;

        Vector3 pos = transform.position;
        Vector3 dir = Quaternion.Euler(0, 0, turningAngle) * Vector3.up;

        Gizmos.DrawLine(pos, pos + dir);
    }
}
