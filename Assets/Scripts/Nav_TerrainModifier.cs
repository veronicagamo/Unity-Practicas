using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class Nav_TerrainModifier : MonoBehaviour
{
    private NavMeshModifier _meshSurface;
    void Start()
    {
        _meshSurface = GetComponent<NavMeshModifier>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if (_meshSurface.AffectsAgentType(agent.agentTypeID))
            {
                agent.speed /= NavMesh.GetAreaCost(_meshSurface.area);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if (_meshSurface.AffectsAgentType(agent.agentTypeID))
            {
                agent.speed *= NavMesh.GetAreaCost(_meshSurface.area);
            }
        }
    }
}
