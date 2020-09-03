using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringPrediction :MonoBehaviour
{
    private GameObject fireOrigin;
    private GameObject fireTarget;

    private Transform fireOriginTrans;
    private Transform fireTargetTrans;

    private float projectileVelocity;
    private float maxTargetDistance;
    private bool setup;

    private Vector3 lastPos;

    private const float firingDirUpdateFreq = 0.1f;
    private float t;

    public void Initialize(GameObject fireOrigin, GameObject fireTarget, float projectileVelocity,float maxTargetDistance)
    {
        this.fireOrigin = fireOrigin;
        this.fireTarget = fireTarget;
        this.projectileVelocity = projectileVelocity;
        this.maxTargetDistance = maxTargetDistance;

        fireOriginTrans = fireOrigin.transform;
        fireTargetTrans = fireTarget.transform;

        lastPos = new Vector3(fireTargetTrans.position.x, fireTargetTrans.position.y, fireTargetTrans.position.z) ;

        setup = true;

        t = 0;
    }

    void Update()
    {
        if (setup)
        {
            if (Vector3.Distance(fireOriginTrans.position, fireTargetTrans.position) <= maxTargetDistance &&
                t >= firingDirUpdateFreq)
            {
                AcquireTargetLock(fireTarget);
                t = 0;
            }

            t += Time.deltaTime;
            lastPos = new Vector3(fireTargetTrans.position.x, fireTargetTrans.position.y, fireTargetTrans.position.z);

        }
    }


    void AcquireTargetLock(GameObject target)
    {
        bool acquireTargetLockSuccess;
    //    PlayerShip playerShipScript = target.GetComponent<playership>();
        Vector3 targetVelocity = (fireTargetTrans.position - lastPos) / Time.deltaTime;
        Vector3 direction = CalculateInterceptCourse(target.transform.position, targetVelocity, fireOriginTrans.position, projectileVelocity, out acquireTargetLockSuccess);
        if (acquireTargetLockSuccess)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            fireOriginTrans.rotation =
                targetRotation; //Quaternion.Slerp(fireOriginTrans.rotation, targetRotation, Time.deltaTime * 10);
            /*    if (Mathf.Abs(fireOriginTrans.rotation.eulerAngles.y - targetRotation.eulerAngles.y) < 5)
                    Fire();*/
        }
    }
    public static Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 aInterceptorPos, float aInterceptorSpeed, out bool aSuccess)
    {
        aSuccess = true;
        Vector3 targetDir = aTargetPos - aInterceptorPos;
        float iSpeed2 = aInterceptorSpeed * aInterceptorSpeed;
        float tSpeed2 = aTargetSpeed.sqrMagnitude;
        float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
        float targetDist2 = targetDir.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
        if (d < 0.1f)
            aSuccess = false;
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDist2;
        float S2 = (-fDot1 + sqrt) / targetDist2;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
                return Vector3.zero;
            else
                return (S2) * targetDir + aTargetSpeed;
        }
        else if (S2 < 0.0001f)
            return (S1) * targetDir + aTargetSpeed;
        else if (S1 < S2)
            return (S2) * targetDir + aTargetSpeed;
        else
            return (S1) * targetDir + aTargetSpeed;
    }

}
