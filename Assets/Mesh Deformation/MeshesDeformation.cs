using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Controllers;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MeshesDeformation : MonoBehaviour
{

	[SerializeField] private MeshFilter[]   meshFilters               = default;
	[SerializeField] private MeshCollider[] colliders                 = default;
	[SerializeField] private float          impactDamage              = 1f;
	[SerializeField] private float          deformationRadius         = 0.5f;
	[SerializeField] private float          maxDeformation            = 0.5f;
	[SerializeField] private float          minVelocity               = 2f;
	[SerializeField, Range(0.01f,0.2f)] private float          delayTimeDeform           = 0.02f;
	private                  float          minVertsDistanceToRestore = 0.002f;
	private                  float          vertsRestoreSpeed         = 2f;
	private                  Vector3[][]    originalVertices;
	private                  float          nextTimeDeform = 0f;
	private                  bool           isRepairing    = false;
	private                  bool           isRepaired     = false;
	// MeshRenderer                            MeshRenderer;
	// Bounds                                  bound;

	[SerializeField]Vector3 maxLimit = Vector3.zero;
	[SerializeField]Vector3 minLimit = Vector3.zero;

	[SerializeField] bool PerformOnMultiMeshes = false;

	[Button]
	public void GetComponents()
	{
		meshFilters    = new MeshFilter[1];
		meshFilters[0] = GetComponent<MeshFilter>();
		MeshCollider[] data = GetComponents<MeshCollider>();
		if (data.Length == 1)
		{
			colliders    = new MeshCollider[1];
			colliders[0] = data[0];
		}
	}

	[Button]
	public void CalculateLocalVertz()
	{
		var       mesh     = meshFilters[0].sharedMesh;
		Vector3[] vertices = mesh.vertices;



		Vector3 localPos = vertices[0];

		maxLimit = Vector3.zero;
		minLimit = Vector3.zero;
		// maxLimit.x = localPos.x;
		// minLimit.x = localPos.x;
		
		foreach (var VARIABLE in vertices)
		{
			localPos  = VARIABLE;

			localPos.x *= transform.lossyScale.x;
			localPos.y *= transform.lossyScale.y;
			localPos.z *= transform.lossyScale.z;
			
			
			if (localPos.x > maxLimit.x)
			{
				maxLimit.x = localPos.x;
			}
			if (localPos.y > maxLimit.y)
			{
				maxLimit.y = localPos.y;
			}
			
			if (localPos.z > maxLimit.z)
			{
				maxLimit.z = localPos.z;
			}
			
		
			if (localPos.x < minLimit.x)
			{
				minLimit.x = localPos.x;
			}
			if (localPos.y < minLimit.y)
			{
				minLimit.y = localPos.y;
			}
			if (localPos.z < minLimit.z)
			{
				minLimit.z = localPos.z;
			}
		}



	}

	private void Start()
	{
		originalVertices = new Vector3[meshFilters.Length][];
		for (int i = 0; i < meshFilters.Length; i++)
		{
			originalVertices[i] = meshFilters[i].mesh.vertices;
			meshFilters[i].mesh.MarkDynamic();
		}

		// MeshRenderer = GetComponent<MeshRenderer>();
		// bound        = MeshRenderer.bounds;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (!isRepairing)
			{
				isRepairing = true;
			}
		}

		RestoreMesh();
	}

	private void DeformationMesh(Mesh mesh, Transform localTransform, Vector3 contactPoint, Vector3 contactVelocity, int i, bool updateCollider)
	{
		bool    hasDeformated     = false;
		Vector3 localContactPoint = localTransform.InverseTransformPoint(contactPoint);
		Vector3 localContactForce = localTransform.InverseTransformDirection(contactVelocity);
		// if (localContactForce.y > 0f)
		// {
		// 	localContactForce.y = -localContactForce.y;
		// }

		Vector3[] vertices = mesh.vertices;

		// bound.center = transform.position;
		// bound = MeshRenderer.bounds;
		for (int j = 0; j < vertices.Length; j++)
		{
			float distance = (localContactPoint - vertices[j]).magnitude;
			if (distance <= deformationRadius)
			{
				vertices[j] += localContactForce * (deformationRadius - distance) * impactDamage;
				Vector3 deformation = vertices[j]                     - originalVertices[i][j];
				if (deformation.magnitude > maxDeformation)
				{
					vertices[j]   = originalVertices[i][j] + deformation.normalized * maxDeformation;
			
				}
				vertices[j].x = Mathf.Clamp(vertices[j].x, minLimit.x, maxLimit.x);
				vertices[j].y = Mathf.Clamp(vertices[j].y, minLimit.y, maxLimit.y);
				vertices[j].z = Mathf.Clamp(vertices[j].z, minLimit.z, maxLimit.z);
				hasDeformated = true;
			}
		}

		if (hasDeformated && updateCollider)
		{
			mesh.vertices = vertices;
			// mesh.RecalculateNormals();
			// mesh.RecalculateBounds();
			if (colliders.Length > 0)
			{
				if (colliders[i] != null)
				{
					colliders[i].sharedMesh = mesh;
				}
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Obstacle"))
		{
			// Debug.Log(collision.gameObject.name);
			// Debug.Log(collision.relativeVelocity.magnitude);
			if (Time.time > nextTimeDeform)
			{
				// Debug.Log((collision.gameObject.GetComponent<Rigidbody>().velocity - transform.position).magnitude);
				if (collision.relativeVelocity.magnitude > minVelocity)
				{
					
						VFXManager.Instance.HitParticle.transform.position = collision.contacts[0].point;
						VFXManager.Instance.HitParticle.Emit(1);
						
				
					
						VFXManager.Instance.sparkParticle.transform.position = collision.contacts[0].point;
						VFXManager.Instance.sparkParticle.Emit(1);
					
					isRepaired                                         = false;
					if (PerformOnMultiMeshes)
					{
						for (int j = 0; j < collision.contacts.Length; j++)
						{
							Vector3 contactPoint = collision.contacts[j].point;
							// Vector3 contactVelocity = collision.relativeVelocity * 0.02f;
							Vector3 contactVelocity = collision.relativeVelocity * 0.02f;
							for (int i = 0; i < meshFilters.Length; i++)
							{
								if (meshFilters[i] != null)
								{
									if (j == (collision.contacts.Length /2) - 1)
									{
										DeformationMesh(meshFilters[i].mesh, meshFilters[i].transform, contactPoint, contactVelocity, i, true);
									}
									else
									{
										DeformationMesh(meshFilters[i].mesh, meshFilters[i].transform, contactPoint, contactVelocity, i, false);
									}
								}
							}
						}
					}
					else
					{
						Vector3 contactPoint    = collision.contacts[0].point;
						Vector3 contactVelocity = collision.relativeVelocity * 0.02f;
						if (contactVelocity.magnitude < 0.05)
						{
							contactVelocity = contactVelocity * 5;
						}
						for (int i = 0; i < meshFilters.Length; i++)
						{
							if (meshFilters[i] != null)
							{
								DeformationMesh(meshFilters[i].mesh, meshFilters[i].transform, contactPoint, contactVelocity, i, true);
							}
						}

						nextTimeDeform = Time.time + delayTimeDeform;
					}
				}
			}
		}

		// if (collision.gameObject.CompareTag("ObstacleSin"))
		// {
		// 	// Debug.Log((collision.gameObject.GetComponent<Rigidbody>().velocity - transform.position).magnitude);
		// 	// if (Time.time > nextTimeDeform)
		// 	{
		// 		isRepaired = false;
		// 		if (PerformOnMultiMeshes)
		// 		{
		// 			for (int j = 0; j < collision.contacts.Length / 2; j++)
		// 			{
		// 				Vector3 contactPoint = collision.contacts[j].point;
		// 				// Vector3 contactVelocity = collision.relativeVelocity * 0.02f;
		// 				Vector3 contactVelocity = Vector3.ClampMagnitude(collision.relativeVelocity * 1, 0.1f);
		// 				for (int i = 0; i < meshFilters.Length; i++)
		// 				{
		// 					if (meshFilters[i] != null)
		// 					{
		// 						if (j == (collision.contacts.Length / 2) - 1)
		// 						{
		// 							DeformationMesh(meshFilters[i].mesh, meshFilters[i].transform, contactPoint, contactVelocity, i, true);
		// 						}
		// 						else
		// 						{
		// 							DeformationMesh(meshFilters[i].mesh, meshFilters[i].transform, contactPoint, contactVelocity, i, false);
		// 						}
		// 					}
		// 				}
		// 			}
		// 		}
		// 		else
		// 		{
		// 			Vector3 contactPoint    = collision.contacts[0].point;
		// 			Vector3 contactVelocity = Vector3.ClampMagnitude(collision.relativeVelocity * 1, 0.1f);
		// 			for (int i = 0; i < meshFilters.Length; i++)
		// 			{
		// 				if (meshFilters[i] != null)
		// 				{
		// 					DeformationMesh(meshFilters[i].mesh, meshFilters[i].transform, contactPoint, contactVelocity, i, true);
		// 				}
		// 			}
		//
		// 			// nextTimeDeform = Time.time + delayTimeDeform;
		// 		}
		// 	}
		// }
		

	}

	private void RestoreMesh()
	{
		if (!isRepaired && isRepairing)
		{
			isRepaired = true;
			for (int i = 0; i < meshFilters.Length; i++)
			{
				Mesh      mesh      = meshFilters[i].mesh;
				Vector3[] vertices  = mesh.vertices;
				Vector3[] origVerts = originalVertices[i];
				for (int j = 0; j < vertices.Length; j++)
				{
					vertices[j] += (origVerts[j] - vertices[j]) * Time.deltaTime * vertsRestoreSpeed;
					if ((origVerts[j] - vertices[j]).magnitude > minVertsDistanceToRestore)
					{
						isRepaired = false;
					}
				}

				mesh.vertices = vertices;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				if (colliders[i] != null)
				{
					colliders[i].sharedMesh = mesh;
				}
			}

			if (isRepaired)
			{
				isRepairing = false;
				for (int i = 0; i < meshFilters.Length; i++)
				{
					if (colliders[i] != null)
					{
						colliders[i].sharedMesh = meshFilters[i].mesh;
					}
				}
			}
		}
	}

}
