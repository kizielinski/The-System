using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplerPositionSet : MonoBehaviour
{
    public Grappler grapple;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PointInBoxCollider(grapple.transform.position, this.gameObject.GetComponent<BoxCollider2D>()))
        {
            grapple.enabled = true;
            grapple.newPointX = this.gameObject.transform.position.x;
            grapple.newPointY = this.gameObject.transform.position.y;
        }
        else
        {
            grapple.enabled = false;
        }
    }

    bool PointInBoxCollider(Vector3 point, BoxCollider2D box)
    {
        Vector2 center = box.bounds.center;
        Vector2 extents = box.bounds.extents;

        Vector2 rightTop = center + extents;
        Vector2 leftTop = center + new Vector2(-extents.x, extents.y);
        Vector2 rightBottom = center + new Vector2(extents.x, -extents.y);
        Vector2 leftBottom = center - extents;
        if (point.x < rightTop.x && point.x > leftTop.x && point.y < rightTop.y && point.y > rightBottom.y)
            return true;
        return false;
    }
}
