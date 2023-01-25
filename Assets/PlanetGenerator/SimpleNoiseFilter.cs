using UnityEngine;

namespace PlanetGenerator
{
    public class SimpleNoiseFilter : INoiseFilter {
        private readonly NoiseSettings.SimpleNoiseSettings _settings;
        private readonly Noise _noise;

        public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings, int seed)
        {
            this._settings = settings;
            _noise = new Noise(seed);
        }

        public float Evaluate(Vector3 point)
        {
            float noiseValue = 0;
            float frequency = _settings.baseRoughness;
            float amplitude = 1;

            for (int i = 0; i < _settings.numLayers; i++)
            {
                float v = _noise.Evaluate(point * frequency + _settings.centre);
                noiseValue += (v + 1) * .5f * amplitude;
                frequency *= _settings.roughness;
                amplitude *= _settings.persistence;
            }

            noiseValue = noiseValue - _settings.minValue;
            return noiseValue * _settings.strength;
        }
    }
}
