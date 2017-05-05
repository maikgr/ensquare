using UnityEngine;
using System.Collections;

public class MoveIntersection : MonoBehaviour {
	public Vector2 direction1, direction2, boxSize;
	public GameObject arrow;
    public Vector2 movementDirection;

	private Transform intersection;
	private bool created = false; //Check if the arrows has instantiated or not

    void Start()
    {
        movementDirection = Vector2.zero;
    }

    void Update()
    {
        if (created && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform != null && hit[i].collider.CompareTag("Direction Arrow"))
                {
                    movementDirection = new Vector2(hit[i].transform.localPosition.x, hit[i].transform.localPosition.y);
                }
            }
        }
    }

    //Destroy the forks when the player leaves
	void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Player"){
			foreach (Transform forks in intersection){
				GameObject.Destroy(forks.gameObject);
			}
			//Reset values
            movementDirection = Vector2.zero;
			created = false;
		}
	}

    public void InstantiateArrows(Vector2 direction)
    {
        //Set the new position
        Vector2 newPos = new Vector2(intersection.position.x, intersection.position.y) + direction;
        //Set the new rotation based on the given direction
        float newRotation = 0f;
        if (direction.y == 0)
        {
            newRotation = (1f - direction.x) * 90f;
        }
        else if (direction.x == 0)
        {
            newRotation = direction.y * 90f;
        }

        //Instantiate the direction arrow
        GameObject forkArrow = Instantiate(arrow, newPos, Quaternion.Euler(0f, 0f, newRotation)) as GameObject;

        //Set the object to the direction of the parent
        forkArrow.transform.SetParent(intersection);
    }

    public void CreateIntersection()
    {
        intersection = GetComponent<Transform>();
        InstantiateArrows(direction1);
        InstantiateArrows(direction2);
        created = true;
    }
}