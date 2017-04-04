using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    public int one;
	// Use this for initialization
	void Start () {
        Debug.Log("Start");
    }
	
    // Use this to
    void Awake() {
        Debug.Log("Awake");
    }
        
	// Update is called once per frame
	void Update () {
        Debug.Log("Update time:"+Time.deltaTime);
    }

    void Fixpdate() {
        Debug.Log("Fixupdate time:" + Time.deltaTime);
    }
}
