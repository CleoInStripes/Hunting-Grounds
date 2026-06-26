using UnityEngine;

public class BloodBullet : MonoBehaviour
{
    public int speed = 1;
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.linearVelocity = transform.forward * speed;        
    }
}
