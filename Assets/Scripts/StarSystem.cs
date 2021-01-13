using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.
using System.Linq;

public class StarSystem
{
    public static int GetNumberOfStars()
    {
        int num_stars = 1;
        // start with one, even if min is > 1

        int min_stars = Constants.min_stars;
        int max_stars = Constants.max_stars;
        Debug.Assert(min_stars <= max_stars);

        float rng = 100 * Random.Range(0f, 1f);
        /*
        i.e. if rng = 99.1;
        this is inside the 2 stars bracket as per the "float cumulative" below
        */

        // SSP is the Star System Probability Table
        List<KeyValuePair<int, float>> SSP = new List<KeyValuePair<int, float>>()
        {
            new KeyValuePair<int, float>(1, 79.340000103f),
            new KeyValuePair<int, float>(2, 17.33f),
            new KeyValuePair<int, float>(3,  2.63f),
            new KeyValuePair<int, float>(4,  0.61f),
            // for 1-4 stars, the percentage is based on emperical(such as it is) data
            // for 5-16 stars, a pow(n,0.5) ratio is used, dropping the % as n increases
            new KeyValuePair<int, float>(5,  0.0450109375f),
            new KeyValuePair<int, float>(6,  0.02250546875f),
            new KeyValuePair<int, float>(7,  0.011252734375f),
            new KeyValuePair<int, float>(8,  0.0056263671875f),
            new KeyValuePair<int, float>(9,  0.00281318359375f),
            new KeyValuePair<int, float>(10, 0.001406591796875f),
            new KeyValuePair<int, float>(11, 0.0007032958984375f),
            new KeyValuePair<int, float>(12, 0.00035164794921875f),
            new KeyValuePair<int, float>(13, 0.000175823974609375f),
            new KeyValuePair<int, float>(14, 8.79119873046875E-05f),
            new KeyValuePair<int, float>(15, 4.39559936523438E-05f),
            new KeyValuePair<int, float>(16, 2.19779968261719E-05f),

            /*
            // for greater chance of higher multiplicities, use this table
            new KeyValuePair<int, float>(5,  12.5f),
            new KeyValuePair<int, float>(7,  12.5f),
            new KeyValuePair<int, float>(9,  12.5f),
            new KeyValuePair<int, float>(11, 12.5f),
            new KeyValuePair<int, float>(13, 12.5f),
            new KeyValuePair<int, float>(14, 12.5f),
            new KeyValuePair<int, float>(15, 12.5f),
            new KeyValuePair<int, float>(16, 12.5f),
            */
        };

        float cumulative = 0f;
        for (int i = 0; i < SSP.Count; i++)
        {
            cumulative += SSP[i].Value;
            if (rng < cumulative)
            {
                num_stars = SSP[i].Key;
                break;
            }
        }
        //Debug.Log(rng + " " + num_stars);

        if (num_stars < min_stars)
        {
            num_stars = min_stars;
        }
        if (num_stars > max_stars)
        {
            num_stars = max_stars;
        }
        return num_stars;
    }

    public static float[] GetNodeEccentricities(int[] h1)
    {
        int nodes = h1.Length;
        float[] es = new float[nodes];
        for (int n = 0; n < nodes; n++)
        {
            es[n] = Random.Range(0f, 0.4f) + Random.Range(0f, 0.4f) / (n + 1);
        }
        return es;
    }

    public static float[] GetNodeDistances(int[][]h)
    {
        //have to assume something about the maximum system hierarchy here
        Debug.Assert(Constants.max_sys_hierarchy <= 5);
        // with a system hierarchy of 5, node
        // hierarchies will be anywhere from 0-4

        int nodes = h[1].Length;
        float[] distances = new float[nodes];
        int sys_h = h[0][0];
        int rng;

        for (int n = 0; n < nodes; n++)
        {
            if (sys_h == 1)
            {
                rng = Random.Range(0, 100);
                distances[0] = 800f;
                if (rng < 10)
                {
                    distances[0] = 500f;
                }
            }
            if (sys_h == 2)
            {
                if (h[1][n] == 0) { distances[n] = 1000f; }
                if (h[1][n] == 1) { distances[n] = 100f; }
            }
            if (sys_h == 3)
            {
                if (h[1][n] == 0) { distances[n] = 5000f; }
                if (h[1][n] == 1) { distances[n] = 500f; }
                if (h[1][n] == 2) { distances[n] = 50f; }
            }
            if (sys_h == 4)
            {
                if (h[1][n] == 0) { distances[n] = 12500f; }
                if (h[1][n] == 1) { distances[n] = 2500f; }
                if (h[1][n] == 2) { distances[n] = 500f; }
                if (h[1][n] == 3) { distances[n] = 50f; }
            }
            if (sys_h == 5)
            {
                if (h[1][n] == 0) { distances[n] = 25000f; }
                if (h[1][n] == 1) { distances[n] = 2500f; }
                if (h[1][n] == 2) { distances[n] = 500f; }
                if (h[1][n] == 3) { distances[n] = 50f; }
                if (h[1][n] == 4) { distances[n] = 5f; }
            }
            //Debug.Log("Node "+n+":   "+distances[n]);
        }

        for (int n = 0; n < nodes; n++)
        {
            distances[n] = Mathf.Floor(Random.Range(0.8f, 1.2f)*distances[n]);
        }
        return distances;
    }

