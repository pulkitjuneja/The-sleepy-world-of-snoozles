using UnityEngine;


public abstract class PlayerState {
    public abstract void Enter (PlayerController playerController, PlayerState previousState);
    public abstract void Exit (PlayerController playerController, PlayerState nextState);
    public abstract void Update (PlayerController playerController);

}

public class AwakeState : PlayerState {
    float cooldownTimer;

    public override void Enter(PlayerController playerController, PlayerState previousState)
    {
        string previousStateType = previousState.GetType().ToString();
        if(previousStateType == "AsleepState") {
            cooldownTimer = 30.0f;
        }

    }

    public void Move (PlayerController playerController) {

        float SpeedX = Input.GetAxis("Horizontal");
        float SpeedY = Input.GetAxis("Vertical");

        Vector3 speedNormalized = new Vector3(SpeedX, SpeedY, 0.0f).normalized;  

        if( Mathf.Abs(SpeedX) > 0 || Mathf.Abs(SpeedY) > 0) {
            playerController.Animator.SetFloat("DirectionX", SpeedX);
            playerController.Animator.SetFloat("DirectionY", SpeedY);
        }

        if(SpeedX<0)
        {
            playerController.SpriteRenderer.flipX = true; ;
        }

        if (SpeedX > 0)
        {
            playerController.SpriteRenderer.flipX = false;
        }

        playerController.Animator.SetFloat("Speed", speedNormalized.magnitude);
        playerController.Rigidbody2D.MovePosition(playerController.transform.position + 
            
            speedNormalized * playerController.playerSpeedAwake);

    }

    public override void Update (PlayerController playerController) {
        if(cooldownTimer > 0) {
            cooldownTimer -= Time.deltaTime;
        }
        Move(playerController);
        Attack(playerController);
        TransitionToSleep(playerController);
        
    }

    public void TransitionToSleep (PlayerController playerController) {
        if(Input.GetKeyDown(KeyCode.C) && cooldownTimer <=0) {
            playerController.ChangeState(this, new AsleepState());
        }
    }

    public override void Exit (PlayerController playerController, PlayerState nextState) {
            playerController.SleepingBodyRef = GameObject.Instantiate(playerController.SleepingBody, 
                playerController.transform.position, playerController.SleepingBody.transform.rotation);
    }

    public void Attack(PlayerController playerController)
    {
        if (Input.GetMouseButtonUp(0))
        {
            playerController.Animator.SetTrigger("PlayerAttack");
            playerController.attacking = true;
        }

        else
        {
            playerController.attacking = false;
        }
    }

}

public class AsleepState : PlayerState {

    float SleepStateTimer;    
    public override void Enter (PlayerController playerController, PlayerState previousState) {
        SleepStateTimer = 20.0f;
        playerController.SpriteRenderer.color = Color.blue;
        
    }

    public void Move (PlayerController playerController) {

        float SpeedX = Input.GetAxis("Horizontal");
        float SpeedY = Input.GetAxis("Vertical");

        Vector3 speedNormalized = new Vector3(SpeedX, SpeedY, 0.0f).normalized;  

        if( Mathf.Abs(SpeedX) > 0 || Mathf.Abs(SpeedY) > 0) {
            playerController.Animator.SetFloat("DirectionX", SpeedX);
            playerController.Animator.SetFloat("DirectionY", SpeedY);
        }

        playerController.Animator.SetFloat("Speed", speedNormalized.magnitude);
        playerController.Rigidbody2D.MovePosition(playerController.transform.position +  speedNormalized * playerController.playerSpeedAsleep);
    }

    public override void Update(PlayerController playerController)
    {
        Move(playerController);
        ProcessAimAndFire(playerController);
        AttemptWakeUp(playerController);
        SleepStateTimer -= Time.deltaTime;
        if(SleepStateTimer <= 0) {
            playerController.ChangeState(this, new AwakeState());
        }
    }

    public void AttemptWakeUp (PlayerController playerController) {
        if(Input.GetKeyDown(KeyCode.C) &&  playerController.isInRaneOfBody) {
            playerController.ChangeState(this, new AwakeState());
        }
    }

    public override void Exit(PlayerController playerController, PlayerState nextState)
    {
        playerController.SpriteRenderer.color = Color.white;
        playerController.transform.position = playerController.SleepingBodyRef.transform.position;
        GameObject.Destroy(playerController.SleepingBodyRef);
    }

    public void ProcessAimAndFire(PlayerController playerController)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - playerController.transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        playerController.aimTransform.eulerAngles = new Vector3(0f, 0f, angle);
        if (Input.GetMouseButtonDown(0))
        {
            playerController.SpawnProjectilePrefab();
        }

    }
}
