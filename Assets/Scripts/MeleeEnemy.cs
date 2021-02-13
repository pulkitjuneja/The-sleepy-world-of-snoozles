using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private Vector3 startingPosition;
    private Vector3 acceleration;
    private Vector3 direction;
    private Vector3 velocity;
    public float maxSpeed;
    public float mass;
    public GameObject player;
    public GameObject body;
    private float attackCD;
    private float health;
    private bool attack;
    private PlayerState state;

    private void Awake()
    {
        //body = player.GetComponent<PlayerController>().SleepingBody;
    }

    // Start is called before the first frame update
    private void Start()
    {
        startingPosition = transform.position;
        attack = false;
        state = player.GetComponent<PlayerController>().currentPlayerState;
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration * Time.deltaTime;
        startingPosition += velocity * Time.deltaTime;
        direction = velocity.normalized;
        acceleration = Vector3.zero;
        transform.position = startingPosition;
        gameObject.transform.up = direction;
        state = player.GetComponent<PlayerController>().currentPlayerState;
        body = GameObject.Find("SleepingBody(Clone)");
        if (state.GetType().ToString() == "AsleepState")
        {
            this.ApplyForce(Seek(body));
        }

        else
        {
            this.ApplyForce(Seek(player));
        }

        if (attack == true)
        {
            if (Time.time > attackCD)
            {
                // attack player
                float cd = 1f;
                attackCD = Time.time + cd;
            }
        }
    }

    public Vector3 Seek(Vector3 targetPosition)
    {
        // Step 1: Find DV (desired velocity)
        // TargetPos - CurrentPos
        Vector3 desiredVelocity = targetPosition - startingPosition;

        // Step 2: Scale vel to max speed
        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed);
        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * maxSpeed;

        // Step 3:  Calculate seeking steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        // Step 4: Return force
        return seekingForce;
    }

    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
        if (playerScript && state.GetType().ToString() != "AsleepState")
        {
            attack = true;
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }

        else if (state.GetType().ToString() == "AsleepState" && collision.gameObject == body)
        {
            attack = true;
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
        if (playerScript && state.GetType().ToString() != "AsleepState")
        {
            attack = false;
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }

        else if (state.GetType().ToString() == "AsleepState" && collision.gameObject == body)
        {
            attack = false;
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
