using UnityEngine;

public class PistolController : MonoBehaviour
{
    [SerializeField] private float range = 20.0f;
    [SerializeField] private Transform firePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        Debug.Log("Shoot");
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right, range);
        if (!hit) return;

        IKillable killable = hit.collider.GetComponent<IKillable>();
        killable?.Die();
    }

    public void Reload()
    {

    }
}
