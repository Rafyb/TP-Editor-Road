using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public SavingSystem syst;

    public InputField field;
    
    public void OnClickSave()
    {
        syst.Save(field.text);
    }
    
    public void OnClickLoad()
    {
        MapInfos mapData = syst.Load(field.text);
        
        MainEditor.Instance.LoadData(mapData.infosWidth,mapData.infosHeight,mapData.infosTabArray);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
