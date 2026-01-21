using UnityEngine;
using Acreos.ForkliftSim.Gameplay;
using Acreos.ForkliftSim.UI;
using Acreos.ForkliftSim.Vehicle;

namespace Acreos.ForkliftSim.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Environments")]
        [SerializeField] private GameObject _mission1Group;
        [SerializeField] private GameObject _mission2Group;

        [Header("Vehicle")]
        public ForkliftController Forklift;

        [Header("UI References")]
        [SerializeField] private MissionBriefingUI _briefingUI;

        [Header("Scene References")]
        [SerializeField] private DropZone _targetZone;
        [SerializeField] private ParkingZone _parkingZone;

        [Header("Configuration")]
        [SerializeField] private int _winMissionIndex = 2; // Default index for the victory screen

        [Header("Camera")]
        [SerializeField] private ForkliftCamera _mainCamera; 

        // Public Accessors
        public GameObject Mission1Group => _mission1Group;
        public GameObject Mission2Group => _mission2Group;
        public MissionBriefingUI BriefingUI => _briefingUI;
        public DropZone TargetZone => _targetZone;
        public ParkingZone ParkingZone => _parkingZone;
        public int WinMissionIndex => _winMissionIndex;
        public ForkliftCamera MainCamera => _mainCamera;

        private BaseGameState _currentState;
       
        private void Start()
        {
            // Initial Setup
            if (_mission1Group != null) _mission1Group.SetActive(true);
            if (_mission2Group != null) _mission2Group.SetActive(false);

            // State Sequence Definition
            var drivingState = new DrivingState(this);
            var introState = new BriefingState(this, 0, drivingState);

            SwitchState(introState);
        }

        private void Update()
        {
            _currentState?.Tick();
        }

        private void FixedUpdate()
        {
            _currentState?.FixedTick();
        }

        public void SwitchState(BaseGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
    }
}