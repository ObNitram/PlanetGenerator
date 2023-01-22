using System.Collections;
using System.Collections.Generic;
using PlanetGenerator;
using UnityEngine;

public class ColourGenerator
{
    ColourSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    
    
    
    private static readonly int ElevationMinMax = Shader.PropertyToID("_ElevationMinMax");
    private static readonly int PlanetTexture = Shader.PropertyToID("_PlanetTexture");

    public void UpdateSettings(ColourSettings settings)
    {
        this.settings = settings;
        if (texture == null)
        {
            texture = new Texture2D(textureResolution, 1, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp
            };
        }
        
    }
    
    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector(ElevationMinMax, new Vector4(elevationMinMax.min, elevationMinMax.max));
    }

    public void UpdateColours()
    {
        Color[] colours = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colours[i] = settings.gradient.Evaluate(i / (textureResolution - 1f));
        }
        texture.SetPixels(colours);
        texture.Apply();
        settings.planetMaterial.SetTexture(PlanetTexture, texture);
    }
}
