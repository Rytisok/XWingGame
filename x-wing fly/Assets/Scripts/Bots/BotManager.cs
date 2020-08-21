using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public GameObject target;

    public float minFollowDistance = 1;
    public float minFireDistance = 7;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = GetDistanceToTarget();

        FollowTarget(distanceToTarget);
        FireBehaviour(distanceToTarget);
    }

    void FollowTarget(float distanceToTarget)
    {
        if (distanceToTarget > minFollowDistance)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.05f);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 10 * Time.deltaTime);
    }

    void FireBehaviour(float distanceToTarget)
    {
      //  if (distanceToTarget > minFireDistance)
    }

    float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }
}
