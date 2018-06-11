using UnityEngine;
using System.Collections;

/// <summary>
/// Will spawn our object, as well as well show up as a gizmo in the editor.
/// </summary>
public class Creator : MonoBehaviour {
	/// <summary>
	/// The color of the gizmo.
	/// </summary>
	public Color gizmoColor = Color.green;
	
	/// <summary>
	/// The gizmo radius.
	/// </summary>
//	private float gizmoRadius = 1.5f;
	
	/// <summary>
	/// The object to spawn.
	/// </summary>
	public GameObject objectToSpawn;

	/// <summary>
	/// The object to spawn.
	/// </summary>
	public GameObject[] objectsToSpawn;
	public enum SpawnType
	{
		POINT,
		SPHERE
	};
	public SpawnType spawnType;
	
	public bool zeroXPos = false;
	public bool zeroYPos = false;
	public bool zeroZPos =false;
	
	public bool useCreatorScale = false;
	public void Start () {
		GameObject objToSpawn = objectToSpawn;
		if(objectsToSpawn!=null && objectsToSpawn.Length>0)
		{
			objToSpawn = objectsToSpawn[ Random.Range(0,objectsToSpawn.Length) ];
		}
	
		if(objToSpawn)
		{
			Vector3 spawnPos = transform.position;
			if(spawnType==SpawnType.SPHERE)
			{
				spawnPos = transform.position + Random.onUnitSphere * transform.localScale.x;
			}
			if(zeroXPos)
			{
				spawnPos.x=0;
			}
			if(zeroYPos)
			{
				spawnPos.y=0;
			}
			if(zeroZPos)
			{
				spawnPos.z=0;
			}			
			

			
			if(objToSpawn)
			{
				Quaternion rot = transform.rotation;
				if(objToSpawn)
				{
					rot = objToSpawn.transform.rotation;
				}
				GameObject go = (GameObject)Instantiate(objToSpawn,spawnPos,rot);
				if(go &&transform.parent)
				{
					go.transform.parent = transform.parent;
					if(useCreatorScale)
					{
						go.transform.localScale = transform.localScale;
					}
				}
			}
		}
		Destroy (gameObject);
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = gizmoColor;
		Gizmos.DrawSphere(transform.position,transform.localScale.x);
	}
}
