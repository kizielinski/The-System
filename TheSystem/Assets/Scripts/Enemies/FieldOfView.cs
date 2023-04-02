//Kyle Zielinski
//2/10/2023
//This script handles a FOV cone for player/enemies, will also be upgraded to allow more variability.
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField] private LayerMask layerMask;
    private float fov;
    private float startingAngle;
    private Vector3 origin;
    private float viewDistance; //How far cone travels

    [SerializeField] private List<Collider2D> collidersToIgnore;

    public bool playerHit;

    private void Start()
    {
        //defaultValues
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        fov = 45;
        playerHit = false;
        viewDistance = 50f;
    }

    public bool PlayerHitIsNow
    {
        set
        {
            playerHit = value;
        }
    }

    public float ViewDistance
    {
        get
        {
            return viewDistance;
        }
    }

    private void LateUpdate()
    {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero; //FOV origin vertex

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            RaycastHit2D hit = Physics2D.Raycast(origin, Utilities.GetVectorFromAngle(angle), viewDistance, layerMask); // Cast some rays out
            if(hit.collider == null)
            {
                //NoHit
                vertex = origin + Utilities.GetVectorFromAngle(angle) * viewDistance; //Ray/mesh fully extends out
            }
            else if(IgnoreColliders(hit.collider.name))
            {
                vertex = origin + Utilities.GetVectorFromAngle(angle) * viewDistance; //Ray/mesh fully extends out
            }
            else
            {
                //Hit
                vertex = hit.point; //Clip ray

                if(hit.collider.tag == "Player") //If ray hits x specifically do y
                {
                    playerHit = true;
                }
            }

            vertices[vertexIndex] = vertex - origin;

            if(i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        Color32[] colors = new Color32[vertices.Length];
        colors[0] = new Color32(40, 40, 40, 40);
        mesh.colors32 = colors;
    }

    //Sets fov origin to gameobjects current location or other input
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    //Sets aim to desired location
    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = Utilities.GetAngleFromVector(aimDirection) + fov/2;
    }

    private bool IgnoreColliders(string name)
    {
        foreach(Collider2D c in collidersToIgnore)
        {
            if (name == c.name)
            {
                return true;
            }
        }

        return false;
    }

    public void SetFOV(float _fov)
    {
        fov = _fov;
    }
}