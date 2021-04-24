using UnityEngine;

namespace Achievements
{
    [CreateAssetMenu(fileName = "achievement", menuName = "Achievements/New Achievement")]
    public class Achievement : ScriptableObject
    {
        public string title;
        public string description;
        public Sprite icon;
        public string id;
        public float value;
        
        public AchievementType type;
    }

    public enum AchievementType
    {
        Gems, Dwarves, Buildings, Houses, Forges, Taverns, Clicks, Deepness
    }
}