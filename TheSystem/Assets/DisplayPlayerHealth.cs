using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    Stack<GameObject> hearts;

    List<GameObject> heartsList;
 
    void Start()
    {
        hearts = new Stack<GameObject>();
        heartsList = new List<GameObject>();
        Transform[] t = GetComponentsInChildren<Transform>();

        foreach(Transform transform in t)
        {
            hearts.Push(transform.gameObject);
        }
    }

    public void TakeUIDamage()
    {
        GameObject g = hearts.Pop().gameObject;
        g.SetActive(false);
        heartsList.Add(g);
    }

    public void UIRespawn()
    {
        foreach(GameObject g in heartsList)
        {
            g.SetActive(true);
            hearts.Push(g);
        }
    }
}
