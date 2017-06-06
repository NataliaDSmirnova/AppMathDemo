using UnityEngine;
using System;
using System.Collections;

public class AppleWF : MonoBehaviour
{
	int[] triangles;
	// Use this for initialization
	void Start ()
	{
		Invoke("initMesh", 18 * 0.29f);
	}
	void initMesh() {
		print ("there");
		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = mf.mesh;
		mesh.Clear();

		mesh.vertices = transform.parent.gameObject.GetComponent<AppleCalc> ().vertices;
		mesh.normals = transform.parent.gameObject.GetComponent<AppleCalc> ().normals;
		mesh.triangles = transform.parent.gameObject.GetComponent<AppleCalc> ().trianglesWF;
	}

	// Update is called once per frame
	void Update ()
	{
	}
}

