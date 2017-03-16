using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class myParticleSystem
	{
		
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
			constraints.Add (Constraint (p1, p2));
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

		void drawTriangle (Particle*p1, Particle*p2, Particle*p3) {
			// TODO: Drawing triangle
		}

		public myParticleSystem () {
			// Create all the particles
			for (int i = 0; i < particleWidth; i++) {
				for (int j = 0; j < particleHeight; j++) {
					int idx = j * particleHeight + i;
					Vector3 position = Vector3 (width * (i / (float)particleWidth),
						                   -height * (j / (float)particleHeight),
						                   0.0);
					particles[idx] = myParticle(
				}
			}
		}
	}
}

