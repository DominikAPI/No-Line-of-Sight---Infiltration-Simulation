using UnityEngine;

public class GuardManager : MonoBehaviour
{
    [SerializeField] private GameObject guardPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateGuardAtPosition(new Vector3(-3, -3));
        InstantiateGuardAtPosition(new Vector3(-5, 2.5f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InstantiateGuardAtPosition(Vector3 position)
    {
        GuardEntity entity = GuardFactory.CreateEliteGuard();
        entity.OnTargetDetected += DetectionMessage;
        var guard = Instantiate(guardPrefab, position, Quaternion.identity);
        var controller = guard.GetComponentInChildren<GuardController>();
        controller.Initialize(entity);
    }

    public void DetectionMessage(IDetectable detectable, string message)
    {
        Debug.Log(message);
    }
}
