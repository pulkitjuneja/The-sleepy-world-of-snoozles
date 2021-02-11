using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float projectileSpeed = 20f;
    public Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d.velocity = transform.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AstralEnemy astralEnemy = collision.GetComponent<AstralEnemy>();
        if (astralEnemy)
        {
            Debug.Log("Enemy Hit");
        }
        else
        {
            Debug.Log(collision.transform.name + "Hit");
        }
        Destroy(this.gameObject);
    }

}
