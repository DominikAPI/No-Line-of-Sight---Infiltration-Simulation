using UnityEngine;

public class GuardManager : MonoBehaviour
{
    [SerializeField] private GameObject guardPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GuardEntity entity = GuardFactory.CreateEliteGuard();
        entity.OnTargetDetected += DetectionMessage;
        var guard = Instantiate(guardPrefab, new Vector3(-3, -3), Quaternion.identity);
        var controller = guard.GetComponent<GuardController>();
        controller.Initialize(entity);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DetectionMessage(IDetectable detectable, string message)
    {
        Debug.Log(message);
    }
}
