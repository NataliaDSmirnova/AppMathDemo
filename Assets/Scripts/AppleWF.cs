using UnityEngine;
using System.Collections;

public class AppleWF : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Invoke("initMesh", 18 * 0.6f);
	}
	void initMesh() {
		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = mf.mesh;
		mesh.Clear();
		mesh.vertices = transform.parent.gameObject.GetComponent<AppleCalc>().vertices;
		mesh.triangles = transform.parent.gameObject.GetComponent<AppleCalc>().triangles;
		mesh.normals = transform.parent.gameObject.GetComponent<AppleCalc>().normals;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}

