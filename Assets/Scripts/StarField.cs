using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarField : MonoBehaviour
{
    [SerializeField]
    private GameObject star;
    [SerializeField]
    private GameObject node;
    private GameObject[] star_array;
    private GameObject[] h_array;
    private GameObject[] h_lr;
    private int num_stars;
    private List<Vector2Int> tree;
    private int[][] hierarchy;
    private List<int>[,] children;
    private float[] distances;
    private float[] eccentricities;
    private List<Vector3> coords;
    private string sys_name;
    [SerializeField]
    private StarData[] stars;
    private float[][] oe;
    private Vector3[] vectorL;
    private Vector3[] vectorR;

    private float scale_factor;
    private float t;
    private float t_scale;
    private bool msg_New = false;

    [SerializeField]
    private StarDataList starCatalog;


    // Use this for initialization
    void Start()
    {
        Debug.Log("### START ### ### ###");
        star_array = new GameObject[num_stars];
        for (int i = 0; i < num_stars; i++)
        {
            float star_scale_factor = stars[i].radius * scale_factor / 2.15f;
            // scale_factor is in u/AU (not to be confused with star_scale_factor)
            // If scale_factor = 1, then a star 215x as wide as the sun should be one u wide
            //because the sun's radius is appx 1/215 AU.

            GameObject go = Instantiate(star, Vector3.zero, Quaternion.identity) as GameObject;
            //go.transform.localScale = 0.01f * Vector3.one * star_scale_factor; // sun diameter * some scale factor for the star in question
            go.transform.localScale = 2f * Vector3.one; // a constant size for all

            go.name = "Star " + i + "  Type (" + stars[i].star_type + ")  " + "  Rad (" + stars[i].radius + ")  " + "Mass (" + stars[i].mass + ")";

            star_array[i] = go;

            /*
            // use if the basic star prefab is in play (dull color will exactly match the star's catalog color)
            Material sm = star_array[i].GetComponent<Renderer>().material;
            sm.color = star_color;
            */

            // ######### WIP ############# PLAYING AROUND WITH STAR COLOR HERE ############## WIP ################

            Color star_color = new Color(stars[i].colorR, stars[i].colorG, stars[i].colorB, 1f);
            // intensify the color
            float H, S, V;
            Color.RGBToHSV(new Color(star_color.r, star_color.g, star_color.b, 1.0f), out H, out S, out V);
            //Debug.Log("H: " + H + " S: " + S + " V: " + V);

            //ramp up the saturation and value a bit, but not too much
            float new_sat = S + 1f / 2f;
            float new_val = V + 1f / 2f;
            {
                star_color = Color.HSVToRGB(H, new_sat, new_val);
            }

            TrailRenderer tr = star_array[i].GetComponent<TrailRenderer>();
            Material mtr = tr.material;
            mtr.color = star_color;
            tr.enabled = false;

            // change the glowing star color to a stylized version of the real color from the catalog
            ParticleSystem sps = star_array[i].GetComponent<ParticleSystem>();
            
            var spsmain = sps.main;
            //start color of the particle system
            spsmain.startColor = star_color;

            /*
            //gradient (but not useful at this time)
            var col = sps.colorOverLifetime;

            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] {
                                                new GradientColorKey(star_color, 0.5f), new GradientColorKey(star_color, 0.5f)
                                                },
                         new GradientAlphaKey[] {
                                                new GradientAlphaKey(1f, 0.15f), new GradientAlphaKey(1f, 0.85f),
                                                new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0f, 1f)
                                                });

            col.color = grad;
            */
        }

        if (num_stars == 1)
        {
            t_scale = 1f;
        }
        else
        {
            t_scale = 1f / (num_stars * oe[0][9]);
            // base time scale is a function of the widest binary hierarchy length (node 0)

            int h = hierarchy[1].Length;
            h_array = new GameObject[h];
            h_lr = new GameObject[h];
            for (int i = 0; i < h; i++)
            {
                GameObject go = Instantiate(node, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.localScale = 1f * Vector3.one;  // a constant size for all
                go.name = ("Node " + i + "("+distances[i]+")");
                h_array[i] = go;

                if (i != 0)
                {
                    int p = SysHierarchy.GetParentIndex(tree, i);
                    h_lr[i] = new GameObject();
                    h_lr[i].AddComponent<LineRenderer>();
                    LineRenderer lr = h_lr[i].GetComponent<LineRenderer>();
                    lr.startWidth = 1f;
                    lr.endWidth = 1f;
                    lr.material.color = Color.blue;
                    lr.name = "Node line: " + p + "-" + i;
                }
            }
        }
        t = 0.0f;

        // from 0 to 1 is a full orbit
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Update__Text_Panel_Star_Count__data();

        if (num_stars > 1)
        {
            for (int i = 0; i < num_stars; i++)
            {
                TrailRenderer tr = star_array[i].GetComponent<TrailRenderer>();
                if (tr.enabled == false)
                {
                    tr.enabled = true;
                }
            }

            int h = hierarchy[1].Length;
            for (int i = 0; i < h; i++)
            {
                vectorL[i] = scale_factor * coords[i];
                vectorR[i] = scale_factor * coords[i];

                float[] masses = StarSystem.GetContributingStars(children, i, stars);

                if (i != 0)
                {
                    int p = SysHierarchy.GetParentIndex(tree, i);

                    int nL = tree[p][0];
                    int nR = tree[p][1];

                    if (i == nL)
                    {
                        vectorL[i] = vectorL[p];
                        vectorR[i] = vectorL[p];
                        h_array[i].transform.position = vectorL[p];
                        LineRenderer lr = h_lr[i].GetComponent<LineRenderer>();
                        lr.SetPosition(0, h_array[p].transform.position);
                        lr.SetPosition(1, h_array[i].transform.position);
                    }
                    if (i == nR)
                    {
                        vectorL[i] = vectorR[p];
                        vectorR[i] = vectorR[p];
                        h_array[i].transform.position = vectorR[p];
                        LineRenderer lr = h_lr[i].GetComponent<LineRenderer>();
                        lr.SetPosition(0, h_array[p].transform.position);
                        lr.SetPosition(1, h_array[i].transform.position);
                    }
                }

                float osm = (t * oe[0][10] / oe[i][10]);
                //Debug.Log("Orbital Period for node " + i + " " + oe[i][10]);
                if (osm > 1)
                {
                    osm -= Mathf.Floor(osm);
                }

                // oe[i][2] is smaP and oe[i][5] is smaS
                vectorL[i] += OrbitalMechanics.GetPosition(osm, oe[i][2], eccentricities[i], scale_factor, false);
                vectorR[i] += OrbitalMechanics.GetPosition(1 - osm, oe[i][5], eccentricities[i], scale_factor, true);

                if (masses[2] > -1)
                {
                    star_array[(int)masses[2]].transform.position = vectorL[i];
                    //Debug.Log("Left planet # " + masses[2]);
                }
                if (masses[3] > -1)
                {
                    star_array[(int)masses[3]].transform.position = vectorR[i];
                    //Debug.Log("Right planet # " + masses[3]);
                }
                t += t_scale;
            }
        }
        if (msg_New == true)
        {
            msg_New = false;
            enabled = false;

            for (int s = num_stars -1; s >= 0; s--)
            {
                Destroy(star_array[s]);
            }
            if (num_stars > 1)
            {
                for (int n = hierarchy[1].Length - 1; n >= 0; n--)
                {
                    Destroy(h_array[n]);
                    Destroy(h_lr[n]);
                }
            }
            SetupScene();
            Start();

            Debug.Log("NEW SYSTEM");
            Vector3 mCamPos = Camera.main.transform.position;
            Quaternion mCamRot = Camera.main.transform.rotation;
            Vector3 newPos = new Vector3(0f, 400f, 0f);
            Quaternion newRot = Quaternion.Euler(90f, 0f, 0f);
            Camera.main.transform.position = newPos;
            Camera.main.transform.rotation = newRot;
        }
    }

    public void SetupScene()
    {
        Debug.Log("### SETUP SCENE ### ### ### ###");
        scale_factor = 1.0f;

        num_stars = StarSystem.GetNumberOfStars();
        stars = new StarData[num_stars];

        if (num_stars > 1)
        {
            tree = SysHierarchy.CreateSysTree(num_stars);
            hierarchy = SysHierarchy.GetHierarchy(tree);
            int ch = hierarchy[0][0];
            while (ch > Constants.max_sys_hierarchy)
            {
                //Debug.Log("####### ####### FAILED HIERARCHY ####### #######");
                //Debug.Log("Attempted hierarchy " + ch);
                //Debug.Log("maximum hierarchy   " + Constants.max_sys_hierarchy);
                //Debug.Log("####### TRYING AGAIN....");
                tree = null;
                hierarchy = null;
                //if (num_stars > 8) { num_stars--; }
                //with hierarchy 5 we should be ok up to 16 stars
                tree = SysHierarchy.CreateSysTree(num_stars);

                hierarchy = SysHierarchy.GetHierarchy(tree);
                ch = hierarchy[0][0];
            }
            //SysHierarchy.PrintHierarchy(hierarchy);

            children = SysHierarchy.GetChildren(tree);

            for (int i = 0; i < num_stars; i++)
            {
                int r = Random.Range(0, starCatalog.starDataList.Count);
                stars[i] = StarSystem.GetRandomStar(starCatalog);
            }
            //StarSystem.PrintStars(stars);     // ##############################################################

            int h = hierarchy[1].Length;
            for (int i = 0; i < h; i++)
            {
                float[] masses = StarSystem.GetContributingStars(children, i, stars);
                if (masses[0] < masses[1])
                {
                    int lt = tree[i][0];
                    int rt = tree[i][1];
                    tree[i] = new Vector2Int(rt, lt);

                    List<int> lch = children[i, 0];
                    List<int> rch = children[i, 1];
                    //Debug.Log("LEFT " + stars[children[i,0][0]].mass);
                    //Debug.Log("RIGHT" + stars[children[i,1][0]].mass);
                    //Debug.Log("------------------");
                    children[i, 0] = rch;
                    children[i, 1] = lch;
                    //Debug.Log("LEFT " + stars[children[i,0][0]].mass);
                    //Debug.Log("RIGHT" + stars[children[i,1][0]].mass);
                }
            }

            //SysHierarchy.PrintTree(tree);

            //SysHierarchy.PrintChildren(children);

            eccentricities = StarSystem.GetNodeEccentricities(hierarchy[1]);
            //StarSystem.PrintEccentricities(eccentricities);

            distances = StarSystem.GetNodeDistances(hierarchy);
            //StarSystem.PrintNodeDistances(distances);

            coords = StarSystem.GetStarCoords(tree, distances);
            //StarSystem.PrintCoords(coords);

            StarSystem.PrintNumberOfStars(num_stars);

            oe = new float[h][];
            vectorL = new Vector3[h];
            vectorR = new Vector3[h];

            for (int i = 0; i < h; i++)
            {
                float[] masses = StarSystem.GetContributingStars(children, i, stars);
                oe[i] = OrbitalMechanics.GetOrbitalElements(distances[i], masses[0], masses[1], eccentricities[i]);
                //Debug.Log("########################## OE FOR NODE: " + i);
                //OrbitalMechanics.PrintOrbitalElements(oe[i]);
                // or
                //OrbitalMechanics.PrintOrbitalPeriod(oe[i]);
                vectorL[i] = Vector3.zero;
                vectorR[i] = Vector3.zero;
            }
            scale_factor = 900.0f / (1f * oe[0][9]);  // h0 is the widest distance; oe[7] is maxS
            Debug.Log("SCALE FACTOR in 'Unity units' / AU   :  " + scale_factor);
            Debug.Log("1/SF         in AU / 'Unity units'   :  " + 1 / scale_factor);
        }
        else
        {
            tree = null;
            hierarchy = null;
            children = null;
            eccentricities = null;
            distances = null;
            coords = null;

            stars[0] = StarSystem.GetRandomStar(starCatalog); // starCatalog.starDataList[1048]; //
            //StarSystem.PrintStars(stars);     //#############################################
        }
        sys_name = StarSystem.GenerateRandomSystemName();
        //StarSystem.PrintSystemName(sys_name);
    }

    public void SetMsgNew(bool value)
    {
        msg_New = value;
    }

    public void New()
    {
        FindObjectOfType<StarField>().msg_New = true;
        GameObject canvasMain = GameObject.Find("Canvas_Main");
        canvasMain.SetActive(false);
        GameObject canvasStats = GameObject.Find("Canvas_Stats");
        canvasStats.SetActive(false);
    }

    public void Update__Text_Panel_Star_Count__data()
    {
        if (GameObject.Find("Text_Panel_Star_Count__data") != null)
        {
            GameObject textGO = GameObject.Find("Text_Panel_Star_Count__data");
            textGO.GetComponent<Text>().text = num_stars.ToString();
            Debug.Log("updated number of stars");
        }
    }
}