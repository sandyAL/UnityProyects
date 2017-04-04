using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    Vector3 dragOrigin;
    public float mouseX;
    public float mouseY;
    public float factor=10;
    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleMouseMovment();
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
    }

    public void HandleMouseMovment()
    {
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("Pressed left click.");
            //Traslation
            float cameraTranslationX = (Input.mousePosition.x - mouseX) * factor * Time.deltaTime;
            float cameraTranslationY = (Input.mousePosition.y - mouseY) * factor * Time.deltaTime;
            //transform.Translate(cameraTranslationX, 0, cameraTranslationY);
            transform.position = transform.position + transform.rotation*(new Vector3(cameraTranslationX, cameraTranslationY, (float)1.0));
            //this.gameObject.transform.FindChild("MainCamera").gameObject.transform.Translate(cameraTranslationX, 0, 0);
        }


        if (Input.GetMouseButton(1))
        {
            //Debug.Log("Pressed right click.");
            //Rotation
            float cameraRotaionY = (Input.mousePosition.x - mouseX) * factor * Time.deltaTime;
            float cameraRotationX = (Input.mousePosition.y - mouseY) * factor * Time.deltaTime;
            transform.Rotate(0, cameraRotaionY, 0);
            this.gameObject.transform.FindChild("MainCamera").gameObject.transform.Rotate(cameraRotationX, 0, 0);
        }


        if (Input.GetMouseButton(2))
        {
            //Debug.Log("Pressed middle click.");
            //Zoom
        }

        if (Input.mouseScrollDelta[1] != 0)
        {
            transform.Translate(transform.rotation*(new Vector3(1,0,1)) * Time.deltaTime * Input.mouseScrollDelta[1]*factor);
        }
        Debug.Log(Input.mouseScrollDelta);

    }
}
