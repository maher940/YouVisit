using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Move : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
	private GameObject speech;

    /// <summary>
    /// Speed at which planet flys toward player
    /// </summary>
    private float speed;
    /// <summary>
    /// how fast the planet spins
    /// </summary>
	private float rot_speed;
    /// <summary>
    /// the posiition in which to view the planet, where the planet goes to when clicked
    /// </summary>
    [SerializeField]
    private GameObject viewpos;
    /// <summary>
    /// reference to the revole script
    /// </summary>
    [SerializeField]
	public revolve revolve;
    /// <summary>
    /// orginal postion of the planet
    /// </summary>
    [SerializeField]
    private GameObject orginalpos;
    /// <summary>
    /// origin of planet
    /// </summary>
	private Vector3 origin = Vector3.zero;
    /// <summary>
    /// reference to main camera
    /// </summary>
    [SerializeField]
    private GameObject camera_main;
    /// <summary>
    /// if the planet needs to move
    /// </summary>
	private bool needs_to_move = false;
    /// <summary>
    /// timer to keep track if planet is currently moving
    /// </summary>
	float moving = 1.0f;
    /// <summary>
    /// bool for when the planet is clicked
    /// </summary>
    public bool clicked;

    /// <summary>
    /// used to make sure only one planet can be clicked
    /// </summary>
    static public GameObject planet;


  
    /// <summary>
    /// reference to a move_script
    /// </summary>
    Planet_Move move_script;

    /// <summary>
    /// reference to where the planet will be placed when blackhole is present
    /// </summary>
    public Transform blackhole;
    /// <summary>
    /// boolean to control the moving of the planet to blackhole
    /// </summary>
    private bool moveToBH;

    void Awake () {
        clicked = false;
        speed = 300;
		rot_speed = revolve.speed;
		origin = orginalpos.transform.position;

        //link up the reference of the 'blackhole' position
        blackhole = GameObject.Find("Sun_2").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

		//moves the planet to the view position
		if (clicked == true) {
			
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, viewpos.transform.position, step);
			moving = 1.0f;
			needs_to_move = true;

		}
        //moves the planet to its orginal position
		if (clicked == false && needs_to_move == true) {
			float step = speed * Time.deltaTime;

			revolve.speed = rot_speed;
			origin = orginalpos.transform.position;
			transform.position = Vector3.MoveTowards (transform.position, origin , step);
			moving -= 1.0f * Time.deltaTime;		
			if (moving <= 0.0f) {
				needs_to_move = false;
			}
		}
        //moves the planet to the blackhole
        if(moveToBH)
        {
            clicked = false;
            movePlanetToBlackhole();
        }

	}
    /// <summary>
    /// sets planet origin to its position
    /// </summary>
    public void SetOrigin()
    {
        
        origin = gameObject.transform.position;
  

    }


    /// <summary>
    /// moves the planet to its view positon
    /// </summary>
    public void Move()
    {
        
        if (clicked == false)
        {
			speech.SetActive (false);
            //if the planet variable is null it sets the planet that was clicked to it and sets clicked to true which will trigger the if statement in the update function
            if(planet == null)
            {
                clicked = true;
              

                viewpos.transform.parent = null;

                planet = transform.gameObject;

                return;
            }
            //if the planet that was clicked does not equal the planet variable it will send the gameobject in the planet varibale to its orginal postion
            //and then this gameobject will become the planet variable to get moved to the view position
            if (planet != transform.gameObject)
            {
               


                move_script = planet.GetComponent<Planet_Move>();

                
                move_script.Move();

                clicked = true;
                

                viewpos.transform.parent = null;

               

                planet = transform.gameObject;

                return;

                

            }
            
           
        }
        //if the planet was already clicked it will send it back to orbit
        else if(clicked == true)
        {

            clicked = false;
           

            viewpos.transform.parent = camera_main.transform;

            planet = null;

            return;
        }

    }

    /// <summary>
    /// Moves planets into the black hole
    /// </summary>
    public void movePlanetToBlackhole()
    {
        moveToBH = true;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(this.transform.position, blackhole.position, step);
        moving = 1.0f;
    }

}
