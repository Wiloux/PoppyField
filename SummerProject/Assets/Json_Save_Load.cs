using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Json_Save_Load : MonoBehaviour
{
    public Inventaire inventory;
    public static Json_Save_Load ins;

    private string file = "inventory.txt";

    private void Awake()
    {
        ins = this;
        try
        {
            Load();
        }
        catch
        {
            inventory = GetComponent<Inventaire>();
        }
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(inventory);
        WriteToFile(file, json);
    }

    public void Load()
    {
        inventory = GetComponent<Inventaire>();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, inventory);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        writer.Write(json);
        
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        Debug.Log(path);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.Log("Fichier non existant");
        }
        return "";
    }

    private string GetFilePath(string fileName)
    {
        return Application.dataPath + "/" + fileName;
    }
}
