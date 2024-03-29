using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct RNAData
{
    public int rna;
}

public class RNABank : MonoBehaviour
{
    int rna = 0;
    public TMPro.TextMeshProUGUI rnaText;

    public string rnaFilePath = "save_data/rna.json";

    public int RNA
    {
        get { return rna; }
        private set 
        {
            rna = value;
            int last = 0;
            for (int i = 0; i < rnaFilePath.Length; i++)
            {
                if (rnaFilePath[i] == '/' || rnaFilePath[i] == '\\')
                    last = i;
            }
            if (last != 0)
            {
                string subdir = rnaFilePath.Substring(0, last);
                if (!Directory.Exists(subdir))
                    Directory.CreateDirectory(subdir);
            }
            RNAData newData;
            newData.rna = rna;
            File.WriteAllText(rnaFilePath, JsonUtility.ToJson(newData));

            UpdateRNAText(value);
        }
    }

    private void Start()
    {
        try
        {
            RNAData loadedData = JsonUtility.FromJson<RNAData>(File.ReadAllText(rnaFilePath));
            rna = loadedData.rna;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.LogWarning($"Error attempting to load saved RNA data: {e.Message}");
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
        rnaText.text = "RNA Samples: " + amount;
    }
}
