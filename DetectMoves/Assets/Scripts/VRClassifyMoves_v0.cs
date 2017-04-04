using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class VRClassifyMoves_v0 : MonoBehaviour {
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
        motiveData.GetComponent<VRHandsStatusData_v0>().UpdateHandsStatus+= new ChangeHandsStatusHandler(OnUpdatedHandsStatusReceived);
        //Debug.Log(GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials[0]);
        handsMaterials = GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials;
        //Debug.Log(handsMaterials[0]);
        currentHandStatus = new HandStatus();
        currentHandStatus.id = -1;
        logRight = "";
        logLeft = "";
        logGeneral = "";

    }

    int getMoveId(HandStatus newHandStatus)
    {
        if (!newHandStatus.handMoving[0] && !newHandStatus.handMoving[1])
            return (int)MaterialColors.White;
        if (newHandStatus.handMoving[0] && !newHandStatus.handMoving[1])
            if(newHandStatus.distance <0)
                return (int)MaterialColors.Blue;
            else
                return (int)MaterialColors.Cyan;


        if (!newHandStatus.handMoving[0] && newHandStatus.handMoving[1])
            if (newHandStatus.distance < 0)
                return (int)MaterialColors.Orange;
            else
                return (int)MaterialColors.Red;
        
        if (newHandStatus.handMoving[0] && newHandStatus.handMoving[1])
        {
            if (newHandStatus.distance == 0)
                return (int)MaterialColors.Yellow;
            if (newHandStatus.distance < 0)
                return (int)MaterialColors.Brown;
            return (int)MaterialColors.Purple;
        }
            
        return (int)MaterialColors.White;
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
