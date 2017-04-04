using UnityEngine;
using System.Collections;
using System.Xml;

public class VRHandsStatusData_v0 : MonoBehaviour {

    public event ChangeHandsStatusHandler UpdateHandsStatus;
    
    HandStatus currentStatus = new HandStatus();
    bool sendUpdate = false;
    bool stableStatus = false;
    int countFramesIquals = 0;
    public GameObject SlipStreamObject;
    public Material[] ballMaterials = new Material[3];
    public int numMaterials = 3;
    public int windowSize = 10;
    public int sameFrameWindowSaze;
    public int numberOfBalls = 800;
    public double limInferiorVariance=0.0001;
    public double limSuperiorVariance=0.0007;
    public double limInferiorDistance = 0.001;
    public double limSuperiorDistance = 0.01;
    string nameBall;
    GameObject[] Trail;
    int numberCurrentBall;
    int positionOnSampleBallArray;
    public Vector3[,] samplePositions;
    Vector3[] averagePosition = new Vector3[3];
    public string[] handId = new string[2] { "R", "L" };
    int IDStatus = 0;
    public int speed = 20;
    int frames;
    public int numFramesToActualizeCameraPosition = 1;
    int factor = 1;
    void Start () 
	{
        sameFrameWindowSaze = windowSize;
        frames = -1;
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
        frames = (frames + 1) % numFramesToActualizeCameraPosition;
        numberCurrentBall = (numberCurrentBall + 1) % numberOfBalls;
        positionOnSampleBallArray = (positionOnSampleBallArray + 1) % windowSize;
        //Debug.Log("On Received");
        XmlDocument xmlDoc= new XmlDocument();
		xmlDoc.LoadXml(Packet);

		XmlNodeList rigidBodiesList = xmlDoc.GetElementsByTagName("Body");
		int nRG=rigidBodiesList.Count;
        //Debug.Log(nRG);

        
        //nRG = 1;
        for (int k = 0;k<2;k+=1)
        {
            int id = System.Convert.ToInt32(rigidBodiesList[k].Attributes["ID"].InnerText);
            //POSITION
            
            float x = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["x"].InnerText) * factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["y"].InnerText) * factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["z"].InnerText) * factor;
            //ROTATION
            float qx = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qx"].InnerText);
            float qy = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qy"].InnerText);
            float qz = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qz"].InnerText);
            float qw = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qw"].InnerText);

            //z = -z;
            
            Vector3 newPosition = new Vector3(-x, y, z);
            //Quaternion newOrientation = new Quaternion(qx, qy, -qz, -qw);
            Quaternion newOrientation = new Quaternion(qx, -qy, -qz, qw);
            Vector3 oldPosition = samplePositions[k,positionOnSampleBallArray];
            
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
            GameObject hand;

            string objectName = nameBall + k.ToString() + '_' + numberCurrentBall.ToString();
            string handName = handId[k] + "Hand";
            
            currentBall = GameObject.Find(objectName);
            hand = GameObject.Find(handName);
            
            hand.transform.position = newPosition;
            hand.transform.rotation = newOrientation;
            
            currentBall.transform.position = newPosition;
            currentStatus.handPosition[k] = newPosition;
            if (sampleVariance < limInferiorVariance)
            {
                currentBall.GetComponent<Renderer>().material = ballMaterials[0];
                if (currentStatus.handMoving[k])
                {
                    currentStatus.handMoving[k] = false;
                    sendUpdate = true;
                    countFramesIquals = 0;
                }


            }
            else
            {
                if (sampleVariance > limSuperiorVariance)
                    {
                    currentBall.GetComponent<Renderer>().material = ballMaterials[2];
                    }
                else
                    {
                    currentBall.GetComponent<Renderer>().material = ballMaterials[1];
                    //currentStatus.handMoving[k] = true;
                    }

                if (!currentStatus.handMoving[k])
                    { 
                    currentStatus.handMoving[k] = true;
                    countFramesIquals = 0;
                    sendUpdate = true;
                    }
            }
            
        }
        countFramesIquals += 1;
        //TO-DO Update hand direction 
        //Debug.Log(UpdateHandsStatus);
        if (sendUpdate && countFramesIquals > sameFrameWindowSaze && UpdateHandsStatus != null)
        {
            
            currentStatus.id = IDStatus;
            IDStatus += 1;
            //currentStatus.distance = Vector3.SqrMagnitude(currentStatus.handPosition[0] - currentStatus.handPosition[1]);
            
            float dist0 = Vector3.Distance(samplePositions[0, 0], samplePositions[1, 0]);
            //Debug.Log("0.0");
            float dist9 = Vector3.Distance(samplePositions[0, 9], samplePositions[1, 9]);
            //Debug.Log(dist0.ToString() +"  "+ dist9.ToString());
            float difDist = dist0 - dist9;
            //Debug.Log(difDist);
            //Debug.Log("0");
            if (difDist<limInferiorDistance)
            {
                currentStatus.distance = -1;
            }
            else
            {
                if (difDist > limSuperiorDistance)
                    currentStatus.distance = 1;
                else
                    currentStatus.distance = 0;
            }
            //Debug.Log("1");
            UpdateHandsStatus(this, currentStatus);
            sendUpdate = false;
            //Debug.Log(countFramesIquals);
        }
        //UPDATE CAMERA
        int headID = 2;
        if(nRG>headID)
        {
            Debug.Log("update camera position");
            int id = System.Convert.ToInt32(rigidBodiesList[headID].Attributes["ID"].InnerText);
            //POSITION
            //int factor = 10;
            //int factor = 2;
            float x = (float)System.Convert.ToDouble(rigidBodiesList[headID].Attributes["x"].InnerText) * factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[headID].Attributes["y"].InnerText) * factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[headID].Attributes["z"].InnerText) * factor;

            //habia cambiado -x y -z
            z = -z;

            Vector3 position = new Vector3(-x, y, -z);

            GameObject head;
            string objectName = "VRDisplayTracked";
            head = GameObject.Find(objectName);
            //Debug.Log(head);
            //head.transform.position = position;
            head.transform.position = Vector3.MoveTowards(head.transform.position, position, speed * Time.deltaTime);
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
