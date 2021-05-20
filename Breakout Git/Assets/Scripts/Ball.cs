using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float speed = 20f;
    Rigidbody rigidbody;
    Vector3 _velocity;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
    }

    void Launch()
    {
        rigidbody.velocity = Vector3.up * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
        _velocity = rigidbody.velocity;

        if (!renderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rigidbody.velocity =  Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }

}
