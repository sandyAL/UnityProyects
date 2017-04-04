using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChargingSpace : MonoBehaviour {
    public GameObject hand1;
    public GameObject hand2;
    public bool isCarringObject = false;
    public string carringObjectName;
    string baseName = "Hand";
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    hand1 = GameObject.Find(baseName+"1");
        hand2 = GameObject.Find(baseName+"2");
        Vector3 direction;
        if (hand1 != null && hand2 != null)
        {
            direction = hand2.transform.position - hand1.transform.position;
            float distance = Vector3.Magnitude(direction);

            //Rotate
            //Vector3 targetDir = resta;
            float step = 20 * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0F);
            //Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
            //float dirX = Vector3.Angle(Vector3.right, direction);
            //if (dirX < 0) dirX = dirX + 360;
            //float dirY = Vector3.Angle(Vector3.up, direction);
            //if (dirY < 0) dirY = dirY + 360;
            //float dirZ = Vector3.Angle(Vector3.forward, direction);
            //if (dirZ < 0) dirZ = dirZ + 360;
            //transform.eulerAngles = (new Vector3(dirX, dirY, dirZ));
            //Debug.Log(dirX.ToString() +" "+ dirY.ToString() + " " + dirZ.ToString());
            //transform.rotation = Quaternion.Euler(dirX, dirY, dirZ);
            //transform.rotation = Quaternion.Euler(45, 90, 135);
            //transform.eulerAngles = (new Vector3(dirX,-dirY,-dirZ));
            //Translate
            Vector3 position = (float)0.5 * (hand1.transform.position + hand2.transform.position);
            transform.position = position;

            //Scalate
            Vector3 sizeBC = GetComponent<BoxCollider>().size;
            sizeBC.z = distance;
            //sizeBC.x = distance;
            //sizeBC.x = distance;
            GetComponent<BoxCollider>().size = sizeBC; ;
        }
        else Debug.Log("Hand null");
        Debug.Log(hand1.GetComponent<Rigidbody>().velocity.ToString()+" - "+ hand2.GetComponent<Rigidbody>().velocity.ToString());
    }

    void OnTriggerEnter(Collider collision)
    {
        /*if(!isCarringObject && collision.gameObject.tag=="Collectable")
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }*/
    }

    void OnTriggerStay(Collider collision)
    {
        if (!isCarringObject)
        {
            //Debug.Log("not carring object");
            if (collision.gameObject.tag == "Collectable")
            {
                CollectableObject collectableObject = collision.gameObject.GetComponent<CollectableObject>();
                List<string> collisionNames = collectableObject.currentCollisionsNames;
                if (collisionNames.Contains(baseName + "1") && collisionNames.Contains(baseName + "2"))
                {
                    //Debug.Log("Touched by hands");
                    isCarringObject = true;
                    collision.gameObject.transform.parent = transform;
                    carringObjectName = collision.gameObject.name;
                    collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
        else if( collision.gameObject.name == carringObjectName)
        {
            List<string> collisionNames = collision.gameObject.GetComponent<CollectableObject>().currentCollisionsNames;
            if (!collisionNames.Contains(baseName + "1") || !collisionNames.Contains(baseName + "2"))
            {
                collision.gameObject.transform.parent = null;
                isCarringObject = false;
                carringObjectName = "";
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        /*if(collision.gameObject.tag=="Collectable")
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }*/
    }
}
