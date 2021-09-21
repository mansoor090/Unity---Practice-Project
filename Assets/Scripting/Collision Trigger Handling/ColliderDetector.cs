using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{

	public bool hasInitialized = false;

	//events
	public Dictionary<string, List<Action<Collider>>>  triggerHandlers   = new Dictionary<string, List<Action<Collider>>>();
	public Dictionary<string, List<Action<Collision>>> collisionHandlers = new Dictionary<string, List<Action<Collision>>>();


	public void init()
	{
		hasInitialized    = true;
		triggerHandlers   = new Dictionary<string, List<Action<Collider>>>();
		collisionHandlers = new Dictionary<string, List<Action<Collision>>>();
	}


	public void RegisterForCollision(string objectTag, Action<Collision> OnCollision)
	{
		if (!hasInitialized)
		{
			init();
		}

		// find tag
		if (collisionHandlers.ContainsKey(objectTag))
		{
			collisionHandlers[objectTag].Add(OnCollision);
		}
		else
		{
			// if not exist
			var list = new List<Action<Collision>>();
			list.Add(OnCollision);
			collisionHandlers.Add(objectTag, list);
		}
	}

	public void RegisterForTrigger(string objectTag, Action<Collider> OnTrigger)
	{
		if (!hasInitialized)
		{
			init();
		}

		// find tag
		if (triggerHandlers.ContainsKey(objectTag))
		{
			triggerHandlers[objectTag].Add(OnTrigger);
		}
		else
		{
			// if not exist
			var list = new List<Action<Collider>>();
			list.Add(OnTrigger);
			triggerHandlers.Add(objectTag, list);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		string tagReceived = other.collider.tag;
		
		if (!collisionHandlers.ContainsKey(tagReceived))
		{
			return;
		}

		foreach (var actions in collisionHandlers[tagReceived])
		{
			actions?.Invoke(other);
		}
	}

}