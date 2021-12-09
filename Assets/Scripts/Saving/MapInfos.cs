using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapInfos
{
    public int infosWidth;
    public int infosHeight;

    public int[] infosTabArray;
    //public int[,] infosTab;
    //public GameObject[,] infosTiles;

    public void SaveDatas(int _saveWidth, int _saveHeight, int[,] saveTab)
    {
        infosWidth = _saveWidth;
        infosHeight = _saveHeight;

        infosTabArray = new int[infosWidth * infosHeight];
        int index = 0;

        for (int i = 0; i < infosWidth; i++)
        {
            for (int j = 0; j < infosHeight; j++)
            {
                infosTabArray[index++] = saveTab[i, j];
            }
        }
        //infosTab = saveTab;
        //infosTiles = saveTiles;
    }
}
