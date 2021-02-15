using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float playerSpeedAwake;
    public float playerSpeedAsleep;
    public Animator Animator;
    public Rigidbody2D Rigidbody2D;
    public SpriteRenderer SpriteRenderer;
    public GameObject SleepingBody;
    [HideInInspector] public GameObject SleepingBodyRef;
    public bool isInRaneOfBody;

    public Transform aimTransform;
    public Transform projectileSpawnTransform;
    public GameObject playerProjectilePrefab;

    public PlayerState currentPlayerState;

    public float health;
    [HideInInspector] public bool attacking;

    public HealthBar healthBar;

    public void Awake()
    {
        currentPlayerState = new AwakeState();

    }
    private void Start() {
        health = 10;
        healthBar.SetMaxHealth(health);
    }

    void Update () 
    {
        currentPlayerState.Update(this);
        healthBar.SetHealth(health);
        if(health<=0)
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    public void ChangeState (PlayerState previousState, PlayerState newState) {
        previousState.Exit(this, newState);
        newState.Enter(this, previousState);
        currentPlayerState = newState;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Respawn") {
            isInRaneOfBody = true;
        }

        if (other.GetComponent<EnemyProjectile>())
        {
            health--;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Respawn") {
            isInRaneOfBody = false;
            Debug.Log(isInRaneOfBody);
        }    
    }

    public void SpawnProjectilePrefab()
    {
        Instantiate(playerProjectilePrefab, projectileSpawnTransform.transform.position, projectileSpawnTransform.transform.rotation);
    }
}