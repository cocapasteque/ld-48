using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementSystem : MonoBehaviour
    {
        public bool clearAchievementOnAwake;
        public Achievement[] achievements;
        
        private WaitForSeconds _achievementTimeout = new WaitForSeconds(2);
        private Dictionary<Achievement, bool> _unlocked = new Dictionary<Achievement, bool>();

        [Header("Popup Settings")] public string PopupName = "AchievementPopup";
        private UIPopup m_popup;

        public static AchievementSystem Instance;
        
        void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this);
            
            if (clearAchievementOnAwake)
            {
                foreach (var a in achievements)
                {
                    PlayerPrefs.SetInt(a.id, 0);
                }
            }
            
            _unlocked = achievements.ToDictionary(x => x,
                y => PlayerPrefs.GetInt(y.id, 0) == 1);

        }

        private void Start()
        {
            MonitorGemsAchievements();
            MonitorDwarvesAchievements();
            MonitorDeepAchievements();
        }

        public void UnlockAchievement(Achievement achievement)
        {
            Debug.Log($"Unlocked achievement {achievement}");
            PlayerPrefs.SetInt(achievement.id, 1);
            _unlocked[achievement] = true;
            TriggerNotification(achievement);
        }

        public void BuildingBuilt(Building building)
        {
            if (building.GetType() == typeof(House))
            {
                Debug.Log("House built");
            }

            if (building.GetType() == typeof(Forge))
            {
                Debug.Log("Forge built");
            }
        }
        
        private void TriggerNotification(Achievement achievement)
        {
            m_popup = UIPopupManager.GetPopup(PopupName);

            if (m_popup == null)
                return;

            var icon = m_popup.Data.Images[0];
            var title = m_popup.Data.Labels[0].GetComponent<Text>();
            var message = m_popup.Data.Labels[1].GetComponent<Text>();


            m_popup.Data.SetImagesSprites(achievement.icon);
            m_popup.Data.SetLabelsTexts(achievement.title, achievement.description);
            icon.color = Color.white;
            title.color = Color.white;
            message.color = Color.white;

            UIPopupManager.ShowPopup(m_popup, m_popup.AddToPopupQueue, false);
        }

        private void MonitorGemsAchievements()
        {
            Debug.Log("Starting gems monitoring");
            var gemsAchievements = achievements.Where(x => x.type == AchievementType.Gems);
            if (gemsAchievements.Any())
            {
                StartCoroutine(Work());
            }

            IEnumerator Work()
            {
                while (true)
                {
                    var currentGems = DiggingManager.Instance.Gems;
                    foreach (var ga in gemsAchievements)
                    {
                        if (!_unlocked.ContainsKey(ga) || _unlocked[ga]) continue;
                        
                        if (currentGems >= ga.value)
                        {
                            UnlockAchievement(ga);
                        }

                        yield return null;
                    }
                    yield return _achievementTimeout;
                }
            }
        }

        private void MonitorDwarvesAchievements()
        {
            Debug.Log("Starting dwarves monitoring");
            var dwarvesAchievements = achievements.Where(x => x.type == AchievementType.Dwarves);
            if (dwarvesAchievements.Any())
            {
                StartCoroutine(Work());
            }

            IEnumerator Work()
            {
                while (true)
                {
                    var currentDwarves = DiggingManager.Instance.Dwarves;
                    foreach (var da in dwarvesAchievements)
                    {
                        if (!_unlocked.ContainsKey(da) || _unlocked[da]) continue;
                        if (currentDwarves >= da.value)
                        {
                            UnlockAchievement(da);
                        }

                        yield return null;
                    }
                    yield return _achievementTimeout;
                }
            }
        }

        private void MonitorDeepAchievements()
        {
            Debug.Log("Starting Deep monitoring");
            var deepAchievements = achievements.Where(x => x.type == AchievementType.Deepness);
            if (deepAchievements.Any())
            {
                StartCoroutine(Work());
            }

            IEnumerator Work()
            {
                while (true)
                {
                    var currentDeepLevel = DiggingManager.Instance.Depth;
                    foreach (var da in deepAchievements)
                    {
                        if (!_unlocked.ContainsKey(da) || _unlocked[da]) continue;
                        if (currentDeepLevel >= da.value)
                        {
                            UnlockAchievement(da);
                        }

                        yield return null;
                    }

                    yield return _achievementTimeout;
                }
            }
        }
    }
}