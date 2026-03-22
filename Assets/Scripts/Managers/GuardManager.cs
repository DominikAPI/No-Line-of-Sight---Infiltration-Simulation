using System;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    [SerializeField] private GameObject guardPrefab;
    [SerializeField] private List<Transform> guardSpawnPoints;
    private List<IResetable> guards;
    private List<GameObject> guardObjects;
    public Action<IDetectable, string> OnDetection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        guards = new List<IResetable>();
        guardObjects = new List<GameObject>();

        guardSpawnPoints.ForEach(point => guardObjects.Add(InstantiateGuardAtPosition(point.position, guards, point.rotation)));
    }

    private GameObject InstantiateGuardAtPosition(Vector3 position, List<IResetable> guardList, Quaternion rotation)
    {
        GuardEntity entity = GuardFactory.CreateEliteGuard();
        entity.OnTargetDetected += DetectionMessage;
        var guard = Instantiate(guardPrefab, position, rotation, transform);
        var controller = guard.GetComponentInChildren<GuardController>();
        controller.Initialize(entity);
        guardList.Add(controller);
        return guard;
    }

    public void DetectionMessage(IDetectable detectable, string message) => OnDetection?.Invoke(detectable, message);

    public void ResetGuards() => guards.ForEach(guard => guard.ResetObject());
}
