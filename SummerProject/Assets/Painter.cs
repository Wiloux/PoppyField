using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Painter : MonoBehaviour
{
    public Material Mat;
    public float Hardness;
    public Color Color;
    public float w;
    public Renderer Rend;

    // Update is called once per frame
    void Update()
    {
        Mat.SetColor("Color_E0B335C9", Color);
        Mat.SetVector("Vector3_3828B19D", (Vector4)this.transform.position);
        Mat.SetFloat("Vector1_9C4FE784", this.transform.localScale.x * 0.5f);
        Mat.SetFloat("Vector1_7B9012FA", Hardness);


    
    }

    void IDK()
    {
        CommandBuffer command = new CommandBuffer();
        command.name = "UV Space Renderer";
        command.SetRenderTarget("");
        command.DrawRenderer(Rend, Mat, 0);
        Graphics.ExecuteCommandBuffer(command);
    }
}
