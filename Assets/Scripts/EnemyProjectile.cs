using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Vector3 direction;
    public AstralEnemy owner;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if(!CheckBounds())
        {
            //Destroy the bullet
            if(owner)
                owner.BulletDestroyed();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //NOTE uncomment after player projectile is created
        //if(collision.GetComponent<PlayerController> || collision.GetComponent<PlayerProjectile>)
        //{
        //    
        //    
        //  //Destroy the bullet
            //owner.BulletDestroyed();
            //Destroy(gameObject);
        //    
        //    
        //}
    }

    bool CheckBounds()
    {
        //Checking the bounds of the screen
        float totalCameraHeight = Camera.main.orthographicSize;
        float totalCameraWidth = totalCameraHeight * Camera.main.aspect;

        //Checking if any bullet is out of the bounds of the screen
        //Condition if bullet is outside of the screen
       if (transform.position.x > totalCameraWidth ||
           transform.position.x < -totalCameraWidth ||
           transform.position.y > totalCameraHeight ||
           transform.position.y < -totalCameraHeight)
       {
            return false;
       }

        return true;
    }
}
