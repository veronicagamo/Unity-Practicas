using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemies;
    private float prevTime;
    private float prevSaveTime;
    private float logInterval = 1;
    private float logSaveInterval = 10;
    private Positions playerPos;
    private Positions enemyPos;

    void Start()
    {
        prevTime = Time.realtimeSinceStartup;
        prevSaveTime = prevTime;
        playerPos = new Positions();
        enemyPos = new Positions();
    }

    void Update()
    {
        float currentTime = Time.realtimeSinceStartup;
        if (currentTime > prevTime + logInterval)
        {
            prevTime += logInterval;
            CharacterPosition cp = new CharacterPosition(player.name, currentTime, player.transform.position);
            playerPos.positions.Add(cp);
            foreach (GameObject enemy in enemies)
            {
                CharacterPosition en = new CharacterPosition(enemy.name, currentTime, enemy.transform.position);
                enemyPos.positions.Add(en);
            }
        }
        if (currentTime > prevSaveTime + logSaveInterval)
        {
            prevSaveTime += logSaveInterval;
            SaveCSVToFile();
            SaveJSONToFile();
            SaveXMLToFile();
        }
    }

    private void SaveCSVToFile()
    {
        string data = "Name; Timestamp; x; y; z\n";
        foreach (CharacterPosition cp in playerPos.positions)
        {
            data += cp.ToCSV() + "\n";
        }
        foreach (CharacterPosition cp in enemyPos.positions)
        {
            data += cp.ToCSV() + "\n";
        }
        FileManager.WriteToFile("positions.csv", data);
    }

    private void SaveJSONToFile()
    {
        string data = "[";
        foreach (CharacterPosition cp in playerPos.positions)
        {
            data += JsonUtility.ToJson(cp) + ",\n";
        }
        foreach (CharacterPosition cp in enemyPos.positions)
        {
            data += JsonUtility.ToJson(cp) + ",\n";
        }
        data += "]";
        FileManager.WriteToFile("positions.json", data);

        FileManager.WriteToFile("playerPositions.json", JsonUtility.ToJson(playerPos));
        FileManager.WriteToFile("enemyPositions.json", JsonUtility.ToJson(enemyPos));
    }

    private void SaveXMLToFile()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Positions));
        using (FileStream stream = new FileStream("playerPositions.xml", FileMode.Create))
        {
            serializer.Serialize(stream, playerPos);
        }
        using (FileStream stream = new FileStream("enemyPositions.xml", FileMode.Create))
        {
            serializer.Serialize(stream, enemyPos);
        }
    }
}
