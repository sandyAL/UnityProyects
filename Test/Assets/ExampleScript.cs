using UnityEngine;
using System.Collections;

public class ExampleScript : MonoBehaviour {

    //public GameObject myGameObject;
    public Transform myTransform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //myGameObject.transform.position = Vector3.up * Time.time;
        myTransform.position = Vector3.up * Time.time;
    }
}
