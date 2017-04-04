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

public class Body : MonoBehaviour {
	
	public GameObject SlipStreamObject;
    public Color flourColor, objectColor;
    bool floorInitialized;
    MeshFilter mf;
    Mesh floor;
	// Use this for initialization
	void Start () 
	{
		SlipStreamObject.GetComponent<SlipStream>().PacketNotification += new PacketReceivedHandler(OnPacketReceived);
        flourColor = Color.white;
        objectColor = Color.blue;
        floorInitialized = false;
        floor = new Mesh();
        GetComponent<MeshFilter>().mesh = floor;
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
            Color bodyColor = Color.white;
            //Debug.Log(i);

            if (i == 1)
                bodyColor = objectColor;

            //ID
            int id = System.Convert.ToInt32(rigidBodiesList[i].Attributes["ID"].InnerText);
            //Debug.Log("ID:"+id.ToString());
            //POSITION
            int k = 10;
            float x = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["x"].InnerText) * k;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["y"].InnerText) * k;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["z"].InnerText) * k;
            //Debug.Log("x,y,z:" + x.ToString()+","+ y.ToString()+ "," +z.ToString());
            //ROTATION
            float qx = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qx"].InnerText);
            float qy = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qy"].InnerText);
            float qz = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qz"].InnerText);
            float qw = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["qw"].InnerText);
            //Debug.Log("qx,qy,qz,qw:" + qx.ToString() + "," + qy.ToString() + "," + qz.ToString()+"," + qw.ToString());
            //== coordinate system conversion (right to left handed) ==--
            z = -z;
            qz = -qz;
            qw = -qw;

            //== body pose ==--
            Vector3 position = new Vector3(x, y, z);
            Quaternion orientation = new Quaternion(qx, qy, qz, qw);


            //== locate or create bone object ==--
            string objectName = "Body" + id.ToString();

            GameObject body;

            body = GameObject.Find(objectName);

            if (body == null)
            {
                body = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);
                body.transform.localScale = scale;
                body.name = objectName;
                body.AddComponent<scriptObjectCollinder>();
            }

            //== set body's pose ==--
            body.transform.position = position;
            body.transform.rotation = orientation;
            body.GetComponent<Renderer>().material.color = bodyColor;
            //body.GetComponent<Collider>().isTrigger = true;
            //== get markers == --
            
            XmlNodeList rbMarkersList = xmlDoc.GetElementsByTagName("Marker"+ id.ToString());
            int nMarkers = rbMarkersList.Count;
            //Debug.Log("RB"+id+": "+nMarkers +" ,"+ nMarkers2);
            Vector3[] vertices =  new Vector3[nMarkers];
            //Vector2[] vertexs;
            for (int m=0;m<nMarkers;m++)
            {
                string markerName = objectName + "_Marker" + m.ToString();
                GameObject marker;
                marker = GameObject.Find(markerName);
                if (marker == null)
                {
                    marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Vector3 scale = new Vector3(0.4f, 0.4f, 0.4f);
                    marker.transform.localScale = scale;    
                    marker.name = markerName;
                }
                marker.transform.parent = body.transform;
                float xM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["x"].InnerText) * k;
                float yM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["y"].InnerText) * k;
                float zM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["z"].InnerText) * k;
                Vector3 markerPosition = new Vector3(xM, yM, -zM);
                vertices.SetValue(markerPosition, m);
                marker.transform.position = markerPosition;
                marker.GetComponent<Renderer>().material.color = bodyColor;
            }
            //Debug.Log(vertices);
            
            if (i == 0 && !floorInitialized)
            {

                floorInitialized= true;
     
                Vector3 max=Vector3.zero, min=Vector3.zero;

                for (int j = 0; j < vertices.Length; j++)
                {
                    max = Vector3.Max(max, vertices[j]);
                    min = Vector3.Min(min, vertices[j]);
                }

                
                Vector3[] vertFloor = new Vector3[] {
                    new Vector3(min[0],(float)-2.698733,-min[2]),
                    new Vector3(min[0],(float)-2.698733 ,-max[2]),
                    new Vector3(max[0],(float)-2.698733,-max[2]),
                    new Vector3(max[0],(float)-2.698733,-min[2]),
                    };

                Vector3[] norm = new Vector3[] {
                    Vector3.up,
                    Vector3.up,
                    Vector3.up,
                    Vector3.up,
                    };

                int[] trianglesIndices = new int[] {
                    0,1,2,
                    2,3,0
                };

                floor.vertices = vertFloor;
                floor.normals = norm;
                floor.triangles = trianglesIndices;
                
                GetComponent<MeshFilter>().mesh = floor;
                //GetComponent<MeshCollider>().
                body.transform.gameObject.AddComponent<MeshCollider>();
                body.transform.GetComponent<MeshCollider>().sharedMesh = floor;
                body.transform.gameObject.AddComponent<Rigidbody>
                //transform.gameObject.AddComponent<MeshCollider>();
                //transform.GetComponent<MeshCollider>().sharedMesh = floor;
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
        Debug.Log("onpostrender");
        Graphics.DrawMeshNow(floor, Vector3.zero, Quaternion.identity, 0);
    }

    void OnColisionEnter(Collision col)
    {
        Debug.Log("Body Collision");
        //if (col.gameObject.name == "Body2")
        //{
        //    transform.GetComponent<Renderer>().material.color = Color.yellow;

        //        }
    }


    void OnColisionStay(Collision col)
    {
        Debug.Log("Body Collision Stay");
    }

    void OnColisionExit(Collision col)
    {
        Debug.Log("Body Collision Exit");
    }
}
