using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public Vector3 rotationSpeed;

    public PlanetGenerator.Planet planet;
    
    // Start is called before the first frame update
    private void Start()
    {
        planet.GeneratePlanet();
    }
    
    
    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
