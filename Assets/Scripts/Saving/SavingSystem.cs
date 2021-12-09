using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    #region Fields

    private MainEditor mainEditor;

    #endregion

    #region UnityInspector

    [SerializeField] private MapInfos mapInfos;

    [SerializeField] private KeyCode manualSaveKey = KeyCode.F5;
    [SerializeField] private KeyCode manuelLoadKey = KeyCode.F9;

    #endregion

    #region Behaviour

    private void Awake()
    {
        mainEditor = GetComponent<MainEditor>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(manualSaveKey))
        {
            Save("defaultMap");
        }
        if(Input.GetKeyDown(manuelLoadKey))
        {
            Load("defaultMap");
        }
    }

    public void Save(string _fileName)
    {
        Debug.Log("Save");
        mapInfos.SaveDatas(mainEditor.width, mainEditor.height, mainEditor._Tab);

        XmlSerializer serializer = new XmlSerializer(typeof(MapInfos));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingFiles/XML/" + _fileName+ ".xml", FileMode.Create);
        serializer.Serialize(stream, mapInfos);
        stream.Close();
    }

    public MapInfos Load(string _fileName)
    {
        Debug.Log("Load");

        XmlSerializer serializer = new XmlSerializer(typeof(MapInfos));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingFiles/XML/" + _fileName + ".xml", FileMode.Open);
        mapInfos = serializer.Deserialize(stream) as MapInfos;
        stream.Close();

        return mapInfos;
    }

    #endregion
}
