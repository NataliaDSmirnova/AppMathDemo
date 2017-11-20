using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraftAnim : MonoBehaviour {
	float time = 0.0f;

    private int nSections = 32; // количество долек (должно быть кратно 2 * sectionRatio)
    private int nSegments = 16; // количество сегментов в линии (должно быть кратно 3)
	float lenCurvedCone = 0.04f;
	float rMedium = 0.01f;
	float rTopPart = 0.08f;
	float rShift = 0.75f;
	float angle = Mathf.PI / 4.0f;

    CurvedCone curvedCone;
    Vector3[] MeshVB;
    Vector3[] MeshNB;
    int[] MeshIB;

    public void find_VB_and_NB(float param0_1, float param1_0){
        // baseCone.find_VB_and_NB(param0_1, param1_0, 0f);
        curvedCone.find_VB_and_NB(param0_1, param1_0, 0.5f);

		curvedCone.VB.CopyTo(MeshVB, 0);	
		curvedCone.NB.CopyTo(MeshNB, 0);
		curvedCone.solidIB.CopyTo(MeshIB, 0);

		// baseCone.VB.CopyTo(MeshVB, nSections * (nSegments + 1) * 4);
		// baseCone.NB.CopyTo(MeshNB, nSections * (nSegments + 1) * 4);
		// baseCone.solidIB.CopyTo(MeshIB, 3 * nSections * (2 * nSegments));
    }

	void Start () {
        curvedCone = new CurvedCone(nSections, nSegments, lenCurvedCone, rMedium, rTopPart, rShift, angle);

        MeshVB = new Vector3[4 * nSections * (nSegments + 1) * 4];
        MeshNB = new Vector3[4 * nSections * (nSegments + 1) * 4];
    	MeshIB = new int[4 * 3 * nSections * (2 * nSegments)];
	}
	
    public void OnRenderObject()
    {
        AppleScript appSc = transform.root.gameObject.GetComponent<AppleScript>();
        float dT = appSc.times[appSc.timeEnd[appSc.name]] - appSc.times[appSc.timeStart[appSc.name]];
        float timeS = appSc.times[appSc.timeStart[appSc.name]] + appSc.times[appSc.timeStart[gameObject.name]] * dT;
        float timeF = appSc.times[appSc.timeStart[appSc.name]] + appSc.times[appSc.timeEnd[gameObject.name]] * dT;

        if(time == timeF) {
            GetComponent<MeshFilter>().mesh.Clear();
            return;
        }
		time += Time.deltaTime;
		if(time < timeS) return;
        if(time > timeF) time = timeF;

		float T = (float)(time - timeS) / (timeF - timeS);
		if(T > 1) T = 1;

		float param1_0 = ((float)Mathf.Cos(T * Mathf.PI) + 1) / 2.0f;
		float param0_1 = ((float)Mathf.Sin(T * Mathf.PI - Mathf.PI / 2.0f) + 1) / 2.0f;

        find_VB_and_NB(param0_1, param1_0);
    
	    // gl.glTranslatef(0.0f, param0_1*baseCone.getLen(), 0.0f);
        // curvedCone.drawSolid(gl);
        // gl.glTranslatef(0.0f, -param0_1*baseCone.getLen(), 0.0f);
        // baseCone.drawSolid(gl);

		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = mf.mesh;
		mesh.vertices = MeshVB;
		mesh.triangles = MeshIB;
		mesh.RecalculateNormals();
		// mesh.normals = MeshNB;
	}
}
