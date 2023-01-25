namespace PlanetGenerator
{
   public static class NoiseFilterFactory
   {
      public static INoiseFilter CreateNoiseFilter(NoiseSettings settings, int seed)
      {
         return settings.filterType switch
         {
            NoiseSettings.FilterType.Simple => new SimpleNoiseFilter(settings.simpleNoiseSettings, seed),
            NoiseSettings.FilterType.Ridgid => new RidgidNoiseFilter(settings.ridgidNoiseSettings, seed),
            _ => null
         };
      }
   }
}
