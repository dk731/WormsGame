using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WormBody : MonoBehaviour
{

    public float speed = 1f;

    public float minNonKinematicSpeed = 1f;

    private Rigidbody2D rb;

    private bool needToStop = false;
    private bool needToStop1 = false;

    public bool onGround = false;

    public float onMoveUpperMovement = 0.01f;

    public Vector3 myLocalScale = new Vector3( 0.17f, 0.17f, 1f );

    private WeaponHolder weaponHolder;

    private float timeBeforeJump = 0;

    public bool fireMod = false;



    // Start is called before the first frame update
    void Start()
    {
        this.weaponHolder = transform.GetChild( 0 ).gameObject.GetComponent<WeaponHolder>();
        this.rb = this.GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame


    private void OnCollisionEnter2D( Collision2D collision )
    {
        if ( this.rb.velocity.magnitude <= this.minNonKinematicSpeed )
        {
            this.needToStop = true;
            this.onGround = true;
        }
    }

    private void OnCollisionStay2D( Collision2D collision )
    {
        if ( this.rb.velocity.magnitude <= this.minNonKinematicSpeed )
        {
            this.needToStop = true;
            this.onGround = true;
        }
    }

    private void OnCollisionExit2D( Collision2D collision )
    {
        if ( !needToStop1 )
            this.onGround = false;
    }

    private void Update()
    {

        float inputHorizontal = Input.GetAxis( "Horizontal" );
        float isJumping = Input.GetAxis( "Jump" );

        if ( Input.GetKeyUp( "t" ) )
        {
            this.fireMod = !this.fireMod;
            if ( !this.fireMod )
            {
                if ( this.transform.localScale.x < 0 )
                    this.weaponHolder.transform.rotation = Quaternion.Euler( 0, 0, 90 );
                else
                    this.weaponHolder.transform.rotation = Quaternion.Euler( 0, 0, 270 );
            }

        }




        if ( inputHorizontal != 0 && this.onGround && !this.fireMod )
        {
            if ( inputHorizontal < 0 )
            {
                this.myLocalScale.x = -Math.Abs( this.myLocalScale.x );
                this.transform.localScale = this.myLocalScale;
            }
            else
            {
                this.myLocalScale.x = Math.Abs( this.myLocalScale.x );
                this.transform.localScale = this.myLocalScale;
            }

            this.rb.bodyType = RigidbodyType2D.Dynamic;
            this.rb.velocity = new Vector2( inputHorizontal * this.speed, this.rb.velocity.y );
            this.needToStop = false;
            this.needToStop1 = false;

            if ( isJumping != 0 && this.timeBeforeJump <= 0 )
            {
                Vector2 tmp = inputHorizontal * new Vector2( 1.5f, 2f );
                tmp.y = Math.Abs( tmp.y );
                rb.velocity = tmp;
                this.timeBeforeJump = 3;
                this.onGround = false;
            }
        }
        else if ( this.onGround && !this.fireMod )
        {
            if ( isJumping != 0 && this.timeBeforeJump <= 0 )
            {
                this.rb.bodyType = RigidbodyType2D.Dynamic;
                this.needToStop = false;
                this.needToStop1 = false;
                Vector2 tmp = new Vector2( 0.3f, 4 );
                if ( this.myLocalScale.x > 0 )
                    tmp.x *= -1;
                rb.velocity = tmp;
                this.timeBeforeJump = 3;
                this.onGround = false;
            }

        }
        if ( Input.GetKey( "1" ) )
        {
            this.weaponHolder.getNewWeapon( "Hand" );
        }
        else if ( Input.GetKey( "2" ) )
        {
            this.weaponHolder.getNewWeapon( "Bazooka" );
        }

        this.timeBeforeJump -= Time.deltaTime;
    }

    void onDeath()
    {
        Destroy( this );
    }

    private void FixedUpdate()
    {

        if ( this.transform.position.y <= GlobalVariables.waterLevel )
            this.onDeath();


        if ( needToStop1 )
        {
            this.rb.bodyType = RigidbodyType2D.Kinematic;
            this.rb.velocity = Vector2.zero;

        }
        if ( needToStop )
        {
            needToStop1 = true;
            needToStop = false;
        }

    }

}
