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

		Vector3 topLeft;
		Vector3 topRight;

		public int particleWidth = 5; // number of particles in width
		public int particleHeight = 5; // number of particles in height
		public float width = 5.0f;
		public float height = 5.0f;
		public int iterations = 100;
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

		void Start () {
			topLeft = new Vector3(0.0f, 0.0f, 0.0f);
			topRight = new Vector3 (width, 0.0f, 0.0f);

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
					particles[idx] = new myParticle (position);
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


//					Debug.Log ("Start Madness");
//					Debug.Log (getParticle (i, j).getPos ());
//					Debug.Log (getParticle (i + 1, j).getPos ());
//					Debug.Log (getParticle (i + 1, j + 1).getPos ());
//					Debug.Log (getParticle (i, j + 1).getPos ());
//
//					Debug.Log ("Stop madness");

					float what = 5.6f;
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
//			getParticle(0, 0).setPinned(true);
//			getParticle (particleWidth - 1, 0).setPinned (true);


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
				
			mesh.vertices = vert;
			mesh.triangles = tria;
			mesh.RecalculateNormals ();
			GetComponent<MeshFilter> ().mesh = mesh;
		}
	



		void FixedUpdate() {
			addWindForce (windDirection);

			for (int i = 0; i < particles.Length ; i++) {
				particles [i].computeDynamics();
			}

			for (int i = 0; i < particles.Length ; i++) {
				particles [i].updateParticle ();
			}

			//Satisfy the Contraints
			for (int i = 0; i < iterations; i++) {
				for (int j = 0; j < constraints.Count; j++) {
					constraints [j].satisfy ();
				}

				vert [0] = topLeft;
				vert [getParticleIdx(particleWidth - 1, 0)] = topRight;

				getParticle (0, 0).setPos (topLeft);
				getParticle (particleWidth - 1, 0).setPos (topRight);

			}


			for (int i = 0; i < particles.Length ; i++) {
				vert[i] = particles [i].getPos();
			}

			GetComponent<MeshFilter> ().mesh.vertices = vert;
		}

		void OnPreRender() {
			GL.wireframe = true;
		}
	}
}

