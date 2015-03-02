using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private PlayerPhysics playerPhysics;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
	}
	
	// Update is called once per frame
	void Update () {
		getInput ();
		checkCollisions ();
		movePlayer ();
	}

	private void movePlayer() {
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.move (amountToMove * Time.deltaTime);
	}

	private void setMove() {
	}

	private void getInput() {
		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = incrementSpeed (currentSpeed, targetSpeed, acceleration);
	}

	private void checkCollisions() {
		if (playerPhysics.isGrounded) {
			amountToMove.y = 0;
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;	
			}
		}
		if (playerPhysics.hasHitWall) {
			currentSpeed = 0;
			targetSpeed = 0;
		}
	}

	private float incrementSpeed(float currentSpeed, float targetSpeed, float acceleration){
		if (currentSpeed == targetSpeed){
			return currentSpeed;
		}
		float direction = Mathf.Sign(targetSpeed  - currentSpeed);
		currentSpeed += acceleration * Time.deltaTime * direction;
		if(direction == Mathf.Sign(targetSpeed - currentSpeed)){
			return currentSpeed;
		}
		return targetSpeed;
	}
}
