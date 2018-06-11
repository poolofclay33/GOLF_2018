using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// A simple script which helps set up the models.
/// </summary>
public class SetupParts : EditorWindow {

//	private int m_scene = 1;
//	private int m_nomToBake = 18;
//	private int m_offset=0;
	
   [MenuItem ("Window/Parts Setup")]
    static void Init () {
	EditorWindow.GetWindow (typeof (SetupParts));
	}
   void OnGUI () {
		 GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
       if(GUILayout.Button("Setup" ) )
		{
			setupParts ();
		}
  
	}
	void setupParts()
	{
		GameObject go = GameObject.Find("Tiles");
		Transform t = go.transform;
		Debug.Log ("setupParts " + t.childCount);
		for(int i=0; i<t.childCount; i++)
		{
			setUpPart ( t.GetChild(i) );
		}
	}
	
	void setUpPart(Transform t)
	{
		if(t)
		{
			int n = t.childCount;
			for(int i=0; i<n;i++)
			{
				Debug.Log ("handle transform dad " + t.name);
				Transform t0 = t.GetChild(i);
				if(t0)
				{
					handleTransform(t0);
				}
			}
		}
	}
	void handleTransform(Transform t)
	{
		Debug.Log ("handle transform t" + t.name);
		string name = t.name;
		t.transform.position = Vector3.zero;
		//t.rotation = Quaternion.AngleAxis(0,Vector3.zero);
		t.localScale = new Vector3(1,1,1);
		
		if((name.Contains("_walls") || name.Contains("_obstacle")) )
		{
			//Transform t0 = t.GetChild(0);
			//if(t0)
			{
				handleBorder(t);
			}
		}
		if(name.Contains("_hole"))
		{
			//Transform t0 = t.GetChild(0);
			//if(t0)
			{			
				handleHole(t);
			}
		}

		if(name.Contains("_floor") )
		{
			//Transform t0 = t.GetChild(0);
			//if(t0)
			{
				handleGround(t);
			}
		}

	}
	void handleBorder(Transform t)
	{
		MeshRenderer mr = t.GetComponent<MeshRenderer>();
		if(mr)
		{
		//	mr.material = (Material)Resources.Load("defaultMatX");
		}
		
		MeshCollider meshCollider = t.GetComponent<MeshCollider>();
		if(meshCollider==null)
		{
			meshCollider = t.gameObject.AddComponent<MeshCollider>();
		}
		
		if(meshCollider)
		{
			meshCollider.gameObject.layer = 8;
			meshCollider.material = (PhysicMaterial) Resources.Load("PhysicMaterial/WoodWalls");
		}	
	}
	void handleGround(Transform t)
	{
		MeshRenderer mr = t.GetComponent<MeshRenderer>();
		if(mr)
		{
			//mr.material = (Material)Resources.Load("defaultMatX");
		}

		MeshCollider meshCollider = t.GetComponent<MeshCollider>();
		if(meshCollider==null)
		{
			meshCollider = t.gameObject.AddComponent<MeshCollider>();
		}
		
		if(meshCollider)
		{
			meshCollider.material = (PhysicMaterial) Resources.Load("PhysicMaterial/Grass");
		}	
	}
	void handleHole(Transform t)
	{
		MeshRenderer mr = t.GetComponent<MeshRenderer>();
		if(mr)
		{
			//mr.material = (Material)Resources.Load("defaultMatX");
		}

		MeshCollider meshCollider = t.GetComponent<MeshCollider>();
		if(meshCollider==null)
		{
			meshCollider = t.gameObject.AddComponent<MeshCollider>();
		}
		
		if(meshCollider)
		{
			meshCollider.gameObject.layer = 10;
			meshCollider.material = (PhysicMaterial) Resources.Load("PhysicMaterial/Goal");
		}	
		BoxCollider boxCollider = t.GetComponent<BoxCollider>();
		if(boxCollider==null)
		{
			boxCollider = t.gameObject.AddComponent<BoxCollider>();
		}

		if(boxCollider)
		{
			Vector3 center = boxCollider.center;
			center.y -= 0.01f;
			boxCollider.center = center;
			boxCollider.isTrigger=true;
		}
		VictoryTrigger vt = t.GetComponent<VictoryTrigger>();
		if(vt==null)
		{
			t.gameObject.AddComponent<VictoryTrigger>();
		}
		
	}
}
