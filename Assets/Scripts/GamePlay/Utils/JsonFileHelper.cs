﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonFileHelper
{
    public static void SaveToFile(string filePath, object serializedObject )
    {
        if (!File.Exists(filePath))
            File.Create(filePath);

        string json = JsonUtility.ToJson(serializedObject);

        using (StreamWriter writer = new StreamWriter(File.OpenWrite(filePath)))
        {
            writer.WriteLine(json);
        }
    }

    public static object LoadFromFile<T>(string filePath) where T: class
    {

        StringBuilder jsonStrBuilder = new StringBuilder();
        using (StreamReader reader = new StreamReader(File.OpenRead(filePath)))
        {
            while (!reader.EndOfStream)
            {
                var rowData =  reader.ReadLine();
                jsonStrBuilder.AppendLine(rowData);
            }
        }

        return JsonUtility.FromJson<T>(jsonStrBuilder.ToString());
    }
}
