using UnityEngine;
using System.Collections;
/// <summary>
/// Hill trigger.
/// </summary>
public class HillTrigger : MonoBehaviour {
	
	/// <summary>
	/// The draw gizmos.
	/// </summary>
	public bool drawGizmos=true;
	
	/// <summary>
	/// The color.
	/// </summary>
	public Color color = new Color(0.5f,1,0.5f,0.5f);
	
	/// <summary>
	/// The hill phys X mat.
	/// </summary>
	public PhysicMaterial hillPhysXMat;

	public void OnDrawGizmos()
	{
		if(drawGizmos)
		{
		    Gizmos.color = color;
		    Gizmos.DrawCube (transform.position, transform.rotation * transform.localScale);
		}
	}
	
	void OnTriggerEnter(Collider col)	
	{
		BallScript bs = col.gameObject.GetComponent<BallScript>();
		if(bs)
		{
			bs.onHill(true,hillPhysXMat);
		}
	}
	void OnTriggerExit(Collider col)
	{
		BallScript bs = col.gameObject.GetComponent<BallScript>();
		if(bs)
		{
			bs.onHill(false,hillPhysXMat);
		}
	}
}
