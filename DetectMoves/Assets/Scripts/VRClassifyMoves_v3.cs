using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class VRClassifyMoves_v3 : MonoBehaviour {
    public GameObject motiveData;
    public HandStatus currentHandStatus;
    int RIGHT = 0, LEFT = 1;
    // gDMaterials=new globalDefinitions();
    public double thresholdDistance=0.01;
    //public string[] handId = new string[2] { "R", "L" };
    Material[] handsMaterials;
    // Use this for initialization
    int rhandColor = 0;
    int lhandColor = 0;
    int handColor = 0;
    string logRight, logLeft,logGeneral;
    GameObject testObject;
    void Start () {
        motiveData.GetComponent<VRHandsStatusData_v3>().UpdateHandsStatus+= new ChangeHandsStatusHandler(OnUpdatedHandsStatusReceived);
        //motiveData.GetComponent<VRHandsStatusData_v2>().handColor = (int)MaterialColors.;
        //Debug.Log(GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials[0]);
        handsMaterials = GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials;
        //Debug.Log(handsMaterials[0]);
        currentHandStatus = new HandStatus();
        currentHandStatus.id = -1;
        logRight = "";
        logLeft = "";
        logGeneral = "";
        testObject = GameObject.Find("ObjetoPrueba");
    }

    public int angleThresholdOneHand;
    public int angleThresholdTwoHands;
    

    int getRotationType(Vector3[] orientation, float angle)
    {

        // TWO HANDS POSITION
        /*
        if ((orientation[0].x > (90 - angleThresholdTwoHands) && orientation[0].x < (90 + angleThresholdTwoHands) )//&&( orientation[0].z < angleThresholdTwoHands && orientation[0].z > -angleThresholdTwoHands) 
            &&
            (orientation[1].x > (270 - angleThresholdTwoHands) && orientation[1].x < (270 + angleThresholdTwoHands) )//&&( orientation[1].z < angleThresholdTwoHands && orientation[1].z > -angleThresholdTwoHands)
            )
        {
            Debug.Log("Case 1");
            return 3;
        }*/
        float angleScaled=angle;
        float orientationYScaled=orientation[0].y;
        if (angle > 180) angleScaled = angle - 360;
        if (orientation[0].y > 180) orientationYScaled = orientation[0].y - 360;
        // ONE HAND -RIGHT HAND
        //Debug.Log(angle.ToString()+ " "+orientation[0].y.ToString());
        if ((orientation[0].z > (277 - angleThresholdOneHand) && orientation[0].z < (277 + angleThresholdOneHand))
            && (Math.Sign(angleScaled)==-Math.Sign(orientationYScaled) )
            )
        {    
            return 1;
        }

        if ((orientation[0].z > (277 - angleThresholdOneHand) && orientation[0].z < (277 + angleThresholdOneHand)) 
            && (Math.Sign(angleScaled) == Math.Sign(orientationYScaled)) 
            )
        {
            return 2;
        }
        // ONE HAND - LEFT HAND
        /*if ((orientation[1].z > (270 - angleThresholdOneHand) && orientation[1].z < (270 + angleThresholdOneHand)) && (orientation[1].x < angleThresholdTwoHands && orientation[1].x > -angleThresholdTwoHands))
        {
            return 2;
        }*/
        return -1;
    }
    public int directionThresholdOneHand;
    int checkAngleMovement(float[] direction, int type)
    {
        //rigth hand
        if (type == 1)
        {
            if (direction[0] > (180 - directionThresholdOneHand) && direction[0] < (180 + directionThresholdOneHand))
            {

                return (int)MaterialColors.Blue;
            }
        }
        if (type == 2)
        {
            if (direction[0] > (360 - directionThresholdOneHand) || direction[0] < (0 + directionThresholdOneHand))
            {
                return (int)MaterialColors.Cyan;
            }
        }
        
        //left hand
        /*if (type ==2)
        {

        }*/
        //two hands
        /*if (type == 3)
        {

        }*/
        return (int)MaterialColors.Silver;
        //return (int)MaterialColors.White;
    }
    int getMoveId(HandStatus newHandStatus)
    {
        //Debug.Log(newHandStatus.angleMovement[0]);
        bool debugMoveId = false;
        //NOT MOVING
        if (!newHandStatus.handMoving[0] && !newHandStatus.handMoving[1])
        {
            
            return (int)MaterialColors.White;
        }

        //MOVING ONLY RIGHT
        if (newHandStatus.handMoving[0] && !newHandStatus.handMoving[1])
        {
            if (debugMoveId || (getRotationType(newHandStatus.handRotation, newHandStatus.angleNormal) == 1))
            {
                int type = checkAngleMovement(newHandStatus.angleMovement, 1);
                
                if(type != (int)MaterialColors.Silver)
                {
                    Vector3 direction = new Vector3(newHandStatus.handDirection[0].x, 0, newHandStatus.handDirection[0].z);
                    testObject.transform.position += direction.normalized;
                }
                return type;
                /*if (newHandStatus.distance < 0)
                {
                    //PULL
                    return (int)MaterialColors.Blue;
                }
                else
                {
                    //PUSH
                    return (int)MaterialColors.Cyan;
                }*/

            }
            else if (debugMoveId || (getRotationType(newHandStatus.handRotation, newHandStatus.angleNormal) == 2))
            {
                int type = checkAngleMovement(newHandStatus.angleMovement, 2);

                if (type != (int)MaterialColors.Silver)
                {
                    Vector3 direction = new Vector3(newHandStatus.handDirection[0].x, 0, newHandStatus.handDirection[0].z);
                    testObject.transform.position += direction.normalized;
                }
                return type;
            }
            else
            {
                return (int)MaterialColors.Silver;
                //return (int)MaterialColors.White;
            }
        }
        //MOVING ONLY LEFT
        if (!newHandStatus.handMoving[0] && newHandStatus.handMoving[1])
        {
            if (debugMoveId || getRotationType(newHandStatus.handRotation, newHandStatus.angleNormal) == 2)
            {
                if (newHandStatus.distance < 0)
                {
                    //PULL
                    return (int)MaterialColors.Orange;
                }
                else
                {
                    //PUSH
                    return (int)MaterialColors.Red;
                }
            }
            else
            {
                return (int)MaterialColors.Silver;
            }
        }
        //MOVING BOUTH
        if (newHandStatus.handMoving[0] && newHandStatus.handMoving[1])
        {
            if (debugMoveId || getRotationType(newHandStatus.handRotation, newHandStatus.angleNormal) == 3)
            {
                if (newHandStatus.distance == 0)
                {
                    //TOGETHER
                    return (int)MaterialColors.Yellow;
                }
                if (newHandStatus.distance < 0)
                {
                    //SQUEEZE
                    return (int)MaterialColors.Brown;
                }
                //SPREAD
                return (int)MaterialColors.Purple;
            }
            else
            {
                return (int)MaterialColors.Silver;
            }
        }

        return (int)MaterialColors.Silver;
    }

    private void OnUpdatedHandsStatusReceived(object sender, HandStatus newHandStatus)
    {
        //Debug.Log("On received status");
        GameObject RHand = GameObject.Find("rightHand");
        GameObject LHand = GameObject.Find("leftHand");
        //Debug.Log(newHandStatus.handRotation[0]);
        handColor=getMoveId(newHandStatus);
        motiveData.GetComponent<VRHandsStatusData_v2>().handColor = handColor;
        //Debug.Log(handsMaterials[handColor]);
        RHand.GetComponent<ChangeMaterial>().currentMaterial(handsMaterials[handColor]);
        LHand.GetComponent<ChangeMaterial>().currentMaterial(handsMaterials[handColor]);
        
        /*Debug.Log("Receive update:"+ newHandStatus.id.ToString());
        if (newHandStatus.handMoving[0] != currentHandStatus.handMoving[0])
        {
            logGeneral += newHandStatus.id.ToString() + "\tR: " + currentHandStatus.handMoving[0].ToString() + " --> " + newHandStatus.handMoving[0].ToString() + "\n";
            //if (newHandStatus.handMoving[0])
            //{
                logRight += newHandStatus.id.ToString() + "\t";
            //}
            
            
            GameObject RHand = GameObject.Find("rightHand");

            if (newHandStatus.handMoving[0] == true)
                rhandColor = 0;
            else
                rhandColor = 1;
            RHand.GetComponent<ChangeMaterial>().currentMaterial(handsMaterials[rhandColor]);
        }
        
        if (newHandStatus.handMoving[1] != currentHandStatus.handMoving[1])
        {
            logGeneral += newHandStatus.id.ToString() + "\tL: " + currentHandStatus.handMoving[1].ToString() + " --> " + newHandStatus.handMoving[1].ToString()+"\n";
            //if (newHandStatus.handMoving[0])
            //{
                logLeft += newHandStatus.id.ToString() + "\t";
            //}
            GameObject LHand = GameObject.Find("leftHand");
            if (newHandStatus.handMoving[1] == true)
                lhandColor = 0;
            else
                lhandColor = 1;
            LHand.GetComponent<ChangeMaterial>().currentMaterial(handsMaterials[lhandColor]);
        }
        */
        //currentHandStatus = newHandStatus;
        currentHandStatus.update(newHandStatus);
        //File.WriteAllText("Output.txt", logRight + "\n" + logLeft+ "\n \n"+ logGeneral);
        //Debug.Log(newHandStatus.id.ToString() + ":\t" + newHandStatus.handMoving[0].ToString() +"\t:\t" + newHandStatus.handMoving[1].ToString());
    }

    // Update is called once per frame
    void Update () {
        //TODO hacer mas eficiente esta parte
        testObject = GameObject.Find(GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().selectedObjectName);
	}
}
