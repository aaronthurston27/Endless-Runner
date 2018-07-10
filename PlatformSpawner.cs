using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    #region prefabs

    [SerializeField]
    private Platform[] platformPrefabs;
    
    #endregion

    #region main variables

    public Platform currentPlayerPlatform;
    public Platform nextPlatform;

    public List<Platform> platforms;
    public Obstruction[] obstructions;

    public float maxLength = 10; // Max platform length.
    public float maxJumpDistance = 5; // Max distance between each floor platform.
    public bool spawnObstructions; // Is the platform spawner allowed to spawn obstructions?
    public int maxObstructions = 2;

    public float platformScaling = 1;
    #endregion

    #region static reference and variables

    private static PlatformSpawner instance;

    public static PlatformSpawner Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<PlatformSpawner>();
            return instance;
        }
    }

    #endregion

    #region monobehavior methods

    void Start()
    {
        if(!currentPlayerPlatform)
        {
            Debug.Log("Starting platform not set!");
        }
        else
        {
            platforms.Add(currentPlayerPlatform);
        }

        foreach (Platform k in FindObjectsOfType<Platform>())
        {
            if(!platforms.Contains(k))
                platforms.Add(k);
        }

    }
	void Update ()
    {
        if(platforms.Count < 5) // If we start running out of platforms, instantiate new ones.
        {
            CreatePlatform();
        }
    }
	
    #endregion

    #region methods

    private void CreatePlatform()
    {
        Platform toCreate = Instantiate(platformPrefabs[0]); // Right now, we are only spawning floor platforms.
        toCreate.gameObject.SetActive(true);
        toCreate.transform.localScale = new Vector3(toCreate.transform.localScale.x * platformScaling, 1f, (platformScaling) * Random.Range(1, maxLength));
        if(platforms.Count >= 1)
        {
            Platform last = platforms[platforms.Count - 1]; // Most recent platform. We want to create a new platform and place it in front of this one.
            float edge = last.GetComponent<MeshCollider>().bounds.size.z / 2; // Size of the most recent platform.
            float newEdge = toCreate.GetComponent<MeshCollider>().bounds.size.z / 2; // Size of newly created platform. Changes as scale increases.
            toCreate.transform.position = new Vector3(0f, 0f, last.transform.position.z + edge + Random.Range(3.5f, maxJumpDistance) + newEdge);
            platforms.Add(toCreate);
        }
        if (spawnObstructions)
        {
            int numberOfObstructions = Random.Range(2, maxObstructions);
            while (numberOfObstructions > 0)
            {
                StartCoroutine(SpawnObstruction(toCreate));
                --numberOfObstructions;
            }
        }
        
    }

    private IEnumerator SpawnObstruction(Platform parentPlatform)
    {
        yield return new WaitForEndOfFrame();
        if (parentPlatform)
        {
            Obstruction toSpawn = Instantiate(obstructions[Random.Range(0, obstructions.Length)]); // Obstruction we want to spawn on our platform.
            toSpawn.transform.SetParent(parentPlatform.transform);
            Vector3 bounds = parentPlatform.gameObject.GetComponent<MeshCollider>().bounds.size;
            Vector3 spawnedTransform = new Vector3(0, 0, 0);
            spawnedTransform.x = Random.Range(parentPlatform.transform.position.x - (bounds.x / 2), parentPlatform.transform.position.x + (bounds.x / 2));
            spawnedTransform.z = Random.Range(parentPlatform.transform.position.z - (bounds.z / 2), parentPlatform.transform.position.z + (bounds.z / 2));
            toSpawn.transform.position = spawnedTransform;
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion
}
