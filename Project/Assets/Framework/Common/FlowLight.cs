using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowLight : MonoBehaviour
{
    public Material material;
    public float Duration = 0.5f;
    public float Interval = 1f;
    public float Power = 1f;
    public float Angle = 2f;
    public float Scale = 1f;
    public float DirFactor = -1;

    void Start()
    {
        if(material == null)
        {
            if(GetComponent<MaskableGraphic>() != null)
            {
                material = GetComponent<MaskableGraphic>().material;
            }
        }
    }

    void Update()
    {
        UpdateParam();
    }

    void UpdateParam()
    {
        if (material == null) return;

        if (Duration > Interval)
        {
            Debug.LogWarning("ImageFlashEffect.UpdateParam:Duration need less Interval");
            Interval = Duration + 0.5f;
        }

        material.SetFloat("_LightPower", Power);
        material.SetFloat("_LightDuration", Duration);
        material.SetFloat("_LightInterval", Interval);
        material.SetFloat("_LightAngle", Angle);
        material.SetFloat("_LightScale", Scale);
        material.SetFloat("_DirFactor", DirFactor);
    }
}
