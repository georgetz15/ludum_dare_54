using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/Create New Upgrade")]
    public class Upgrade : ScriptableObject
    {
        public enum Type
        {
            Cargo,
            Range
        }

        public Type upgradeType;
        [FormerlySerializedAs("upgradeNum")] public int amount;
        public int cost;
    }
}