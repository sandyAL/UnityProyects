using UnityEngine;
using System.Collections;

public class ChangeMaterial : MonoBehaviour {
    Material mat;
    public void currentMaterial(Material material)
    {
        int numChildren = this.transform.childCount;
        int numC;
        for (int k = 0; k < numChildren; k += 1)
        {
            Transform child = this.transform.GetChild(k);
            child.gameObject.GetComponent<Renderer>().material = material;
            numC = child.transform.childCount;
            for (int i = 0;i<numC;i+=1)
            {
                Transform grandChild = child.transform.GetChild(i);
                grandChild.gameObject.GetComponent<Renderer>().material = material;
            }
        }

    }
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
