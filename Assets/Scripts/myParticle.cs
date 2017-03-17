﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class myParticle
	{
		// Determines if this is fixed
		private bool pinned;

		private int m_dim;

		// State Vector
		private float[] m_state;

		// Derivative of the state vector
		private float[] m_stateDot; 

		// One simulation step
		private float m_deltaT; 

		private float m_mass = 1.0f;
		private Vector3 m_pos;
		private Vector3 m_vel;
		private Vector3 m_gravity;

		public myParticle(Vector3 position) {
			m_dim = 12;
			m_gravity = new Vector3 (0.0f, -9.8f, 0.0f);

			m_state = new float[12];
			m_state [0] = position.x;
			m_state [1] = position.y;
			m_state [2] = position.z;
			m_state [3] = 0.0f;
			m_state [4] = 0.0f;
			m_state [5] = 0.0f;
			m_state [6] = 0.0f;
			m_state [7] = 0.0f;
			m_state [8] = 0.0f;
			m_state [9] = 0.0f;
			m_state [10] = 0.0f;
			m_state [11] = 0.0f;

			m_pos = new Vector3 (position.x, position.y, position.z);
			m_vel = new Vector3 (0.0f, 0.0f, 0.0f);

			m_stateDot = new float[12];

			setMass (m_mass);
		}

		public myParticle (myParticleSystem parent)
		{
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

		public Vector3 getPos() {
			return m_pos;
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
			// Add your code here
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

			// Add default force gravity
			addForce (m_mass * m_gravity);
		}

		//given the state compute stateDot based on the dynamics of the particle
		// state: a vector containing the state of the particle in terms of its position, velocity, forces,
		//        mass and time to live.
		// stateDot: a vector containing the derivatives of the stateVector
		// deltaT: change in time
		// Given the state, computes stateDot
		public void computeDynamics(float[] state, float[] stateDot, float deltaT) {
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
			*  10 : timeToLive
			*  11 : not defined
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
		public void updateParticle () {
			if (!pinned) {
				float deltaT = Time.deltaTime;
				computeForces ();
				updateState (deltaT);
			}
		}
	
	}

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
			Vector3 p12 = p2.getPos() - p1.getPos();
			float currentDistance = getlength();

			Vector3 correction = p12 * (1.0f - restDistance / currentDistance) * 0.5f;

			// Add force to satisfy the constraint
			// TODO: Damping if necessary. Is this too much?
			p1.addForce (correction);
			p2.addForce (correction);

		}


	}
}

