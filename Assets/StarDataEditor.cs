#if (UNITY_EDITOR) 

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class StarDataEditor : EditorWindow
{

    public StarDataList starDataList;
    private int viewIndex = 1;

    [MenuItem("Window/Star Data Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(StarDataEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            starDataList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(StarDataList)) as StarDataList;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Star Data Editor", EditorStyles.boldLabel);
        if (starDataList != null)
        {
            if (GUILayout.Button("Show Star List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = starDataList;
            }
        }
        if (GUILayout.Button("Open Star List"))
        {
            OpenStarList();
        }
        if (GUILayout.Button("Import Star List"))
        {
            ImportStarList();
        }
        if (GUILayout.Button("New Star List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = starDataList;
        }
        GUILayout.EndHorizontal();



        if (starDataList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Star List", GUILayout.ExpandWidth(false)))
            {
                CreateNewStarDataList();
            }
            if (GUILayout.Button("Open Existing Star List", GUILayout.ExpandWidth(false)))
            {
                OpenStarList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (starDataList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < starDataList.starDataList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Star", GUILayout.ExpandWidth(true)))
            {
                AddStar();
            }
            if (GUILayout.Button("Delete Star", GUILayout.ExpandWidth(true)))
            {
                DeleteStar(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (starDataList.starDataList == null)
                Debug.Log("wtf");
            if (starDataList.starDataList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Star", viewIndex, GUILayout.ExpandWidth(false)), 1, starDataList.starDataList.Count);
                //Mathf.Clamp (viewIndex, 1, starDataList.starDataList.Count);
                EditorGUILayout.LabelField("of   " + starDataList.starDataList.Count.ToString() + "  stars", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();



                string concatString = starDataList.starDataList[viewIndex - 1].star_class + starDataList.starDataList[viewIndex - 1].subclass + starDataList.starDataList[viewIndex - 1].lum_class;

                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                starDataList.starDataList[viewIndex - 1].star_type = EditorGUILayout.TextField("Type", concatString as string);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();



                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].star_class = EditorGUILayout.TextField("Class", starDataList.starDataList[viewIndex - 1].star_class as string);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].subclass = EditorGUILayout.IntField("Subclass", starDataList.starDataList[viewIndex - 1].subclass, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].lum_class = EditorGUILayout.TextField("Lum Class", starDataList.starDataList[viewIndex - 1].lum_class as string);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].mass = EditorGUILayout.FloatField("Mass", starDataList.starDataList[viewIndex - 1].mass, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].luminosity = EditorGUILayout.FloatField("Luminosity", starDataList.starDataList[viewIndex - 1].luminosity, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].radius = EditorGUILayout.FloatField("Radius", starDataList.starDataList[viewIndex - 1].radius, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].temp = EditorGUILayout.IntField("Temp", starDataList.starDataList[viewIndex - 1].temp, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].color_index = EditorGUILayout.FloatField("Color Index", starDataList.starDataList[viewIndex - 1].color_index, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].abs_mag = EditorGUILayout.FloatField("Abs Mag", starDataList.starDataList[viewIndex - 1].abs_mag, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].bolo_corr = EditorGUILayout.FloatField("Bolo Corr", starDataList.starDataList[viewIndex - 1].bolo_corr, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].bolo_mag = EditorGUILayout.FloatField("Bolo Mag", starDataList.starDataList[viewIndex - 1].bolo_mag, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();



                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].colorR = EditorGUILayout.FloatField("Color (R)", starDataList.starDataList[viewIndex - 1].colorR, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].colorG = EditorGUILayout.FloatField("Color (G)", starDataList.starDataList[viewIndex - 1].colorG, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].colorB = EditorGUILayout.FloatField("Color (B)", starDataList.starDataList[viewIndex - 1].colorB, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                Color color = new Color(starDataList.starDataList[viewIndex - 1].colorR, starDataList.starDataList[viewIndex - 1].colorG, starDataList.starDataList[viewIndex - 1].colorB, 1f);

                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                color = EditorGUILayout.ColorField("Color", color, GUILayout.ExpandWidth(false));
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();



                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].density = EditorGUILayout.FloatField("Density", starDataList.starDataList[viewIndex - 1].density, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].rlRigid = EditorGUILayout.FloatField("RL (Rigid)", starDataList.starDataList[viewIndex - 1].rlRigid, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].rlFluid = EditorGUILayout.FloatField("RL (Fluid)", starDataList.starDataList[viewIndex - 1].rlFluid, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].minHZ = EditorGUILayout.FloatField("Min HZ", starDataList.starDataList[viewIndex - 1].minHZ, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                starDataList.starDataList[viewIndex - 1].maxHZ = EditorGUILayout.FloatField("Max HZ", starDataList.starDataList[viewIndex - 1].maxHZ, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
            else
            {
                GUILayout.Label("This Star Data List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(starDataList);
        }
    }

    void CreateNewStarDataList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        starDataList = CreateStarDataList.Create();
        if (starDataList)
        {
            starDataList.starDataList = new List<StarData>();
            string relPath = AssetDatabase.GetAssetPath(starDataList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenStarList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Star Data List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            starDataList = AssetDatabase.LoadAssetAtPath(relPath, typeof(StarDataList)) as StarDataList;
            if (starDataList.starDataList == null)
                starDataList.starDataList = new List<StarData>();
            if (starDataList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void ImportStarList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Data File (.csv)", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            /*
                // full path (THE FILE THAT YOU SELECT)
                Debug.Log(absPath);

                // dir     "............Assets"
                Debug.Log(Application.dataPath);

                // file    "Assets............"
                Debug.Log(relPath);
            */



            StreamReader inp_stm = new StreamReader(absPath);

            viewIndex = 1;
            starDataList = CreateStarDataList.Create();
            if (starDataList)
            {
                starDataList.starDataList = new List<StarData>();
                string assetPath = AssetDatabase.GetAssetPath(starDataList);
                EditorPrefs.SetString("ObjectPath", assetPath);
            }
            if (starDataList)
            {
                while (!inp_stm.EndOfStream)
                {
                    string line = inp_stm.ReadLine();
                    // Do Something with the input. 
                    line.Split("\n"[0]);
                    if (line.Length > 2)
                    {
                        Debug.Log(line);
                        string[] cols = line.Split('\t');
                        StarData newStar = StarData.Init(cols);
                        starDataList.starDataList.Add(newStar);
                        viewIndex = starDataList.starDataList.Count;
                    }
                }
                inp_stm.Close();
            }
        }
    }

    void AddStar()
    {
        StarData newStar = new StarData();
        starDataList.starDataList.Add(newStar);
        viewIndex = starDataList.starDataList.Count;
    }

    void DeleteStar(int index)
    {
        starDataList.starDataList.RemoveAt(index);
    }
}

#endif
