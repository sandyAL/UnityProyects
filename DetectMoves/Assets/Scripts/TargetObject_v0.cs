using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject_v0 : MonoBehaviour {
    public bool isViewed;
    public bool isSelected;
    Behaviour halo;
    Material originalMaterial;
    Material selectedMaterial; 
	// Use this for initialization
	void Start () {
        isViewed = false;
        isSelected = false;
        halo = (Behaviour)GetComponent("Halo");
        originalMaterial = GetComponent<Renderer>().material;
        selectedMaterial = GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials[(int)MaterialColors.Red];
    }
	
	// Update is called once per frame
	void Update () {
        if (isSelected)
        {
            //halo.enabled = true; 
            GetComponent<Renderer>().material = selectedMaterial;
        }
        else
        {
            //halo.enabled = false;
            GetComponent<Renderer>().material = originalMaterial;
        }
	}
}
