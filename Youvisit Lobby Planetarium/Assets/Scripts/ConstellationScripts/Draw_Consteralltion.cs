using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_Consteralltion : MonoBehaviour {

    // Use this for initialization
    /// <summary>
    /// Array of the points in the constellation
    /// </summary>
    [SerializeField]
    private GameObject[] points;
    /// <summary>
    /// List of the points positions
    /// </summary>
    public List<Vector3> positions;
    /// <summary>
    /// Keeps track of the positions reached
    /// </summary>
    public List<Vector3> positionsreached;
    /// <summary>
    /// keeps track of points reached
    /// </summary>
    public List<GameObject> pointsreached;
    /// <summary>
    /// Holds the movers needed to complete the constellation
    /// </summary>
    public List<GameObject> movers;
    /// <summary>
    /// Speed of movers
    /// </summary>
    private float speed;
    /// <summary>
    /// bool to see if a constellation has been looked at
    /// </summary>
    private bool GazeOver;
    /// <summary>
    /// Position of constellation
    /// </summary>
    private  Vector3 orginalpos;
    /// <summary>
    /// Reference to a mover prefab
    /// </summary>
    public GameObject mover;

   
    /// <summary>
    /// Keeps track of movers index
    /// </summary>
    private int j;
    /// <summary>
    /// keeps track of points index
    /// </summary>
    private int k;

    /// <summary>
    /// poistion to start drawing at
    /// </summary>
    private Vector3 pos;
    /// <summary>
    /// point to start drawing at
    /// </summary>
    private GameObject point;
    /// <summary>
    /// bool to see if the constellation is starting
    /// </summary>
    private bool first;
    /// <summary>
    /// bool to see if constellation is finished
    /// </summary>
    public bool finished;

  

    void Start () {

        finished = false;
        first = true;
        
        GazeOver = false;
        speed = 10;
        k = 1;
      
        j = 0;
        orginalpos = transform.position;

        pos = points[0].transform.position;

        point = points[0];

      

        for(int w = 1; w < points.Length; w++)
        {
            positions.Add(points[w].transform.position);
            
        }
        movers.Add(mover);

    }

    // Update is called once per frame
    void Update() {
        
        if (GazeOver == true)
        {
            Draw();
        }
        else if(GazeOver == false)
        {
            
        }

    }

    /// <summary>
    /// Uses event trigger
    /// called when a constellation is looked at
    /// changes bool based on its previous value
    /// </summary>
    public void GazeOn()
    {
        if(finished == false)
        {
            GazeOver = true;
        }
        else
        {
            GazeOver = false;
        }
    }
    /// <summary>
    /// not used
    /// </summary>
    public void GazeOff()
    {
        GazeOver = false;
    }

    /// <summary>
    /// Draws the constellation from one point to another
    /// </summary>
    public void Draw()
    {
        //keeps going if the constellation is not done
        if (finished == false)
        {
            float step = speed * Time.deltaTime;

            
            //moves a gameobject with a trail render to the next position 
            if (first == true)
            {
                movers[j] = Instantiate(mover, pos, Quaternion.identity);
                first = false;
            }

            //moves the object with the trail render to the next point in the constellation, simulates drawing
            if (j < points.Length - 1)
            {
                movers[j].transform.position = Vector3.MoveTowards(movers[j].transform.position, points[k].transform.position, step);

                if(k == points.Length -1 && movers[j].transform.position == points[k].transform.position)
                {
                    finished = true;
                }
                
            }
            //checks to see if the constellation is done
            if (k < points.Length - 1 && movers[j].transform.position == points[k].transform.position)
            {
                //checks to see if the point in the constellation has already been drawn too
                //if it has not it will add the next position to be moved too
                if (!pointsreached.Contains(points[k + 1]))
                {
                    
                    point = points[k];

                    pointsreached.Add(point);
                }
                //if the position has been drawn to already it will teleport to that position and start drawing from it to prevent overlapping
                else
                {
                    point = points[k + 1];
                }

                k++;
                j++;
                movers.Add(mover);
                //creates the gameobject with the trailrender
                movers[j] = Instantiate(mover, point.transform.position, Quaternion.identity);
            }

        }
    }

  
    /// <summary>
    /// Uses event trigger
    /// called when the user stops looking at the constellation
    /// Erases the constellation lines if it is unfinished
    /// </summary>
    public void Erase()
    {
        
        GazeOver = false;
        //checks to see if the constellation has finished drawing and if not it will delete the lines
        if (finished == false)
        {
            pos = points[0].transform.position;
            //detroys the gameobjects with the constellations to delete the trail renders
            for (int q = 0; q < movers.Count; q++)
            {
                

                DestroyObject(movers[q]);
            }


            //clears the lists to start fresh
            movers.Clear();

            positionsreached.Clear();

            pointsreached.Clear();

            positions.Clear();




            first = true;
            k = 1;
  
            j = 0;
            orginalpos = transform.position;


            //readds the points needed to complete the constellation
            for (int w = 1; w < points.Length; w++)
            {
                positions.Add(points[w].transform.position);
                
            }
            //sets up the first one
            movers.Add(mover);
        }
    }
}
