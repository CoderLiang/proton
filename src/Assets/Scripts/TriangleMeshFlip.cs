using UnityEngine;
using System.Collections;
using System.Linq;

public class TriangleMeshFlip : MonoBehaviour {
	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.triangles = mesh.triangles.Reverse().ToArray();
	}
}
