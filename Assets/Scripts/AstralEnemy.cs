using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralEnemy : MonoBehaviour
{
    int health;
    [SerializeField] Transform player;
    [SerializeField] EnemyProjectile projectile;
    int projectileCount;
    public const int MAX_COUNT = 3;
    private Vector3 target;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<PlayerController>().gameObject.transform; //Uncomment
        InvokeRepeating("LaunchProjectile", 2.0f, 1.0f);       
        projectileCount = 0;
        health = 3;
    }

    void LaunchProjectile()
    {
        if(/*player.isSleeping &&*/projectileCount<MAX_COUNT)
        {
            projectileCount++;
            var newBullet = Instantiate(projectile, transform.position + direction, Quaternion.identity);
            newBullet.owner = this;
            newBullet.direction = direction;
        }
    }

    public void BulletDestroyed()
    {
        projectileCount--;
    }

    // Update is called once per frame
    void Update()
    {
        target = player.position;
        direction = (target - transform.position).normalized;
        transform.right = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //NOTE uncomment after player projectile is created
        //if(collision.GetComponent<PlayerProjectile>)
        //{
        //    health--;
        //    if(health<=0)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
