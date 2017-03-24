using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class Constraint {
		// Length between particles p1 and p2 in rest
		private float restLength;
		private myParticle p1;
		private myParticle p2;

		public Constraint(myParticle _p1, myParticle _p2) {
			p1 = _p1;
			p2 = _p2;
			restLength = Vector3.Magnitude(p2.getPos () - p1.getPos ());
		}

		// Current length of the constraint
		public float getlength() {
			return Vector3.Magnitude(p2.getPos () - p1.getPos ());
		}

		public void satisfy() {
			// Vector from p1 to p2
			Vector3 delta = p2.getPos() - p1.getPos();
			float deltaLength = Vector3.Magnitude(delta);
			float diff = (deltaLength - restLength) / deltaLength;

//			if (deltaLength > restLength + 5.0f) {
//				// float what = 0984.0f;
//				//Debug.Log ("Super breaking constraint");
//				// TODO: WHY IS IT HERE. IT SHOULDN"T BE HERE.
//				// Debug.Log(-delta * 0.5f * diff);
//			}


			p1.offsetPos (delta * 0.7f * diff);
			p2.offsetPos (-delta * 0.7f * diff);

			// Faster, Approximate
			//delta*=restLength*restLength/(delta*delta+restLength*restLength)-0.5;
			//				
			//			p1.offsetPos (delta);
			//			p2.offsetPos (-delta);
		}


	}
}

