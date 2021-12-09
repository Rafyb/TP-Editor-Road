using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEditor : MonoBehaviour
{
    public int width;
    public int height;

    private int[,] _tab; 
    private GameObject[,] _tiles;
    public List<GameObject> roads;
    private GameObject _preview;
    
    void Start()
    {
        _tab = new int[width,height];
        _tiles = new GameObject[width,height];
        //_pos = new Vector2[widht,height];
        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                _tab[x,y] = -1;
                _tiles[x, y] = null;
            }
        }

        Camera.main.transform.position = new Vector3(width / 2, 5, height / 2);

        _preview = GameObject.Instantiate(roads[0], Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos.y = 0f;
        pos.x = Mathf.Round(pos.x);
        pos.z = Mathf.Round(pos.z);

        Debug.Log(pos.x +" "+pos.z);

        if(pos.x >= 0 && pos.x < width && pos.z >= 0 && pos.z < height) _preview.transform.position = pos;
        
        if(Input.GetMouseButtonDown(0)) OnClick();
    }

    void OnClick()
    {
        AddRoad(_preview.transform.position);
        
        
        
    }

    public int[,] GetData()
    {
        return _tab;
    }

    private void AddRoad(Vector3 pos)
    {
        int x = (int) pos.x;
        int z = (int) pos.z;
        
        if(_tab[x, z] != -1) return;
        
        
            
        _tiles[x, z] = Instantiate(roads[0], pos , Quaternion.identity);
        _tab[x, z] = 0;
        
        UpdateTile(x,z);

        if (_tab[x + 1, z] != -1) UpdateTile(x + 1, z);
        if (_tab[x - 1, z] != -1) UpdateTile(x - 1, z);
        if (_tab[x, z - 1] != -1) UpdateTile(x, z - 1);
        if (_tab[x, z + 1] != -1) UpdateTile(x, z + 1);
    }

    private void UpdateTile(int posX, int posZ)
    {
        int voisins = 0;
        int id = 0;

   
        if (_tab[posX + 1, posZ] != -1) voisins++;
        if (_tab[posX - 1, posZ] != -1) voisins++;
        if (_tab[posX , posZ + 1] != -1) voisins++;
        if (_tab[posX , posZ - 1] != -1) voisins++;
  
        
        if (voisins == 0) id = 0;
        if (voisins == 4) id = 6;

        if (id != _tab[posX, posZ])
        {
            Destroy(_tiles[posX, posZ]);
            _tiles[posX, posZ] = Instantiate(roads[id], new Vector3(posX,0,posZ) , Quaternion.identity);
            _tab[posX, posZ] = id;
        }
    }
}
