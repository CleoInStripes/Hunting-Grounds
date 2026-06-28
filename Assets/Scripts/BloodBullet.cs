using UnityEngine;

public class BloodBullet : MonoBehaviour
{
    public int speed = 1;
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        GameObject myObject = gameObject;
    }

    void Update()
    {
        rb.linearVelocity = transform.forward * speed; 
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy: " + other.name);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
