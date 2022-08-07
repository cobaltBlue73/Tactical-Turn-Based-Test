using System;
using UnityEngine;

namespace UnitSystem
{
    [CreateAssetMenu(fileName = nameof(Faction), menuName = nameof(UnitSystem) + "/" + nameof(Faction), order = 0)]
    public class Faction : ScriptableObject, IEquatable<Faction>
    {
        public bool Equals(Faction other) =>
            other && GetInstanceID().Equals(other.GetInstanceID());

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Faction)obj);
        }

        public override int GetHashCode() => GetInstanceID();

        public static bool operator ==(Faction left, Faction right) => Equals(left, right);

        public static bool operator !=(Faction left, Faction right) => !Equals(left, right);
    }
}