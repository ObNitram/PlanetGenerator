using UnityEngine;

namespace PlanetGenerator
{
    public class TerrainFace
    {
        private readonly ShapeGenerator _shapeGenerator;
        private readonly Mesh _mesh;
        private readonly int _resolution;
        private readonly Vector3 _localUp;
        private readonly Vector3 _axisA;
        private readonly Vector3 _axisB;

        public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
        {
            this._shapeGenerator = shapeGenerator;
            this._mesh = mesh;
            this._resolution = resolution;
            this._localUp = localUp;

            _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            _axisB = Vector3.Cross(localUp, _axisA);
        }

        public void ConstructMesh()
        {
            Vector3[] vertices = new Vector3[_resolution * _resolution];
            int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
            int triIndex = 0;

            Vector2[] uv = (_mesh.uv.Length == vertices.Length)? _mesh.uv : new Vector2[vertices.Length]; // For not losing the UVs

            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    int i = x + y * _resolution;
                    Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                    Vector3 pointOnUnitCube = _localUp + (percent.x - .5f) * 2 * _axisA + (percent.y - .5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    float unscaledElevation = _shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                    vertices[i] = pointOnUnitSphere * _shapeGenerator.GetScaledElevation(unscaledElevation);
                    uv[i].y = unscaledElevation;

                    if (x != _resolution - 1 && y != _resolution - 1)
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + _resolution + 1;
                        triangles[triIndex + 2] = i + _resolution;

                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + _resolution + 1;
                        triIndex += 6;
                    }
                }
            }

            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
            _mesh.uv = uv;
        }

        public void UpdateUVs(ColourGenerator colorGenerator)
        {
            Vector2[] uv = _mesh.uv;
            
            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    int i = x + y * _resolution;
                    Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                    Vector3 pointOnUnitCube = _localUp + (percent.x - .5f) * 2 * _axisA + (percent.y - .5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                    uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere);
                }
            }

            _mesh.uv = uv;
        }
    }
}