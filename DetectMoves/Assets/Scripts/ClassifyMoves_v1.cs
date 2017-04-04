using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ClassifyMoves_v1 : MonoBehaviour {
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
    void Start () {
        motiveData.GetComponent<HandsStatusData_v1>().UpdateHandsStatus+= new ChangeHandsStatusHandler(OnUpdatedHandsStatusReceived);
        //Debug.Log(GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials[0]);
        handsMaterials = GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials;
        //Debug.Log(handsMaterials[0]);
        currentHandStatus = new HandStatus();
        currentHandStatus.id = -1;
        logRight = "";
        logLeft = "";
        logGeneral = "";

    }
    public int angleThresholdOH;
    public int angleThresholdTH;
    float oneHandRight = 285;
    int getRotationType(Vector3[] orientation)
    {
        Debug.Log(orientation[0].ToString());
        //Debug.Log(orientation[1].ToString());
        //Debug.Log((orientation[0].z > ( 90 - angleThresholdTH)).ToString() + " " + (orientation[0].z < ( 90 + angleThresholdTH)).ToString());
        //Debug.Log((orientation[1].z > (270 - angleThresholdTH)).ToString() + " " + (orientation[1].z < (270 + angleThresholdTH)).ToString());
        if ((orientation[0].z > ( 90 - angleThresholdTH) && orientation[0].z < ( 90 + angleThresholdTH)) &&
            (orientation[1].z > (270 - angleThresholdTH) && orientation[1].z < (270 + angleThresholdTH)) )
        {
            Debug.Log("Case 1");
            return 1;
        }
        //Debug.Log((orientation[0].z > (oneHandRight - angleThresholdOH)).ToString() + " " + (orientation[0].z < (oneHandRight + angleThresholdOH)).ToString() );
        //0Debug.Log((orientation[1].z > ( 90 - angleThresholdOH)).ToString() + " " + (orientation[1].z < ( 90 + angleThresholdOH)).ToString());
        if ((orientation[0].z > (oneHandRight - angleThresholdOH) && orientation[0].z < (oneHandRight + angleThresholdOH) ) ||
            (orientation[1].z > 90 - angleThresholdOH && orientation[1].z < 90 + angleThresholdOH))
        {
            Debug.Log("Case 0");
            return 0;
        }
        
        return -1;
    }

    int getMoveId(HandStatus newHandStatus)
    {
        //NOT MOVING
        if (!newHandStatus.handMoving[0] && !newHandStatus.handMoving[1])
            return (int)MaterialColors.White;

        //MOVING ONLY RIGHT
        if (newHandStatus.handMoving[0] && !newHandStatus.handMoving[1])
        {
            if (getRotationType(newHandStatus.handRotation)==0)
            {
                if (newHandStatus.distance < 0)
                {
                    //PULL
                    return (int)MaterialColors.Blue;
                }
                else
                {
                    //PUSH
                    return (int)MaterialColors.Cyan;
                }
            }
            else
            {
                return (int)MaterialColors.Silver;
            }
        }
        //MOVING ONLY LEFT
        if (!newHandStatus.handMoving[0] && newHandStatus.handMoving[1])
        {
            if (getRotationType(newHandStatus.handRotation) == 0)
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
            if (getRotationType(newHandStatus.handRotation) == 1)
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
        handColor=getMoveId(newHandStatus);
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
	
	}
}
