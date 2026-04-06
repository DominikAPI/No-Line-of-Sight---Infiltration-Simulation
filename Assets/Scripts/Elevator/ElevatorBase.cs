using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class ElevatorBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected GameObject door;
    [SerializeField] protected Transform standingPoint;
    [SerializeField] protected float doorDelta;
    [SerializeField] protected AudioClip doorSound;

    protected AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        float duration = 1.8f;

        Vector3 start = door.transform.position;
        Vector3 end = start - doorDelta * door.transform.up;

        audioSource.PlayOneShot(doorSound);
        yield return MoveOverTime(door.transform, start, end, duration);
    }

    public abstract void Interact(PlayerController player);
}
