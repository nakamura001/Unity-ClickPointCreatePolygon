using UnityEngine;
using System.Collections;

public class ClickPointCreatePolygon : MonoBehaviour {
	
	public Material cubeMaterial;
	GameObject[] cubeList;
	int cubeIndex = 0;
	
	void CubePosReset() {
		Vector3 cubePos = new Vector3(0, 0, -100f);
		for (int i=0; i<cubeList.Length; i++) {
			cubeList[i].transform.position = cubePos;
		}
		cubeIndex = 0;
	}
	
	GameObject CreateCube (Vector3 pos)
	{
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.GetComponent<Renderer>().material = cubeMaterial;
		cube.transform.position = pos;
		float s = 0.02f;
		cube.transform.localScale = new Vector3 (s, s, s);
		return cube;
	}
	
	void AddPolygonPoint ()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 1f;
		cubeList [cubeIndex++].transform.position = Camera.main.ScreenToWorldPoint (mousePos);
	}
	
	void CreatePolygon ()
	{
		string objectName = "Polygon";
		Mesh mesh = new Mesh ();
				
		Vector3[] vertices = new Vector3[cubeIndex];
		for (int i=0; i<cubeIndex; i++) {
			vertices [i] = cubeList [i].transform.position;
		}
		mesh.vertices = vertices;

		Vector2[] verticesXY = new Vector2[cubeIndex];
		for (int i=0; i<cubeIndex; i++) {
			Vector3 pos = cubeList [i].transform.position;
			verticesXY [i] = new Vector2 (pos.x, pos.y);			
		}
		Triangulator tr = new Triangulator (verticesXY, Camera.main.transform.position);
		int[] indices = tr.Triangulate ();
		Debug.Log(indices.Length);
		mesh.triangles = indices;
		
		//  UVデータの設定は今回は省略
		mesh.uv = new Vector2[cubeIndex];

		mesh.RecalculateNormals ();	// 法線の再計算
		mesh.RecalculateBounds ();	// バウンディングボリュームの再計算
		;
		
		GameObject newGameobject = new GameObject (objectName);
		
		MeshRenderer meshRenderer = newGameobject.AddComponent<MeshRenderer> ();
		meshRenderer.material = new Material (Shader.Find ("Diffuse"));
		
		
		MeshFilter meshFilter = newGameobject.AddComponent<MeshFilter> ();
		meshFilter.mesh = mesh;
	}

	void Start ()
	{
		cubeList = new GameObject[11];
		Vector3 cubePos = new Vector3 (0, 0, -100f);
		for (int i=0; i<cubeList.Length; i++) {
			cubeList [i] = CreateCube (cubePos);
		}
		CubePosReset ();
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {
			if (cubeIndex <= cubeList.Length - 2) {
				AddPolygonPoint ();
			}
		} else if (Input.GetMouseButtonUp (1)) {
			if (cubeIndex >= 2) {
				AddPolygonPoint();
				CreatePolygon ();
				CubePosReset ();
			}
		}
	}
}
