using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] int savePointID;
    [SerializeField] bool canSaveGame;
    [SerializeField] bool isActiveSavePoint = false;

    public bool IsActiveSavePoint
    {
        get { return isActiveSavePoint; }
        set { isActiveSavePoint = value; }
    }

    public int SavePointID
    {
        get { return savePointID; }
        set { savePointID = value; }
    }

    private void Start()
    {
        canSaveGame = false;
    }

    private void Update()
    {
        if(canSaveGame)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SpawnHandler.instance.SaveGame(savePointID);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canSaveGame = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canSaveGame = false;
        }
    }

    public void SetSpawn()
    {
        isActiveSavePoint = true;
    }
}
