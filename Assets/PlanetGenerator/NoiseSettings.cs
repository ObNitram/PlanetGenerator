using UnityEngine;

namespace PlanetGenerator
{
    [System.Serializable]
    public class NoiseSettings {

        public enum FilterType { Simple, Ridgid };
        public FilterType filterType;

        [ConditionalHide("filterType", 0)]
        public SimpleNoiseSettings simpleNoiseSettings;
        [ConditionalHide("filterType", 1)]
        public RidgidNoiseSettings ridgidNoiseSettings;
        
        
        [System.Serializable]
        public class SimpleNoiseSettings
        {
            public float strength = 1;
            [Range(1, 6)] public int numLayers = 1;
            public float baseRoughness = 1;
            public float roughness = 2;
            [Range(0, 1)] public float persistence = .5f;
            public Vector3 centre;
            [Range(0, 2)] public float minValue;
        }
        
        [System.Serializable]
        public class RidgidNoiseSettings : SimpleNoiseSettings
        {
            [Range(0, 1)] 
            public float weightMultiplier = .8f;
        }
    }
}
