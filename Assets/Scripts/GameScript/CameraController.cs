using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float zoomInValue = 15;
	public float zoomOutValue = 30;
	public float zoomSpeed = 20;
	public Vector3 centerCam;
	public Vector2 zoomInMinPos = new Vector2 (2.5f, -12.5f);
	public Vector2 zoomInMaxPos = new Vector2 (9.5f, 10f);
    public Vector2 battleCameraPos = new Vector2(5.5f, 6);
    public float battleCameraZoom = 4f;
	public bool zoomedIn = false;
    public int shakeDuration;

    [HideInInspector]
    public bool battleCamera = false;

	private Transform player;
    private GameObject checkPlayer;

    private bool zooming = false;
	private TurnsController turnsControl;

	// Initialization
	void Start () {
		//Find an active player
		turnsControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnsController>();
	}
	
	void Update () {
        if (!battleCamera && turnsControl.thisPlayerTurn != null)
        {
            //Assign player's position
            player = turnsControl.thisPlayerTurn.transform;

            //Change the zoom when key is pressed and not while zooming
            if (Input.GetKeyDown(KeyCode.Z) && !zooming)
            {
                zoomedIn = !zoomedIn;
                zooming = true;

                //Zoom in to player's position
                if (zoomedIn)
                {
                    StartCoroutine(CameraZoom(zoomInValue, PlayerViewPos()));
                }

                //Zoom out to the center of the board
                else if (!zoomedIn)
                {
                    StartCoroutine(CameraZoom(zoomOutValue, centerCam));
                }
            }
        }
	}

	void LateUpdate () {
		//Keep following the player while zoomed and not in battle mode
		if (zoomedIn && !zooming && !battleCamera) {
			transform.position = PlayerViewPos();
		}
	}

	//Camera's zoom smooth movement
	IEnumerator CameraZoom (float to, Vector3 targetPos){
		Vector3 startPos = transform.position;
		float from = GetComponent<Camera>().orthographicSize;
		int i = 0;
		while (GetComponent<Camera>().orthographicSize != to || transform.position != targetPos) {
			GetComponent<Camera>().orthographicSize = Mathf.Lerp(from, to, i/zoomSpeed);
			transform.position = Vector3.Lerp(startPos, targetPos, i/zoomSpeed);
			i++;
			yield return null;
		}
		zooming = false;
	}

	//Determine ZoomIn's camera position
	Vector3 PlayerViewPos (){
		float viewPosX = Mathf.Clamp(player.transform.position.x, zoomInMinPos.x, zoomInMaxPos.x);
		float viewPosY = Mathf.Clamp(player.transform.position.y, zoomInMinPos.y, zoomInMaxPos.y);
		Vector3 playerViewPos = new Vector3 (viewPosX, viewPosY, centerCam.z);
		return playerViewPos;
	}

    public IEnumerator BattleView()
    {
        //Deactivate camera control
        battleCamera = true;

        //Save camera original value
        Vector3 originPos = transform.position;
        float originSize = GetComponent<Camera>().orthographicSize;

        //Change camera value
        transform.position = new Vector3 (battleCameraPos.x, battleCameraPos.y, transform.position.z);
        GetComponent<Camera>().orthographicSize = battleCameraZoom;

        //Prevent the code to continue until battleCamera switched off
        while (battleCamera)
            yield return null;

        //Reset camera value
        transform.position = originPos;
        GetComponent<Camera>().orthographicSize = originSize;
    }

    public IEnumerator ShakeScreen()
    {
        //Initialization
        Vector3 originalPos = transform.localPosition;
        int temp = shakeDuration;

        //Shake the screen while on duration
        while (temp > 0)
        {
            Vector2 shakePos = new Vector2(transform.localPosition.x, transform.localPosition.y) + Random.insideUnitCircle * (temp / (temp*2));
            transform.localPosition = new Vector3(shakePos.x, shakePos.y, transform.localPosition.z);
            yield return null;
            temp -= 1;
        }

        //Reset camera's position
        transform.localPosition = originalPos;
    }
}