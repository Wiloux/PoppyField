using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Xml_Manager : MonoBehaviour
{
    public Inventaire inventory;
    public static Xml_Manager ins;

    private void Awake()
    {
        ins = this;
        try
        {
            loadInventory();
        }
        catch
        {
            inventory = GetComponent<Inventaire>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void saveInventory()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Inventaire));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingFiles/XML/inventory_data", FileMode.Create);
        serializer.Serialize(stream, inventory);
        stream.Close();
    }

    public void loadInventory()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Inventaire));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingFiles/XML/inventory_data", FileMode.Open);
        inventory = serializer.Deserialize(stream) as Inventaire;
        stream.Close();
    }
}
