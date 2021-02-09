using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float playerSpeedAwake;
    public float playerSpeedAsleep;
    public Animator Animator;
    public Rigidbody2D Rigidbody2D;
    public SpriteRenderer SpriteRenderer;
    public GameObject SleepingBody;
    [HideInInspector] public GameObject SleepingBodyRef;
    public bool isInRaneOfBody;

    PlayerState currentPlayerState;

    private void Start() {
        currentPlayerState = new AwakeState();    
    }

    void Update () {
        currentPlayerState.Update(this);
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
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Respawn") {
            isInRaneOfBody = false;
            Debug.Log(isInRaneOfBody);
        }    
    }
}