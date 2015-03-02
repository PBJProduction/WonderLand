using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
  public LayerMask collisionMask;
  [HideInInspector]
  public bool isGrounded;
  [HideInInspector]
  public bool hasHitWall;

  private BoxCollider collider;
  private Vector3 size;
  private Vector3 center;
  private Ray ray;
  private RaycastHit hit;
  private const float RAY_SPACE = .005f;
  private const int NUM_RAYCAST = 10;
  
  void Start () {
    collider = GetComponent<BoxCollider> ();
    size = collider.size;
    center = collider.center;
  }
  
  public void move (Vector2 moveAmount) {
    Vector2 position = transform.position;
    float deltaY = computeDeltaY (moveAmount.y, position);
    float deltaX = computeDeltaX (moveAmount.x, position);
    transform.Translate (new Vector2 (deltaX, deltaY));
  }

  private float computeDeltaX (float deltaX, Vector2 position) {
    hasHitWall = false;
    for (int i = 0; i < NUM_RAYCAST; ++i) {
      float direction = Mathf.Sign (deltaX);
      float x = position.x + center.x + size.x / 2 * direction;
      float y = position.y + center.y - size.y / 2 + size.y / (NUM_RAYCAST - 1) * i;
      
      ray = new Ray (new Vector2 (x, y), new Vector2 (direction, 0));
      Debug.DrawRay (ray.origin, ray.direction);
      
      if (Physics.Raycast (ray, out hit, Mathf.Abs (deltaX) + RAY_SPACE, collisionMask)) {
        hasHitWall = true;
        return computeDelta (direction);
      }
    }
    return deltaX;
  }

  private float computeDeltaY (float deltaY, Vector2 position) {
    isGrounded = false;
    for (int i = 0; i < NUM_RAYCAST; ++i) {
      float direction = Mathf.Sign (deltaY);
      float x = (position.x + center.x - size.x / 2) + size.x / (NUM_RAYCAST - 1) * i;
      float y = position.y + center.y + size.y / 2 * direction;
      
      ray = new Ray (new Vector2 (x, y), new Vector2 (0, direction));
      Debug.DrawRay (ray.origin, ray.direction);
      
      if (Physics.Raycast (ray, out hit, Mathf.Abs (deltaY) + RAY_SPACE, collisionMask)) {
        isGrounded = true;
        return computeDelta (direction);
      }
    }
    return deltaY;
  }

  private float computeDelta (float direction) {
    float distance = Vector3.Distance (ray.origin, hit.point);

    if (distance > RAY_SPACE) {
      return distance * direction - RAY_SPACE * direction;
    }
    return 0;
  }
}
