using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntExtension 
{
    /** @return the angle in degrees of this vector (point) relative to the x-axis. Angles are towards the positive y-axis (typically
    *         counter-clockwise) and between 0 and 360. */

    public static string FormatToLetterNotation(this int value)
    {
        int   magnitude = 0;
        int   divider   = 1;
        float newValue  = value;
        while ((float)value / (float)(divider) > 1)
        {
            divider *= 10;
            magnitude++;
        }
        if (magnitude <= 3) return newValue.ToString();

        switch (magnitude % 3)
        {
            case 1:
                newValue = Mathf.FloorToInt(value /(float) divider*10 * 100f) / 100f;
                // 1.23 over 1000
                break;
            case 2:
                newValue = Mathf.FloorToInt(value / (float)divider*100 * 10f) / 10f;
                //12.3 over 1000
                break;

            case 0:
                newValue = Mathf.FloorToInt(value / (float)divider*1000 * 1f) / 1f;
                //123 over 1000
                break;

            default:
                break;

        }

        /*	
         *	
         *	example:
         *	1
                12
                132
                1.23K 
                12.2K
                123K
                1.12M*/

        //choose letter to put there..
        // k,m,b,t,q

        string magnitudeLetter = "";
        switch ((magnitude-1) / 3)
        {
            case 1:
                magnitudeLetter = "k";
                break;
            case 2:
                magnitudeLetter = "m";
                break;
            case 3:
                magnitudeLetter = "b";
                break;
            case 4:
                magnitudeLetter = "t";
                break;
            case 5:
                magnitudeLetter = "q";
                break;
        }
        return newValue.ToString() + magnitudeLetter;

    }
}
