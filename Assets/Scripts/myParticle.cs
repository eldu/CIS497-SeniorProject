using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class myParticle
	{
		// Determines if this is fixed
		// private bool pinned;
		public List<Constraint> cs; // list of constraints
		public Vector3 k; // Spring Constant

		private int m_dim;
		private bool pinned;

		// State Vector
		private float[] m_state;

		// Derivative of the state vector
		private float[] m_stateDot; 

		// One simulation step
		private float m_deltaT; 

		private float m_mass = 1.0f;
		private Vector3 m_pos;
		private Vector3 m_opos; // Old Position;
		private Vector3 m_vel;
		private Vector3 m_gravity;
		private Vector3 m_wind;

		private myParticleSystem m_ps;

		public myParticle(myParticleSystem ps, Vector3 position) {
			cs = new List<Constraint> ();
			k = new Vector3(0.1f, 0.01f, 0.01f);

			m_ps = ps;

			pinned = false;
	
			m_gravity = new Vector3 (0.0f, 0.0f, 0.0f);
			m_wind = new Vector3 (0.0f, 0.0f, 0.0f);

			m_state = new float[10];
			m_state [0] = position.x;            // Current Position
			m_state [1] = position.y;
			m_state [2] = position.z;
			m_state [3] = 0.0f;                  // Current Velocity
			m_state [4] = 0.0f;
			m_state [5] = 0.0f;
			m_state [6] = m_mass * m_gravity[0]; // Force
			m_state [7] = m_mass * m_gravity[1];
			m_state [8] = m_mass * m_gravity[2];
			m_state [9] = m_mass;                // Mass

			m_pos = new Vector3 (position.x, position.y, position.z);
			m_opos = new Vector3 (position.x, position.y, position.z);
			m_vel = new Vector3 (0.0f, 0.0f, 0.0f);

			m_stateDot = new float[10];

			setMass (m_mass);
		}

		public void addConstraint(Constraint c) {
			cs.Add (c);
		}
			
		public void setPinned(bool b) {
			pinned = b;
		}

		// Set the particle state vector
		public void setState(float[] newState) {
			for (int i = 0; i < m_dim; i++)
				m_state[i] = newState[i];

			m_pos[0] = m_state[0];
			m_pos[1] = m_state[1]; 
			m_pos[2] = m_state[2];

			m_vel[0] = m_state[3];
			m_vel[1] = m_state[4];
			m_vel[2] = m_state[5];
		}

		// Returns Position in local space
		public Vector3 getPos() {
			return m_pos;
		}

		public void setPos(Vector3 newPos) {
			m_pos = newPos;

			m_state [0] = m_pos.x;
			m_state [1] = m_pos.y;
			m_state [2] = m_pos.z;
		}

		public void offsetPos(Vector3 offset) {
			//if (!pinned) {
				m_pos += offset;

				m_state [0] = m_pos.x;
				m_state [1] = m_pos.y;
				m_state [2] = m_pos.z;
			//}
		}

		// Approximate velocity based on the old position, current position, and a small deltatime
		// TODO: Check on this
		public void updateVel(float deltaTime) {
			m_vel = (m_pos - m_opos) / deltaTime;

			m_state [3] = m_vel [0];
			m_state [4] = m_vel [1];
			m_state [5] = m_vel [2];

			m_stateDot [0] = m_vel [0];
			m_stateDot [1] = m_vel [1];
			m_stateDot [2] = m_vel [2];
		}

		// Get the state vector
		public float[] getState () {
			return m_state;
		}

		// Get the stateDot  vector
		public float[] getStateDot () {
			return m_stateDot;
		}

		// Sets the mass
		public void setMass(float _mass) {
			m_state[9] = _mass;
			m_mass = _mass;
		}

		// Gets the mass
		public float getMass() {
			return m_state[9];
		}

		// Updates the particle state
		// Uses RK2
		public void updateState(float deltaT) {
			// COmpute Dynamics
			// computeDynamics (m_state, m_stateDot, deltaT);

			// RK2 Integration
			// Predicted state at t_k+1
			float[] p_state = new float[12];

			// x_p(t_k+1) = x(t_k) + v(t_k) * deltaT;
			p_state[0] = m_state[0] + m_stateDot[0] * deltaT;
			p_state[1] = m_state[1] + m_stateDot[1] * deltaT;
			p_state[2] = m_state[2] + m_stateDot[2] * deltaT;

			// v_p(t_k+1) = v(t_k) + a(t_k) * deltaT;
			p_state[3] = m_state[3] + m_stateDot[3] * deltaT;
			p_state[4] = m_state[4] + m_stateDot[4] * deltaT;
			p_state[5] = m_state[5] + m_stateDot[5] * deltaT;

			// x(t_k+1) = x(t_k) + deltaT / 2 * (v(t_k) + v_p(t_k+1))
			m_state[0] = m_state[0] + deltaT / 2.0f * (m_stateDot[0] + p_state[3]);
			m_state[1] = m_state[1] + deltaT / 2.0f * (m_stateDot[1] + p_state[4]);
			m_state[2] = m_state[2] + deltaT / 2.0f * (m_stateDot[2] + p_state[5]);

			// v(t_k+1) = v(t_k) + deltaT / 2 * (a(t_k) + a_p(t_k+1));
			// a_p(t_k+1) = a + deltaT * f(a) = a + deltaT * 0 = a
			// a(t_k) = a_p(t_k+1)
			// float m = m_state[9];
			m_state[3] = m_state[3] + deltaT * m_stateDot[3];
			m_state[4] = m_state[4] + deltaT * m_stateDot[4];
			m_state[5] = m_state[5] + deltaT * m_stateDot[5];

			// Update Position and Velocity
			m_opos = m_pos; // Set old position;

			m_pos[0] = m_state[0];
			m_pos[1] = m_state[1];
			m_pos[2] = m_state[2];
			  
			m_vel[0] = m_state[3];
			m_vel[1] = m_state[4];
			m_vel[2] = m_state[5];
		}

		// Add force
		public void addForce(Vector3 force) {
			m_state [6] += force [0];
			m_state [7] += force [1];
			m_state [8] += force [2];
		}

		// computer forces on this particle
		public void computeForces() {
			m_state [6] = 0;
			m_state [7] = 0;
			m_state [8] = 0;

			// Add gravity and wind forces
			addForce (m_mass * m_gravity + m_wind);

			// Take Away Spring
			for (int i = 0; i < cs.Count; i++) {
				addForce (-k * (cs [i].getlength () - cs [i].getRestLength ()));
			}

			// Reset wind and constraint forces
			m_wind.Set(0.0f, 0.0f, 0.0f);
		}

		public void addWindForce(Vector3 force) {
			m_wind = force;
		}

		//given the state compute stateDot based on the dynamics of the particle
		// state: a vector containing the state of the particle in terms of its position, velocity, forces,
		//        mass and time to live.
		// stateDot: a vector containing the derivatives of the stateVector
		// deltaT: change in time
		// Given the state, computes stateDot
		public void computeDynamics() {
			float[] state = m_state; 
			float[] stateDot = m_stateDot;
			float deltaT = m_deltaT;
			/*	State vector:
			*  0 : position x
			*  1 : position y
			*  2 : position z
			*  3 : velocity x
			*  4 : velocity y
			*  5 : velocity z
			*  6 : force x
			*  7 : force y
			*  8 : force z
			*  9 : mass
			*/

			// Derivatives of position is the velocity;
			stateDot[0] = state[3];
			stateDot[1] = state[4];
			stateDot[2] = state[5];

			// Derivatives of the velocity
			// v = f/m
			float m = state[9];
			stateDot[3] = state[6] / m;
			stateDot[4] = state[7] / m;
			stateDot[5] = state[8] / m;


			// Force and mass are constant therefore their derivatives are 0
			stateDot[6] = 0; // force x
			stateDot[7] = 0; // force y
			stateDot[8] = 0; // force z
			stateDot[9] = 0; // mass
		}

		// Computes one simulation step update
		public void updateParticle (float deltaTime) {
			//if (!pinned) {
				computeForces ();
				updateState (deltaTime);
			//}
		}

		// Pushes 
		public void sphereCollision(Vector3 center, float radius) {
			Vector3 dir = getPos () - center;
			float length = dir.magnitude;
			if (length < radius) {
				offsetPos(dir.normalized * (radius - length));
			}
		}
	}
}

