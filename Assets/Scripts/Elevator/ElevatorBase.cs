using System.Collections;
using UnityEngine;

public abstract class ElevatorBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected GameObject door;
    [SerializeField] protected Transform standingPoint;
    [SerializeField] protected float doorDelta;

    protected IEnumerator MoveOverTime(Transform target, Vector3 start, Vector3 end, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            target.position = Vector3.Lerp(start, end, t);
            time += Time.deltaTime;
            yield return null;
        }

        target.position = end;
    }

    public IEnumerator OpenDoor()
    {
        float duration = 0.75f;

        Vector3 start = door.transform.position;
        Vector3 end = start - doorDelta * Vector3.up;

        yield return MoveOverTime(door.transform, start, end, duration);
    }

    public abstract void Interact(PlayerController player);
}
