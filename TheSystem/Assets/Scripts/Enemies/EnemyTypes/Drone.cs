//Kyle Zielinski
//2/10/2023
//This script handles enemy drone hovering and interaction with our FOV cone.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    // Start is called before the first frame update
    private FieldOfView fov;
    private Vector3 currentPos;
    public float stepValue; 
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        isAlive = true;
        StartCoroutine(Hover());
        walkSpeed = 20;
        stepValue = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //Cone alignment
        fov.SetAimDirection(Vector3.down);
        fov.SetOrigin(transform.position);
    }

    /// <summary>
    /// Runs a thread to constantly hover the drone even when attacking
    /// </summary>
    /// <returns></returns>
    private IEnumerator Hover()
    {
        currentPos = transform.position;

        float cosValue = 0;

        while (isAlive)
        {
            currentPos.x += walkSpeed * Mathf.Cos(cosValue) * Time.deltaTime;
            cosValue += stepValue * Time.deltaTime;
            transform.position = currentPos;
            yield return null;
        }
    }

}
