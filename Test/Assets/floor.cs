using UnityEngine;
using System.Collections;

public class floor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3[] vertices = new Vector3[]
        {
            //new Vector3( 85, 0, 50),
            new Vector3( 1, 0, 1),
            //new Vector3(-140, 0, 50),
            new Vector3(-1, 0, 1),
            //new Vector3( 85, 0,-62),
            new Vector3( 1, 0,-1),
            //new Vector3(-140, 0,-62)
            new Vector3(-1, 0,-1)
        };

        Vector3[] normals = new Vector3[]
        {
            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0)
        };

        int[] triangleIndices = new int[]
        {
            0, 2, 3,
            3, 1, 0
        };

        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf.sharedMesh == null)
        {
            mf.sharedMesh = new Mesh();
            Debug.Log("nuevo mesh");
        }
        //Color[] whiteVert = new Color[]
        //{
        //    Color.white,
        //    Color.white,
        //    Color.white,
        //    Color.white
        //};

        Mesh mesh = mf.sharedMesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangleIndices;
        // mesh.colors = whiteVert;

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = true;
    
    }
}
