using UnityEngine;

public class AppleSolid : MonoBehaviour
{
    public float phi = 0.5f;
    public float theta = 0.25f;

    private int nSections = 32; // ���������� �����
    private int nSegments = 32; // ���������� ��������� � �����
    private float len = 0.92f; // ����� ����� � �����
    private float rApple = 0.25f;
    private Vector3[] VB;
    private Vector3[] NB;
    private Vector3[] VBcopy;
    private Vector3[] NBcopy;
    private short[] wireAppleIB;
    float time = 0;
    public void Start()
    {
        initVertexArray();
        initIndexArrayForApple();
    }
    public void initVertexArray()
    {
        VB = new Vector3[2 * nSegments + 1];
        NB = new Vector3[2 * nSegments + 1];
		VBcopy = new Vector3[2 * nSegments + 1];
        NBcopy = new Vector3[2 * nSegments + 1];

        FindVertexCoordsForApple(1.0f, 0.0f);
    }
    public void initIndexArrayForApple()
    {
        short[] lineOrder = new short[nSegments * 12];
        for (int i = 0, k = 1; i < nSegments - 1; i++, k++)
        {
            lineOrder[6 + 6 * i] = (short)(k);
            lineOrder[6 + 6 * i + 1] = (short)(k + nSegments);
            lineOrder[6 + 6 * i + 2] = (short)(k + 1);

            lineOrder[6 + 6 * i + 3] = (short)(k + nSegments);
            lineOrder[6 + 6 * i + 4] = (short)(k + 1 + nSegments);
            lineOrder[6 + 6 * i + 5] = (short)(k + 1);

            lineOrder[6 + 6 * ( nSegments + i) + 0] = (short)(2 * nSegments + 1 + k + 1);
            lineOrder[6 + 6 * ( nSegments + i) + 1] = (short)(2 * nSegments + 1 + k + nSegments);
            lineOrder[6 + 6 * ( nSegments + i) + 2] = (short)(2 * nSegments + 1 + k);

            lineOrder[6 + 6 * ( nSegments + i) + 3] = (short)(2 * nSegments + 1 + k + 1);
            lineOrder[6 + 6 * ( nSegments + i) + 4] = (short)(2 * nSegments + 1 + k + 1 + nSegments);
            lineOrder[6 + 6 * ( nSegments + i) + 5] = (short)(2 * nSegments + 1 + k + nSegments);
        }
		lineOrder[0] = (short)(1);
		lineOrder[1] = (short)(0);
		lineOrder[2] = (short)(nSegments + 1);

		lineOrder[3] = (short)(2 * nSegments + 1 + 1);
		lineOrder[4] = (short)(2 * nSegments + 1 + nSegments + 1);
		lineOrder[5] = (short)(2 * nSegments + 1 + 0);
        wireAppleIB = lineOrder;
    }
	
