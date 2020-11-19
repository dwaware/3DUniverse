using UnityEngine;


public class Constants
{
    // ######################################################################################
    // gconAUMSYR = gravitational constant in AU, solar masses and years
    public static float gconAUMSYR = 39.5f;


    // ######################################################################################
    /*
    minimum stars per system is 1
    increasing this above 1 may still result in less stars if
    a reasonable hierarchy requires multiple attempts to find
    */
    public static int min_stars = 2;  // let's make it interesting


    // ######################################################################################
    /*
    maximum number of stars
    all observed systems to date have  1-7 stars
    allow for very small chances above 7 up to and including 16
    */
    public static int max_stars = 16;  // eventually reduce this to 8?


    // ######################################################################################
    /*
    maximum hierarchy level for mult-star systems
    systems should be somewhat balanced, i.e.

    *       *-*
    +--------+
    *        *
    
    In this example the 5 star system hierarchy is 3.
    The node hierarchies are:

    0 main branch
    1 secondary branch
    1 another secondary branch
    2 one tertiary branch

    Set the maxiumum hierarchical level to log2(num_stars) rounded up
    ceil(log(16,2)) = 4

    Adding 1 for good measure:  4 + 1 = 5
    
    What we are saying here is that for the purposes of our application, if a large maximum value is set
    for the number of stars per system (i.e. n=16) we are going to cap the hierarchy at a reasonable value
    even though it could theoretically be as high as n-1

    Note: In nature, n=7 is the largest value found to date: AR Cassiopeiae and Nu Scorpii.  The maximum hierarchy is 4 found to date is the Gliese system, with n=5.
    
    Based on our rules:
    * more than 4 stars is unlikely (3% chance)
    * more than 8 stars is extemeley unlikely
    * more than 16 stars per system is prohibited

    For a system hierarchy of 5, node hierarchies will contain a:

    0 main branch

    as well as one or more:

    1 secondary branch(es)
    2 tertiary branches(es)
    3 etc
    4 etc as per the mobile diagram classification system https://en.wikipedia.org/wiki/Star_system#Higher_multiplicities
    */

    public static int max_sys_hierarchy = Mathf.CeilToInt(Mathf.Log(max_stars, 2)) + 1;
}



// Alpha Centauri A-B (application will work with real-world examples!)
/*
a = 23.41f;
mP = 1.1f;
mS = 0.907f;
e = 0.5179f;
*/

// Alpha Centauri AB-C
/*
a = 8616;
mP = 1.1f + 0.907f;
mS = 0.123f;
e = 0.5f;
*/