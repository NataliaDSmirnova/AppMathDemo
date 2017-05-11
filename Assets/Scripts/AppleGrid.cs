using UnityEngine;

public class AppleGrid : MonoBehaviour
{
    private int nSections = 16; // количество долек
    private int nSegments = 20; // количество сегментов в линии
    private float len = 0.92f; // длина линии в сетке
    private float rApple = 0.25f;
    private Vector3[] VB;
    private Vector3[] VBcopy;
    private short[] gridIB;
    private short[] wireAppleIB;
    float time = 0;
    // When added to an object, draws colored rays from the
    // transform position.
    public int lineCount = 100;
    public float radius = 3.0f;
    public void Start()
    {
        initVertexArray();
        initIndexArrayForAppear();
        initIndexArrayForApple();
    } 
    public void initVertexArray()
    {
        VBcopy = new Vector3[2 * nSegments + 1];
        FindVertexCoordsForApple(1.0f, 0.0f);
    }
    public void initIndexArrayForAppear()
    {
        short[] lineOrder = new short[2 * nSegments * 2];
        for (int i = 0; i < nSegments * 2; i++)
        {
            lineOrder[2 * i] = (short)(i);
            lineOrder[2 * i + 1] = (short)(i + 1);
        }
        gridIB = lineOrder;
    }
    public void initIndexArrayForApple()
    {
        short[] lineOrder = new short[2 * nSegments * 2];
        for (int i = 0, k = 1; i < nSegments; i++, k++)
        {
            lineOrder[2 * i] = (short)(i);
            lineOrder[2 * i + 1] = (short)(i + 1);
            lineOrder[2 * nSegments + 2 * i] = (short)(k);
            lineOrder[2 * nSegments + 2 * i + 1] = (short)(k + nSegments);
        }
        wireAppleIB = lineOrder;
    }
    public void FindVertexCoordsForApple(float param0_1, float param1_0)
    {
        Vector3[] Coords = new Vector3[2 * nSegments + 1];

        float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSections);
        float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSections);
        float t, x, y;
        float xCoord, yCoord;
        for (int i = 0, j = 0, k = nSegments; i <= nSegments; i++, ++j, ++k)
        {
            t = (float)((float)(i) / nSegments * Mathf.PI * param0_1 - Mathf.PI);
            x = 2 * rApple * (float)Mathf.Sin(t) - rApple * (float)Mathf.Sin(2 * t);
            y = 2 * rApple * (float)Mathf.Cos(t) - rApple * (float)Mathf.Cos(2 * t);

            xCoord = -param0_1 * x + param1_0 * len / nSegments * i;
            yCoord = param0_1 * y + param1_0 * -rApple;

            Coords[j].x = xCoord;
            Coords[j].z = yCoord;
            Coords[j].y = 0;

            if (i == 0)
                continue;
            Coords[k].x = cosAlpha * xCoord;
            Coords[k].z = yCoord;
            Coords[k].y = -sinAlpha * xCoord;
        }
        VB = Coords;
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
            Coords[2 * i].z = -rApple;
            Coords[2 * i].y = sinAlpha * i * length;

            if (i == nSegments)
                break;
            Coords[2 * i + 1].x = cosAlpha * (i + 1) * length;
            Coords[2 * i + 1].z = -rApple;
            Coords[2 * i + 1].y = sinAlpha * (i + 1) * length;
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
        y = Mathf.Sin(angle) * cy + Mathf.Cos(angle) * cz;
    }
    void rotateY(ref float x, ref float y, ref float z, float angle)
    {
        float cx = x, cy = y, cz = z;
        x = Mathf.Cos(angle) * cx + Mathf.Sin(angle) * cz;
        y = cy;
        y = -Mathf.Sin(angle) * cx + Mathf.Cos(angle) * cz;
    }

    void rotateZ(ref float x, ref float y, ref float z, float angle)
    {
        float cx = x, cy = y, cz = z;
        x = Mathf.Cos(angle) * cx - Mathf.Sin(angle) * cy;
        y = Mathf.Sin(angle) * cx + Mathf.Cos(angle) * cy;
        y = cz;
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

    void rotateAroundPoint(ref Vector3[] VV, Vector3 angles)
    {
        Vector3 center = VV[0];
        for (int i = 1; i < VV.Length; ++i)
        {
            Vector3 cur = VV[i];
            cur = RotatePointAroundPivot(cur, center, angles);
            VV[i] = cur;

        }
    }

    public void OnRenderObject()
    {
        time += Time.deltaTime;
        if (time < 18.0f * 0.3)
        {
            float T = (float)time / (18f * 0.3f);

            float param1_0 = ((float)Mathf.Cos(T * Mathf.PI) + 1) / 2.0f;
            float param0_1 = ((float)Mathf.Sin(T * Mathf.PI - Mathf.PI / 2.0f) + 1) / 2.0f;

            FindVertexCoordsForAppear(1 - param1_0, 1 - param0_1);

            RotateXVec(ref VB, Mathf.PI / 2);
            //rotateAroundPoint(ref VB, new Vector3(90, 0, 0));
            rotateAroundPoint(ref VB, new Vector3(0, 180, 0));

            CreateLineMaterial();
            // Apply the line material
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);

            for (int i = 0; i < gridIB.Length; i += 2)
            {
                // Vertex colors change from red to green
                GL.Color(new Color(0, 1, 0, 0.8F));
                // One vertex at transform position
                GL.Vertex3(VB[gridIB[i]].x, VB[gridIB[i]].y, VB[gridIB[i]].z);
                // Another vertex at edge of circle
                GL.Vertex3(VB[gridIB[i + 1]].x, VB[gridIB[i + 1]].y, VB[gridIB[i + 1]].z);
            }
            for (int j = 0; j < nSections; ++j)
            {
                rotateAroundPoint(ref VB, new Vector3(0, 0, param0_1 * 360.0f / nSections));
                //RotateZVec(ref VB, param0_1 * 2.0f * Mathf.PI / nSections);
                for (int i = 0; i < gridIB.Length; i += 2)
                {
                    // Vertex colors change from red to green
                    GL.Color(new Color(0, 1, 0, 0.8F));
                    // One vertex at transform position
                    GL.Vertex3(VB[gridIB[i]].x, VB[gridIB[i]].y, VB[gridIB[i]].z);
                    // Another vertex at edge of circle
                    GL.Vertex3(VB[gridIB[i + 1]].x, VB[gridIB[i + 1]].y, VB[gridIB[i + 1]].z);
                }
            }
            GL.End();
            GL.PopMatrix();
        } else if (time < 18f * 0.6)
        {
            float T = (float)(time - 18f * 0.3f) / (18f * 0.3f);
            float param1_0 = ((float)Mathf.Cos(T * Mathf.PI) + 1) / 2.0f;
            float param0_1 = ((float)Mathf.Sin(T * Mathf.PI - Mathf.PI / 2.0f) + 1) / 2.0f;

            FindVertexCoordsForApple(param0_1, param1_0);


            //RotateXVec(ref VB, Mathf.PI / 2 * param1_0);
            //rotateAroundPoint(ref VB, new Vector3(90.0f * param1_0, 0, 0));
            RotateXVec(ref VB, Mathf.PI / 2);
            rotateAroundPoint(ref VB, new Vector3(0, 180, 0));

            CreateLineMaterial();
            // Apply the line material
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);

            for (int i = 0; i < wireAppleIB.Length; i += 2)
            {
                // Vertex colors change from red to green
                VB.CopyTo(VBcopy, 0);
                rotateAroundPoint(ref VBcopy, new Vector3(T * 90, 0, 0));
                GL.Color(new Color(0, 1, 0, 0.8F));
                // One vertex at transform position
                GL.Vertex3(VBcopy[wireAppleIB[i]].x, VBcopy[wireAppleIB[i]].y, VBcopy[wireAppleIB[i]].z);
                // Another vertex at edge of circle
                GL.Vertex3(VBcopy[wireAppleIB[i + 1]].x, VBcopy[wireAppleIB[i + 1]].y, VBcopy[wireAppleIB[i + 1]].z);
            }
            for (int j = 0; j < nSections; ++j)
            {
                rotateAroundPoint(ref VB, new Vector3(0, 0, 360.0f / nSections));
                VB.CopyTo(VBcopy, 0);
                rotateAroundPoint(ref VBcopy, new Vector3(T * 90, 0, 0));
                //RotateZVec(ref VB, param0_1 * 2.0f * Mathf.PI / nSections);
                for (int i = 0; i < wireAppleIB.Length; i += 2)
                {
                    // Vertex colors change from red to green
                    GL.Color(new Color(0, 1, 0, 0.8F));
                    // One vertex at transform position
                    GL.Vertex3(VBcopy[wireAppleIB[i]].x, VBcopy[wireAppleIB[i]].y, VBcopy[wireAppleIB[i]].z);
                    // Another vertex at edge of circle
                    GL.Vertex3(VBcopy[wireAppleIB[i + 1]].x, VBcopy[wireAppleIB[i + 1]].y, VBcopy[wireAppleIB[i + 1]].z);
                }
            }
            GL.End();
            GL.PopMatrix();
        }

    }
}