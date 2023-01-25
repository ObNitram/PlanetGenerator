using PlanetGenerator;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UI : MonoBehaviour
    {
        public Planet planet;


        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            Button buttonGenerate = root.Q<Button>("Generate");
            TextField seed = root.Q<TextField>("Seed");
            
            buttonGenerate.clicked += () => { planet.GeneratePlanet(seed.value); };
        }
    }
}