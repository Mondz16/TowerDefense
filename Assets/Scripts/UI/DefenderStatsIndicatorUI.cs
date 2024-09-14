using TowerDefense.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    public class DefenderStatsIndicatorUI : MonoBehaviour
    {
        [SerializeField] private Button _upgradeButton;

        private UIManager _uiManager;
        
        public void InjectUIManager(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public void SetDefenderStatsUI(bool isUpgradeable)
        {
            _upgradeButton.interactable = isUpgradeable;
        }

        public void OnUpgradeButtonClicked()
        {
            _uiManager.OnUpgradeDefenderEventTriggered();
        }

        public void OnSellButtonClicked()
        {
            _uiManager.OnSellDefenderEventTriggered();
        }
    }
}