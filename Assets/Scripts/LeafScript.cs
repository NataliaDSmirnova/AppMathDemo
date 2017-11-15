using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafScript : MonoBehaviour {

	float time = 0f;
    private int nSections = 16; // количество долек
    private int nSegments = 8; // количество сегментов в линии
    private float width = 0.1f; // ширина половинки листа
    private float length = 0.3f; // длина листа
    private float k = 0.5f; // коэффициент при изгибе листа
    private static float widthRatio = (float)Mathf.Sqrt(2.0f) * 2; // нормировочный коэффициент
    private static float lenghtRatio = (float)Mathf.Sqrt(2.0f - (float)Mathf.Sqrt(3.0f)) + 0.1f; // нормировочный коэффициент

    public Vector3[] VB;
    public Vector3[] NB;
    public int[] solidIB;

	// Use this for initialization
	void Start () {

        VB = new Vector3[2 * (nSections + 1) * (nSegments + 1)];
        NB = new Vector3[2 * (nSections + 1) * (nSegments + 1)];
        solidIB = new int[12 * nSections * (nSegments - 1) + 6 * nSections];
        find_VB_and_NB(1.0f, 0.0f);
        initIB();		
	}

    public void initIB(){
        int shift = 0;
        int j = 0;
        for(; j < nSegments - 1; j++, shift += nSections * 12) {
            for (int i = 0; i < nSections; i++) {
                solidIB[shift + i * 12 + 0] = (int) (j * (nSections + 1) + i);
                solidIB[shift + i * 12 + 2] = (int) (j * (nSections + 1) + i + 1);
                solidIB[shift + i * 12 + 1] = (int) (j * (nSections + 1) + i + 1 + nSections);

                solidIB[shift + i * 12 + 3] = (int) (j * (nSections + 1) + i + 1);
                solidIB[shift + i * 12 + 5] = (int) (j * (nSections + 1) + i + 1 + nSections + 1);
                solidIB[shift + i * 12 + 4] = (int) (j * (nSections + 1) + i + 1 + nSections);

                solidIB[shift + i * 12 + 6] = (int) VB.Length / 2 + (j * (nSections + 1) + i);
                solidIB[shift + i * 12 + 7] = (int) VB.Length / 2 + (j * (nSections + 1) + i + 1);
                solidIB[shift + i * 12 + 8] = (int) VB.Length / 2 + (j * (nSections + 1) + i + 1 + nSections);

                solidIB[shift + i * 12 + 9] = (int) VB.Length / 2 + (j * (nSections + 1) + i + 1);
                solidIB[shift + i * 12 + 10] = (int) VB.Length / 2 + (j * (nSections + 1) + i + 1 + nSections + 1);
                solidIB[shift + i * 12 + 11] = (int) VB.Length / 2 + (j * (nSections + 1) + i + 1 + nSections);
            }
        }
        for (int i = 0; i < nSections; i++) {
            solidIB[shift + i * 6 + 0] = (int) (j * (nSections + 1) + i);
            solidIB[shift + i * 6 + 2] = (int) (j * (nSections + 1) + i + 1);
            solidIB[shift + i * 6 + 1] = (int) (j * (nSections + 1) + i + 1 + nSections);

            solidIB[shift + i * 6 + 3] = (int) VB.Length / 2 + (j * (nSections + 1) + i);
            solidIB[shift + i * 6 + 4] = (int) VB.Length / 2 + (j * (nSections + 1) + i + 1);
            solidIB[shift + i * 6 + 5] = (int) VB.Length / 2 + (j * (nSections + 1) + i + 1 + nSections);

        }
    }

    public void find_VB_and_NB(float param0_1, float param1_0){
        float newLength = length * param0_1;
        float newWidth = width * param0_1;
        float newK = k;
        float ratio = (lenghtRatio + lenghtRatio*lenghtRatio*lenghtRatio) / (1 + lenghtRatio*lenghtRatio*lenghtRatio*lenghtRatio);
        float newShift = newLength * ratio;

        float p, a, b, shift;
        float x, y, z;
        float tgAlpha;
        float tg2_Alpha;
        for(int i = 0; i <= nSegments; i++)
        {
            a = i * newLength / nSegments;
            b = i * newWidth / nSegments;
            shift = newShift - a * ratio;
            for(int j = 0; j <= nSections; j++)
            {
                p = (float)j / nSections;
                x = a * (p + p*p*p) / (1 + p*p*p*p) + shift;
                z = b * (p - p*p*p) / (1 + p*p*p*p);
                y = (float)Mathf.Sqrt(newK * z);

                VB[i * (nSections + 1) + j] = new Vector3(x, y, z);
                VB[VB.Length / 2 + i * (nSections + 1) + j] = new Vector3(x, y, z);

                if(y == 0) {
                    NB[i * (nSections + 1) + j] = new Vector3(0.0f, 0.0f, -1.0f);
                    NB[NB.Length / 2 + i * (nSections + 1) + j] = -NB[i * (nSections + 1) + j];
                }
                else {
                    tgAlpha = newK / (2 * y*y*y);
                    tg2_Alpha = tgAlpha*tgAlpha;
                    NB[i * (nSections + 1) + j] = new Vector3(0.0f, (float) Mathf.Sqrt(1 / (tg2_Alpha + 1)), -(float) Mathf.Sqrt(tg2_Alpha / (tg2_Alpha + 1)));
                    NB[NB.Length / 2 + i * (nSections + 1) + j] = -NB[i * (nSections + 1) + j];
                }
            }
        }
    }

    // public void drawAndGrowSolid(GL10 gl, float param0_1, float param1_0, float graftAngle, float graftCurvedConeLen, float graftBaseConeLen) {
    //     find_VB_and_NB(param0_1, param1_0);

    //     // float g = length * (float)Mathf.cos(graftAngle) + graftCurvedConeLen * (float)Mathf.sin(graftAngle) + length;
    //     // float w = graftCurvedConeLen * (float)Mathf.cos(graftAngle) + length * (float) Mathf.sin(graftAngle) + graftBaseConeLen;
    //     // gl.glTranslatef(g, w, 0.0f);

    //     // float Alpha = graft.getAngle();
    //     // gl.glRotatef(-15.0f, 0.0f, 0.0f, 1.0f);
    //     // float h = -graft.getrShift() * (float)Mathf.cos(Alpha) + graft.getCurvedConeLen() * (float)Mathf.sin(Alpha) + graft.getrShift();
    //     // float s = graft.getCurvedConeLen() * (float)Mathf.cos(Alpha) + graft.getrShift() * (float)Mathf.sin(Alpha) + length;
    //     // gl.glTranslatef(-s * param0_1, h * param0_1, 0.0f);

    //     // drawLeafSolid(gl);
    //     // gl.glScalef(1.0f, 1.0f, -1.0f);
    //     // drawLeafSolid(gl);
    // }
	private Quaternion rotation = Quaternion.AngleAxis(-15, Vector3.forward);
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

		float T = 1;

		float param1_0 = ((float)Mathf.Cos(T * Mathf.PI) + 1) / 2.0f;
		float param0_1 = ((float)Mathf.Sin(T * Mathf.PI - Mathf.PI / 2.0f) + 1) / 2.0f;

        find_VB_and_NB(param0_1, param1_0);

		Vector3[] VBAll = new Vector3[2 * VB.Length];
		Vector3[] NBAll = new Vector3[2 * NB.Length];
		int[] IBAll = new int[2 * solidIB.Length];

		// rotation *= Quaternion.AngleAxis(100 * param1_0 *  param1_0 , Vector3.left);
		for(int i = 0; i < VB.Length; ++i) VBAll[i] = rotation * new Vector3(length * (-0.8f - param0_1) + VB[i].x, 0.8f + VB[i].y, VB[i].z);
		for(int i = 0; i < VB.Length; ++i) VBAll[i + VB.Length] = rotation * new Vector3(length * (-0.8f - param0_1) + VB[i].x, 0.8f -VB[i].y, VB[i].z);

		for(int i = 0; i < NB.Length; ++i) NBAll[i] = rotation * new Vector3(NB[i].x, NB[i].y, NB[i].z);
		for(int i = 0; i < NB.Length; ++i) NBAll[i + NB.Length] = rotation * new Vector3(NB[i].x, - NB[i].y, NB[i].z);

		for(int i = 0; i < solidIB.Length; ++i) IBAll[i] = solidIB[i];
		for(int i = 0; i < solidIB.Length; i+=3) {
			IBAll[i + solidIB.Length] = solidIB[i] + VB.Length;
			IBAll[i + solidIB.Length + 1] = solidIB[i + 2] + VB.Length;
			IBAll[i + solidIB.Length + 2] = solidIB[i + 1] + VB.Length;
		}

		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = mf.mesh;
		mesh.vertices = VBAll;
		mesh.triangles = IBAll;
		mesh.normals = NBAll;

		GL.PushMatrix();
		GL.MultMatrix(transform.localToWorldMatrix);
		GL.Begin(GL.LINES);
			float linet = T * 1.3f;
			GL.Color(new Color(40f / 255, 180f / 255, 10f / 255, 1F));
			Vector3 v1 = rotation * new Vector3((-0.5f) * length, 0.8f, 0);
			GL.Vertex3(v1.x, v1.y, v1.z);
			Vector3 v2 = rotation * new Vector3((-0.5f - linet) * length, 0.8f, 0);
			GL.Vertex3(v2.x, v2.y, v2.z);
		GL.End();
		GL.PopMatrix();

	}
}
