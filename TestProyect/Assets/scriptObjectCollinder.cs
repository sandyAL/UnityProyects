using UnityEngine;
using System.Collections;

public class scriptObjectCollinder : MonoBehaviour {

    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    //void Update () {

    //}
    void OnColisionEnter(Collision col)
    {
        Debug.Log("Script Collision");
        //if (col.gameObject.name == "Body2")
        //{
        //    transform.GetComponent<Renderer>().material.color = Color.yellow;

//        }
    }


    void OnColisionStay(Collision col)
    {
        Debug.Log("Script Collision Stay");
    }

    void OnColisionExit(Collision col)
    {
        Debug.Log("Script Collision Exit");
    }
}