    public void FindVertexCoordsForApple(float param0_1, float param1_0, float normparam0_1 = 0)
    {
        Vector3[] Coords = new Vector3[2 * nSegments + 1];
        Vector3[] Normals = new Vector3[2 * nSegments + 1];

        float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSections);
        float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSections);

        float sinAlpha2 = (float)Mathf.Sin(Mathf.PI / nSections);

        float t = -Mathf.PI, x, y;
        float xCoord, yCoord;
        
        float tmax = (float)(param0_1 * Mathf.PI - Mathf.PI);
        int i = 0, j = 0, k = nSegments;
        for (; t < tmax; i++, ++j, ++k)
        {
            x = 2 * rApple * (float)Mathf.Sin(t) - rApple * (float)Mathf.Sin(2 * t);
            y = 2 * rApple * (float)Mathf.Cos(t) - rApple * (float)Mathf.Cos(2 * t);

            xCoord = -x;
            yCoord = y;

            Coords[j].x = xCoord;
            Coords[j].y = yCoord;
            Coords[j].z = 0;

            t = (float)((float)(i+1) / nSegments * Mathf.PI - Mathf.PI);
            if (i == 0) continue;
            Coords[k].x = cosAlpha * xCoord;
            Coords[k].y = yCoord;
            Coords[k].z = -sinAlpha * xCoord;
            
            Normals[j] = normparam0_1 * Vector3.Cross(Coords[j] - Coords[j - 1], new Vector3(0, 0, 1)) + 
						 (1 - normparam0_1) * Vector3.Cross(Coords[j] - Coords[j - 1], new Vector3(sinAlpha2, 0, sinAlpha2));
            Normals[k] = normparam0_1 * Vector3.Cross(Coords[j] - Coords[j - 1], new Vector3(sinAlpha, 0, cosAlpha)) + 
						 (1 - normparam0_1) * Vector3.Cross(Coords[j] - Coords[j - 1], new Vector3(sinAlpha2, 0, sinAlpha2));
			Normals[j].Normalize();
			Normals[k].Normalize();
            if (i == 1) {
	            Normals[0] = Normals[1];
	            Normals[k - 1] = Normals[k];
			}

        }
        do {
            x = 2 * rApple * (float)Mathf.Sin(tmax) - rApple * (float)Mathf.Sin(2 * tmax);
            y = 2 * rApple * (float)Mathf.Cos(tmax) - rApple * (float)Mathf.Cos(2 * tmax);
            Coords[i].x = -x;        
            Coords[i].y = y;
            Coords[i].z = 0;

            Coords[k].x = -cosAlpha * x;
            Coords[k].y = y;
            Coords[k].z = sinAlpha * x;

            Normals[i] = normparam0_1 * Vector3.Cross(Coords[i] - Coords[i - 1], new Vector3(0, 0, 1)) + 
						 (1 - normparam0_1) * Vector3.Cross(Coords[i] - Coords[i - 1], new Vector3(sinAlpha2, 0, sinAlpha2));
            Normals[k] = normparam0_1 * Vector3.Cross(Coords[i] - Coords[i - 1], new Vector3(sinAlpha, 0, cosAlpha)) + 
						 (1 - normparam0_1) * Vector3.Cross(Coords[i] - Coords[i - 1], new Vector3(sinAlpha2, 0, sinAlpha2));
			Normals[i].Normalize();
			Normals[k].Normalize();
            if (i == 1) {
	            Normals[0] = Normals[1];
	            Normals[k - 1] = Normals[k];
			}

            ++k;
            ++i;
        } while(i <= nSegments);
        
        VB = Coords;
		NB = Normals;
    }
    
    public void FindVertexCoordsForAppear(float param0_1, float param1_0)
    {
        Vector3[] Coords = new Vector3[2 * nSegments + 1];
        float phy = 2.0f * (float)(Mathf.PI / nSections);
        float cosAlpha, sinAlpha, length;
        for (int i = 0; i <= nSegments; i++)
        {
            cosAlpha = (float)Mathf.Cos(i * phy);
            sinAlpha = (float)Mathf.Sin(i * phy);
            length = param0_1 * len / nSegments;
            Coords[2 * i].x = cosAlpha * i * length;
            Coords[2 * i].y = 0;
            Coords[2 * i].z = sinAlpha * i * length;

            if (i == nSegments)
                break;
            Coords[2 * i + 1].x = cosAlpha * (i + 1) * length;
            Coords[2 * i + 1].y = 0;
            Coords[2 * i + 1].z = sinAlpha * (i + 1) * length;
        }
        VB = Coords;
    }


    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    void rotateX(ref float x, ref float y, ref float z, float angle)
    {
        float cx = x, cy = y, cz = z;
        x = cx;
        y = Mathf.Cos(angle) * cy - Mathf.Sin(angle) * cz;
        z = Mathf.Sin(angle) * cy + Mathf.Cos(angle) * cz;
    }
    void rotateY(ref float x, ref float y, ref float z, float angle)
    {
        float cx = x, cy = y, cz = z;
        x = Mathf.Cos(angle) * cx + Mathf.Sin(angle) * cz;
        y = cy;
        z = -Mathf.Sin(angle) * cx + Mathf.Cos(angle) * cz;
    }

    void rotateZ(ref float x, ref float y, ref float z, float angle)
    {
        float cx = x, cy = y, cz = z;
        x = Mathf.Cos(angle) * cx - Mathf.Sin(angle) * cy;
        y = Mathf.Sin(angle) * cx + Mathf.Cos(angle) * cy;
        z = cz;
    }

    void RotateXVec(ref Vector3[] VV, float angle)
    {
        for (int i = 0; i < VV.Length; ++i)
        {
            rotateX(ref VV[i].x, ref VV[i].y, ref VV[i].z, angle);
        }
    }
    void RotateYVec(ref Vector3[] VV, float angle)
    {
        for (int i = 0; i < VV.Length; ++i)
        {
            rotateY(ref VV[i].x, ref VV[i].y, ref VV[i].z, angle);
        }
    }

    void RotateZVec(ref Vector3[] VV, float angle)
    {
        for (int i = 0; i < VV.Length; ++i)
        {
            rotateZ(ref VV[i].x, ref VV[i].y, ref VV[i].z, angle);
        }
    }
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    void rotateAroundPoint(ref Vector3[] VV, Vector3 angle, float scale)
    {
        Vector3 center = VV[0];
        for (int i = 1; i < VV.Length; ++i)
        {
            Vector3 cur = VV[i];
            cur = RotatePointAroundPivot(cur, center, angle);
            VV[i] = new Vector3(scale * cur.x, scale * cur.y, scale * cur.z);

        }
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

		FindVertexCoordsForApple(1.0f, 0.0f, 0.5f);
	
		//RotateXVec(ref VB, Mathf.PI / 2 * param1_0);
		//rotateAroundPoint(ref VB, new Vector3(90.0f * param1_0, 0, 0));
		for (int i = VB.Length - 1; i >= 0; --i)
		{
			VB[i].y -= VB[0].y + 2 * rApple;
		}
		rotateAroundPoint(ref VB, new Vector3(0, 180, 0), 1);
		rotateAroundPoint(ref NB, new Vector3(0, 180, 0), 1);

		Vector3[] MeshVB = new Vector3[nSections * 2 * (2 * nSegments + 1)];
		int[] MeshIB = new int[nSections * wireAppleIB.Length];
		Vector3[] MeshNB = new Vector3[nSections * 2 * (2 * nSegments + 1)];
		rotateAroundPoint(ref VB, new Vector3(0, -360f * phi - 360.0f / nSections, 0), 1);
		rotateAroundPoint(ref NB, new Vector3(0, -360f * phi - 360.0f / nSections, 0), 1);
		for (int j = 0; j < nSections; ++j)
		{
			rotateAroundPoint(ref VB, new Vector3(0, 360.0f / nSections	, 0), 1);
			VB.CopyTo(VBcopy, 0);
			VBcopy.CopyTo(MeshVB, j * 2 * (2 * nSegments + 1));
			VBcopy.CopyTo(MeshVB, j * 2 * (2 * nSegments + 1) + (2 * nSegments + 1));

			rotateAroundPoint(ref NB, new Vector3(0, 360.0f / nSections	, 0), 1);
			NB.CopyTo(NBcopy, 0);
			NBcopy.CopyTo(MeshNB, j * 2 * (2 * nSegments + 1));
			NBcopy.CopyTo(MeshNB, j * 2 * (2 * nSegments + 1) + (2 * nSegments + 1));

			// GL.Color(new Color(40f / 255, 180f / 255, 10f / 255, 0.8F));
		}
		
		for (int j = 0; j < nSections; ++j)
		{
			wireAppleIB.CopyTo(MeshIB, j * wireAppleIB.Length);
			for(int i = j * wireAppleIB.Length; i < (j + 1) * wireAppleIB.Length; ++i) {
				MeshIB[i] += (2 * nSegments + 1) * 2 * j;
			}
		}
		for (int j = 0; j < 2 * phi * nSections - 1; ++j)
		{
			for(int i = j * wireAppleIB.Length / 2; i < j * wireAppleIB.Length / 2 + theta * wireAppleIB.Length / 2; ++i) {
				MeshIB[i] = 0;
			}
			for(int i = (j + 1) * wireAppleIB.Length / 2; i < (j + 1) * wireAppleIB.Length / 2 + theta * wireAppleIB.Length / 2; ++i) {
				MeshIB[i] = 0;
			}
		}

		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = mf.mesh;
		mesh.vertices = MeshVB;
		mesh.triangles = MeshIB;
		mesh.normals = MeshNB;
//		mesh.RecalculateNormals();
    }
}