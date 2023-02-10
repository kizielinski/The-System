using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities 
{

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVector(Vector3 aimDirection)
    {
        aimDirection = aimDirection.normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if(angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

}
