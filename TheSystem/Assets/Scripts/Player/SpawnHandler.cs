using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{    
    public static SpawnHandler instance;
    //Required list of spawnPoints (min one)
    [Tooltip ("You need to manually populate this field.")]
    [SerializeField] List<Campfire> spawnPoints;

    [Tooltip("Enable loading which spawnPoint is active from SaveFile")]
    [SerializeField] bool useSaveFile;
    Campfire spawnCampfire;
    private Vector3 spawnPoint;
    private SaveData saveData;

    public Vector3 SpawnPoint
    {
        get 
        { return spawnPoint; }
    }

    //Extendible struct for handling all necessary SaveData
    private struct SaveData
    {
        public Dictionary<int, bool> saveRoomData;
    }

    private void Awake()
    {
        saveData.saveRoomData = new Dictionary<int, bool>();

        int iterator = 0;

        //Assign all spawnpoints a unique value 
        foreach (Campfire c in spawnPoints)
        {
            c.SavePointID = iterator;
            saveData.saveRoomData.Add(c.SavePointID, c.IsActiveSavePoint);
            iterator++;
        }

        if (instance == null)
        { instance = this; }

        //Throw an error if this tool isn't utilized correctly
        if (spawnPoints == null || spawnPoints.Count < 1)
        {
            throw new System.Exception("There are no player spawnpoints loaded. Please load player spawnpoints");
        }

        //If data exists, load it
        Debug.Log(System.IO.File.Exists(Application.persistentDataPath + "\\saves\\saves.ai"));
        if (System.IO.File.Exists(Application.persistentDataPath + "\\saves\\saves.ai") && useSaveFile)
        {
            LoadData();
        }
        else
        {
            bool existsActiveSavePoint = false; //Dumb bool to prevent human error
            foreach (Campfire c in spawnPoints)
            {
                if (c.IsActiveSavePoint)
                {
                    spawnCampfire = c;
                    existsActiveSavePoint = true;
                }
            }

            //If there was no active Save point, set first save point to the current save point.
            if(!existsActiveSavePoint)
            {
                spawnPoints[0].IsActiveSavePoint = true;
                spawnCampfire = spawnPoints[0];
            }
        }

        Vector3 spawnPos = spawnCampfire.transform.position;
        spawnPos.z = 0.0f; //Zero out Z value to prevent animation errors
        spawnPoint = spawnPos;
    }

    /// <summary>
    /// Takes in an active spawn ID
    /// Writes saveData to file, currently only ID location of current campfire.
    /// </summary>
    /// <param name="ID"></param>
    public void SaveGame(int ID)
    {
        spawnCampfire.IsActiveSavePoint = false;
        saveData.saveRoomData[ID] = true;

        spawnCampfire = spawnPoints[ID];
        spawnCampfire.IsActiveSavePoint = true;

        Debug.LogWarning("Saving data...");
        string savePath = Application.persistentDataPath + "\\saves";
        Debug.LogWarning(savePath);

        if(!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
            File.Create(savePath + "\\saves.ai");
        }

        savePath += "\\saves.ai";
        BinaryWriter binaryStream = new BinaryWriter(File.OpenWrite(savePath));

        foreach (KeyValuePair<int, bool> element in saveData.saveRoomData)
        {
            binaryStream.Write(element.Key);
            binaryStream.Write(element.Value);
        }

        binaryStream.Close();

        Debug.LogWarning("Saving complete...");
    }

    /// <summary>
    /// Loads binary file of saveData at game start
    /// </summary>
    public void LoadData()
    {
        Debug.LogWarning("Loading data...");

        string savePath = Application.persistentDataPath + "\\saves";

        if (!System.IO.Directory.Exists(savePath))
        {
            Debug.LogError("No save file found! Cannot continue game initialization properly...be warned.");
            return;
        }
        else
        {
            savePath += "\\saves.ai";
        }

        BinaryReader binaryInputStream = new BinaryReader(File.OpenRead(savePath));

        for(int i = 0; i < saveData.saveRoomData.Count; i++)
        {
            int key = binaryInputStream.ReadInt32();
            bool value = binaryInputStream.ReadBoolean();

            spawnPoints[key].IsActiveSavePoint = value;

            if(value)
            {
                spawnCampfire = spawnPoints[key];
            }
        }

        binaryInputStream.Close();
        Debug.LogWarning("Loading complete...");
    }

    /// <summary>
    /// If player dies, this method is called and respawns the player
    /// </summary>
    public void RespawnPlayer()
    {
        Vector3 newPos = spawnCampfire.transform.position;
        newPos.z = 0.0f;
        Player.instance.transform.position = newPos;
    }
}
