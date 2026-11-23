using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class bull : MonoBehaviour
{
    Rigidbody rb;
    private float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>();
       rb.freezeRotation = true;
    }

    // Update is called once per frameS
    void Update()
    {
        timer += Time.deltaTime;
        rb.AddForce(Vector3.forward * 30f, ForceMode.Force);
        //rb.linearVelocity = Vector3.forward * 20f;
        if (timer >= 5f)
        {
            Destroy(gameObject);
        }

    }
}
