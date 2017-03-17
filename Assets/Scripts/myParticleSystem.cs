using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class myParticleSystem : MonoBehaviour
	{

		public Material material; // Simple Material
		public int particleWidth = 5; // number of particles in width
		public int particleHeight = 5; // number of particles in height
		public float width = 5.0f;
		public float height = 5.0f;

		List<myParticle> particles;
		List<Constraint> constraints;

		myParticle getParticle(int x, int y) {
			return particles [y * particleWidth + x];
		}

		public void makeConstraint(myParticle p1, myParticle p2) {
			constraints.Add (new Constraint (p1, p2));
		}

		// Not normalized
		// Length of normal = parallelagram formed by p1, p2, and p3
		Vector3 getTriangleNormal(myParticle p1, myParticle p2, myParticle p3) {
			Vector3 v12 = p2.getPos () - p1.getPos ();
			Vector3 v13 = p3.getPos () - p1.getPos ();

			return Vector3.Cross (v12, v13);
		}

		void wind(myParticle p1, myParticle p2, myParticle p3, Vector3 windDir) {
			Vector3 normal = getTriangleNormal (p1, p2, p3);
			Vector3 normalDir = Vector3.Normalize (normal);
			Vector3 force = normal * Vector3.Dot (normalDir, windDir); // based on area of the triangle as well
			p1.addForce (force);
			p2.addForce (force);
			p3.addForce (force);
		}

		void drawTriangle (myParticle p1, myParticle p2, myParticle p3) {
			// Drawing triangle
			// Reference: https://docs.unity3d.com/ScriptReference/GL.TRIANGLES.html

			Vector3 p1pos = p1.getPos ();
			Vector3 p2pos = p2.getPos ();
			Vector3 p3pos = p3.getPos ();

			// Debug.Log ("Triangle: " + p1pos.ToString () + " " + p2pos.ToString () + " " + p3pos.ToString ());

			GL.Vertex3(p1pos[0], p1pos[1], p1pos[2]);
			GL.Vertex3(p2pos[0], p2pos[1], p2pos[2]);
			GL.Vertex3(p3pos[0], p3pos[1], p3pos[2]);
		}

		// Similar to OnPostRender
		// OnRenderObject: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnRenderObject.html
		// OnPostRender: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnPostRender.html
		// Draws after the camera has finished rendering the scene
		void OnRenderObject() {
			material.SetPass(0);

			GL.PushMatrix();
			GL.MultMatrix (transform.localToWorldMatrix);
			GL.Begin(GL.TRIANGLES);

			for (int i = 0; i < particleWidth - 1; i++) {
				for (int j = 0; j < particleHeight - 1; j++) {
					Vector3 p1pos = getParticle(i, j).getPos ();
					Vector3 p2pos = getParticle(i + 1, j).getPos ();
					Vector3 p3pos = getParticle (i, j + 1).getPos ();

					drawTriangle (getParticle (i, j), getParticle (i + 1, j), getParticle (i, j + 1));
					drawTriangle (getParticle (i + 1, j), getParticle (i + 1, j + 1), getParticle (i, j + 1));
				}
			}

			GL.End();
			GL.PopMatrix();
		}

		void Start () {
			if (!material) {
				Debug.Log ("here");
				material = new Material (Shader.Find ("Diffuse"));
			}

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

			particles = new List<myParticle>();
			constraints = new List<Constraint>();

			// Create all the particles
			for (int i = 0; i < particleWidth; i++) {
				for (int j = 0; j < particleHeight; j++) {
					Vector3 position = new Vector3 (width * (i / (float)particleWidth),
						                   -height * (j / (float)particleHeight),
						                   0.0f);
					particles.Add(new myParticle (position));
				}
			}

			// Create all of the constraints
			// Neighbors
			// A -  B
			// |
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
			getParticle (particleHeight - 1, 0).setPinned(true);

			//drawCloth ();
		}
	}
}

