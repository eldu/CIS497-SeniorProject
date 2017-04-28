using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace AssemblyCSharp
{
	public class myParticleSystem : MonoBehaviour
	{
		public GameObject[] obstacles;
		public Vector3 k;

		private Vector3[] vert;
		private int[] tria;
		private Mesh mesh;

		Vector3 topLeft;
		Vector3 topRight;

		public int particleWidth = 5; // number of particles in width
		public int particleHeight = 5; // number of particles in height
		public float width = 5.0f;
		public float height = 5.0f;
		public int iterations = 5;
		public Vector3 windDirection = new Vector3 (0.5f, 0.0f, 0.2f);

		private myParticle[] particles;
		List<Constraint> constraints;

		int getParticleIdx(int i, int j) {
			return j * particleWidth + i;
		}

		myParticle getParticle(int i, int j) {
			return particles [j * particleWidth + i];
		}

		public void makeConstraint(myParticle p1, myParticle p2) {
			constraints.Add (new Constraint (p1, p2));
		}

		// Add wind force to the entire cloth
		void addWindForce(Vector3 direction) {
			for (int i = 0; i < particles.Length; i++) {
				particles [i].addWindForce (windDirection);
			}
		}

		void Start () {
			k = new Vector3 (0.01f, 0.01f, 0.01f);

			// Set Obstacles
			obstacles = GameObject.FindGameObjectsWithTag("Obstacle");


			// Set Pin Local Locations
			topLeft = new Vector3(0.0f, 0.0f, 0.0f);
			topRight = new Vector3 (width, 0.0f, 0.0f);


			// Create
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
			// Stretch Spring, Shear Springs
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
				makeConstraint (getParticle (i, particleHeight - 1), getParticle (i + 1, particleHeight - 1));
			}

			// Right most
			for (int j = 0; j < particleHeight - 1; j++) {
				makeConstraint (getParticle(particleWidth - 1, j), getParticle(particleWidth - 1, j + 1));
			}

			// Bend Constraints
//			for(int i = 0; i <particleWidth - 2; i++)
//			{
//				for(int j=0; j < particleHeight - 2; j++)
//				{
//					makeConstraint(getParticle(i, j), getParticle(i + 2,j));
//					makeConstraint(getParticle(i, j), getParticle(i, j + 2));
//					makeConstraint(getParticle(i, j), getParticle(i + 2,j + 2));
//					makeConstraint(getParticle(i + 2, j), getParticle(i, j + 2));			
//				}
//			}
//
//			// Bottom Row
//			for (int i = 0; i < particleWidth - 2; i++) {
//				makeConstraint (getParticle (i, particleHeight - 2), getParticle (i + 2, particleHeight - 2));
//			}
//
//			// Right most
//			for (int j = 0; j < particleHeight - 2; j++) {
//				makeConstraint (getParticle(particleWidth - 2, j), getParticle(particleWidth - 2, j + 2));
//			}
//
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
				particles [i].updateVel (Time.deltaTime); // Update Velocities
				particles [i].computeDynamics();
			}

			for (int i = 0; i < particles.Length ; i++) {
				particles [i].updateParticle (Time.fixedDeltaTime);
			}

			//Satisfy the Contraints
			for (int i = 0; i < iterations; i++) {
				for (int j = 0; j < constraints.Count; j++) {
					constraints [j].satisfy ();
				}
					
				// HARDCODED PINNING.
				getParticle (0, 0).setPos (transform.position + topLeft);
				getParticle (particleWidth - 1, 0).setPos (transform.position + topRight);


				//SPHERE COLLISION
				for (int k = 0; k < obstacles.Length; k++) {
					float radius = obstacles [k].transform.localScale.x * 0.55f;
					for (int j = 0; j < particles.Length; j++) {
						particles [j].sphereCollision (obstacles [k].transform.position, radius);
					}
				}
			}

			// Sets Vertices of the Mesh to the Particle Positions
			for (int i = 0; i < particles.Length ; i++) {
				vert[i] = particles [i].getPos() - transform.position;
			}
			GetComponent<MeshFilter> ().mesh.vertices = vert;
		}


//		void OnDrawGizmos() {
//			for (int i = 0; i < particles.Length; i++) {
//				Gizmos.DrawSphere (particles [i].getPos () + transform.position, 0.1f);
//			}
//		}
	}
}

