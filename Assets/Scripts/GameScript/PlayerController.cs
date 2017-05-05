using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public int maxHealthPoint;
    public float moveTime = 20;
    public LayerMask intersectionLayer;

    [HideInInspector]
    public int lastMove, movement, attackBuff = 0, defenseBuff = 0, healthPoint, diceNumber;
    [HideInInspector]
    public bool warped, battled, cardEvent; //Check if the player has warped, battled, and drawn card
    [HideInInspector]
    public Vector2 startPos;
    [HideInInspector]
    public Animator animate;
    [HideInInspector]
    public bool moved = false; //Check if player has moved

    private Vector2 endPos;
    private MoveDirection move;
    private Vector2 floorPos;
    private TurnsController turnsControl;
    private float lerpTime = 0;

    //Triggers
    private bool haltMovement, intersection, dead;

    void Start()
    {
        //Initiatlization
        maxHealthPoint = GameObject.FindGameObjectWithTag("GameSetting").GetComponent<GameSettings>().maxHealth;
        healthPoint = maxHealthPoint;
        startPos = new Vector2(transform.position.x, transform.position.y);
        animate = GetComponentInChildren<Animator>();
        turnsControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnsController>();
    }

    void Update()
    {
        //Warp bug workaround
        if (GetComponentInChildren<Transform>().localScale == Vector3.zero)
            GetComponentInChildren<Transform>().localScale = new Vector3(0.75f, 0.75f, 1f);

        //When the player died
        if (healthPoint <= 0)
        {
            //Add the player to dead players list
            if (!dead)
            {
                bool registered = false;
                for (int i = 0; i < turnsControl.deadPlayers.Count; i++)
                {
                    if (this.gameObject == turnsControl.deadPlayers[i])
                        registered = true;
                }
                if (!registered)
                {
                    turnsControl.deadPlayers.Add(this.gameObject);
                    dead = true;
                }
            }

            if (GetComponentInChildren<SpriteRenderer>().color != new Color(0.098f, 0.098f, 0.098f))
            {
                //Temp: make the player's sprite covered with grey shade over time
                SpriteRenderer spriteColor = GetComponentInChildren<SpriteRenderer>();
                Color startColor = spriteColor.color;
                Color endColor = new Color(0.098f, 0.098f, 0.098f);
                if (lerpTime == 0)
                    lerpTime = Time.time + 2f; //Set the color gradual change to 2 seconds
                spriteColor.color = Color.Lerp(startColor, endColor, Time.time / lerpTime);
            }
        }
    }

    public IEnumerator MovePlayer(int playerMovement)
    {
        lastMove = playerMovement;
        for (int movementCount = playerMovement; movementCount > 0; movementCount--)
        {
            //Initialization
            Vector2 moveDirection = Vector2.zero;
            Vector3 endPos;

            //Set the dice number
            turnsControl.DiceNumberDisplay(movementCount);
            
            //Raycast to get the floor's script
            RaycastHit2D[] floor = Physics2D.RaycastAll(transform.position, Vector2.zero, 0f);

            for (int index = 0; index < floor.Length; index++)
            {
                //Immediately get the direction if it's a floor
                if (floor[index].collider.CompareTag("Floor Tile"))
                {
                    moveDirection = floor[index].collider.GetComponent<MoveDirection>().movementDirection;
                    break;
                }

                //Wait until movement direction get assigned if it's an intersection
                else if (floor[index].collider.CompareTag("Intersection Tile"))
                {
                    floor[index].collider.GetComponent<MoveIntersection>().CreateIntersection();
                    while (floor[index].collider.GetComponent<MoveIntersection>().movementDirection == Vector2.zero)
                        yield return null;
                    moveDirection = floor[index].collider.GetComponent<MoveIntersection>().movementDirection;
                    break;
                }
            }

            //Set the player's end position
            if (moveDirection == Vector2.zero)
                Debug.Log("Cannot find movement direction");
            else
            {
                endPos = transform.position + new Vector3(moveDirection.x, moveDirection.y, 0f);

                //Move the player's pawn smoothly
                float remainingDist = Vector3.Distance(transform.position, endPos);
                float i = 0;
                while (remainingDist > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, endPos, i / 20);
                    remainingDist = Vector3.Distance(transform.position, endPos);
                    i++;
                    yield return null;
                }
            }
        }
        //End the player's movement;
        playerMovement = 0;

        //Destroy the dice
        turnsControl.DiceEnd();

        //Check for event tile when the player's movement has ended
        if (!CheckEvent())
            GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>().CheckOtherPlayer(gameObject);
    }

    bool CheckEvent()
    {
        //Initialize
        EventManager eventScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();

        //Raycast to check if there's any event tile
        RaycastHit2D[] checkEvents = Physics2D.RaycastAll(transform.position, Vector2.zero);

        for (int i = 0; i < checkEvents.Length; i++)
        {
            if (checkEvents[i].collider.CompareTag("Warp Tile"))
            {
                StartCoroutine(eventScript.WarpPlayer(gameObject));
                return true;
            }
            else if (checkEvents[i].collider.CompareTag("Battle Tile"))
            {
                GameObject battleTarget = eventScript.FindRandomEnemy(gameObject);
                eventScript.SetBattleground(gameObject, battleTarget);
                return true;
            }
            else if (checkEvents[i].collider.CompareTag("Card Tile"))
            {
                StartCoroutine(eventScript.DrawCard(gameObject));
                return true;
            }
        }
        return false;
    }

    public void EndPlayerTurn()
    {
        turnsControl.endPhase = true;
        moved = false;
    }
}