using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPosition
{
    private string name;
    private float timestamp;
    private Vector3 position;
    public CharacterPosition(string name, float timestamp, Vector3 position)
    {
        this.name = name;
        this.timestamp = timestamp;
        this.position = position;
    }
    public CharacterPosition()
    {

    }
    public override string ToString()
    {

        return $"{name}; {timestamp}; {position.x}; {position.y}; {position.z}";
    }
    public string ToCSV()
    {

        return $"{name} {timestamp} {position.x}";
    }
}
