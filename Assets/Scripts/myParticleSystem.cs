using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace AssemblyCSharp
{
	public class myParticleSystem : MonoBehaviour
	{
		private Vector3[] vert;
		private int[] tria;
		private Mesh mesh;

		public Material material; // Simple Material
		public int particleWidth = 5; // number of particles in width
		public int particleHeight = 5; // number of particles in height
		public float width = 5.0f;
		public float height = 5.0f;
		public int iterations = 20;
		public Vector3 windDirection = new Vector3 (0.5f, 0.0f, 0.2f);

		private myParticle[] particles;
		List<Constraint> constraints;

		public Vector3[] getVertices() {
			return vert;
		}

		int getParticleIdx(int i, int j) {
			return j * particleWidth + i;
		}

		myParticle getParticle(int i, int j) {
			return particles [j * particleWidth + i];
		}

		public void setVertex(int idx, Vector3 value) {
			vert [idx] = value;
		}

		public void makeConstraint(myParticle p1, myParticle p2) {
			constraints.Add (new Constraint (p1, p2));
		}

		// Not normalized
		// Length of normal = parallelagram formed by p1, p2, and p3
		Vector3 getTriangleNormal(myParticle p1, myParticle p2, myParticle p3) {
			Vector3 v12 = p2.getPos () - p1.getPos ();
			Vector3 v13 = p3.getPos () - p1.getPos ();
			v12.Normalize ();
			v13.Normalize ();

			return Vector3.Cross (v12, v13);
		}

		// Just gets a normalized normal from the triangle points
		Vector3 getNormal(Vector3 p1, Vector3 p2, Vector3 p3) {
			Vector3 v12 = p2 - p1;
			Vector3 v13 = p3 - p1;
			Vector3 normal = Vector3.Cross (v12, v13);
			normal.Normalize ();
		
			return normal;
		}
			
		// Adds a wind force to a triangle
		void addWindToTriangle(myParticle p1, myParticle p2, myParticle p3, Vector3 windDir) {
			Vector3 normal = getTriangleNormal (p1, p2, p3);
			Vector3 normalDir = Vector3.Normalize (normal);
			Vector3 force = normal * Vector3.Dot (normalDir, windDir); // based on area of the triangle as well
			p1.addWindForce (force);
			p2.addWindForce (force);
			p3.addWindForce (force);
		}

		// Add wind force to the entire cloth
		void addWindForce(Vector3 direction) {
			for (int i = 0; i < particleWidth - 1; i++) {
				for (int j = 0; j < particleHeight - 1; j++) {
					addWindToTriangle (getParticle (i, j), getParticle (i + 1, j), getParticle (i, j + 1), direction);
					addWindToTriangle (getParticle (i + 1, j), getParticle (i + 1, j + 1), getParticle (i, j + 1), direction);
				}
			}
		}

//		void drawTriangle (myParticle p1, myParticle p2, myParticle p3) {
//			// Drawing triangle
//			// Reference: https://docs.unity3d.com/ScriptReference/GL.TRIANGLES.html
//			// Use Tri.cs to doing smaller test cases with GL.Triangles
//
//			Vector3 p1pos = p1.getPos ();
//			Vector3 p2pos = p2.getPos ();
//			Vector3 p3pos = p3.getPos ();
//
//			// Debug.Log ("Triangle: " + p1pos.ToString () + " " + p2pos.ToString () + " " + p3pos.ToString ());
//			Vector3 normal = getTriangleNormal (p1, p2, p3);
//			// normal.Normalize ();
//			GL.Color(new Color(normal.x, normal.y, normal.z, 1));
//			GL.Vertex3(p1pos[0], p1pos[1], p1pos[2]);
//			GL.Vertex3(p2pos[0], p2pos[1], p2pos[2]);
//			GL.Vertex3(p3pos[0], p3pos[1], p3pos[2]);
//
//			// DEBUG: TODO: Hope this isn't instruding..
//			Debug.DrawLine (p1pos, p2pos);
//			Debug.DrawLine (p2pos, p3pos);
//			Debug.DrawLine (p3pos, p1pos);
//
//			// Debug.Log ("Triangle: " + p1pos + " " + p2pos + " " + p3pos);
//		}



//		// Similar to OnPostRender
//		// OnRenderObject: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnRenderObject.html
//		// OnPostRender: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnPostRender.html
//		// Draws after the camera has finished rendering the scene
//		void OnRenderObject() {
//			material.SetPass(0);
//
//			GL.PushMatrix();
//			GL.MultMatrix (transform.localToWorldMatrix);
//			GL.Begin(GL.TRIANGLES);
//
//			for (int i = 0; i < particleWidth - 1; i++) {
//				for (int j = 0; j < particleHeight - 1; j++) {
//					drawTriangle (getParticle (i, j), getParticle (i + 1, j), getParticle (i, j + 1));
//					drawTriangle (getParticle (i + 1, j), getParticle (i + 1, j + 1), getParticle (i, j + 1));
//				}
//			}
//
//			GL.End();
//			GL.PopMatrix();
//		}

		void Start () {
			if (!material) {
				// Debug.Log ("here");
				material = new Material (Shader.Find ("Diffuse"));
			}

//			// Unity has a built-in shader that is useful for drawing
//			// simple colored things. In this case, we just want to use
//			// a blend mode that inverts destination colors.			
//			var shader = Shader.Find ("Hidden/Internal-Colored");
//			material = new Material (shader);
//			material.hideFlags = HideFlags.HideAndDontSave;
//			// Set blend mode to invert destination colors.
//			material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
//			material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//			// Turn off backface culling, depth writes, depth test.
//			material.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
//			material.SetInt ("_ZWrite", 0);
//			material.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

			particles = new myParticle[particleWidth * particleHeight];
			constraints = new List<Constraint>();
	
			// Create a mesh
			mesh = new Mesh();
			vert = new Vector3[particleWidth * particleHeight];
			tria = new int[(particleWidth - 1) * (particleHeight - 1) * 6];

			// Create all the particles
			int idx = 0;
			for (int j = 0; j < particleHeight; j++) {
				for (int i = 0; i < particleWidth; i++) {
					Vector3 position = new Vector3 (width * (i / (float) (particleWidth - 1)),
						-height * (j / (float) (particleHeight - 1)),
						                   0.0f);
					particles[idx] = new myParticle (this, idx, position);
					vert [idx++] = position;
				}
			}

			// Create all of the constraints
			// Neighbors
			// A -  B
			// |  X
			// C    D
			for (int i = 0; i < particleWidth - 1; i++) {
				for (int j = 0; j < particleHeight - 1; j++) {
					// A - B
					makeConstraint (getParticle (i, j), getParticle (i + 1, j));
					// A - C
					makeConstraint (getParticle (i, j), getParticle (i, j + 1));
					// A - D
					makeConstraint (getParticle (i, j), getParticle (i + 1, j + 1));
					// B - C
					makeConstraint (getParticle (i + 1, j), getParticle(i, j + 1));
				}
			}

			// Bottom Row
			for (int i = 0; i < particleWidth - 1; i++) {
				makeConstraint (getParticle (i, particleHeight - 1), 
					getParticle (i + 1, particleHeight - 1));
			}

			// Right most
			for (int j = 0; j < particleHeight - 1; j++) {
				makeConstraint (getParticle(particleWidth - 1, j),
					getParticle(particleWidth - 1, j + 1));
			}

			// Pin top two corners
			getParticle(0, 0).setPinned(true);
			getParticle (particleWidth - 1, 0).setPinned (true);


			// Create the triangles
			idx = 0;
			for (int i = 0; i < particleWidth - 1; i++) {
				for (int j = 0; j < particleHeight - 1; j++) {
					tria [idx++] = getParticleIdx (i, j);
					tria [idx++] = getParticleIdx (i + 1, j);
					tria [idx++] = getParticleIdx (i, j + 1);
						 
					tria [idx++] = getParticleIdx (i + 1, j);
					tria [idx++] = getParticleIdx (i + 1, j + 1);
					tria [idx++] = getParticleIdx (i, j + 1);
				}
			}

			Vector2[] uvs = new Vector2[tria.Length];

			// Finish off the mesh
//			for (int i=0; i < uvs.Length; i++) {
//				uvs[i] = new Vector2(vert[i].x, vert[i].z);
//			}

			mesh.vertices = vert;
			// mesh.uv = uvs;
			mesh.triangles = tria;

			mesh.RecalculateNormals ();
			;

			GetComponent<MeshFilter> ().mesh = mesh;
		}
	

		void FixedUpdate() {
			addWindForce (windDirection);
				
			for (int i = 0; i < particles.Length ; i++) {
				particles [i].updateParticle ();
			}

			//Satisfy the Contraints
			for (int i = 0; i < iterations; i++) {
				for (int j = 0; j < constraints.Count; j++) {
					constraints [j].satisfy ();
				}
			}


			for (int i = 0; i < particles.Length ; i++) {
				vert[i] = particles [i].getPos();
			}

			GetComponent<MeshFilter> ().mesh.vertices = vert;
		}
	}
}

