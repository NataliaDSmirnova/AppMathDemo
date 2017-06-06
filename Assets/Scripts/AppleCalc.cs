using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AppleCalc : MonoBehaviour {
	public int nSections = 96; // количество долек (должно быть кратно 2 * sectionRatio)
	public static int sectionRatio = 4; // количество долек в проволочной модели будет nSections/sectionRatio
	public int nSegments = 60; // количество сегментов в линии (должно быть кратно 3)
	public static int segmentRatio = 4; // количество сегментов в проволочной модели будет nSegments/segmentRatio
	public float rApple = 0.25f; //параметр яблока
	public double solidWFRatio = 0.5;

	private float[] bottomHalfVB;
	private float[] bottomHalfNB;
	private int[] bottomHalfIB;
	private float[] topHalfVB;
	private float[] topHalfNB;
	private int[] topHalfIB;

	private float[] bottomHalfWireFrameVB;
	private int[] bottomHalfWireFrameIB;
	private float[] topHalfWireFrameVB;
	private int[] topHalfWireFrameIB;

	public Vector3[] vertices;
	public Vector3[] normals;
	public int[] trianglesSolid;
	public int[] trianglesWF;
	private List<int> triangles;

	// Use this for initialization
	void Start () {
		initTopHalfVB();
		initTopHalfNB();
		initTopHalfIB();
		initBottomHalfVB();
		initBottomHalfNB();
		initBottomHalfIB();

		initTopHalfWireFrameVB();
		initTopHalfWireFrameIB();
		initBottomHalfWireFrameVB();
		initBottomHalfWireFrameIB();

		InitFun ();
	}

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

	private void InitFun()
	{
		Vector3[] vertices2 = new Vector3[(bottomHalfVB.Length / 3) * (nSections + 1)];
		Vector3[] normals2 = new Vector3[(bottomHalfVB.Length / 3) * (nSections + 1)];
		int[] triangles2 = new int[bottomHalfIB.Length * (nSections + 1)];

		Vector3[] loval2 = new Vector3[(bottomHalfVB.Length / 3)];
		Vector3[] lovalnorm2 = new Vector3[(bottomHalfVB.Length / 3)];

		int[] lovalind2 = new int[bottomHalfIB.Length];

		for (int i = 0; i < loval2.Length; ++i)
		{
			loval2[i] = new Vector3(bottomHalfVB[3 * i], bottomHalfVB[3 * i + 1], bottomHalfVB[3 * i + 2]);
			lovalnorm2[i] = new Vector3(bottomHalfNB[3 * i], bottomHalfNB[3 * i + 1], bottomHalfNB[3 * i + 2]);
		}
		bottomHalfIB.CopyTo(lovalind2, 0);
		loval2.CopyTo(vertices2, 0);
		lovalnorm2.CopyTo(normals2, 0);
		lovalind2.CopyTo(triangles2, 0);

		for (int j = 0; j < nSections; ++j)
		{
			RotateYVec(ref loval2, 2 * Mathf.PI / nSections);
			RotateYVec(ref lovalnorm2, 2 * Mathf.PI / nSections);
			//rotateAroundPoint(ref loval, new Vector3(0, 360.0f / nSections, 0));
			for (int i = 0; i < lovalind2.Length; ++i)
			{
				lovalind2[i] += loval2.Length;
			}
			loval2.CopyTo(vertices2, (j + 1) * loval2.Length);
			lovalnorm2.CopyTo(normals2, (j + 1) * lovalnorm2.Length);
			lovalind2.CopyTo(triangles2, (j + 1) * lovalind2.Length);
		}

		Vector3[] vertices1 = new Vector3[(topHalfVB.Length / 3) * (nSections + 1)];
		Vector3[] normals1 = new Vector3[(topHalfVB.Length / 3) * (nSections + 1)];
		int[] triangles1 = new int[topHalfIB.Length * (nSections + 1)];

		//Vertices//
		Vector3[] loval = new Vector3[(topHalfVB.Length / 3)];
		Vector3[] lovalnorm = new Vector3[(topHalfVB.Length / 3)];
		int[] lovalind = new int[topHalfIB.Length];
		for (int i = 0; i < loval.Length; ++i)
		{
			loval[i] = new Vector3(topHalfVB[3 * i], topHalfVB[3 * i + 1], topHalfVB[3 * i + 2]);
			lovalnorm[i] = new Vector3(topHalfNB[3 * i], topHalfNB[3 * i + 1], topHalfNB[3 * i + 2]);
		}
		topHalfIB.CopyTo(lovalind, 0);
		loval.CopyTo(vertices1, 0);
		lovalnorm.CopyTo(normals1, 0);
		lovalind.CopyTo(triangles1, 0);

		for (int j = 0; j < nSections; ++j)
		{
			RotateYVec(ref loval, 2 * Mathf.PI / nSections);
			RotateYVec(ref lovalnorm, 2 * Mathf.PI / nSections);
			//rotateAroundPoint(ref loval, new Vector3(0, 360.0f / nSections, 0));
			for (int i = 0; i < lovalind.Length; ++i)
			{
				lovalind[i] += loval.Length;
			}
			loval.CopyTo(vertices1, (j + 1) * loval.Length);
			lovalnorm.CopyTo(normals1, (j + 1) * lovalnorm.Length);
			lovalind.CopyTo(triangles1, (j + 1) * lovalind.Length);
		}


		vertices = new Vector3[2* (vertices1.Length + vertices2.Length)];
		normals = new Vector3[2* (normals1.Length + normals2.Length)];
		trianglesSolid = new int[triangles1.Length + triangles2.Length];

		vertices1.CopyTo(vertices, 0);
		vertices2.CopyTo(vertices, vertices1.Length);
		normals1.CopyTo(normals, 0);
		normals2.CopyTo(normals, normals1.Length);
		triangles1.CopyTo(trianglesSolid, 0);
		for (int i = 0; i < triangles2.Length; ++i)
		{
			triangles2[i] += vertices1.Length;
		}

		triangles2.CopyTo(trianglesSolid, triangles1.Length);

		normals1.CopyTo(normals, normals1.Length + normals2.Length);
		normals2.CopyTo(normals, normals1.Length + normals2.Length + normals1.Length);
		for (int i = 0; i < normals1.Length + normals2.Length; ++i)
			normals [i] = -normals [i];
		vertices1.CopyTo(vertices, vertices1.Length + vertices2.Length);
		vertices2.CopyTo(vertices, vertices1.Length + vertices2.Length + vertices1.Length);

		triangles = new List<int>();
		triangles.AddRange(trianglesWF);
		triangles.AddRange(trianglesSolid);
		int threshIndex = (int)(triangles.Count * solidWFRatio);
		threshIndex -= threshIndex % 3;

		List<int> tS = triangles.GetRange (0, threshIndex);
		List<int> tWF = triangles.GetRange (threshIndex, (triangles.Count - threshIndex));
		tS.AddRange (tS);
		tWF.AddRange (tWF);
		for (int i = 0; i < tS.Count / 2; ++i) tS [i] += vertices1.Length + vertices2.Length;
		for (int i = 0; i < tS.Count / 2; i += 3) { 
			int tmp = tS [i + 1];
			tS [i + 1] = tS [i + 2];
			tS [i + 2] = tmp; 
		}
		for (int i = 0; i < tWF.Count / 2; ++i) tWF [i] += vertices1.Length + vertices2.Length;
		for (int i = 0; i < tWF.Count / 2; i += 3) { 
			int tmp = tWF [i + 1];
			tWF [i + 1] = tWF [i + 2];
			tWF [i + 2] = tmp; 
		}
		trianglesSolid = tS.ToArray();
		trianglesWF = tWF.ToArray();
	}


	public void initTopHalfVB()
	{
  		float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSections);
		float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSections);
		float t, x, y;

		float[] fillAppleCoords = new float[6 * 2 * nSegments / 3 + 3];

		for (int i = 0, j = 0, k = 3 * 2 * nSegments / 3; i <= 2 * nSegments / 3; i++, j += 3, k += 3)
		{
			t = (float)((float)(nSegments - i) / (nSegments) * Mathf.PI - Mathf.PI);
			x = -(2 * rApple * (float)Mathf.Sin(t) - rApple * (float)Mathf.Sin(2 * t));
			y = 2 * rApple * (float)Mathf.Cos(t) - rApple * (float)Mathf.Cos(2 * t);

			fillAppleCoords[j] = x;
			fillAppleCoords[j + 1] = y + 0.75f;
			fillAppleCoords[j + 2] = 0.0f;

			if (i == 0)
				continue;
			fillAppleCoords[k] = cosAlpha * x;
			fillAppleCoords[k + 1] = y + 0.75f;
			fillAppleCoords[k + 2] = -sinAlpha * x;

		}
		topHalfVB = fillAppleCoords;
	}

	public void initTopHalfNB()
	{
		float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSections);
		float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSections);
		float t, f, f1, f2, r;

		float[] normalCoords = new float[3 * (2 * nSegments / 3 + 1) + 3 * 2 * nSegments / 3];

		for (int i = 0, j = 0, k = 3 * 2 * nSegments / 3; i <= 2 * nSegments / 3; i++, j += 3, k += 3)
		{
			t = (float)((float)(nSegments - i) / (nSegments) * Mathf.PI - Mathf.PI);
			f = ((float)Mathf.Sin(2 * t) - (float)Mathf.Sin(t)) / ((float)Mathf.Cos(2 * t) - (float)Mathf.Cos(t));
			f1 = ((float)Mathf.Sin(2 * t) - (float)Mathf.Sin(t));
			f2 = -((float)Mathf.Cos(2 * t) - (float)Mathf.Cos(t));
			r = (float)Mathf.Sqrt(f1 * f1 + f2 * f2);

			if (f == 0) {
				normalCoords[j] = 0.0f;
				normalCoords[j + 1] = -1.0f;
				normalCoords[j + 2] = 0.0f;
			} else {
				normalCoords[j] = f1 / r;
				normalCoords[j + 1] = f2 / r;
				normalCoords[j + 2] = 0.0f;
			}
			if (i == 0) continue;
			normalCoords[k] = cosAlpha * normalCoords[j];
			normalCoords[k + 1] = normalCoords[j + 1];
			normalCoords[k + 2] = -sinAlpha * normalCoords[j];
		}
		topHalfNB = normalCoords;
	}

	public void initTopHalfIB()
	{
		int[] Order = new int[3 * (2 * 2 * nSegments / 3 - 1)];
		Order[0] = (short)(0);
		Order[1] = (short)(1);
		Order[2] = (short)(1 + 2 * nSegments / 3);
		for (int i = 1; i < 2 * nSegments / 3; i++)
		{
			Order[6 * i - 3] = (short)(i);
			Order[6 * i + 1 - 3] = (short)(i + 1);
			Order[6 * i + 2 - 3] = (short)(i + 1 + 2 * nSegments / 3);

			Order[6 * i + 3 - 3] = (short)(i);
			Order[6 * i + 4 - 3] = (short)(i + 1 + 2 * nSegments / 3);
			Order[6 * i + 5 - 3] = (short)(i + 2 * nSegments / 3);
		}
		topHalfIB = Order;
	}

	public void initBottomHalfVB()
	{
		float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSections);
		float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSections);
		float t, x, y;

		float[] fillAppleCoords = new float[3 * (nSegments / 3 + 1) + 3 * nSegments / 3];

		for (int i = 0, j = 0, k = 3 * nSegments / 3; i <= nSegments / 3; i++, j += 3, k += 3)
		{
			t = (float)((float)(i) / (nSegments) * Mathf.PI - Mathf.PI);
			x = -(2 * rApple * (float)Mathf.Sin(t) - rApple * (float)Mathf.Sin(2 * t));
			y = 2 * rApple * (float)Mathf.Cos(t) - rApple * (float)Mathf.Cos(2 * t);

			fillAppleCoords[j] = x;
			fillAppleCoords[j + 1] = y + 0.75f;
			fillAppleCoords[j + 2] = 0.0f;

			if (i == 0)
				continue;
			fillAppleCoords[k] = cosAlpha * x;
			fillAppleCoords[k + 1] = y + 0.75f;
			fillAppleCoords[k + 2] = -sinAlpha * x;
		}
		bottomHalfVB = fillAppleCoords;
	}

	public void initBottomHalfNB()
	{
		float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSections);
		float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSections);
		float t, f, f1, f2, r;

		float[] normalCoords = new float[3 * (nSegments / 3 + 1) + 3 * nSegments / 3];

		for (int i = 0, j = 0, k = 3 * nSegments / 3; i <= nSegments / 3; i++, j += 3, k += 3)
		{
			t = (float)((float)(i) / (nSegments) * Mathf.PI - Mathf.PI);
			f = ((float)Mathf.Sin(2 * t) - (float)Mathf.Sin(t)) / ((float)Mathf.Cos(2 * t) - (float)Mathf.Cos(t));
			f1 = ((float)Mathf.Sin(2 * t) - (float)Mathf.Sin(t));
			f2 = -((float)Mathf.Cos(2 * t) - (float)Mathf.Cos(t));
			r = (float)Mathf.Sqrt(f1 * f1 + f2 * f2);

			if (f == 0)
			{
				normalCoords[j] = 0.0f;
				normalCoords[j + 1] = -1.0f;
				normalCoords[j + 2] = 0.0f;
			}
			else
			{
				normalCoords[j] = f1 / r;
				normalCoords[j + 1] = f2 / r;
				normalCoords[j + 2] = 0.0f;
			}

			if (i == 0)
				continue;

			normalCoords[k] = cosAlpha * normalCoords[j];
			normalCoords[k + 1] = normalCoords[j + 1];
			normalCoords[k + 2] = -sinAlpha * normalCoords[j];
		}

		bottomHalfNB = normalCoords;
	}

	public void initTopHalfWireFrameVB()
	{
		int nSeg = nSegments / segmentRatio;
		int nSect = nSections / sectionRatio;
		float[] Coords = new float[3 * 2 * 2 * nSeg / 3 + 3];

		float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSect);
		float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSect);
		float t, x, y;
		for (int i = 0, j = 0, k = 3 * 2 * nSeg / 3; i <= 2 * nSeg / 3; i++, j += 3, k += 3)
		{
			t = (float)((float)(nSeg - i) / nSeg * Mathf.PI - Mathf.PI);
			x = 2 * rApple * (float)Mathf.Sin(t) - rApple * (float)Mathf.Sin(2 * t);
			y = 2 * rApple * (float)Mathf.Cos(t) - rApple * (float)Mathf.Cos(2 * t);

			Coords[j] = x;
			Coords[j + 1] = y;
			Coords[j + 2] = 0;

			if (i == 0)
				continue;
			Coords[k] = cosAlpha * x;
			Coords[k + 1] = y;
			Coords[k + 2] = -sinAlpha * x;
		}
		topHalfWireFrameVB = Coords;
	}

	public void initBottomHalfIB()
	{
		int[] trianglesOrder = new int[3 * (2 * nSegments / 3 - 1)];
		trianglesOrder[0] = (short)(0);
		trianglesOrder[1] = (short)(1);
		trianglesOrder[2] = (short)(1 + nSegments / 3);
		for (int i = 1; i < nSegments / 3; i++)
		{
			trianglesOrder[6 * i + 2 - 3] = (short)(i);
			trianglesOrder[6 * i + 1 - 3] = (short)(i + 1);
			trianglesOrder[6 * i - 3] = (short)(i + 1 + nSegments / 3);

			trianglesOrder[6 * i + 5 - 3] = (short)(i);
			trianglesOrder[6 * i + 4 - 3] = (short)(i + 1 + nSegments / 3);
			trianglesOrder[6 * i + 3 - 3] = (short)(i + nSegments / 3);
		}
		bottomHalfIB = trianglesOrder;
	}

	public void initTopHalfWireFrameIB()
	{
		int nSeg = nSegments / segmentRatio;
		int[] lineOrder = new int[2 * 2 * nSeg / 3 * 2];
		for (int i = 0, k = 1; i < 2 * nSeg / 3; i++, k++)
		{
			lineOrder[2 * i] = (short)(i);
			lineOrder[2 * i + 1] = (short)(i + 1);
			lineOrder[2 * 2 * nSeg / 3 + 2 * i] = (short)(k);
			lineOrder[2 * 2 * nSeg / 3 + 2 * i + 1] = (short)(k + 2 * nSeg / 3);
		}
		topHalfWireFrameIB = lineOrder;
	}

	public void initBottomHalfWireFrameVB()
	{
		int nSeg = nSegments / segmentRatio;
		int nSect = nSections / sectionRatio;
		float[] Coords = new float[3 * 2 * nSeg / 3 + 3];

		float sinAlpha = (float)Mathf.Sin(2 * Mathf.PI / nSect);
		float cosAlpha = (float)Mathf.Cos(2 * Mathf.PI / nSect);
		float t, x, y;
		for (int i = 0, j = 0, k = 3 * nSeg / 3; i <= nSeg / 3; i++, j += 3, k += 3)
		{
			t = (float)((float)(i) / nSeg * Mathf.PI - Mathf.PI);
			x = 2 * rApple * (float)Mathf.Sin(t) - rApple * (float)Mathf.Sin(2 * t);
			y = 2 * rApple * (float)Mathf.Cos(t) - rApple * (float)Mathf.Cos(2 * t);

			Coords[j] = x;
			Coords[j + 1] = y;
			Coords[j + 2] = 0;

			if (i == 0)
				continue;
			Coords[k] = cosAlpha * x;
			Coords[k + 1] = y;
			Coords[k + 2] = -sinAlpha * x;
		}
		bottomHalfWireFrameVB = Coords;
	}

	public void initBottomHalfWireFrameIB()
	{
		int nSeg = nSegments / segmentRatio;
		int[] lineOrder = new int[2 * nSeg / 3 * 2];
		for (int i = 0, k = 1; i < nSeg / 3; i++, k++)
		{
			lineOrder[2 * i] = (short)(i);
			lineOrder[2 * i + 1] = (short)(i + 1);
			lineOrder[2 * nSeg / 3 + 2 * i] = (short)(k);
			lineOrder[2 * nSeg / 3 + 2 * i + 1] = (short)(k + nSeg / 3);
		} 
		bottomHalfWireFrameIB = lineOrder;
	}


	// Update is called once per frame
	void Update () {

	}
}