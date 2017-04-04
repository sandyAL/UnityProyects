using UnityEngine;
using System.Collections;

public class Handle : MonoBehaviour {
    string baseName = "Hand";
    public string namecollision;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
        //Debug.Log("collisionEnter");
        //Debug.Log(collision.gameObject.name);
        namecollision = collision.gameObject.name;
        if(collision.gameObject.name == (baseName + "1") || collision.gameObject.name == (baseName + "2"))
        {
            //Debug.Log(transform.parent.gameObject.GetComponent<CollectableObject>().currentCollisionsNames );
            transform.parent.gameObject.GetComponent<CollectableObject>().currentCollisionsNames.Add(collision.gameObject.name);
        }
    }

    /*void OnTriggerEnter(Collider collision)
    {
        Debug.Log("triggerEnter");
        Debug.Log(collision.gameObject.name);
        namecollision = collision.gameObject.name;
        if (collision.gameObject.name == (baseName + "1") || collision.gameObject.name == (baseName + "2"))
        {
            //Debug.Log(transform.parent.gameObject.GetComponent<CollectableObject>().currentCollisionsNames );
            transform.parent.gameObject.GetComponent<CollectableObject>().currentCollisionsNames.Add(collision.gameObject.name);
        }
    }*/



    void OnCollisionStay(Collision collision)
    {
        //Debug.Log("collisionStay");
        namecollision = collision.gameObject.name;
        if (collision.gameObject.name == (baseName + "1") || collision.gameObject.name == (baseName + "2"))
        {
            ;
            //gameObject.transform.parent.gameObject.GetComponent<CollectableObject>().currentCollisionsNames.Add(collision.gameObject.name);
        }
    }

    
    void OnCollisionExit(Collision collision) {
       // Debug.Log("collisionExit");
        if (collision.gameObject.name == (baseName + "1") || collision.gameObject.name == (baseName + "2"))
        {
         //   Debug.Log("No colision mano-manija");
            transform.parent.gameObject.GetComponent<CollectableObject>().currentCollisionsNames.Remove(collision.gameObject.name);
        }
    }
}
