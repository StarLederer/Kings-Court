﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICore
{
	public class AICombatBrain : MonoBehaviour
	{
		public float attackSpeed = 1f;
		[SerializeField] private LayerMask playerLayerMask;

		private AIBrainInterface brainInterface;

		//--------------------------
		// MonoBehaviour methods
		//--------------------------
		private void Awake()
		{
			brainInterface =  GetComponent<AIBrainInterface>();
		}

		private void Update()
		{
			// Debugging for damage
			if (Input.GetKeyDown(KeyCode.T))
			{
				brainInterface.TakeDamage();
			}

			switch (brainInterface.type)
			{
				case AIType.target:
					foreach (AIBrainInterface entityInterface in GetEnemiesInRadius(1.5f))
					{
						entityInterface.Stun(7f); //target can stun for testing purposes
					}
					break;
				case AIType.assassin:
					foreach (AIBrainInterface entityInterface in GetEnemiesInRadius(1.5f))
					{
						entityInterface.TakeDamage();
					}
					break;
				case AIType.stunner:
					foreach (AIBrainInterface entityInterface in GetEnemiesInRadius(1.5f))
					{
						entityInterface.Stun(5f);
						brainInterface.Stun(7f);
					}
					break;
			}
		}

		//--------------------------
		// AICombat methods
		//--------------------------
		public List<AIBrainInterface> GetEnemiesInRadius(float radius)
		{
			List<AIBrainInterface> visibleEntities = new List<AIBrainInterface>();

			// geting all AI entities in radus
			Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, radius, playerLayerMask);

			// converting colliders to AIEntities
			foreach (Collider collider in collidersInRadius)
			{
				// self check
				//if (collider.gameObject == gameObject) continue;
				// no need since there is a teammate check and you are your own teammate

				// does not contain AIControlBrain check
				if (collider.gameObject.GetComponent<AIBrainInterface>() == null) continue;

				// teammate check
				if (collider.gameObject.GetComponent<AIBrainInterface>().team == brainInterface.team) continue;

				// adding to visibleEntites list
				visibleEntities.Add(collider.gameObject.GetComponent<AIBrainInterface>());
			}

			return visibleEntities;
		}
	}
}