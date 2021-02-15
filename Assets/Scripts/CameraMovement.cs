using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{

    public float zoomSpeed = 1;
    public float targetOrtho;
    public float smoothZoomSpeed = 2.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 5.0f;
    public float dragSpeed = 0.1f;
    public float focusSmoothSpeed = 0.1f;

    private Vector2 minPoint = new Vector2( 0, 0 );
    private Vector2 maxPoint = new Vector2( 0, 0 );

    private Vector3 prevMousePos;

    private TerrainSC terrainScript;

    private float targetPosX;
    private float targetPosY;

    public float movementSpeed = 0.03f;

    private float startOrtho;

    private Camera myCamera;

    public List<GameObject> wormsList;
    public GameObject focusedWorm;

    public float focusSize = 3;

    public bool followWorm = false;


    void Start()
    {
        myCamera = this.GetComponent<Camera>();
        this.targetOrtho = myCamera.orthographicSize;
        terrainScript = GameObject.Find( "Terrain" ).GetComponent<TerrainSC>();
        this.startOrtho = myCamera.orthographicSize;

        ////////
        this.focusedWorm = GameObject.Find( "Worm" ).gameObject;
        ////////

        //Cursor.lockState = CursorLockMode.Locked;

        GlobalVariables.NewRound();

    }

    public void mapGenerated()
    {
        float aspecRatio = Screen.width / ( float ) Screen.height;
        this.minPoint = new Vector2( -terrainScript.map_size / 2 - 4, terrainScript.map_size / aspecRatio + 2 );
        this.maxPoint = new Vector2( terrainScript.map_size / 2 + 4, -0.5f );
    }

    void Update()
    {
        float onMouseDrag = Input.GetAxis( "Fire2" );
        float scroll = Input.GetAxis( "Mouse ScrollWheel" );
        float horizontalKeys = Input.GetAxis( "Arrows Horizontal" );
        float verticalKeys = Input.GetAxis( "Arrows Vertical" );

        Vector3 posOffset = Vector3.zero;

        if ( scroll != 0.0f )
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp( targetOrtho, minOrtho, maxOrtho );
        }
        if ( horizontalKeys != 0.0f )
        {
            posOffset.x += horizontalKeys * movementSpeed;
        }
        if ( verticalKeys != 0.0f )
        {
            posOffset.y -= verticalKeys * movementSpeed;
        }
        if ( onMouseDrag != 0 )
        {
            Vector3 curMousePos = Input.mousePosition;
            if ( this.prevMousePos.x < 0 )
                this.prevMousePos = curMousePos;

            this.transform.position = this.transform.position - ( curMousePos - this.prevMousePos ) * this.dragSpeed * this.myCamera.orthographicSize / this.startOrtho;
            this.prevMousePos = curMousePos;
            this.followWorm = false;
        }
        else
            this.prevMousePos = new Vector2( -1000, -1000 );



        if ( Input.GetKeyUp( "f" ) )
        {
            if ( !this.followWorm )
                this.targetOrtho = this.focusSize;
            this.followWorm = !this.followWorm;
        }




        if ( followWorm )
        {
            Vector3 tmp = focusedWorm.transform.position;
            tmp.z = 0;
            tmp.x = Mathf.MoveTowards( this.transform.position.x, tmp.x, focusSmoothSpeed * Time.deltaTime );
            tmp.y = Mathf.MoveTowards( this.transform.position.y, tmp.y, focusSmoothSpeed * Time.deltaTime );
            this.transform.position = tmp;
        }

        if ( posOffset.magnitude > 0.001 )
            this.followWorm = false;

        this.transform.position += posOffset;




        myCamera.orthographicSize = Mathf.MoveTowards( Camera.main.orthographicSize, targetOrtho, smoothZoomSpeed * Time.deltaTime );

        float size = myCamera.orthographicSize * 2;
        float width = size * ( float ) Screen.width / Screen.height;

        Vector2 leftTop = new Vector2( this.transform.position.x - width / 2f, this.transform.position.y + size / 2f );
        Vector2 bottomRight = new Vector2( this.transform.position.x + width / 2f, this.transform.position.y - size / 2f );

        posOffset = Vector2.zero;



        if ( leftTop.x < this.minPoint.x )
            posOffset.x = this.minPoint.x - leftTop.x;

        else if ( bottomRight.x > this.maxPoint.x )
            posOffset.x = this.maxPoint.x - bottomRight.x;


        if ( leftTop.y > this.minPoint.y )
            posOffset.y = this.minPoint.y - leftTop.y;

        else if ( bottomRight.y < this.maxPoint.y )
            posOffset.y = this.maxPoint.y - bottomRight.y;

        this.transform.position += posOffset;

    }


}