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

public class Test : MonoBehaviour {
    MeshFilter mf;
    void Start(){
        mf = GetComponent<MeshFilter>();
        Mesh _mesh = mf.mesh;
        //_mesh.vertices
        //Graphics.DrawMeshNow(_mesh, Vector3.zero, Quaternion.identity);
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = new Vector3(0, 0, 0);
        //Debug.Log("Hello World!");
        //Quaternion newRotation = Quaternion.Euler(45, 0, 0);
        //cube.transform.rotation = Quaternion.Slerp();
        //Vector3 position = new Vector3((float)-0.036863, (float) -0.102216, (float)0.02225);
        //Quaternion orientation = new Quaternion((float)-0.001171, (float)-0.019582, (float)0.00044, (float)0.999807);

    }

   
}

