﻿using UnityEngine;

namespace PlanetGenerator
{
    public class Planet : MonoBehaviour {

        // Start is called before the first frame update
        private void Start()
        {
            GeneratePlanet();
        }
        
        
        [Range(2,256)]
        public int resolution = 10;
        public bool autoUpdate = true;

        public ShapeSettings shapeSettings;
        public ColourSettings colourSettings;

        [HideInInspector]
        public bool shapeSettingsFoldout;
        [HideInInspector]
        public bool colourSettingsFoldout;

        private readonly ShapeGenerator _shapeGenerator = new ShapeGenerator();
        private readonly ColourGenerator _colourGenerator = new ColourGenerator();

        [SerializeField, HideInInspector] 
        private MeshFilter[] meshFilters;

        private TerrainFace[] _terrainFaces;

        public int seed;
        

        void Initialize()
        {
            _shapeGenerator.UpdateSettings(shapeSettings, seed);
            _colourGenerator.UpdateSettings(colourSettings, seed);
            
            if (meshFilters == null || meshFilters.Length == 0)
            {
                meshFilters = new MeshFilter[6];
            }
            _terrainFaces = new TerrainFace[6];

            Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i].sharedMesh = new Mesh();
                }
                meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;
                
                _terrainFaces[i] = new TerrainFace(_shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            }
        }

        public void GeneratePlanet()
        {
            Initialize();
            GenerateMesh();
            GenerateColours();
        }

        public void GeneratePlanet(string seedParameter)
        {
            this.seed = Animator.StringToHash(seedParameter);
            GeneratePlanet();
        }

        public void OnShapeSettingsUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateMesh();
            }
        }

        public void OnColourSettingsUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateColours();
            }
        }

        void GenerateMesh()
        {
            foreach (TerrainFace face in _terrainFaces)
            {
                face.ConstructMesh();
            }
            _colourGenerator.UpdateElevation(_shapeGenerator.elevationMinMax);
        }

        void GenerateColours()
        {
           _colourGenerator.UpdateColours();

           for (int i = 0; i < 6; i++)
           {
               if (meshFilters[i].gameObject.activeSelf)
               {
                   _terrainFaces[i].UpdateUVs(_colourGenerator);
               }
           }
        }
    }
}
