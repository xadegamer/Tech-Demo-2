using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class EnemyAIActions 
{
    public static void LookAtTarget(this Transform ownerTranfrom, Vector3 targetPosition)
    {
        Vector3 tragetPosition = new Vector3(targetPosition.x, ownerTranfrom.position.y, targetPosition.z);
        ownerTranfrom.LookAt(tragetPosition);
    }

    public static void LookAtTargetSmooth(this Transform ownerTranfrom, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - ownerTranfrom.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        ownerTranfrom.rotation = Quaternion.Slerp(ownerTranfrom.rotation, lookRotation, Time.deltaTime * 5);
    }

    public static void LookAtTargetSmooth(this Transform ownerTranfrom, Transform targetPosition)
    {
        Vector3 direction = (targetPosition.position - ownerTranfrom.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        ownerTranfrom.rotation = Quaternion.Slerp(ownerTranfrom.rotation, lookRotation, Time.deltaTime * 5);
    }

    public static bool CheckDistanceToTarget (this Transform ownerPos, Transform targetPos, float range)
    {
        if (Vector3.Distance(ownerPos.position, targetPos.position) < range) return true;
        return false;
    }

   public static bool RandomPosition(Vector3 center, float range, out Vector3 result)
   {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        NavMeshHit hit;
        bool isValid = NavMesh.SamplePosition(randomPoint, out hit, .5f, NavMesh.AllAreas);

        while (!isValid)
        {
            randomPoint = center + Random.insideUnitSphere * range;
            isValid = NavMesh.SamplePosition(randomPoint, out hit, .5f, NavMesh.AllAreas);
        }

        result = hit.position;
        return true;
    }

    public static void TeleportToPosition(NavMeshAgent navAgent,  Vector3 newPos)
    {
        navAgent.Warp(newPos);
    }

    public static GameObject FindClosestObject(this Transform ownerPos, List<GameObject> gameObjects)
    {
        float distanceToClosestObject = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject currentObject in gameObjects)
        {
            float distanceToEnemy = (currentObject.transform.position - ownerPos.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestObject)
            {
                distanceToClosestObject = distanceToEnemy;
                closestObject = currentObject;
            }
        }

        return closestObject;
    }
    
    public static IEnumerator KnockBack(this NavMeshAgent navAgent, Vector3 direction,float knockBackDuration, float normalSpeed)
    {
        navAgent.speed = 10;
        navAgent.angularSpeed = 0;//Keeps the enemy facing forwad other than spinning
        navAgent.acceleration = 999;

        float currentDuration = knockBackDuration;

        while (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            navAgent.velocity = direction * 20;//Knocks the enemy back when appropriate 

            yield return null;
        }

        //Reset to default values
        navAgent.speed = normalSpeed;
        navAgent.angularSpeed = 999;
        navAgent.acceleration = 999;
    }
}
