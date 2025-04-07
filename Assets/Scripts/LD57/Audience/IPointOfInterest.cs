namespace LD57.Audience {
   public interface IPointOfInterest {
      public string DisplayName { get; }
      public int Interest { get; }
      public int FindingScore { get; }
   }
}