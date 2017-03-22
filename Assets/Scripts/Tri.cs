using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is an experimental playground for me to to mess with GL Triangles and see how things work
// without having to mess around a lot with the cloth simulation.
public class Tri : MonoBehaviour {
	public Material material;
	public Vector3 p1;
	public Vector3 p2;
	public Vector3 p3;
	public Vector3 p4;
	public Vector3 p5;
	public Vector3 p6;

	private Vector3[] vert;
	private int[] tria;

	// Use this for initialization
	void Start () {
		p1 = new Vector3 (0, 0, 0);
		p2 = new Vector3 (1, 0, 0);
		p3 = new Vector3 (1, 1, 0);
		p4 = new Vector3 (1, 0, 0);
		p5 = new Vector3 (2, 0, 0);
		p6 = new Vector3 (2, 1, 0); 

		Mesh mesh = new Mesh();

		vert = new Vector3[6];
		vert [0] = p1;
		vert [1] = p2;
		vert [2] = p3;
		vert [3] = p4;
		vert [4] = p5;
		vert [5] = p6;

		tria = new int[6];
		tria [0] = 0;
		tria [1] = 1;
		tria [2] = 2;
		tria [3] = 3;
		tria [4] = 4;
		tria [5] = 5;


		Vector2[] uvs = new Vector2[tria.Length];

		for (int i=0; i < uvs.Length; i++) {
			uvs[i] = new Vector2(vert[i].x, vert[i].z);
		}
			
		mesh.vertices = vert;
		mesh.triangles = tria;
		mesh.uv = uvs;

		mesh.RecalculateNormals ();

		GetComponent<MeshFilter> ().mesh = mesh;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

//	void OnRenderObject() {
//		Vector3 normal = getNormal (p1, p2, p3);
//		normal.Normalize ();
//
//		material.SetPass(0);
//
//		GL.PushMatrix();
//		GL.MultMatrix (transform.localToWorldMatrix);
//		GL.Begin(GL.TRIANGLES);
//		GL.Color(new Color(normal.x, normal.y, normal.z, 1));
//
//		GL.Vertex3(p1[0], p1[1], p1[2]);
//		GL.Vertex3(p2[0], p2[1], p2[2]);
//		GL.Vertex3(p3[0], p3[1], p3[2]);
//
//		GL.Color(new Color(1, 0, 1, 1));
//
//		GL.Vertex3(p4[0], p4[1], p4[2]);
//		GL.Vertex3(p5[0], p5[1], p5[2]);
//		GL.Vertex3(p6[0], p6[1], p6[2]);
//
//		GL.End();
//		GL.PopMatrix();
//	}
//
//	Vector3 getNormal(Vector3 p1, Vector3 p2, Vector3 p3) {
//		Vector3 v12 = p2 - p1;
//		Vector3 v13 = p3 - p1;
//
//		return Vector3.Cross (v12, v13);
//	}

	public class what {
		public Vector3 value;

		public what(Vector3 _v) {
			value = _v;
		}
	}
}
