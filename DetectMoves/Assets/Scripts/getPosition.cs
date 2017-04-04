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

public class getPosition : MonoBehaviour {
	
	public GameObject SlipStreamObject;
    public Material[] ballMaterials = new Material[10];
    public int numMaterials = 10;
    public int windowSize = 50;
    public int numberOfSamples = 16;
    int frames;
    string nameBall;
    GameObject Trail;
    int numberCurrentBall;
    void Start () 
	{
        frames = -1;
        nameBall = "ball";
        numMaterials = 10;
        Trail = new GameObject();
        numberCurrentBall = -1;

        for (int i = 0; i < numberOfSamples * windowSize; i+=1)
        {
            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.name = nameBall + i.ToString();
            ball.GetComponent<Renderer>().material = ballMaterials[(i/windowSize) % numMaterials];
            ball.transform.parent = Trail.transform;
            ball.transform.localScale = new Vector3(0.005F, 0.005F, 0.005F);
        }
        SlipStreamObject.GetComponent<SlipStream>().PacketNotification += new PacketReceivedHandler(OnPacketReceived);
    }
	

	// packet received
	void OnPacketReceived(object sender, string Packet)
	{
        frames=(frames+1)%60;
        numberCurrentBall = (numberCurrentBall + 1) % (windowSize * numberOfSamples);
        //Debug.Log("On Received");
		XmlDocument xmlDoc= new XmlDocument();
		xmlDoc.LoadXml(Packet);

		XmlNodeList rigidBodiesList = xmlDoc.GetElementsByTagName("Body");
		int nRG=rigidBodiesList.Count;
        //Debug.Log(nRG);
        int i = 0;
        //Debug.Log("                 revisar si hay un tercer cuerpo rigido");
        if(nRG>i)
        {
            int id = System.Convert.ToInt32(rigidBodiesList[i].Attributes["ID"].InnerText);
            //POSITION
            int factor = 1;
            float x = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["x"].InnerText) * factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["y"].InnerText) * factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[i].Attributes["z"].InnerText) * factor;

            z = -z;

            Vector3 position = new Vector3(x, y, z);

            GameObject currentBall;
            string objectName = nameBall+ numberCurrentBall.ToString();
            currentBall = GameObject.Find(objectName);
            currentBall.transform.position = position;
            //Debug.Log(head);
           // head.transform.position = position;
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
