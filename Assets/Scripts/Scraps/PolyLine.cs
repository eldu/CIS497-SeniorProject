//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class PolyLine : MonoBehaviour {
//	public float length;
//	public float radius;
//	public float friction;
//
//	public List<Point> pts;
//
//
//
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//		
//	}
//
//	public class PolyLineData {
//		public PolyLineData(float _length, float _radius, float _friction) {
//			length = _length;
//			radius = _radius;
//			friction = _friction;
//
//			pts = new List<Point>();
//			for (int i = 0; i < _length; i++) {
//				pts.Add(new Point());
//			}
//		}
//
//		public void Update(float _radius, float _length, float _friction, Vector3 _wind, float _gravity) {
//
//			for (int i = 1; i < pts.Count; i++) {
//				Point prev = pts[i - 1];
//				Point curr = pts [i];
//				curr.pos += curr.vel;
//
//				Vector3 d = prev.pos - curr.pos;
//				float da = Mathf.Atan2 (d.y, d.x);
//
//				double px = curr.x + Mathf.Cos (da) * _length;
//				double py = curr.y + Mathf.Sin (da) * _length;
//
//
//
//			}
//		
//		}
//	}
//
//	public class Point {
//		public Vector3 pos;
//		public Vector3 opos;
//		public Vector3 vel;
//
//		public Point(Vector3 _pos) {
//			pos = _pos;
//		}
//
//		public Point() {}
//	}
//}
