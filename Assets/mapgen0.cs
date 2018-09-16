using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class mapgen0 : MonoBehaviour {


    public int width;
    public int height;

    [Range(0,100)]
    public int control_big;

    public GameObject mycell;
    public GameObject[,] all_cells;

    public string seed;
    public bool use_rnd_seed;
    [Range(0, 100)]
    public int randfill;
    private static System.Random random;

    private int[,] map;
    private int[,] stat;

    public static string RandomString(int length)
    {
        random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(System.Linq.Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
    void GenerateMap()
    {
        map = new int[width,height];
        stat = new int[width, height];
        all_cells = new GameObject[width,height];

        Fillrandom();

         for(int x = 0; x < width; x++)
         {
             for (int y = 0; y < height; y++)
             {
                 stat[x, y] = map[x, y];
             }
         }
     //   stat = map;

    }

    void Fillrandom()
    {
        if (use_rnd_seed) { seed = RandomString(20); }
        System.Random rrpm = new System.Random(seed.GetHashCode());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) { map[x, y] = 0; }
                else { map[x, y] = (rrpm.Next(0, 100) < randfill) ? 1 : 0; }

              /*- GameObject temp_cell=  Instantiate(mycell, new Vector2(-width / 2 + x, -height / 2 + y), Quaternion.identity) as GameObject;
                if (map[x, y] == 1) { temp_cell.GetComponent<SpriteRenderer>().color = Color.white; }
                else { temp_cell.GetComponent<SpriteRenderer>().color = Color.black; }*/
              //  all_cells[x,y] = temp_cell;
            }

        }
    }
    void Cellrenderupdater()
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (map[x, y] == 1) { all_cells[x, y].GetComponent<SpriteRenderer>().color = Color.white; }
                else { all_cells[x, y].GetComponent<SpriteRenderer>().color = Color.black; }
            }
        }
    }
    void Gameoflife()
    {
        //map = stat;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = stat[x, y];
            }
        }
       //Cellrenderupdater();

        for (int x = 1; x < width - 1; x++)
        {

            for (int y = 1; y < height - 1; y++)
            {
                int neighbors = map[x - 1, y - 1] + map[x - 0, y - 1] + map[x + 1, y - 1] +
                                map[x - 1, y + 0] + 0 + map[x + 1, y + 0] +
                                map[x - 1, y + 1] + map[x - 0, y + 1] + map[x + 1, y + 1];

                if (map[x, y] == 1)
                {
                    stat[x, y] = (neighbors == 2 || neighbors == 3) ? 1 : 0;
                  

                }
                else
                {
                     stat[x, y] = (neighbors == 3) ? 1 : 0;
                   
                }

            }
        }
    }
    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.red : Color.black;
                    Vector2 pos = new Vector2(-width / 2 + x + 10.0f, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
    
    void Control()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 posi = Input.mousePosition;
            //  Debug.Log(posi);
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);

                // hitInfo.transform.GetComponent<SpriteRenderer>().color = Color.black;
                // Debug.Log(hitInfo.transform.gameObject.transform.position);
                Vector3 temp = new Vector3(width / 2, height / 2,0);
                Vector2 pipos = hitInfo.transform.position + temp;
                int a = (int)pipos.x;
                int b = (int)pipos.y;
                for(int x=a-control_big/2; x< a+control_big / 2 +1 && x>0 && x<width; x++)
                {
                    for (int y=b-control_big/2;y<b+control_big/2 +1 && y>0 && y< height;y++)
                    {
                        stat[x, y] = 1;
                    }
                }
               
            }
            else
            {
                Debug.Log("No hit");
                
            }
        }
       
    }

    void ControlII()
    {
        
    }
    // Use this for initialization
    void Start () {
        GenerateMap();

    }
	
	// Update is called once per frame
	void Update () {
        Gameoflife();
        Control();
	}
}
