using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float waterLevel = 0;
    public static float windDir = 0;
    public static float maxWind = 2;

    public static float Map( float OldValue, float OldMin, float OldMax, float NewMin, float NewMax )
    {

        float OldRange = ( OldMax - OldMin );
        float NewRange = ( NewMax - NewMin );
        float NewValue = ( ( ( OldValue - OldMin ) * NewRange ) / OldRange ) + NewMin;

        return ( NewValue );
    }

    public static void NewRound()
    {
        GlobalVariables.windDir = Random.Range( -maxWind, maxWind );
        GlobalVariables.waterLevel += 0.1f;
    }

}
