namespace PlanetGenerator
{
    public class MinMax 
    {
        public float min { get; private set; }
        public float max { get; private set; }
    
        public MinMax()
        {
            this.min = float.MaxValue;
            this.max = float.MinValue;
        }
    
        public void AddValue(float value)
        {
            if (value < min)
            {
                min = value;
            }
            if (value > max)
            {
                max = value;
            }
        }
    }
}
