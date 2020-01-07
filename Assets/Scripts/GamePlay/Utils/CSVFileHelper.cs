using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVFileHelper
{
  public static List<string> LoadDataFromFile(string filePath)
    {
      if (!File.Exists(filePath))
      {
            Debug.LogError("File at: " + filePath + " not exists!");
          return null;
      }

      using (var reader = new StreamReader(File.OpenRead(filePath)))
      {
          List<string> result = new List<string>();
        while (!reader.EndOfStream)
        {
            var row = reader.ReadLine();
            result.Add(row);
        }
        return result;
      }
    }



    
}
