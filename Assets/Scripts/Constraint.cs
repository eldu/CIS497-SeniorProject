using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class Constraint {
		// Length between particles p1 and p2 in rest
		public float restLength;
		private myParticle p1;
		private myParticle p2;

		public Constraint(myParticle _p1, myParticle _p2) {
			p1 = _p1;
			p2 = _p2;
			restLength = Vector3.Magnitude(p2.getPos () - p1.getPos ());

			p1.addConstraint (this);
			p2.addConstraint (this);
		}

		public float getRestLength() {
			return restLength;
		}

		// Current length of the constraint
		public float getlength() {
			return Vector3.Magnitude(p2.getPos () - p1.getPos ());
		}

		public void satisfy() {
			Vector3 delta = p2.getPos() - p1.getPos();
			float deltaLength = Vector3.Magnitude(delta);
			float diff = (deltaLength - restLength) / deltaLength;

			p1.offsetPos (delta * 0.5f * diff);
			p2.offsetPos (-delta * 0.5f * diff);
		}
	}
}

