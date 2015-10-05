using UnityEngine;
using System.Collections;

public class polygonObjectScript : MonoBehaviour {
	public bool isValid = false;
	public string type = "polygon";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!isValid) {
			Destroy (this.gameObject);
		}
	}

	public void updateMesh(Vector2[] points, int num){
		GetComponent<MeshFilter> ().mesh.Clear ();
		GetComponent<MeshFilter> ().mesh = CreateMesh (points, num);
	}

	Mesh CreateMesh(Vector2[] points, int num){
		//Create a new mesh
		Mesh mesh = new Mesh();
		
		//Vertices
		Vector3[] vertex = new Vector3[points.Length];
		
		int x;
		for(x = 0; x < points.Length; x++)
		{
			vertex[x] = points[x];
		}
		
		//UVs
		Vector2[] uvs = new Vector2[vertex.Length];
		
		for(x = 0; x < vertex.Length; x++)
		{
			if((x%2) == 0)
			{
				uvs[x] = new Vector2(0,0);
			}
			else
			{
				uvs[x] = new Vector2(1,1);
			}
		}
		
		//Triangles
		var tris = new int[3 * (vertex.Length - 2)];    //3 verts per triangle * num triangles
		int C1;
		int C2;
		int C3;
		
		if(num == 0)
		{
			C1 = 0;
			C2 = 1;
			C3 = 2;
			
			for(x = 0; x < tris.Length; x+=3)
			{
				tris[x] = C1;
				tris[x+1] = C2;
				tris[x+2] = C3;
				
				C2++;
				C3++;
			}
		}
		else
		{
			C1 = 0;
			C2 = vertex.Length - 1;
			C3 = vertex.Length - 2;
			
			for(x = 0; x < tris.Length; x+=3)
			{
				tris[x] = C1;
				tris[x+1] = C2;
				tris[x+2] = C3;
				
				C2--;
				C3--;
			}  
		}
		
		//Assign data to mesh
		mesh.vertices = vertex;
		mesh.uv = uvs;
		mesh.triangles = tris;
		
		//Recalculations
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();  
		mesh.Optimize();
		
		//Name the mesh
		mesh.name = "MyMesh";

		//Return the mesh
		return mesh;
		
	}
}
