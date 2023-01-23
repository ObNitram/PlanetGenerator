using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public Vector3 rotationSpeed;
    
    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
