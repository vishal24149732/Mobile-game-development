using UnityEngine;

public class gunn : MonoBehaviour
{
    public GameObject bull;
    public Transform Spawn;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float timer = 0f;
    float nextShot = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.2f )
        {
            Shoot();
            timer = 0f;
        }
        
    }

    void Shoot()
    {
        GameObject bullet = Instantiate (bull, Spawn.position, Spawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Rigidbody playerRb = GetComponentInParent<Rigidbody>();
        //rb.AddForce(Spawn.forward * speed, ForceMode.Force);
        //rb.linearVelocity = Spawn.forward * speed + playerRb.linearVelocity;
        rb.AddForce(playerRb.linearVelocity, ForceMode.VelocityChange);
    }
}
