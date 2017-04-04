using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_v0 : MonoBehaviour {

    Vector3 originRay;
    Vector3 direction;
    GameObject fwd;
    RaycastHit hitInfo;
    public GameObject selectedObject;
    public string selectedObjectName;
	// Use this for initialization
	void Start () {
        fwd = GameObject.Find("fwd");
        selectedObjectName = "";
    }
	
	// Update is called once per frame
	void Update () {
        
        //Debug.Log(this.transform.parent.transform.position);
        //Debug.Log(transform.position);
        originRay = transform.position;
        direction = transform.forward;
        this.transform.GetChild(0).position = originRay + direction;
        //fwd.transform.position = originRay + transform.forward ;
        //Debug.DrawRay(originRay, direction*100.0f, Color.blue);
        //Lanzar el rayo
        //bool res = Physics.Raycast(originRay, direction, out hitInfo, 100.0f, mask);
        //bool res = Physics.Raycast(originRay, direction, out hitInfo);
        //Debug.Log (res);
        //Debug.Log(Physics.Raycast(originRay, direction, out hitInfo, 100.0f));
        //Debug.Log(Physics.Raycast(originRay, direction, out hitInfo));
        //
        if (Physics.Raycast(originRay, direction, out hitInfo,100.0f))
        {
            //Debug.Log(hitInfo.transform.gameObject.name);
            //Debug.Log(hitInfo.transform.gameObject.GetComponent<TargetObject_v0>().isSelected);
            if (selectedObjectName == "") // Si no hay un objeto seleccionado, selecciono el que provoco el hit
            {
                selectedObject = hitInfo.transform.gameObject;
                //Debug.Log(selectedObject.name);
                selectedObjectName = selectedObject.name;
                GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().selectedObjectName = selectedObjectName;
                selectedObject.GetComponent<TargetObject_v0>().isSelected = true;
            }
            else if (selectedObjectName != hitInfo.transform.gameObject.name) // si tengo un objeto seleccionado y es diferente al que hice hit, deselecciono el primero y selecciono el segundo
            {
                selectedObject.GetComponent<TargetObject_v0>().isSelected = false;
                selectedObject = hitInfo.transform.gameObject;
                selectedObjectName = selectedObject.name;
                GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().selectedObjectName = selectedObjectName;
                selectedObject.GetComponent<TargetObject_v0>().isSelected = true;
            }
        }
        
        /*
        //ACTIVAR ESTA PARTE SOLO PARA PRUEBAS DE SELECCION 
        else //Si no señalando uno lo deselecciona
        {
            selectedObject.GetComponent<TargetObject_v0>().isSelected = false;
            selectedObject = null;
            selectedObjectName = "";
        }*/

        // Si el rayo choca
        ////Si se tiene un objeto seleccionado
        //////si es diferente
        ////////deseleccionar el objeto seleccionado actual, quitar su halo
        ////////seleccionar el nuevo objeto, seleccionar su halo 
        ////////TODO PASAR EL NOMBRE AL DETECTOR DE MOVIMIENTOS O DESDE EL DETECTOR ACCEDER AQUI PARA OBTENER EL NOMBRE
        // Si el rayo no choca
        //// Si se tiene un objeto seleccionado deseleccionarlo (desabilitar el halo)
    }
}
