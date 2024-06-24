using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class FileManager
{
    public static bool WriteToFile(string filename, string data)
    {

        string fullpath = Path.Combine(Application.persistentDataPath, filename);

        try
        {
            File.WriteAllText(fullpath, data);
            Debug.Log("Fichero guardado correctamente en:" + fullpath);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Error al guardar el fichero en:" + fullpath + "con el error" + e);
            return false;

        }
    }
}
