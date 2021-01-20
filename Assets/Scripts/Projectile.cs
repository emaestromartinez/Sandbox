using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rigidbody2d;

    private GameObject ruby;
    private Vector2 rubyPosition;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        ruby = GameObject.FindGameObjectWithTag("Player");
        rubyPosition = ruby.GetComponent<Rigidbody2D>().position;
    }

    private void Update()
    {

        if (Vector3.Distance(rigidbody2d.position, ruby.GetComponent<Rigidbody2D>().position) > 5.0f)
        {
            Destroy(gameObject);
        }


    }

    public void Launch(Vector2 direction, float force)
    {

        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.FixRobot();
        }

        Destroy(gameObject);
    }
}
