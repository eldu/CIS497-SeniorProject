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

	// Use this for initialization
	void Start () {
		p1 = new Vector3 (0, 0, 0);
		p2 = new Vector3 (1, 0, 0);
		p3 = new Vector3 (1, 1, 0);

		// Unity has a built-in shader that is useful for drawing
		// simple colored things. In this case, we just want to use
		// a blend mode that inverts destination colors.			
		var shader = Shader.Find ("Hidden/Internal-Colored");
		material = new Material (shader);
		material.hideFlags = HideFlags.HideAndDontSave;
		// Set blend mode to invert destination colors.
		material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
		material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
		// Turn off backface culling, depth writes, depth test.
		material.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		material.SetInt ("_ZWrite", 0);
		material.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void OnRenderObject() {
		material.SetPass(0);

		GL.PushMatrix();
		GL.MultMatrix (transform.localToWorldMatrix);
		GL.Begin(GL.TRIANGLES);

		GL.Vertex3(p1[0], p1[1], p1[2]);
		GL.Vertex3(p2[0], p2[1], p2[2]);
		GL.Vertex3(p3[0], p3[1], p3[2]);

		GL.End();
		GL.PopMatrix();
	}
}
