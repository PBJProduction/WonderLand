using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameCamera))]
public class GameManager : MonoBehaviour {
	
	public GameObject player;
	private GameCamera mainCamera;

	
	void Start () {
    mainCamera = GetComponent<GameCamera>();
		setupPlayer();
	}
	
	private void setupPlayer() {
    mainCamera.SetTarget((Instantiate(player,Vector3.zero,Quaternion.identity) as GameObject).transform);
	}
}
