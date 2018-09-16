using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class mapgen : MonoBehaviour {
    public int width;
    public int height;

    public int cpt;

    public GameObject dead;
    public GameObject livs;
    public GameObject mycell;

    public string seed;
    public bool use_rnd_seed;
    [Range(0, 100)]
    public int randfill;
    private static System.Random random;

    int[,] map;
    int[,] stat;
    private void Start()
    {
        
        Generatemap();
        stat = map;
        cpt = 1;
    }

    private void Mappster()
    {
        for (int x=0;x<width;x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject temp_cell=Instantiate(mycell, new Vector2(-width / 2 + x, -height / 2 + y), Quaternion.identity);
                if (map[x, y] == 1) { temp_cell.GetComponent<SpriteRenderer>().color = Color.white; }
                else { temp_cell.GetComponent<SpriteRenderer>().color = Color.black; }
            }
        }

    }
    public static string RandomString(int length)
    {
        random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(System.Linq.Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
    private void Generatemap()
    {
        map = new int[width, height];
        stat = new int[width, height];
       
          Randomfillmap();
       // Inifill();
    }

    void Inifill()
    {
        for (int x=0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {
                map[x, y] = 0;
            }
        }

      /*
      
        map[60, 45]=1; 
        map[60, 46]=1; 
        map[60, 47]=1; 
        map[60, 48]=1; 
        map[60, 49]=1; 
        map[60, 50]=1; 
        map[60, 51]=1; 
        map[60, 52]=1; 
        map[60, 53]=1; 
      
      */
    }

    void Randomfillmap()
    {
        if (use_rnd_seed)
        {
            // seed = Time.time.ToString();
            seed = RandomString(10);
        }

        System.Random rrpm = new System.Random(seed.GetHashCode());
        for(int x=0; x<width;x++)
        {
            for (int y = 0; y < height; y++)
            {
                  if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                  {
                      map[x, y] = 0;
                  }
                  else
                  {
                      map[x, y] = (rrpm.Next(0, 100) < randfill) ? 1 : 0;
                  }

                // map[x, y] = (rrpm.Next(0, 100) < randfill) ? 1 : 0;
                //Vector2 pos = new Vector2(-width / 2 + x  , -height / 2 + y );
                //Instantiate(mycell, pos, Quaternion.identity);
            }
        }
        Mappster();
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
                    Vector2 pos = new Vector2(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

    void Gameoflife()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                map[x,y] = stat[x,y];
            }
        }
        
        for(int x = 1; x < width-1; x++)
        {
            
            for(int y = 1; y < height-1; y++)
            {
                int neighbors = map[x - 1, y - 1] + map[x - 0, y - 1] + map[x + 1, y - 1] +
                                map[x - 1, y + 0] + 0 + map[x + 1, y + 0] +
                                map[x - 1, y + 1] + map[x - 0, y + 1] + map[x + 1, y + 1];
               // string s = "Cord" + x.ToString() + "," + y.ToString() + " neighbors::" + neighbors.ToString();
                
                
                if(map[x,y]==1)
                {
                    //stat[x, y] = (neighbors == 2 || neighbors == 3) ? 1 : 0;
                   if(neighbors <2 || neighbors > 3)
                    {
                        stat[x, y] = 0;
                    }
                    else
                    {
                        stat[x, y] = 1;
                    }
                  
                }
                else
                {
                    // stat[x, y] = (neighbors == 3) ? 1 : 0;
                    if (neighbors == 3)
                    {
                        stat[x, y] = 1;
                    }
                    else
                    {
                        stat[x, y] = 0;
                    }
                }
              /* Vector2 pos = new Vector2(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f);
                if (map[x, y] == 1)
                {
                    
                    Instantiate(livs, pos, Quaternion.identity);
                }
                else
                {
                    Instantiate(dead, pos, Quaternion.identity);
                }
                */
            }
        }
        
    }
    private void Control()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 posi = Input.mousePosition;
          //  Debug.Log(posi);
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);

                hitInfo.transform.GetComponent<SpriteRenderer>().color = Color.black;
                Debug.Log(hitInfo.transform.gameObject.transform.position);
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
    private void Update()
    {
      //  System.Threading.Thread.Sleep(1000);
        Gameoflife();
        Control();
    }
}
