using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daynight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    /// <summary>
    /// Method for roating movment for the light 
    /// </summary>
    void Update () {
        transform.RotateAround(Vector3.zero, Vector3.right, Time.deltaTime);
        transform.LookAt(Vector3.zero);
	}
}
