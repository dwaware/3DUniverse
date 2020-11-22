using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.


public class OrbitalMechanics
{
    public static float GetOrbitalPeriod (float a, float mP, float mS)
    {
        /*
        a = semi major axis relative to the barycenter (BC)
        mP = mass of primary star
        mS = mass of secondary star
        */

        float T = Mathf.Sqrt(((a * a * a * 4 * Mathf.PI * Mathf.PI) / (Constants.gconAUMSYR * (mP + mS))));
        return T;
    }

    public static float[] GetOrbitalElements(float a, float mP, float mS, float e)
    {
        /*
        a = "average distance" between the two bodies orbiting the barycenter (BC)
        mP = mass of primary star
        mS = mass of secondary star
        e = eccentricity
        */

        // orbital elements array
        float[] oe;
        oe = new float[11];

        Debug.Assert(mP >= mS);                     //    mass of primary must be >= mass of secondary so q > 0 && q <= 1

        float q = mS / mP;                          //  0 mass ratio of secondary (smaller) body and the primary (larger) body
        float eXOV = 1 - 2 * (q / (q + 1));         //  1 e at which the orbits will touch; any greater and their orbits will cross
        float smaP = a * (mS / (mP + mS));          //  2 semi major axis for primary body
        float minP = smaP * (1 - e);                //  3 mininum from primary body to BC
        float maxP = smaP * (1 + e);                //  4 maximum from primary body to BC
        float smaS = a * (mP / (mP + mS));          //  5 semi major axis for secondary body
        float minS = smaS * (1 - e);                //  6 minimum from secondary to BC
        float maxS = smaS * (1 + e);                //  7 maximum from secondary to BC
        float minT = a * (1 - e);                   //  8 smallest distance between both bodies
        float maxT = a * (1 + e);                   //  9 largest distance between both bodies
        float orbP = GetOrbitalPeriod(a, mP, mS);   // 10 orbital period T

        oe[0] = q;
        oe[1] = eXOV;
        oe[2] = smaP;
        oe[3] = minP;
        oe[4] = maxP;
        oe[5] = smaS;
        oe[6] = minS;
        oe[7] = maxS;
        oe[8] = minT;
        oe[9] = maxT;
        oe[10] = orbP;

        return oe;
    }

    public static void PrintOrbitalElements(float[] oe)
    {
        Debug.Log("ORBITAL ELEMENTS ###");
        Debug.Log("q:     " + oe[0]);
        Debug.Log("eXOV:  " + oe[1]);
        Debug.Log("smaP:  " + oe[2]);
        Debug.Log("minP:  " + oe[3]);
        Debug.Log("maxP:  " + oe[4]);
        Debug.Log("smaS:  " + oe[5]);
        Debug.Log("minS:  " + oe[6]);
        Debug.Log("maxS:  " + oe[7]);
        Debug.Log("minT:  " + oe[8]);
        Debug.Log("maxT:  " + oe[9]);
        Debug.Log("orbP:  " + oe[10]);
    }

    public static void PrintOrbitalPeriod(float[] oe)
    {
        Debug.Log("ORBITAL PERIOD ###");
        Debug.Log("orbP:  " + oe[10]);
    }

    public static float GetE(float percent, float e, int decpre)
    {
        /*
        percent = percent of complete orbit
        e = eccentricity
        decpre = precision of E in terms of the number of decimal places 
        */

        // All anomalies (angles) are in radians!

        float pi = Mathf.PI;                    // pi
        float M = 2.0f * pi * percent;          // mean anomaly M
        float E;                                // true anomaly F
        float F;                                // eccentric anomaly E
        int maxIter = 30, i = 0;                // max iterations
        float delta = Mathf.Pow(10, -decpre);   // delta condition for F as a function of decimal precision

        if (e < 0.8f) { E = M; }
        else { E = pi; }
        F = E - e * Mathf.Sin(M) - M;
        while ((Mathf.Abs(F) > delta) && (i < maxIter))
        {
            E = E - F / (1.0f - e * Mathf.Cos(E));
            F = E - e * Mathf.Sin(E) - M;
            i+=1;
        }
        E = Mathf.Round(E * Mathf.Pow(10, decpre)) / Mathf.Pow(10, decpre);
        return E;
    }

    public static Vector3 GetPosition(float percent, float sma, float e, float scaleFactor, bool drawRight)
    {
        /*
        percent = percent of complete orbit
        sma = semimajor axis for the star as it orbits the BC
        e = eccentricity
        */

        float M = percent * 2 * Mathf.PI;                                   // M: the circular equivalent angle for that percent orbit
        float E = GetE(percent, e, 3);                                      // E: as a function of M and e, expressed in radians as obtained from the solver
        int LvsRoffset = 1;
        if (drawRight == true) { LvsRoffset = -1; }
        float rx = LvsRoffset * scaleFactor * sma * (Mathf.Cos(E) - e);     // x component of radius vector measured from barycenter to current position
                                                                            // z component of radius vector measured from barycenter to current position
        float rz = -1 * scaleFactor * sma * Mathf.Sin(E) * Mathf.Sqrt(1 - Mathf.Pow(e, 2));
        //float rP = Mathf.Sqrt(rx * rx + rz * rz);                         // magnitude of radius vector

        return new Vector3(rx, 0, rz);
    }
}