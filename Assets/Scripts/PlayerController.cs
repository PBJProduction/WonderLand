using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;

  private float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private PlayerPhysics playerPhysics;
  private Animator animator;
  private bool isJumping;

	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
    animator = GetComponent<Animator> ();
	}
	
  void Update () {
    animatePlayer ();
		getInput ();
		checkCollisions ();
    movePlayer ();
	}

  private void animatePlayer() {
    animationSpeed = incrementSpeed (animationSpeed, Mathf.Abs (targetSpeed), acceleration);
    animator.SetFloat ("Speed", animationSpeed);
  }

	private void movePlayer () {
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
    playerPhysics.move (amountToMove * Time.deltaTime);
    changeDirections ();
	}

	private void getInput () {
    targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = incrementSpeed (currentSpeed, targetSpeed, acceleration);
  }

  private void changeDirections() {
    float direction = Input.GetAxisRaw("Horizontal");
    if (direction != 0) {
      if(direction < 0) {
        transform.eulerAngles = Vector3.up * 180;
      }
      else {
        transform.eulerAngles = Vector3.zero;
      }
    }
  }

	private void checkCollisions () {
		if (playerPhysics.isGrounded) {
			amountToMove.y = 0;
      if(isJumping) {
        isJumping = false;
        animator.SetBool("isJumping", isJumping);
      }
			if (Input.GetButtonDown ("Jump")) {
				amountToMove.y = jumpHeight;	
        isJumping = true;
        animator.SetBool("isJumping", isJumping);
			}
		}
		if (playerPhysics.hasHitWall) {
			currentSpeed = 0;
			targetSpeed = 0;
		}
	}

	private float incrementSpeed (float currentSpeed, float targetSpeed, float acceleration) {
		if (currentSpeed == targetSpeed) {
			return currentSpeed;
		}
		float direction = Mathf.Sign (targetSpeed - currentSpeed);
		currentSpeed += acceleration * Time.deltaTime * direction;
		if (direction == Mathf.Sign (targetSpeed - currentSpeed)) {
			return currentSpeed;
		}
		return targetSpeed;
	}
}
