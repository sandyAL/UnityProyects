using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableObject : MonoBehaviour {
    public bool isCarriedByClaw = false;
    public bool isTouchingEnvironment = false;
    public List<string> currentCollisionsNames;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        string names = "";
        for (int i = 0; i < currentCollisionsNames.Count; i++)
            names = names + " " + currentCollisionsNames[i];
       // Debug.Log(names);
    }

    
    void OnCollisionEnter(Collision collision)
    {
        currentCollisionsNames.Add(collision.gameObject.name);
        //Debug.Log("Added "+collision.gameObject.name);
        /*string names = "";
        for (int i = 0; i < currentCollisionsNames.Count; i++)
            names = names + " " + collision.gameObject.name;*/
    }

    void OnCollisionExit(Collision collision)
    {
        //Debug.Log("oncollision")
       // Debug.Log("Removed " + collision.gameObject.name);
        currentCollisionsNames.Remove(collision.gameObject.name);
        /*string names = "";
        for (int i = 0; i < currentCollisionsNames.Count; i++)
            names = names + " " + collision.gameObject.name;*/
    }

}



/*switch (collision.gameObject.tag)
        {
            case "Environment" :
                {
                    isTouchingEnvironment = true;
                    //If is moved by claw
                    if(transform.parent!=null)
                    {
                        transform.parent = null;
                        GetComponent<Rigidbody>().isKinematic = false;
                        isCarriedByClaw = false;
                    }
                    break;
                }
            case "Claw":
                {
                    //Already andle in Claw
                    /*if (collision.gameObject.transform.childCount==0 && !isTouchingEnvironment)
                    {
                        this.transform.parent =collision.gameObject.transform;
                        GetComponent<Rigidbody>().isKinematic = true;
                        isCarriedByClaw = true;
                    }*//*
                    break;
                }
            case "Collectable":
                {
                    //Do nothing
                    break;
                }
            case "Movable":
                {
                    //Do nothing
                    break;
                }


        }*/
