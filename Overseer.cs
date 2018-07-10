using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Overseer : MonoBehaviour {

    #region main variables

    public bool gameStarted = false;
    private Player player;
    private PlatformSpawner spawner;

    // The z-transform coordinate that when exceeds, resets the player and all platforms back to the origin.
    public float resetPosition = 500f;
    // Distance the player needs to travel before a checkpoint occurs. When a checkpoint is reached, reset the counter, increase the player speed, and make the platforms smaller.
    public float checkpointDistance = 500f;
    [System.NonSerializedAttribute]
    public float distanceTilCheckpoint;

    #endregion

    #region static reference and variables

    private static Overseer instance;

    public static Overseer Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<Overseer>();
            return instance;
        }
    }

    #endregion

    #region UI variables

    public Text scoreText;

    #endregion

    #region monobehavior methods

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player)
            Debug.LogError("Player Not Found!");
        gameStarted = true;
        distanceTilCheckpoint = checkpointDistance;
	}
	
	void Update () {

        scoreText.rectTransform.position = new Vector2(Screen.width - 5f, Screen.height - 15f);
        if(player && gameStarted)
        {
            scoreText.text = "Score: " + (int)player.score;

            if (player.transform.position.z >= resetPosition)
                StartCoroutine(ReturnToWorldOrigin());

            if (distanceTilCheckpoint <= 0f)
                CheckPoint();
        }

	}

    #endregion

    #region methods

    public void GameOver()
    {
        Debug.Log("Game Over!");
        gameStarted = false;
        Time.timeScale = 0;
    }

    private IEnumerator ReturnToWorldOrigin() // If the player goes too far from the origin, shift all platforms and player back to zero.
    {
        // Return player to origin.
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - PlatformSpawner.Instance.currentPlayerPlatform.transform.position.z);

        // Return platforms to origin.
        float previousDistance = 0;
        previousDistance = PlatformSpawner.Instance.platforms[1].transform.position.z - PlatformSpawner.Instance.platforms[0].transform.position.z;
        PlatformSpawner.Instance.platforms[0].transform.position -= new Vector3(0, 0, PlatformSpawner.Instance.platforms[0].transform.position.z);

        for(int i = 1; i < PlatformSpawner.Instance.platforms.Count; ++i)
        {
            if (previousDistance >= PlatformSpawner.Instance.maxJumpDistance * PlatformSpawner.Instance.maxLength * 2)
                previousDistance = 50f;

            float temp = 0;
            if(i < PlatformSpawner.Instance.platforms.Count - 1)
                temp = PlatformSpawner.Instance.platforms[i+1].transform.position.z - PlatformSpawner.Instance.platforms[i].transform.position.z;

            Platform next = PlatformSpawner.Instance.platforms[i];
            Platform prev = PlatformSpawner.Instance.platforms[i - 1];
            next.transform.position = new Vector3(next.transform.position.x, next.transform.position.y, previousDistance + prev.transform.position.z);
            previousDistance = temp;

            yield return new WaitForEndOfFrame();
        }

    }

    private void CheckPoint() // When a player reaches a specified score, increase the speed and make platforms smaller.
    {
        player.runningSpeed += player.runningSpeed * .25f;

        if (PlatformSpawner.Instance.platformScaling >= .25f)
            PlatformSpawner.Instance.platformScaling /= 2f;

        checkpointDistance *= 1.5f;
        distanceTilCheckpoint = checkpointDistance;
    }

    #endregion
}
