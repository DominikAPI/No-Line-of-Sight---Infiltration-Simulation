using System;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    [SerializeField] private GameObject guardPrefab;
    private List<IResetable> guards;
    public Action<IDetectable, string> OnDetection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        guards = new List<IResetable>();
        InstantiateGuardAtPosition(new Vector3(-3, -3), guards);
        InstantiateGuardAtPosition(new Vector3(-5, 2.5f), guards);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject InstantiateGuardAtPosition(Vector3 position, List<IResetable> guardList)
    {
        GuardEntity entity = GuardFactory.CreateEliteGuard();
        entity.OnTargetDetected += DetectionMessage;
        var guard = Instantiate(guardPrefab, position, Quaternion.identity);
        var controller = guard.GetComponentInChildren<GuardController>();
        controller.Initialize(entity);
        guardList.Add(controller);
        return guard;
    }

    public void DetectionMessage(IDetectable detectable, string message) => OnDetection?.Invoke(detectable, message);

    public void ResetGuards() => guards.ForEach(guard => guard.ResetObject());
}
