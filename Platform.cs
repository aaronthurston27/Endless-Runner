using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    #region enum

    public enum PlatformType
    {
        FLOOR,
        WALL_LEFT,
        WALL_RIGHT,
        CEILING
    };

    #endregion

    #region main variables

    public BoxCollider gameBoundaryCollider;

    public PlatformType type;

    #endregion

    #region monobehavior Methods

    void Start () {
        if (!gameBoundaryCollider)
            Debug.LogWarning("game boundary collider not set");
        this.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Platform Container").gameObject.transform, true);
	}
	
	void Update () {
		
	}
    #endregion

    #region methods

    #endregion
}
