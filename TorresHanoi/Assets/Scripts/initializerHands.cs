using UnityEngine;
using System.Collections;
using System.Xml;

//=============================================================================----
// Copyright © NaturalPoint, Inc. All Rights Reserved.
// 
// This software is provided by the copyright holders and contributors "as is" and
// any express or implied warranties, including, but not limited to, the implied
// warranties of merchantability and fitness for a particular purpose are disclaimed.
// In no event shall NaturalPoint, Inc. or contributors be liable for any direct,
// indirect, incidental, special, exemplary, or consequential damages
// (including, but not limited to, procurement of substitute goods or services;
// loss of use, data, or profits; or business interruption) however caused
// and on any theory of liability, whether in contract, strict liability,
// or tort (including negligence or otherwise) arising in any way out of
// the use of this software, even if advised of the possibility of such damage.
//=============================================================================----

// Attach Body.cs to an empty Game Object and it will parse and create visual
// game objects based on bon edata.  Body.cs is meant to be a simple example 
// of how to parse and display skeletal data in Unity.

// In order to work properly, this class is expecting that you also have instantiated
// another game object and attached the Slip Stream script to it.  Alternatively
// they could be attached to the same object.

public class initializerHands : MonoBehaviour {
	
	public GameObject SlipStreamObject;
    public Material handMaterial;
    //public Color flourColor, objectColor;
    //bool floorInitialized;
    //MeshFilter mf;
    //Mesh floor;

    void Start () 
	{
		SlipStreamObject.GetComponent<SlipStream>().PacketNotification += new PacketReceivedHandler(OnPacketReceived);
        GameObject chargingSpace = new GameObject();
        chargingSpace.name = "chargingSpace";
        chargingSpace.AddComponent<BoxCollider>();
        chargingSpace.GetComponent<BoxCollider>().isTrigger = true;
        chargingSpace.AddComponent<ChargingSpace>();
    }
	

	// packet received
	void OnPacketReceived(object sender, string Packet)
	{
        //Debug.Log("On Received");
		XmlDocument xmlDoc= new XmlDocument();
		xmlDoc.LoadXml(Packet);

		XmlNodeList rigidBodiesList = xmlDoc.GetElementsByTagName("Body");
		int nRG=rigidBodiesList.Count;
        //Debug.Log(nRG);

        for (int i = 0; i < nRG; i++)
        {
           
            //Debug.Log(i);

            //ID
            int id = System.Convert.ToInt32(rigidBodiesList[i].Attributes["ID"].InnerText);
            //Debug.Log("ID:"+id.ToString());
            //POSITION
            //int factor = 10;
            int factor = 1;
            float x = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["x"].InnerText) * factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["y"].InnerText) * factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["z"].InnerText) * factor;
            //Debug.Log("x,y,z:" + x.ToString()+","+ y.ToString()+ "," +z.ToString());
            //ROTATION
            float qx = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qx"].InnerText);
            float qy = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qy"].InnerText);
            float qz = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qz"].InnerText);
            float qw = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qw"].InnerText);
            //Debug.Log(i.ToString()+": "+"qx,qy,qz,qw:" + qx.ToString() + "," + qy.ToString() + "," + qz.ToString()+"," + qw.ToString());
            //== coordinate system conversion (right to left handed) ==--
            z = -z;
            qz = -qz;
            qw = -qw;

            //== body pose ==--
            Vector3 position = new Vector3(x, y, z);
            Quaternion orientation = new Quaternion(qx, qy, qz, qw);


            //== locate or create bone object ==--
            string objectName = "Hand" + id.ToString();

            GameObject body;

            body = GameObject.Find(objectName);

            if (body == null)
            {
                body = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 scale = new Vector3(0.02f, 0.02f, 0.02f);
                body.transform.localScale = scale;
                body.name = objectName;
                //TODO add the physics rigid body
                //body.AddComponent<scriptObjectCollinder>();
                Rigidbody rbB = body.AddComponent<Rigidbody>();
                rbB.isKinematic = true;
                rbB.mass = 0.001F;
                //TODO crear la camara desde el punto de vista del RG
                body.AddComponent < Hand > ();
                body.GetComponent<Renderer>().material = handMaterial;
            }

            //== set body's pose ==--
            body.transform.position = position;
            body.transform.rotation = orientation;
            
            
            //== get markers == --
            
            XmlNodeList rbMarkersList = xmlDoc.GetElementsByTagName("Marker"+ id.ToString());
            int nMarkers = rbMarkersList.Count;
            //Debug.Log("RB"+id+": "+nMarkers +" ,"+ nMarkers2);
            
            for (int m=0;m<nMarkers;m++)
            {
                string markerName = objectName + "_Marker" + m.ToString();
                GameObject marker;
                marker = GameObject.Find(markerName);
                if (marker == null)
                {
                    marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Vector3 scale = new Vector3(0.04f, 0.04f, 0.04f);
                    marker.transform.localScale = scale;    
                    marker.name = markerName;
                }
                marker.transform.parent = body.transform;
                float xM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["x"].InnerText) * factor;
                float yM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["y"].InnerText) * factor;
                float zM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["z"].InnerText) * factor;
                Vector3 markerPosition = new Vector3(xM, yM, -zM);
                
                marker.transform.position = markerPosition;
                
            }
            
            
           
            
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
        ;
    }
    void OnPostRender()
    {
  //      Debug.Log("onpostrender");
  //      Graphics.DrawMeshNow(floor, Vector3.zero, Quaternion.identity, 0);
    }

    void OnColisionEnter(Collision col)
    {
  //      Debug.Log("Body Collision");
        //if (col.gameObject.name == "Body2")
        //{
        //    transform.GetComponent<Renderer>().material.color = Color.yellow;

        //        }
    }


    void OnColisionStay(Collision col)
    {
  //      Debug.Log("Body Collision Stay");
    }

    void OnColisionExit(Collision col)
    {
  //      Debug.Log("Body Collision Exit");
    }
}
