using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChargingSpace : MonoBehaviour {
    public GameObject hand1;
    public GameObject hand2;
    public bool isCarringObject = false;
    public string carringObjectName;
    string baseNameHand = "Hand";
    string baseNameHandle = "Handle";
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    hand1 = GameObject.Find(baseNameHand+"1");
        hand2 = GameObject.Find(baseNameHand+"2");
        Vector3 direction;
        if (hand1 != null && hand2 != null)
        {
            direction = hand2.transform.position - hand1.transform.position;
            float distance = Vector3.Magnitude(direction);

            //Rotate
            float step = 20 * Time.deltaTime;
            //Rotar el objeto cargado segun se rotan las manos
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);

            //Translate
            /*/ //Teletransport
            Vector3 position = (float)0.5 * (hand1.transform.position + hand2.transform.position);
            transform.position = position;
            /**/

            /**/ //Movimiento suave
            Vector3 target = 0.5F * (hand1.transform.position + hand2.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            /**/

            //Scalate
            Vector3 sizeBC = GetComponent<BoxCollider>().size;
            sizeBC.z = distance;
            sizeBC.x = 0.01F;
            sizeBC.y = 0.01F;
            GetComponent<BoxCollider>().size = sizeBC; ;
        }
        //else Debug.Log("Hand null");
        if (isCarringObject)
        {
            Transform child = gameObject.transform.GetChild(0);
            if (child.GetComponent<CollectableObject>().currentCollisionsNames.Count == 0 && child.gameObject.GetComponent<CollectableObject>().isCarriedByClaw)
            {
                child.transform.parent = null;
                isCarringObject = false;
                child.gameObject.GetComponent<CollectableObject>().isCarriedByClaw = false;
                carringObjectName = "";
                child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                //child.FindChild(baseNameHandle + "1").gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //child.FindChild(baseNameHandle + "2").gameObject.GetComponent<Rigidbody>().isKinematic = true;

            }
        
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.name[0] == 'H')
            return;
        Debug.Log(collision.gameObject.name);
        if (!isCarringObject)
        {
            Debug.Log("Transformar en hijo");
            Debug.Log(collision.gameObject.tag);
            if (collision.gameObject.tag == "Collectable")
            {
                Debug.Log(" Tiene tag Collectable");
                //TO-DO:: obtener del padre el componente collectableObject disco no lo tiene
                /*transform.parent.gameObject.GetComponent<CollectableObject>().currentCollisionsNames.Add(collision.gameObject.name);*/
                collision.gameObject.transform.parent.gameObject.GetComponent<CollectableObject>();
                /// collision.gameObject.transform.parent.gameObject.GetComponent<CollectableObject>();
                /// original //CollectableObject collectableObject = collision.gameObject.GetComponent<CollectableObject>();
                CollectableObject collectableObject = collision.gameObject.transform.parent.gameObject.GetComponent<CollectableObject>();
                List<string> collisionNames = collectableObject.currentCollisionsNames;
                if (collisionNames.Contains(baseNameHand + "1") && collisionNames.Contains(baseNameHand + "2"))
                {
                    Debug.Log("Touched by hands");
                    isCarringObject = true;
                    collectableObject.isCarriedByClaw = true;
                    //TO-DO:: hacer que el padre del padre sea transform
                    /// collision.gameObject.transform.parent.gameObject.transform.parent 
                    /// original //collision.gameObject.transform.parent = transform;
                    collision.gameObject.transform.parent.gameObject.transform.parent = transform;
                    //TO-DO:: nombre del padre DiscoX
                    /// 
                    /// original //carringObjectName = collision.gameObject.name;
                    carringObjectName = collision.gameObject.transform.parent.gameObject.name;
                    //TO-DO:: Hacer kinematic a todo el mundo
                    /// 
                    /// original //collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    collision.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    // TO-DO :: Deberia hacer kinematic tambien a las manijas??
                }
            }
        }
        else if( collision.gameObject.name == carringObjectName)
        {
            //Debug.Log("Stay collision");
            List<string> collisionNames = collision.gameObject.GetComponent<CollectableObject>().currentCollisionsNames;
            if (!collisionNames.Contains(baseNameHand + "1") || !collisionNames.Contains(baseNameHand + "2"))
            {
                collision.gameObject.transform.parent = null;
                isCarringObject = false;
                collision.gameObject.GetComponent<CollectableObject>().isCarriedByClaw = false;
                carringObjectName = "";
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                //collision.gameObject.transform.FindChild(baseNameHandle + "1").gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //collision.gameObject.transform.FindChild(baseNameHandle + "2").gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        
    }
}
