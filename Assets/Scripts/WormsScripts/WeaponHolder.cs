using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{

    public List<string> allWeaponsNames;
    public List<GameObject> allWeaponsObjects;

    public GameObject focusedWeapon = null;

    private Dictionary<string, GameObject> weaponDict;

    private string currentWeapon;



    // Start is called before the first frame update
    void Start()
    {
        weaponDict = new Dictionary<string, GameObject>();
        if ( this.allWeaponsNames.Count != this.allWeaponsObjects.Count )
            throw new System.Exception( "Length of list of names doest not equals to length of object list" );

        for ( int i = 0; i < this.allWeaponsNames.Count; i++ )
        {
            this.weaponDict.Add( this.allWeaponsNames[i], this.allWeaponsObjects[i] );
        }

        this.allWeaponsNames.Clear();
        this.allWeaponsObjects.Clear();

        this.currentWeapon = "";

        this.getNewWeapon( "Hand" );


    }

    public void getNewWeapon( string weaponName )
    {

        if ( this.currentWeapon.Equals( weaponName ) )
            return;

        this.currentWeapon = weaponName;
        if ( this.focusedWeapon != null )
            Destroy( this.focusedWeapon );

        this.transform.rotation = Quaternion.Euler( 0, 0, 0 );

        this.focusedWeapon = Instantiate( this.weaponDict[this.currentWeapon] );

        this.focusedWeapon.transform.parent = this.transform;
        this.focusedWeapon.transform.localPosition = this.weaponDict[this.currentWeapon].transform.position;
        this.focusedWeapon.transform.localScale = this.weaponDict[weaponName].transform.localScale;
        this.focusedWeapon.transform.rotation = this.weaponDict[this.currentWeapon].transform.rotation;

    }


    // Update is called once per frame
    void Update()
    {



    }
}
