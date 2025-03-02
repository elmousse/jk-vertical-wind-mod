using System.Collections.Generic;
using JumpKing.API;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using JumpKing.Workshop;
using Microsoft.Xna.Framework;

namespace VerticalWindMod
{
    public class VerticalWindFactory : IBlockFactory
    {
        private HashSet<Color> _blockCodes = new HashSet<Color>
        {
            new Color(53, 113, 220)
        };
        
        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return _blockCodes.Contains(blockCode);
        }
        
        public bool IsSolidBlock(Color blockCode)
        {
            return false;
        }
        
        public IBlock GetBlock(
            Color blockCode,
            Rectangle blockRect,
            Level level,
            LevelTexture textureSrc,
            int currentScreen,
            int x,
            int y)
        {
            VerticalWindManager.Instance.AddWindToScreen(currentScreen);
            return null;
        }
    }
}