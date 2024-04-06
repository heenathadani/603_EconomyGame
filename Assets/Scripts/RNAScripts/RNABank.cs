using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public struct RNAData
{
    public int rna;
}

public class RNABank : MonoBehaviour
{
    int rna = 0;
    public TMPro.TextMeshProUGUI[] rnaText;

    public string rnaFilePath = "save_data/rna.json";

    public int RNA
    {
        get { return rna; }
        set
        {
            rna = value;
            
            RNAData newData;
            newData.rna = rna;
            SaveTo(rnaFilePath, newData);

            UpdateRNAText(value);
        }
    }

    /// <summary>
    /// Saves all input data to the specified file path.
    /// Creates a new file at the location if one doesn't exist.
    /// </summary>
    /// <typeparam name="T">Data type to save</typeparam>
    /// <param name="path">Path to save to</param>
    /// <param name="data">The data to save</param>
    public static void SaveTo<T>(string path, T data)
    {
        int last = 0;
        for (int i = 0; i < path.Length; i++)
        {
            if (path[i] == '/' || path[i] == '\\')
                last = i;
        }
        if (last != 0)
        {
            string subdir = path.Substring(0, last);
            if (!Directory.Exists(subdir))
                Directory.CreateDirectory(subdir);
        }
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    private void Start()
    {
        if (File.Exists(rnaFilePath))
        {
            RNAData loadedData = JsonUtility.FromJson<RNAData>(File.ReadAllText(rnaFilePath));
            rna = loadedData.rna;
        }
        else
        {
            Debug.LogWarning($"RNA data file does not exist.");
        }
        UpdateRNAText(RNA);
    }

    public void AddRNA(int amount)
    {
        RNA += amount;
    }

    public bool SpendRNA(int amount)
    {
        if (RNA >= amount)
        {
            RNA -= amount;
            return true;
        }
        return false;
    }

    public void UpdateRNAText(float amount) // Update this method
    {
        foreach (TextMeshProUGUI t in rnaText)
            t.text = $"{amount}";
    }
}
