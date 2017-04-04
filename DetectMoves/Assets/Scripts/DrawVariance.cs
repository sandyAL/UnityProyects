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

public class DrawVariance : MonoBehaviour {

    public GameObject SlipStreamObject;
    public Material[] ballMaterials = new Material[3];
    public int numMaterials = 3;
    public int windowSize = 10;
    public int numberOfBalls = 800;
    public double limInferiorVariance;
    public double limSuperiorVariance;
    string nameBall;
    GameObject[] Trail;
    int numberCurrentBall;
    int positionOnSampleBallArray;
    public Vector3[,] samplePositions;
    Vector3[] averagePosition = new Vector3[3];
    void Start () 
	{
        nameBall = "ball";
        numMaterials = 10;
        Trail = new GameObject[3];
        for(int k = 0; k<3;k++)
        {
            Trail[k] = new GameObject();
            Trail[k].name = "Trail" + k.ToString();
        }
        
        samplePositions = new Vector3[3,windowSize]; 
        numberCurrentBall = -1;
        positionOnSampleBallArray = -1;
        //averagePosition[0] = averagePosition[1] = averagePosition[2] = new Vector3(0.0F,0.0F,0.0F);
        for (int k= 0; k < 3; k++) { 
            for (int i = 0; i < numberOfBalls; i+=1)
            {
                GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                ball.name = nameBall+k.ToString()+ '_' + i.ToString();
                ball.GetComponent<Renderer>().material = ballMaterials[0];
                ball.transform.parent = Trail[k].transform;
                ball.transform.localScale = new Vector3(0.005F, 0.005F, 0.005F);
            }
        }
        SlipStreamObject.GetComponent<SlipStream>().PacketNotification += new PacketReceivedHandler(OnPacketReceived);
    }
	

	// packet received
	void OnPacketReceived(object sender, string Packet)
	{
        numberCurrentBall = (numberCurrentBall + 1) % numberOfBalls;
        positionOnSampleBallArray = (positionOnSampleBallArray + 1) % windowSize;
        //Debug.Log("On Received");
        XmlDocument xmlDoc= new XmlDocument();
		xmlDoc.LoadXml(Packet);

		XmlNodeList rigidBodiesList = xmlDoc.GetElementsByTagName("Body");
		int nRG=rigidBodiesList.Count;
        //Debug.Log(nRG);

        
        //nRG = 1;
        for (int k = 0;k<nRG;k+=1)
        {
            int id = System.Convert.ToInt32(rigidBodiesList[k].Attributes["ID"].InnerText);
            //POSITION
            int factor = 1;
            float x = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["x"].InnerText) * factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["y"].InnerText) * factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["z"].InnerText) * factor;

            //ROTATION
            float qx = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qx"].InnerText);
            float qy = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qy"].InnerText);
            float qz = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qz"].InnerText);
            float qw = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qw"].InnerText);


            //z = -z;
            x = -x;

            
            Vector3 newPosition = new Vector3(x, y, z);
            Vector3 oldPosition = samplePositions[k,positionOnSampleBallArray];
            Quaternion newOrientation = new Quaternion(qx, -qy, -qz, qw);
            if(k==0)
                { 
                Debug.Log(newOrientation.ToString());
                }
            averagePosition[k] = new Vector3( averagePosition[k].x + (newPosition.x - oldPosition.x) / windowSize , 
                                              averagePosition[k].y + (newPosition.y - oldPosition.y) / windowSize , 
                                              averagePosition[k].z + (newPosition.z - oldPosition.z) / windowSize );
            samplePositions[k,positionOnSampleBallArray] = newPosition;
            double sampleVariance = 0.0;
            for (int i = 0;i<windowSize;i+=1)
            {
                sampleVariance += Vector3.SqrMagnitude(samplePositions[k,i] - averagePosition[k]);
            }
            sampleVariance /= windowSize;

            GameObject currentBall;
            string objectName = nameBall + k.ToString() + '_' + numberCurrentBall.ToString();
            //Debug.Log(objectName);
            currentBall = GameObject.Find(objectName);
            currentBall.transform.position = newPosition;
            currentBall.transform.rotation = newOrientation;
            //Debug.Log("euler" + currentBall.transform.eulerAngles.ToString());
            //Debug.Log("local euler" + currentBall.transform.localEulerAngles.ToString());
            if (sampleVariance<limInferiorVariance)
            {
                currentBall.GetComponent<Renderer>().material = ballMaterials[0];
            }
            else if (sampleVariance>limSuperiorVariance)
            {
                currentBall.GetComponent<Renderer>().material = ballMaterials[2];
            }
            else
            {
                currentBall.GetComponent<Renderer>().material = ballMaterials[1];
            }

            /**/
            if (objectName == "ball0_0")
            {
                XmlNodeList rbMarkersList = xmlDoc.GetElementsByTagName("Marker" + id.ToString());
                int nMarkers = rbMarkersList.Count;
                //Debug.Log("RB"+id+": "+nMarkers +" ,"+ nMarkers2);

                for (int m = 0; m < nMarkers; m++)
                {
                    string markerName = objectName + "_Marker" + m.ToString();
                    GameObject marker;
                    marker = GameObject.Find(markerName);
                    if (marker == null)
                    {
                        marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);
                        marker.transform.localScale = scale;
                        marker.name = markerName;
                    }
                    marker.transform.parent = currentBall.transform;
                    float xM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["x"].InnerText) * factor;
                    float yM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["y"].InnerText) * factor;
                    float zM = (float)System.Convert.ToDouble(rbMarkersList[m].Attributes["z"].InnerText) * factor;
                    Vector3 markerPosition = new Vector3(-xM, yM, zM);

                    marker.transform.position = markerPosition;
                    
                }

            }
            /**/
        }


    }
	
	// Update is called once per frame
	void Update () 
	{
        ;
    }
    void OnPostRender()
    {
  
    }
}
