using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    // Start is called before the first frame update



    public float rotationSpeed = 1;

    public WormBody wormBodySC;

    void Start()
    {
        this.wormBodySC = this.transform.parent.gameObject.GetComponent<WormBody>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis( "Vertical" );
        if ( verticalInput != 0 && this.wormBodySC.fireMod )
        {
            Vector3 newRotation = this.transform.rotation.eulerAngles + new Vector3( 0, 0, 1 ) * verticalInput * this.rotationSpeed;

            UnityEngine.Debug.Log( newRotation );

            if ( this.wormBodySC.transform.localScale.x < 0 )
            {
                if ( newRotation.z > 180f )
                {
                    if ( newRotation.z < 270f )
                        newRotation.z = 180f;
                    else
                        newRotation.z = 0f;
                }
            }
            else
            {
                if ( newRotation.z < 360 && newRotation.z > 180 )
                {
                    if ( newRotation.z < 270f )
                        newRotation.z = 180f;
                    else
                        newRotation.z = 0f;
                }
            }


            this.transform.rotation = Quaternion.Euler( newRotation );
        }


    }
}
