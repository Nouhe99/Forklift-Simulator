using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Acreos.ForkliftSim.UI
{
    public class MissionBriefingUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelRoot;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _bodyText;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Button _actionButton;

        // Data Structures
        [System.Serializable]
        public struct BriefingPage
        {
            [TextArea(3, 5)] public string Body;
            public string ButtonLabel;
        }

        [System.Serializable]
        public struct MissionChapter
        {
            public string MissionTitle;
            public List<BriefingPage> Pages;
        }

        [Header("Content Library")]
        [SerializeField] private List<MissionChapter> _allMissions;

        public UnityEvent OnBriefingCompleted;

        private int _currentMissionIndex;
        private int _currentPageIndex;

        private void Start()
        {
            _panelRoot.SetActive(false);
        }

        public void StartMissionBriefing(int missionIndex)
        {
            if (missionIndex >= _allMissions.Count) return;

            _currentMissionIndex = missionIndex;
            _currentPageIndex = 0;

            // Setup button for next behavior
            _actionButton.onClick.RemoveAllListeners();
            _actionButton.onClick.AddListener(OnNextClick);

            _panelRoot.SetActive(true);
            DisplayPage();
        }

        private void DisplayPage()
        {
            var mission = _allMissions[_currentMissionIndex];
            var page = mission.Pages[_currentPageIndex];

            _titleText.text = mission.MissionTitle;
            _bodyText.text = page.Body;
            _buttonText.text = page.ButtonLabel;
        }

        private void OnNextClick()
        {
            var mission = _allMissions[_currentMissionIndex];
            _currentPageIndex++;

            if (_currentPageIndex < mission.Pages.Count)
            {
                DisplayPage();
            }
            else
            {
                CloseBriefing();
            }
        }

        private void CloseBriefing()
        {
            _panelRoot.SetActive(false);
            OnBriefingCompleted?.Invoke();
        }

        public void ShowWinScreen(int winMissionIndex)
        {
            if (winMissionIndex >= _allMissions.Count)
            {
                Debug.LogError("Invalid Win Mission Index!");
                return;
            }

            var mission = _allMissions[winMissionIndex];
            var page = mission.Pages[0];

            _panelRoot.SetActive(true);

            _titleText.text = mission.MissionTitle;
            _bodyText.text = page.Body;
            _buttonText.text = page.ButtonLabel;

            // Force button to Restart behavior
            _actionButton.onClick.RemoveAllListeners();
            _actionButton.onClick.AddListener(RestartGame);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}