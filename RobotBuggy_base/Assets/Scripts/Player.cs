using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;      //Allows us to use SceneManager
using System.Collections.Generic;       //allows the use of lists
using UnityEngine.UI;

namespace Completed
{
    //Robot inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
    public class Player : MovingObject
    {
        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
        public int pointsPerFood = 10;              //Number of points to add to Robot food points when picking up a food object.
        public int pointsPerSoda = 20;              //Number of points to add to Robot food points when picking up a soda object.
        public int wallDamage = 1;                  //How much damage a Robot does to a wall when chopping it.
        public bool hasKey = false;

        public Text foodText;                       //UI Text to display current player food total.
		public Text keyText;

        public AudioClip moveSound1;                //1 of 2 Audio clips to play when player moves.
        public AudioClip moveSound2;                //2 of 2 Audio clips to play when player moves.
        public AudioClip gameOverSound;             //Audio clip to play when player dies.
        public AudioClip eatSound1;
        public AudioClip eatSound2;

        private int food;                           //Used to store player food points total during level.
        public bool isAlive = true;
        private Animator animator;                  //Used to store a reference to the Robot's animator component.
        private bool stuck = false;
        private List<Directions> directions = new List<Directions>();
        private Vector2 previosTile;


        protected override void Start()
        {
            GameManager.instance.AddRobotToList(this);
            animator = GetComponent<Animator>();
            //Set the foodText to reflect the current player food total.
            //foodText.text = "";
			//keyText.text = "";
            base.Start();
        }

		private void OnDisable()
        {
            //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
            GameManager.instance.playerFoodPoints = food;
        }

        public void MoveRobot()
        {
            
            FindDirections();
            //create a robot for each of the directions available
            float previousX = gameObject.transform.position.x;
            float previousY = gameObject.transform.position.y;
            Debug.Log(directions.Count);
            if (directions.Count == 0)
            {
                isAlive = false;
                Dead();
            }
            else
            {
                for (int i = 0; i < directions.Count; i++)
                {
                    //used to make things more clear
                    Directions current = directions[i];
                    //move the active robit to the first available direction
                    if (i == 0)
                    {
                        AttemptMove<Wall>(current.x, current.y);
                        //create a tile in the spot the robot was just in
                        GameObject toInstantiate =
                            BoardManager.Instance.visitedTiles
                                        [Random.Range(0, BoardManager.Instance.visitedTiles.Length)];
                        GameObject instance =
                            Instantiate(toInstantiate,
                                        new Vector3(gameObject.transform.position.x,
                                                    gameObject.transform.position.y, 0f),
                                        Quaternion.identity) as GameObject;

                    }
                    //create new robots for the other directions
                    else
                    {
                        Vector3 position = new Vector3(previousX + directions[i - 1].x,
                                                       previousY + directions[i - 1].y, 0f);
                        BoardManager.Instance.AddRobot(position);
                    }
                }
            }
            directions.Clear();
        }


        //AttemptMove overrides the AttemptMove function in the base class MovingObject
        //AttemptMove takes a generic parameter T which for Robot will be of the type Wall, it also takes integers for x and y direction to move in.
        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
            base.AttemptMove<T>(xDir, yDir);

            //Hit allows us to reference the result of the Linecast done in Move.
           RaycastHit2D hit;
          
           //If Move returns true, meaning Player was able to move into an empty space.
           if (Move (xDir, yDir, out hit)) 
           {
              //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
             SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
           }

            //Since the Robot has moved and lost food points, check if the game has ended.
            CheckIfGameOver();

            //Set the RobotsTurn boolean of GameManager to false now that Robots turn is over.
            GameManager.instance.playersTurn = false;
        }


        //OnCantMove overrides the abstract function OnCantMove in MovingObject.
        //It takes a generic parameter T which in the case of Robot is a Wall which the Robot can attack and destroy.
        protected override void OnCantMove<T>(T component)
        {
            if (!stuck)
            {
                animator.SetTrigger("explosion");
                stuck = true;
            }
            animator.SetTrigger("deadBuggy");
        }


        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.tag == "Exit" && hasKey)
            {

                SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", restartLevelDelay);

                //Disable the Robot object since level is over.
                enabled = false;
            }

            //Check if the tag of the trigger collided with is Food.
            else if (other.tag == "Food")
            {
                hasKey = true;
                //Update foodText to represent current total and notify player that they gained points
                //foodText.text = "+" + pointsPerFood + " Food: " + food;
                foodText.text = "You got the Key!";
               //Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
              SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
                //Disable the food object the Robot collided with.
                other.gameObject.SetActive(false);
            }

            if(other.tag == "Driven")
            {

            }
        }


        //Restart reloads the scene when called.
        private void Restart()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
            //and not load all the scene object in the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }


        //LoseFood is called when an enemy attacks the Robot.
        //It takes a parameter loss which specifies how many points to lose.
        public void Dead()
        {
            //Set the trigger for the player animator to transition to the playerHit animation.
            animator.SetTrigger("killed");

            //Check to see if game has ended.
            CheckIfGameOver();
        }


        //CheckIfGameOver checks if the Robot is out of food points and if so, ends the game.
        private void CheckIfGameOver()
        {
        }

        //find directions that the robot can move to and add them to an array.
        private void FindDirections()
        {
            //clear the directions array from last time

            RaycastHit2D hit;

            //check each direction for a free path. Add any free paths to the directions array

            //up
            if (Move(0, 1, out hit))
            {
                directions.Add(new Directions(0, 1));
            }
            //right
            if (Move(1, 0, out hit))
            {
                directions.Add(new Directions(1, 0));
            }
            //down
            if (Move(-1, 0, out hit))
            {
                directions.Add(new Directions(-1, 0));
            }
            //left
            if (Move(0, -1, out hit))
            {
                directions.Add(new Directions(0, -1));
            }
        }
    }
}
