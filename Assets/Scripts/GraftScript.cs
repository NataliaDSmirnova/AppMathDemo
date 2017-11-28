using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurvedCone {

    private int nSections; // количество долек (должно быть кратно 2 * sectionRatio)

    private static int sectionRatio = 2; // количество долек в проволочной модели будет nSectionsions/sectionRatio
    private int nSegments; // количество сегментов в линии (должно быть кратно 3)
    private static int segmentRatio = 1; // количество сегментов в проволочной модели будет nSegmentsments/segmentRatio
    float rTopPart; // Радиус большего основания усечённого конуса
    float rBottomPart; // Радиус меньшего основания усечённого конуса
    float len; // Высота изгибаемого усечённого конуса
    float angle; // Угол до которого загибается усечённый конус
    float rShift; // Радиус круга по которому изгибается конус

    public Vector3[] VB;
    public Vector3[] NB;
    public int[] solidIB;
    public int[] wireFrameIB;

    public CurvedCone(int nSections, int nSegments, float len, float rBottomPart, float rTopPart, float rShift, float angle){
        this.nSections = nSections;
        this.nSegments = nSegments;
        this.rTopPart = rTopPart;
        this.rBottomPart = rBottomPart;
        this.len = len;
        this.angle = angle;
        this.rShift = rShift;

        VB = new Vector3[nSections * (nSegments + 1) * 4];
        NB = new Vector3[nSections * (nSegments + 1) * 4];
    	solidIB = new int[3 * nSections * (2 * nSegments)];
		wireFrameIB = new int[4 * nSections/sectionRatio * nSegments/segmentRatio];
        find_VB_and_NB(1.0f, 0.0f, 0);
        initIB();
        initWireFrameIB();
    }

    public void initIB() {
        int shift = 0;
        for(int j = 0, i; j < nSegments - 1; j++) {
            i = 0;
            for (; i < nSections - 1; i++) {
                solidIB[shift + i * 6 + 0] = (int) (j * nSections + i);
                solidIB[shift + i * 6 + 2] = (int) (j * nSections + i + nSections);
                solidIB[shift + i * 6 + 1] = (int) (j * nSections + i + 1);

                solidIB[shift + i * 6 + 3] = (int) (j * nSections + i + 1);
                solidIB[shift + i * 6 + 5] = (int) (j * nSections + i + nSections);
                solidIB[shift + i * 6 + 4] = (int) (j * nSections + i + 1 + nSections);
            }
            solidIB[shift + i * 6 + 0] = (int) (j * nSections + i);
            solidIB[shift + i * 6 + 2] = (int) (j * nSections + i + nSections);
            solidIB[shift + i * 6 + 1] = (int) (j * nSections);

            solidIB[shift + i * 6 + 3] = (int) (j * nSections);
            solidIB[shift + i * 6 + 5] = (int) (j * nSections + i + nSections);
            solidIB[shift + i * 6 + 4] = (int) (j * nSections + nSections);
            shift += nSections * 6;
        }
		for (int i = 0; i < nSections - 1; i++) {
			solidIB[shift + i * 3 + 0] = (int) ((nSegments - 1) * nSections + i);
			solidIB[shift + i * 3 + 2] = (int) ((nSegments - 1) * nSections);
			solidIB[shift + i * 3 + 1] = (int) ((nSegments - 1) * nSections + i + 1);
		}

    }

    public void initWireFrameIB(){
        int shift = 0;
        for(int j = 0, i, k; j < nSegments; j += segmentRatio) {
            i = k = 0;
            for (; k < nSections - sectionRatio; i++, k +=sectionRatio) {
                wireFrameIB[shift + i * 4 + 0] = (int) (j * nSections + k);
                wireFrameIB[shift + i * 4 + 1] = (int) (j * nSections + k + segmentRatio*nSections);

                wireFrameIB[shift + i * 4 + 2] = (int) (j * nSections + k + segmentRatio*nSections);
                wireFrameIB[shift + i * 4 + 3] = (int) (j * nSections + k + segmentRatio*nSections + sectionRatio);
            }
            wireFrameIB[shift + i * 4 + 0] = (int) (j * nSections + k);
            wireFrameIB[shift + i * 4 + 1] = (int) (j * nSections + k + segmentRatio*nSections);

            wireFrameIB[shift + i * 4 + 2] = (int) (j * nSections + k + segmentRatio*nSections);
            wireFrameIB[shift + i * 4 + 3] = (int) (j * nSections + segmentRatio*nSections);
            shift += nSections/sectionRatio * 4;
        }
    }

	public void find_VB_and_NB(float param0_1, float param1_0, float extraY){
        float r = rBottomPart * param0_1;
        float deltaR = rTopPart * param0_1 - r;
        float l = len * param0_1;
        float phy = -angle * param0_1;

        Vector3[] coords = new Vector3[nSections * (nSegments + 1)];
        Vector3[] normalCoords = new Vector3[nSections * (nSegments + 1)];

        float n_x = l / (float) Mathf.Sqrt(deltaR*deltaR + l*l);
        float n_y = -deltaR / (float) Mathf.Sqrt(deltaR*deltaR + l*l);
        float x,y,z;
        for(int i = 0; i <= nSegments; i++)
        {
            float sinPhy = (float)Mathf.Sin(i * phy / nSegments);
            float cosPhy = (float)Mathf.Cos(i * phy / nSegments);
            for(int j = 0; j < nSections; j++)
            {
                float sinAlpha = (float)Mathf.Sin(j * 2.0f * Mathf.PI / nSections);
                float cosAlpha = (float)Mathf.Cos(j * 2.0f * Mathf.PI / nSections);
                x = cosAlpha * (r + (i) * deltaR / nSegments) - rShift;
                y = (i) * l / nSegments;
                z = -sinAlpha * (r + (i) * deltaR / nSegments);

                coords[i * nSections + j] = new Vector3(cosPhy * x - sinPhy * y + rShift, sinPhy * x + cosPhy * y + extraY, z);
                normalCoords[i * nSections + j] = new Vector3(cosPhy * (cosAlpha * n_x) - sinPhy * n_y, sinPhy * (cosAlpha * n_x) + cosPhy * n_y, -sinAlpha * n_x);
            }
        }
        VB = coords;
        NB = normalCoords;
    }
}

public class GraftScript : MonoBehaviour {
	float time = 0.0f;

    public bool shrink = false;
    public bool expand = false;

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

        float c = Mathf.Pow(0.001f, 1.0f / ((timeF - timeS) / Time.deltaTime));
        if(shrink) {
            transform.localScale = transform.localScale * c;
            expand = false;
        } else if(expand) transform.localScale = transform.localScale * c;

        find_VB_and_NB(1, 0);
    
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
