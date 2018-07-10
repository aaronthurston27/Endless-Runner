using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    private Player playerToFollow;
    private Vector3 startOffset;

	// Use this for initialization
	void Start () {
        playerToFollow = FindObjectOfType<Player>();
        if (!playerToFollow)
            Debug.LogWarning("Camera cannot find player. Will remain stationary");

        startOffset = this.transform.position - playerToFollow.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerToFollow)
        {
            Vector3 move = startOffset + playerToFollow.transform.position;
            move.y = startOffset.y;
            move.x = 0;
            this.transform.position = move;
        }
		
	}
}
