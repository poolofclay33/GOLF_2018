using UnityEngine;
using System.Collections;
/// <summary>
/// Gizmo draw hole.
/// </summary>
public class GizmoDrawHole : MonoBehaviour 
{
	/// <summary>
	/// The draw gizmo.
	/// </summary>
	public bool drawGizmo=true;
	
	/// <summary>
	/// The color.
	/// </summary>
	public Color color=new Color(0.5f,0.5f,1f,0.5f);
	
	
	/// <summary>
	/// The gizmo size.
	/// </summary>
	public float size=2;
	
	
	public void OnDrawGizmos()
	{
		GameObject go = GameObject.FindWithTag("Hole");
		if(go)
		{
			transform.position = go.transform.position;
		}
		if(drawGizmo)
		{
			if(go)
			{
			    Gizmos.color = color;
			}else{			
				Gizmos.color=Color.red;
			}
		    Gizmos.DrawSphere(transform.position,size);
		}
	}
}
