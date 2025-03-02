using System.Collections.Generic;

namespace VerticalWindMod
{
    public class VerticalWindManager : IVerticalWindQuery
    {
        private static VerticalWindManager _instance;
        public static VerticalWindManager Instance => _instance ?? (_instance = new VerticalWindManager());

        private readonly HashSet<int> _screensWithWind = new HashSet<int>();
        
        public void AddWindToScreen(int screen) =>_screensWithWind.Add(screen);
        
        public void Clear() => _screensWithWind.Clear();
        
        public bool HasWind(int screen) => _screensWithWind.Contains(screen);
    }
    
    public interface IVerticalWindQuery
    {
        bool HasWind(int screen);
    }
}