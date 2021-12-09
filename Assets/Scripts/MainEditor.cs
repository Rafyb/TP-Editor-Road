using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEditor : MonoBehaviour
{
    public static MainEditor Instance;
    
    [Header("Map")]
    public int width;
    public int height;

    private int[,] _tab; 
    private GameObject[,] _tiles;
    public List<GameObject> roads;
    public GameObject _preview;
    
    [Header("Camera")]
    public float minCam;
    public float maxCam;
    public float sensitivity;

    private void Awake()
    {
        Instance = this;
    }

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

        //_preview = GameObject.Instantiate(roads[0], Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos.y = 0f;
        pos.x = Mathf.Round(pos.x);
        pos.z = Mathf.Round(pos.z);


        if(pos.x >= 0 && pos.x < width && pos.z >= 0 && pos.z < height) _preview.transform.position = pos;
        
        if(Input.GetMouseButtonDown(0)) AddRoad(_preview.transform.position);
        if (Input.GetMouseButtonDown(1))
        {
            RemoveRoad(_preview.transform.position);
            int posX = (int) _preview.transform.position.x;
            int posZ = (int) _preview.transform.position.z;
            if (_tab[posX + 1, posZ] != -1) UpdateTile(posX + 1, posZ);
            if (_tab[posX - 1, posZ] != -1) UpdateTile(posX - 1, posZ);
            if (_tab[posX, posZ - 1] != -1) UpdateTile(posX, posZ - 1);
            if (_tab[posX, posZ + 1] != -1) UpdateTile(posX, posZ + 1);
        }
        
        float size = Camera.main.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minCam, maxCam);
        Camera.main.orthographicSize = size;
    }

    private void RemoveRoad(Vector3 pos)
    {
        int posX = (int) pos.x;
        int posZ = (int) pos.z;
        Destroy(_tiles[posX, posZ]);
        _tab[posX, posZ] = -1;
        
        
    }

    private void Clear()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                RemoveRoad(new Vector3(i, 0, j));
            }
        }
    }

    private void InstantiateRoad(int id, int posX, int posZ)
    {
        _tiles[posX, posZ] = Instantiate(roads[id], new Vector3(posX,0,posZ) , roads[id].transform.rotation);
        _tab[posX, posZ] = id;
    }


    public int[,] GetData()
    {
        return _tab;
    }

    public void LoadData(int w, int h, int[] ids)   
    {
        Clear();
        
        width = w;
        height = h;

        _tab = new int[width,height];
        _tiles = new GameObject[width,height];
        
        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                _tab[x,y] = -1;
                _tiles[x, y] = null;
            }
        }
        
        for (int idx = 0; idx < width*height; idx++)
        {
            int id = ids[idx];

            int z = idx % w;
            int x = idx / w;
            
            Debug.Log(x+" "+z+" id:"+id);

            if(id>=0) InstantiateRoad(id,x,z);
        }
    }

    private void AddRoad(Vector3 pos)
    {
        int x = (int) pos.x;
        int z = (int) pos.z;
        
        if(_tab[x, z] != -1) return;
        
        InstantiateRoad(0,x,z);
        
        UpdateTile(x,z);

        if (_tab[x + 1, z] != -1) UpdateTile(x + 1, z);
        if (_tab[x - 1, z] != -1) UpdateTile(x - 1, z);
        if (_tab[x, z - 1] != -1) UpdateTile(x, z - 1);
        if (_tab[x, z + 1] != -1) UpdateTile(x, z + 1);
    }

    private void UpdateTile(int posX, int posZ)
    {
        int id = 0;
        
        int left = -1;
        if (posX - 1 > 0) left = _tab[posX - 1, posZ];
        
        int right = -1;
        if (posX + 1 < width) right = _tab[posX + 1, posZ];
        
        int bot = -1;
        if (posZ - 1 > 0) bot = _tab[posX, posZ - 1];
        
        int top = -1;
        if (posZ + 1 > 0) top = _tab[posX, posZ + 1];
        
        if (top != -1 && bot != -1 && right != -1 && left != -1) id = 6;
        else if (top != -1)
        {
            id = 1;
            if (left != -1 && right != -1)
            {
                id = 7;
            }
            else if (left != -1 && bot != -1)
            {
                id = 10;
            }
            else if (right != -1 && bot != -1)
            {
                id = 8;
            }
            else if (left != -1)
            {
                id = 5;
                
            }
            else if (right != -1)
            {
                id = 2;
            }
        }
        else if (bot != -1)
        {
            id = 1;
            if (left != -1 && right != -1)
            {
                id = 9;
            }
            else if (left != -1)
            {
                id = 4;
                
            }
            else if (right != -1)
            {
                id = 3;
            }
        }
        

        if (id != _tab[posX, posZ])
        {
            RemoveRoad(new Vector3(posX, 0, posZ));
            InstantiateRoad(id,posX,posZ);
            
        }
    }
}
