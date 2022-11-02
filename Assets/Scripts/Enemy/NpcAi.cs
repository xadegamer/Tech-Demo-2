using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NpcAi : MonoBehaviour
{
    public GameObject destinationPoint;
    NavMeshAgent theAgent;
    public GameObject Npc;
    void Start()
    {
        theAgent = Npc.gameObject.GetComponent<NavMeshAgent>();
     //   Npc.gameObject.GetComponent<Animator>().Play("Running");
    }

    // Update is called once per frame
    void Update()
    {
        theAgent.SetDestination(destinationPoint.transform.position);
    }
}