    public static List<Vector3> GetStarCoords(List<Vector2Int> tree, float[] distances)
    {
        int nodes = tree.Count;
        List<Vector3> coords = new List<Vector3>();
        for (int n = 0; n < nodes; n++)
        {
            coords.Add(new Vector3(0f, 0f, 0f));
        }

        int[] orient = new int[nodes];
        for (int i = 0; i < nodes; i++)
        {
            orient[i] = -1;
        }
        for (int i = 0; i < nodes; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int n = tree[i][j];
                if (n > 0)
                {
                    float cx;
                    float cz;
                    int p = SysHierarchy.GetParentIndex(tree, n);
                    float px = coords[p].x;
                    float pz = coords[p].y;

                    if (orient[p] == -1)
                    {
                        orient[n] = 1;
                    }
                    if (orient[p] == 1)
                    {
                        orient[n] = -1;
                    }
                    if (j == 0)
                    {
                        if (orient[n] == -1)
                        {
                            cx = px;
                            cz = pz - distances[p] / 2;
                            //Debug.Log("down");
                        }
                        else
                        {
                            cx = px - distances[p] / 2;
                            cz = pz;
                            //Debug.Log("left");
                        }
                    }
                    else
                    {
                        if (orient[n] == -1)
                        {
                            cx = px;
                            cz = pz + distances[p] / 2;
                            //Debug.Log("up");
                        }
                        else
                        {
                            cx = px + distances[p] / 2;
                            cz = pz;
                            //Debug.Log("right");
                        }
                    }
                    coords[n] = new Vector2(cx, cz);
                }
            }
        }
        return coords;
    }

    public static string GenerateRandomSystemName()
    {
        string system_name;
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int index = Random.Range(0, chars.Length);
        char letter = chars[index];
        system_name = "STC" + "-" + Random.Range(99999, 999999) + "-" + letter;
        return system_name;
    }

    public static float[] GetContributingStars(List<int>[,] all_children, int node, StarData[] stars)
    {
        Debug.Assert(node >= 0);
        Debug.Assert(node < stars.Length);

        //List<int>[,] all_children = new List<int>[nodes, 2];
        //all_children[i, j][v] = Mathf.Abs(all_children[i, j][v]) - 1;

        List<int> ch_left = all_children[node, 0];
        List<int> ch_right = all_children[node, 1];

        float[] mS = new float[4];
        mS[0] = 0f;
        mS[1] = 0f;
        mS[2] = -1f;
        mS[3] = -1f;

        for (int i = 0; i < ch_left.Count; i++)
        {
            mS[0] += stars[ch_left[i]].mass;
        }
        for (int i = 0; i < ch_right.Count; i++)
        {
            mS[1] += stars[ch_right[i]].mass;
        }
        if (ch_left.Count == 1)
        {
            mS[2] = ch_left[0];
        }
        if (ch_right.Count == 1)
        {
            mS[3] = ch_right[0];
        }
        return mS;
    }

    public static string GetRandomLuminosity()
    {
        string lum_string = "";

        float rng = 100 * Random.Range(0f, 1f);

        // LCP is the Luminosity Class Probability Table
        List<KeyValuePair<string, float>> LCP = new List<KeyValuePair<string, float>>()
        {
            new KeyValuePair<string, float>("Ia0",  0.001f),
            new KeyValuePair<string, float>("Ia",   0.004f),
            new KeyValuePair<string, float>("Ib",   0.015f),
            new KeyValuePair<string, float>("II",   0.08f),
            new KeyValuePair<string, float>("III",  0.35f),
            new KeyValuePair<string, float>("IV",   0.55f),
            new KeyValuePair<string, float>("V",   92.50f),
            new KeyValuePair<string, float>("VI",   0.5f),
            new KeyValuePair<string, float>("DA",   1f),
            new KeyValuePair<string, float>("DB",   1f),
            new KeyValuePair<string, float>("DC",   1f),
            new KeyValuePair<string, float>("DO",   1f),
            new KeyValuePair<string, float>("DQ",   1f),
            new KeyValuePair<string, float>("DZ",   1f)
        };

            /*
            0.00001,    # Ia0
            0.00004,    # Ia
            0.00015,    # Ib
            0.0008,     # II
            0.0035,     # III
            0.0055,     # IV
            0.925,      # V
            0.005,      # VI
            0.01,       # DA
            0.01,       # DB
            0.01,       # DC
            0.01,       # DO
            0.01,       # DQ
            0.01,       # DZ
            */


    float cumulative = 0f;
        for (int i = 0; i < LCP.Count; i++)
        {
            cumulative += LCP[i].Value;
            if (rng < cumulative)
            {
                lum_string = LCP[i].Key;
                break;
            }
        }
        //Debug.Log(rng + " " + lum_string);
        return lum_string;
    }

    public static string GetRandomSpectralClass()
    {
        string stellar_class_string = "";

        float rng = 100 * Random.Range(0f, 1f);

        // SCP is the Spectral Class Probability Table
        List<KeyValuePair<string, float>> SCP = new List<KeyValuePair<string, float>>()
        {
            new KeyValuePair<string, float>("O",   0.00003f),
            new KeyValuePair<string, float>("B",   0.13f),
            new KeyValuePair<string, float>("A",   0.6f),
            new KeyValuePair<string, float>("F",   3f),
            new KeyValuePair<string, float>("G",   7.6f),
            new KeyValuePair<string, float>("K",  12.1f),
            new KeyValuePair<string, float>("M",  76.45f),
            new KeyValuePair<string, float>("N",   0.029964f),
            new KeyValuePair<string, float>("WC",  0.000003f),
            new KeyValuePair<string, float>("WN",  0.000003f),
            new KeyValuePair<string, float>("R",   0.03f),
            new KeyValuePair<string, float>("C",   0.03f),
            new KeyValuePair<string, float>("S",   0.03f)
        };

        /*
        0.0000003,      # O
        0.0013,         # B
        0.006,          # A
        0.03,           # F
        0.076,          # G
        0.121,          # K
        0.7645,         # M
        0.00029964,     # N
        0.00000003,     # WC
        0.00000003,     # WN
        0.0003,         # R
        0.0003,         # C
        0.0003          # S
        */


        float cumulative = 0f;
        for (int i = 0; i < SCP.Count; i++)
        {
            cumulative += SCP[i].Value;
            if (rng < cumulative)
            {
                stellar_class_string = SCP[i].Key;
                break;
            }
        }
        //Debug.Log(rng + " " + stellar_class_string);
        return stellar_class_string;
    }

    public static StarData GetRandomStar(StarDataList starCatalog)
    {
        StarData star;
        star = starCatalog.starDataList[0];

        string rl = GetRandomLuminosity();
        string rc = "D";
        if (rl[0] != 'D')
        {
            rc = GetRandomSpectralClass();
        }
        int rsc = Random.Range(0, 10);

        StarData foundItem = starCatalog.starDataList.FirstOrDefault(i => (i.lum_class == rl) && (i.star_class == rc) && (i.subclass == rsc));
        if (foundItem != null)
        {
            star = foundItem;
        }
        Debug.Assert(foundItem != null);
        return star;
    }

    public static void PrintNumberOfStars(int s)
    {
        Debug.Log("NUM STARS ##########");
        Debug.Log("Number of stars: "+s);
    }

    public static void PrintEccentricities(float[] es)
    {
        int l = es.Length;
        Debug.Log("NODE ECCENTRICITIES ####");
        for (int n = 0; n < l; n++)
        {
            Debug.Log("Node " + n + ":   " + es[n]);
        }
    }

    public static void PrintNodeDistances(float[] distances)
    {
        int l = distances.Length;
        Debug.Log("NODE DISTANCES #########");
        for (int n = 0; n < l; n++)
        {
            Debug.Log("Node "+n+":   "+distances[n]);
        }
    }

    public static void PrintCoords(List<Vector3> coords)
    {
        Debug.Log("COORDS ################");
        for (int i = 0; i < coords.Count; i++)
        {
            Debug.Log("Coords for node "+ i + ": " + coords[i].x + " " + coords[i].y);
        }
    }

    public static void PrintStars(StarData[] stars)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            Debug.Log("STAR   : " + i + "   ###############   ");
            Debug.Log("Type   : " + stars[i].star_type);
            Debug.Log("Mass   : " + stars[i].mass);
            Debug.Log("Radius : " + stars[i].radius);
            Debug.Log("Temp   : " + stars[i].temp);
            Debug.Log("ColorR : " + stars[i].colorR);
            Debug.Log("ColorG : " + stars[i].colorG);
            Debug.Log("ColorB : " + stars[i].colorB);
        }
    }

    public static void PrintSystemName(string system_name)
    {
        Debug.Log("SYSTEM NAME #######");
        Debug.Log(system_name);
    }
}
 