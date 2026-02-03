using UnityEngine;

public class GuardController : MonoBehaviour, IKillable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        Debug.Log("Guard died");
    }
}
