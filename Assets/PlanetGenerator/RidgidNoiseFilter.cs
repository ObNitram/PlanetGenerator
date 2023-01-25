using UnityEngine;

namespace PlanetGenerator
{
    public class RidgidNoiseFilter : INoiseFilter
    {
        private readonly NoiseSettings.RidgidNoiseSettings _settings;
        private readonly Noise _noise;

        public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings settings, int seed)
        {
            this._settings = settings;
            _noise = new Noise(seed);
        }

        public float Evaluate(Vector3 point)
        {
            float noiseValue = 0;
            float frequency = _settings.baseRoughness;
            float amplitude = 1;
            float weight = 1;

            for (int i = 0; i < _settings.numLayers; i++)
            {
                float v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + _settings.centre));
                v *= v;
                v *= weight;
                weight = Mathf.Clamp01(v * _settings.weightMultiplier);

                noiseValue += v * amplitude;
                frequency *= _settings.roughness;
                amplitude *= _settings.persistence;
            }

            noiseValue = noiseValue - _settings.minValue;
            return noiseValue * _settings.strength;
        }
    }
}