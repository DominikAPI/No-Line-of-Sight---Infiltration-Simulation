using System.Collections;
using UnityEngine;

public class ElevatorController : ElevatorBase
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);
    [SerializeField] private Transform nextStandingPoint;
    [SerializeField] private ElevatorBase nextElevator;

    private IEnumerator CloseDoor()
    {
        float duration = 0.75f;

        Vector3 start = door.transform.position;
        Vector3 end = start + doorDelta * door.transform.up;

        yield return MoveOverTime(door.transform, start, end, duration);
    }

    private IEnumerator ElevatorSequence(PlayerController player)
    {
        player.DisablePlayerControls();

        yield return MoveOverTime(player.transform, player.transform.position, standingPoint.position, 1f);

        yield return CloseDoor();

        GameManager.Instance.ActivateNextFloor();
        player.transform.position = nextStandingPoint.position;

        yield return _waitForSeconds1;  //Artifical delay
        yield return nextElevator.OpenDoor();

        player.EnablePlayerControls();
        GameManager.Instance.IncrementFloor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player)) player.SetInteractable(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player)) player.RemoveInteractable(this);
    }

    public override void Interact(PlayerController player)
    {
        player.RemoveInteractable(this);
        StartCoroutine(ElevatorSequence(player));
    }
}
