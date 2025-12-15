using UnityEngine;
using Assets.Version2.StatusEffectSystem;

namespace Assets.Version2.DynamicQuadTree
{
    public interface IQuadObject
    {
        public int LayerMask { get;}

        public Vector2 Pos2D { get; }

        public StatusEffectManager StatusManager { get;}
    }
}