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

    public static Vector3 DirectionToTarget(Vector3 target, Vector3 origin)
    {
        Vector3 directionVector = (target - origin).normalized;

        return directionVector;
    }

    public static Vector3 GetTargetPlayer()
    {
        return Player.instance.transform.position;
    }

    public static void ScaleHitBox(BoxCollider2D c, Vector2 dim)
    {
        c.size = dim;
    }

    public static void DefaultHitBox(BoxCollider2D c)
    {
        c.size = Vector2.one;
    }
}
