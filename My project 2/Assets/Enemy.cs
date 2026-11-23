using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private bool Move = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartMoving()
    {
        rb = GetComponent<Rigidbody>();
        Move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Move)
        {
            rb.linearVelocity = Vector3.back * 20f ;
            //rb.AddForce(Vector3.back * 10f, ForceMode.Force);


        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            Destroy(gameObject);
        }
    }
}
