using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    //variables for the player, the box collider of this object, and text mesh
    [SerializeField] Player player;
    [SerializeField] BoxCollider2D thisBox;
    private TextMesh thisMesh;

    // Start is called before the first frame update
    void Start()
    {
        //instantiates the text mesh and sets the alpha to zero
        thisMesh = gameObject.GetComponent<TextMesh>();
        thisMesh.color = new Color(255, 255, 255, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //while the player is inside of the collision box for the text mesh
        if(PointInBoxCollider(player.transform.position, thisBox))
        {
            //the text fades in
            thisMesh.color = new Color(255, 255, 255, thisMesh.color.a + Time.deltaTime);
        }
        else
        {
            //otherwise the text fades out
            gameObject.GetComponent<TextMesh>().color = new Color(255, 255, 255, thisMesh.color.a - Time.deltaTime);
        }
    }

    //method borrowed from Abubakar5415545 at https://forum.unity.com/threads/solved-use-oncollisionenter-or-ontriggerenter-without-rigidbody.539870/
    //thank you Abubakar <3
    bool PointInBoxCollider(Vector3 point, BoxCollider2D box)
    {
        //grabs the center of the box collider and the extents of the box collider
        Vector2 center = box.bounds.center;
        Vector2 extents = box.bounds.extents;

        //calculates the far corners of the box collider using the above calculated center and extents
        Vector2 rightTop = center + extents;
        Vector2 leftTop = center + new Vector2(-extents.x, extents.y);
        Vector2 rightBottom = center + new Vector2(extents.x, -extents.y);
        Vector2 leftBottom = center - extents;

        //checks if the player is within the calculated box and returns true or false
        if (point.x < rightTop.x && point.x > leftTop.x && point.y < rightTop.y && point.y > rightBottom.y)
            return true;
        return false;
    }
}
