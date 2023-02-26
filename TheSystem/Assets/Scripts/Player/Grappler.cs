using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public LineRenderer grapple;

    float distanceX;
    float distanceY;
    Vector2 mousPos;
    //temporary test case, will be removed in final 
    public Camera cameraObj;
    //will be used in the final version instead of mouse
    //Vector2 grappleStart;
    bool isScrapper;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
