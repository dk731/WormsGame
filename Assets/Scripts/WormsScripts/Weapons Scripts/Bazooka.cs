using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : MonoBehaviour
{


    private RotateWeapon weaponHolder;

    public float startProjectileSpeedMag = 3;
    public float maxProjectileTimeTravel = 3;
    public float maxTime;
    private float curShootTime;




    // Start is called before the first frame update
    void Start()
    {
        this.weaponHolder = this.transform.parent.GetComponent<RotateWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( this.weaponHolder.wormBodySC.fireMod )
        {
            if ( Input.GetKeyDown( "space" ) )
            {
                UnityEngine.Debug.Log( "Down" );
            }

            if ( Input.GetKeyUp( "space" ) )
            {
                UnityEngine.Debug.Log( "Up" );
            }
        }
    }
}
