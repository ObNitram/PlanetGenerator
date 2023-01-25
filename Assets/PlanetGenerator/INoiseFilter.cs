using UnityEngine;

namespace PlanetGenerator
{
    public interface INoiseFilter
    {
        float Evaluate(Vector3 point);
    }
}
