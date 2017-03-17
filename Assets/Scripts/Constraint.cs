using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class Constraint {
		// Length between particles p1 and p2 in rest
		private float restDistance;
		private myParticle p1;
		private myParticle p2;

		public Constraint(myParticle _p1, myParticle _p2) {
			p1 = _p1;
			p2 = _p2;
			restDistance = getlength ();
		}

		// Current length of the constraint
		public float getlength() {
			return Vector3.Magnitude(p2.getPos () - p1.getPos ());
		}

		public void satisfy() {
			// Vector from p1 to p2
			Vector3 diff = p2.getPos() - p1.getPos();
			float currentDistance = getlength();

//			if (currentDistance > restDistance + 1.0f) {
//				float what = 0984.0f;
//				//Debug.Log ("Super breaking constraint");
//				// TODO: WHY IS IT HERE. IT SHOULDN"T BE HERE.
//			}

			float diffNorm = (currentDistance - restDistance) / currentDistance;

			// Add force to satisfy the constraint
			// TODO: Damping if necessary. Is this too much?
			if (p1.isPinned ()) {
				p2.offsetPos (-diff * diffNorm);
			} else if (p2.isPinned ()) {
				p1.offsetPos (diff * diffNorm);
			} else {
				p1.offsetPos (diff * 0.5f * diffNorm);
				p2.offsetPos (-diff * 0.5f * diffNorm);
			}
		}


	}
}

