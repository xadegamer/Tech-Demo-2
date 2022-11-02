using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutantBrain : MonoBehaviour
{
    NavMeshAgent theAgent;
    public GameObject Npc;
    public GameObject AttackZone;
    void Start()
    {
        theAgent = Npc.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
           // theAgent.speed = 0;
            //   Npc.gameObject.GetComponent<Animator>().Play("Punching");

            Npc.gameObject.GetComponent<Animator>().SetBool("Run", false);
            Npc.gameObject.GetComponent<Animator>().SetBool("Punch", true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
      //  theAgent.speed = 1;
      //  Npc.gameObject.GetComponent<Animator>().Play("Running");

        Npc.gameObject.GetComponent<Animator>().SetBool("Punch", false);
        Npc.gameObject.GetComponent<Animator>().SetBool("Run", true);
    }

    IEnumerator Attack()
    {
        AttackZone.SetActive (true);
        yield return new WaitForSeconds(.1f);
        AttackZone.SetActive(false);
    }

    public void Move()
    {
        theAgent.speed = 1;
    }

    public void Stop()
    {
        theAgent.speed = 0;
    }
}
