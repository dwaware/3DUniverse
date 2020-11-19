using UnityEngine;
using System.Collections;


[System.Serializable]                                                   //  Our Representation of Star Data
public class StarData
{
    public string star_type = "Type";                                   //  Type
    public string star_class = "Class";                                 //  Class
    public int subclass = 1;                                            //  Subclass
    public string lum_class = "Lum Class";                              //  Lum Class
    public float mass = 1.0f;                                           //  Mass   
    public float luminosity = 1.0f;                                     //  Luminosity
    public float radius = 1.0f;                                         //  Radius
    public int temp = 5000;                                             //  Temp (K)
    public float color_index = 1.0f;                                    //  Color Index
    public float abs_mag = 1.0f;                                        //  Abs Mag
    public float bolo_corr = 1.0f;                                      //  Bolo Corr
    public float bolo_mag = 1.0f;                                       //  Bolo Mag

    public float colorR = 1.0f;                                         //  Color (red)
    public float colorG = 1.0f;                                         //  Color (green)
    public float colorB = 1.0f;                                         //  Color (blue)

    public float density = 1.0f;                                        //  Density
    public float rlRigid = 1.0f;                                        //  Roche Limit (rigid)
    public float rlFluid = 1.0f;                                        //  Roche Limit (fluid)
    public float minHZ = 1.0f;                                          //  Min Habitable Zone
    public float maxHZ = 1.0f;                                          //  Max Habitable Zone


    public static StarData Init(string[] cols)
    {
        StarData star_data = new StarData();

        string star_type_fromFile = cols[0];
        string star_class_fromFile = cols[1];
        int subclass_fromFile = int.Parse(cols[2]);
        string lum_class_fromFile = cols[3];
        float mass_fromFile = float.Parse(cols[4]);
        float luminosity_fromFile = float.Parse(cols[5]);
        float radius_fromFile = float.Parse(cols[6]);
        int temp_fromFile = int.Parse(cols[7]);
        float color_index_fromFile = float.Parse(cols[8]);
        float abs_mag_fromFile = float.Parse(cols[9]);
        float bolo_corr_fromFile = float.Parse(cols[10]);
        float bolo_mag_fromFile = float.Parse(cols[11]);
        float colorR_fromFile = float.Parse(cols[12]);
        float colorG_fromFile = float.Parse(cols[13]);
        float colorB_fromFile = float.Parse(cols[14]);
        float density_fromFile = float.Parse(cols[15]);
        float rlRigid_fromFile = float.Parse(cols[16]);
        float rlFluid_fromFile = float.Parse(cols[17]);
        float minHZ_fromFile = float.Parse(cols[18]);
        float maxHZ_fromFile = float.Parse(cols[19]);

        star_data.star_type = star_type_fromFile;
        star_data.star_class = star_class_fromFile;
        star_data.subclass = subclass_fromFile;
        star_data.lum_class = lum_class_fromFile;
        star_data.mass = mass_fromFile;
        star_data.luminosity = luminosity_fromFile;
        star_data.radius = radius_fromFile;
        star_data.temp = temp_fromFile;
        star_data.color_index = color_index_fromFile;
        star_data.abs_mag = abs_mag_fromFile;
        star_data.bolo_corr = bolo_corr_fromFile;
        star_data.bolo_mag = bolo_mag_fromFile;
        star_data.colorR = colorR_fromFile;
        star_data.colorG = colorG_fromFile;
        star_data.colorB = colorB_fromFile;
        star_data.density = density_fromFile;
        star_data.rlRigid = rlRigid_fromFile;
        star_data.rlFluid = rlFluid_fromFile;
        star_data.minHZ = minHZ_fromFile;
        star_data.maxHZ = maxHZ_fromFile;

        return star_data;
    }
}