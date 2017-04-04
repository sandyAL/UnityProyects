using UnityEngine;
using System.Collections;

public class Hand: MonoBehaviour {
    public bool isCarringObject = false;
    public GameObject body;
    // Use this for initialization
    void Start() {
        body = GameObject.Find("Body");
        
    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.tag == "Collectable" && !isCarringObject)
        {
            collision.gameObject.transform.parent = this.transform;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isCarringObject = true;
        }*/
        //TODO regresar isCarringObject a false
    }
}
