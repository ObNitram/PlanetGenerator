using UnityEngine;

namespace PlanetGenerator
{
    public class ColourGenerator
    {
        ColourSettings settings;
        Texture2D texture;
        const int textureResolution = 50;

        INoiseFilter biomeNoiseFilter;


        private static readonly int ElevationMinMax = Shader.PropertyToID("_ElevationMinMax");
        private static readonly int PlanetTexture = Shader.PropertyToID("_PlanetTexture");

        public void UpdateSettings(ColourSettings settings)
        {
            this.settings = settings;
            if (texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
            {
                texture = new Texture2D(textureResolution, settings.biomeColourSettings.biomes.Length,
                    TextureFormat.RGBA32, false)
                {
                    wrapMode = TextureWrapMode.Clamp
                };
            }

            biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
        }

        public void UpdateElevation(MinMax elevationMinMax)
        {
            settings.planetMaterial.SetVector(ElevationMinMax, new Vector4(elevationMinMax.min, elevationMinMax.max));
        }

        public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
        {
            float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
            heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColourSettings.noiseOffset) *
                             settings.biomeColourSettings.noiseStrength;

            float biomeIndex = 0;
            int numBiomes = settings.biomeColourSettings.biomes.Length;
            float blendRange = settings.biomeColourSettings.blendAmount / 2f + .0001f;

            for (int i = 0; i < numBiomes; i++)
            {
                float distance = heightPercent - settings.biomeColourSettings.biomes[i].startHeight;
                float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
                biomeIndex *= (1 - weight);
                biomeIndex += i * weight;
            }

            return biomeIndex / Mathf.Max(1, numBiomes - 1);
        }

        public void UpdateColours()
        {
            Color[] colours = new Color[texture.width * texture.height];
            int colourIndex = 0;

            foreach (var biome in settings.biomeColourSettings.biomes)
            {
                for (int i = 0; i < textureResolution; i++)
                {
                    Color gradientColour = biome.gradient.Evaluate(i / (textureResolution - 1f));
                    Color tintColour = biome.tint;
                    colours[colourIndex] = gradientColour * (1 - biome.tintPercent) + tintColour * biome.tintPercent;
                    colourIndex++;
                }
            }


            texture.SetPixels(colours);
            texture.Apply();
            settings.planetMaterial.SetTexture(PlanetTexture, texture);
        }
    }
}