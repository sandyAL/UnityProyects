using UnityEngine;
using System.Collections;

public class joints : MonoBehaviour {


    Vector3[] positions ={new Vector3{ x=(float)-0.175809, y=(float)0.912107, z=(float)0.285093},
new Vector3{ x=(float)-0.096199, y=(float)0.907696, z=(float)0.516779},
new Vector3{ x=(float)0.039848, y=(float)0.924258, z=(float)0.239316},
new Vector3{ x=(float)0.098877, y=(float)0.926856, z=(float)0.432776},
new Vector3{ x=(float)-0.140903, y=(float)1.211692, z=(float)0.411514},
new Vector3{ x=(float)0.070068, y=(float)1.106100, z=(float)0.269420},
new Vector3{ x=(float)0.116940, y=(float)1.095669, z=(float)0.384871},
new Vector3{ x=(float)0.101260, y=(float)1.206639, z=(float)0.320149},
new Vector3{ x=(float)-0.032632, y=(float)1.598386, z=(float)0.361339},
new Vector3{ x=(float)-0.117401, y=(float)1.521303, z=(float)0.400897},
new Vector3{ x=(float)0.003428, y=(float)1.522925, z=(float)0.454943},
new Vector3{ x=(float)-0.035302, y=(float)1.324874, z=(float)0.240439},
new Vector3{ x=(float)0.057630, y=(float)1.199325, z=(float)0.221339},
new Vector3{ x=(float)-0.120805, y=(float)0.960271, z=(float)0.068656},
new Vector3{ x=(float)-0.028580, y=(float)1.006090, z=(float)0.120104},
new Vector3{ x=(float)-0.136928, y=(float)0.711169, z=(float)0.095026},
new Vector3{ x=(float)-0.221569, y=(float)0.787996, z=(float)0.139469},
new Vector3{ x=(float)-0.132692, y=(float)0.768142, z=(float)0.094263},
new Vector3{ x=(float)0.047249, y=(float)1.323242, z=(float)0.451066},
new Vector3{ x=(float)0.135893, y=(float)1.196526, z=(float)0.423366},
new Vector3{ x=(float)0.096150, y=(float)0.942585, z=(float)0.628522},
new Vector3{ x=(float)0.127731, y=(float)0.992297, z=(float)0.556579},
new Vector3{ x=(float)0.057964, y=(float)0.697313, z=(float)0.648486},
new Vector3{ x=(float)0.053596, y=(float)0.755637, z=(float)0.655747},
new Vector3{ x=(float)-0.032967, y=(float)0.779325, z=(float)0.652176},
new Vector3{ x=(float)-0.153082, y=(float)0.596870, z=(float)0.281670},
new Vector3{ x=(float)-0.043948, y=(float)0.452259, z=(float)0.190297},
new Vector3{ x=(float)-0.001201, y=(float)0.074476, z=(float)0.238266},
new Vector3{ x=(float)-0.071890, y=(float)0.204584, z=(float)0.252429},
new Vector3{ x=(float)-0.150755, y=(float)0.055536, z=(float)0.363918},
new Vector3{ x=(float)-0.153510, y=(float)0.041256, z=(float)0.251742},
new Vector3{ x=(float)-0.077580, y=(float)0.548539, z=(float)0.496101},
new Vector3{ x=(float)0.018180, y=(float)0.446018, z=(float)0.518373},
new Vector3{ x=(float)0.053446, y=(float)0.091332, z=(float)0.459660},
new Vector3{ x=(float)-0.022478, y=(float)0.306998, z=(float)0.485407},
new Vector3{ x=(float)-0.136413, y=(float)0.037503, z=(float)0.442769},
new Vector3{ x=(float)-0.062509, y=(float)0.053961, z=(float)0.531163}};

string[] names = { "subject_Hip_1_X", "subject_Hip_2_X", "subject_Hip_3_X", "subject_Hip_4_X", "subject_Chest_1_X", "subject_Chest_2_X", "subject_Chest_3_X", "subject_Chest_4_X", "subject_Head_1_X", "subject_Head_2_X", "subject_Head_3_X", "subject_LShoulder_1_X", "subject_LShoulder_2_X", "subject_LUArm_1_X", "subject_LUArm_2_X", "subject_LHand_1_X", "subject_LHand_2_X", "subject_LHand_3_X", "subject_RShoulder_1_X", "subject_RShoulder_2_X", "subject_RUArm_1_X", "subject_RUArm_2_X", "subject_RHand_1_X", "subject_RHand_2_X", "subject_RHand_3_X", "subject_LThigh_1_X", "subject_LThigh_2_X", "subject_LShin_1_X", "subject_LShin_2_X", "subject_LFoot_1_X", "subject_LFoot_2_X", "subject_RThigh_1_X", "subject_RThigh_2_X", "subject_RShin_1_X", "subject_RShin_2_X", "subject_RFoot_1_X", "subject_RFoot_2_X" };
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 37; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = positions[i];
            sphere.name = names[i];
            sphere.transform.localScale = new Vector3((float)0.1, (float)0.1, (float)0.1);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    

	}
}
