using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class SlimeAI : MonoBehaviour {
    private CharacterController2D myController;
    private CharacterController2D playerController;
    private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private bool fixLanding = false;

    float horizontalMove = 0f;
    int direction = 0;
    bool jump = false;

    public float updateRate = 2f;

    private Seeker seeker;

    public Path path;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3f;
    private int currentWayPoint = 0;

    bool playerWasMoving = false;
    bool ignoreFirst = true;

    void Awake()
    {
        // Do not collide with player
        Physics2D.IgnoreLayerCollision(8, 9);
        // Do not collide with other slimes
        Physics2D.IgnoreLayerCollision(9, 9);
    }

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        myController = GetComponent<CharacterController2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Everytime the player stops moving, generate a new path to be followed
        if (playerWasMoving && !playerController.IsMoving())
        {
            StartCoroutine(UpdatePath());

            // Plays movement animation
            animator.SetBool("IsMoving", true);
        }

        // If there's a path available to be followed
        if (path != null)
        {
            // If there's a waypoint left to be followed
            if (currentWayPoint < path.vectorPath.Count)
            {
                pathIsEnded = false;

                // Distance on X axis to next waypoint
                float xDistance = Mathf.Abs(path.vectorPath[currentWayPoint].x - transform.position.x);

                if (xDistance > maxDistance)
                {
                    // Which direction should I go?
                    direction = path.vectorPath[currentWayPoint].x > transform.position.x ? 1 : -1;
                    // Sets horizontal move to correct direction and speed
                    horizontalMove = direction * moveSpeed;
                }

                // Actually moves
                myController.Move(horizontalMove * Time.fixedDeltaTime, jump);
                jump = false;

                if (Mathf.Abs(transform.position.x - path.vectorPath[currentWayPoint].x) < nextWaypointDistance)
                {
                    currentWayPoint++;

                    if (currentWayPoint + 1 < path.vectorPath.Count)
                    {
                        float yDifference = path.vectorPath[currentWayPoint].y - transform.position.y;

                        if (yDifference > 0.5f)
                        {
                            // Plays jumping animation
                            animator.SetBool("IsJumping", true);
                            
                            jump = true;
                            ignoreFirst = fixLanding;
                        }
                    }
                }
            }
            // Just reached the end of path
            else if (currentWayPoint == path.vectorPath.Count)
            {
                // Stops movement animation
                animator.SetBool("IsMoving", false);

                pathIsEnded = true;
                currentWayPoint++;
                StopAllCoroutines();
            }
        }
        
        playerWasMoving = playerController.IsMoving();
    }

    // Run everytime a new path is generated
    public void OnPathComplete (Path p)
    {
        // If there is no error with the path generated
        if(!p.error)
        {
            // Updates path
            path = p;

            // Reset currentWayPoint
            currentWayPoint = 0;
        }
    }

    // Keeps updating path based on updateRate
    IEnumerator UpdatePath ()
    {
        // Updates path
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        // Waits until it's time to run again
        yield return new WaitForSeconds(1 / updateRate);

        // Calls itself again
        StartCoroutine(UpdatePath());
    }

    // When clicked by player
    void OnMouseDown ()
    {
        // load a new scene
        Debug.Log("Ouch!");
    }

    public void OnLanding()
    {
        if (ignoreFirst)
            ignoreFirst = false;
        else
        {
            // Plays landing animation
            animator.SetBool("IsJumping", false);
        }
    }
}
