using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class VRHandsStatusData_v3 : MonoBehaviour {

    public event ChangeHandsStatusHandler UpdateHandsStatus;
    
    HandStatus currentStatus = new HandStatus();
    Material[] handsMaterials;
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
    public double headSize;
    string nameBall;
    //GameObject[] Trail;
    int numberCurrentBall;
    int positionOnSampleBallArray; //Va recorriendo el arreglo de samplePosition actualizando de forma circular //Se actualiza al recibir un nuevo frame // indica la posicion actualizada 
    public Vector3[,] samplePositions; //Guarda las ultimas windowSize posiciones de las manos [2,windowSize]
    Vector3[] averagePosition = new Vector3[3];
    public string[] handId = new string[2] { "R", "L" };
    int IDStatus = 0;
    public int speed = 20;
    int frames;
    int numberFramesReceived;
    public int numFramesToActualizeCameraPosition = 1;
    int factor = 1;
    GameObject head;

    GameObject vrClassifyMoves;
    int angleThresholdOneHand; 
    int angleThresholdTwoHands;
    int directionThresholdOneHand;
    GameObject RHand;
    public int handColor;
    Vector3 headPosition;
    GameObject headCamera;
    void Start () 
	{
        Debug.Log("Start VRHandsStatusData_v2");
        handsMaterials = GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials;
        vrClassifyMoves = GameObject.Find("MovesClassifier");
        Debug.Log(vrClassifyMoves);
        angleThresholdOneHand = vrClassifyMoves.GetComponent<VRClassifyMoves_v3>().angleThresholdOneHand;
        angleThresholdTwoHands = vrClassifyMoves.GetComponent<VRClassifyMoves_v3>().angleThresholdTwoHands;
        directionThresholdOneHand = vrClassifyMoves.GetComponent<VRClassifyMoves_v3>().directionThresholdOneHand;
        RHand = GameObject.Find("rightHand");
        handColor = (int) MaterialColors.White;
        sameFrameWindowSaze = windowSize;
        frames = -1;
        numberFramesReceived = 0;
        nameBall = "ball";
        numMaterials = 10;
        /*Trail = new GameObject[3];
        for(int k = 0; k<3;k++)
        {
            Trail[k] = new GameObject();
            Trail[k].name = "Trail" + k.ToString();
        }*/
        
        samplePositions = new Vector3[3,windowSize]; 
        numberCurrentBall = -1;
        positionOnSampleBallArray = -1;
        
        /*for (int k= 0; k < 3; k++) { 
            for (int i = 0; i < numberOfBalls; i+=1)
            {
                GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                ball.name = nameBall+k.ToString()+ '_' + i.ToString();
                ball.GetComponent<Renderer>().material = ballMaterials[0];
                ball.transform.parent = Trail[k].transform;
                ball.transform.localScale = new Vector3(0.005F, 0.005F, 0.005F);
            }
        }*/
        SlipStreamObject.GetComponent<SlipStream>().PacketNotification += new PacketReceivedHandler(OnPacketReceived);
        string objectName = "VRDisplayTracked";
        head = GameObject.Find(objectName);
        headCamera = GameObject.Find("VRViewer0");
    }

    public int centerOneHand = 270;
	// packet received
	void OnPacketReceived(object sender, string Packet)
	{
        numberFramesReceived = numberFramesReceived + 1;
        /*if (numberFramesReceived % 2 == 1)
            return;*/
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
        //HANDS POSITIONING
        for (int k = 0; k < 2; k += 1)
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
            /*if (k == 0) { 
                File.AppendAllText("output.txt", newOrientation.eulerAngles.x.ToString()+"\t"+ newOrientation.eulerAngles.y.ToString() + "\t" + newOrientation.eulerAngles.z.ToString() + "\n");
            }*/
            if(k==0)
            {
                if ((newOrientation.eulerAngles.z > (centerOneHand - angleThresholdOneHand) && newOrientation.eulerAngles.z < (centerOneHand + angleThresholdOneHand)) && (newOrientation.eulerAngles.x <(-10+angleThresholdOneHand)  || newOrientation.eulerAngles.x > (350-angleThresholdOneHand)) )
                {
                    if(handColor == (int)MaterialColors.White)
                        RHand.GetComponent<ChangeMaterial>().currentMaterial(handsMaterials[(int)MaterialColors.Green]);
                }
                else
                {
                    if(handColor == (int)MaterialColors.Green)
                        RHand.GetComponent<ChangeMaterial>().currentMaterial(handsMaterials[(int)MaterialColors.White]);
                }
            }
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
            //GameObject currentBall;
            GameObject hand;

            string objectName = nameBall + k.ToString() + '_' + numberCurrentBall.ToString();
            string handName = handId[k] + "Hand";


            //currentBall = GameObject.Find(objectName);
   ///// TODO encontrar como moverme entre las manos sin tener que buscar el objeto hacer un GameObject[] hands y acceder segun el k;
            
            if (numberFramesReceived % 2 == 1)
            {
                hand = GameObject.Find(handName);
                hand.transform.position = newPosition;
                hand.transform.rotation = newOrientation;
            }
            //currentBall.transform.position = newPosition;
            currentStatus.handPosition[k] = newPosition;
            currentStatus.handRotation[k] = newOrientation.eulerAngles;
            if (sampleVariance < limInferiorVariance)
            {
                //currentBall.GetComponent<Renderer>().material = ballMaterials[0];
                if (currentStatus.handMoving[k])
                {
                    currentStatus.handMoving[k] = false;
                    sendUpdate = true;
                    countFramesIquals = 0;
                }
            }
            else
            {

                if (!currentStatus.handMoving[k])
                    { 
                    currentStatus.handMoving[k] = true;
                    countFramesIquals = 0;
                    sendUpdate = true;
                    }
            }
            
        }
        countFramesIquals += 1;
        //UPDATE DISTANCE mide la distancia entre las manos, si va aumentando o disminuyendo TODO cambiar esto para una o dos manos movindose
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
            
        }
        //UPDATE CAMERA
        int headID = 2;
       // Debug.Log(nRG);
        if(nRG>headID)
        {
            //Debug.Log("update camera position");
            int id = System.Convert.ToInt32(rigidBodiesList[headID].Attributes["ID"].InnerText);
            //POSITION
            //int factor = 10;
            //int factor = 2;
            float x = (float)System.Convert.ToDouble(rigidBodiesList[headID].Attributes["x"].InnerText) * factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[headID].Attributes["y"].InnerText) * factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[headID].Attributes["z"].InnerText) * factor;

            //habia cambiado -x y -z
            //z = -z;

            headPosition = new Vector3(-x, (float)(y - headSize), z);


            //Debug.Log(head);
            if (numberFramesReceived % 2 == 1)
            {
                head.transform.position = headPosition;
            }
            //Debug.Log(position);
            //head.transform.position = Vector3.MoveTowards(head.transform.position, position, speed * Time.deltaTime);
        }
        //TO-DO Update hand direction 
        if (sendUpdate && countFramesIquals > sameFrameWindowSaze && UpdateHandsStatus != null)
        {
            //RIGHT HAND MOVING
            if (currentStatus.handMoving[0] && !currentStatus.handMoving[1])
            {
                //El movimiento debe ser perpendicular al plano representado por la recta del casco al piso y la mano izquierda
                currentStatus.handDirection[0] = samplePositions[0, positionOnSampleBallArray] - samplePositions[0, (positionOnSampleBallArray + 1 % windowSize)];
                //Debug.Log("handdirection"+currentStatus.handDirection[0]);
                float VRx = headPosition.x;
                float VRy = headPosition.y;
                float VRz = headPosition.z;
                float LHx = currentStatus.handPosition[1].x;
                float LHz = currentStatus.handPosition[1].z;
                /////TODO conectarse a VRViewer y obtener su forward para usarla como normal 
                //Vector3 normal = new Vector3(-VRy*(LHz-VRz),0,VRy*(LHx-VRx)); // normal al plano creado por la mano izquierda y la recta de la cabeza al piso
                Vector3 normal = headCamera.transform.forward;
                //Debug.Log(normal);
                File.AppendAllText("output.txt", normal.x.ToString() + "\t" + normal.y.ToString() + "\t" + normal.z.ToString() + "\n");
                float angle = Mathf.Acos(Vector3.Dot(currentStatus.handDirection[0], normal)/(Vector3.Magnitude(currentStatus.handDirection[0])* Vector3.Magnitude(normal)));
                currentStatus.angleMovement[0] = (int) (angle * 180 /Mathf.PI);
                currentStatus.normal = normal;
                currentStatus.angleNormal = (Mathf.Acos(Vector3.Dot(new Vector3(1,0,0), normal) / Vector3.Magnitude(normal)))*180/Mathf.PI;
                
            }
            //LEFT HAND MOVING
            /*if (!currentStatus.handMoving[0] && currentStatus.handMoving[1])
            {


            }*/
            //BOUTH HAND MOVING
            /*if (currentStatus.handMoving[0] && currentStatus.handMoving[1])
            {


            }*/


        }
        
        if (sendUpdate && countFramesIquals > sameFrameWindowSaze && UpdateHandsStatus != null)
        {
            ////////////////
            //Debug.Log(currentStatus.angleMovement[0]);
            
            UpdateHandsStatus(this, currentStatus);
            
            sendUpdate = false;
        }
        
    }

        // Update is called once per frame
        void Update () 
	{
        //Debug.Log(Time.time);
        //Debug.Log(Time.deltaTime);
    }
    void OnPostRender()
    {
  
    }
}
