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
        
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        Debug.Log(GetComponent<Collider>());
        // if ()
    }
}
