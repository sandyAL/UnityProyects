using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Table Trigger Enter");
        if (col.name == "Body2")
        {
            transform.parent = col.gameObject.transform;
        }
    }


    void OnTriggerStay(Collider col)
    {
        Debug.Log("Table Trigger Stay");
        
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("Table Trigger Exit");
        if (col.name == "Table")
        { 
            Transform parentTransform = transform.parent;
            transform.parent = null;
            // transform.parent = parentTransform;
        }
    }
}
