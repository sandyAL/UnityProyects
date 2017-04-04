using UnityEngine;
using System.Collections;

public class table : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnColisionEnter (Collision col)
    {
        Debug.Log("Table Collision Enter");
        //if (col.gameObject.name == "Body2")
        //{
        //    transform.GetComponent<Renderer>().material.color = Color.yellow;

        //}
    }

    void OnColisionStay(Collision col)
    {
        Debug.Log("Table Collision Stay");
    }

    void OnColisionExit(Collision col)
    {
        Debug.Log("Table Collision Exit");
    }


    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Table Trigger Enter");
        if (col.gameObject.name == "Body2")
        {
            transform.GetComponent<Renderer>().material.color = Color.red;

        }
    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log("Table Trigger Stay");
        if (col.gameObject.name == "Body2")
        {
            transform.GetComponent<Renderer>().material.color = Color.green;

        }
    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log("Table Trigger Exit");
        if (col.gameObject.name == "Body2")
        {
            transform.GetComponent<Renderer>().material.color = new Color((float)45/256, (float)20/256,(float)20 / 256);

        }

    }


}
