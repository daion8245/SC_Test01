using Core;
using Parts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 상점 씬 관리자
    /// 스테이지 클리어 후 다음 스테이지 진입 전 파츠 구매/장착 화면
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        [Header("정보 표시")]
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private TextMeshProUGUI goldText;

        [Header("슬롯 표시")]
        [SerializeField] private TextMeshProUGUI slot1Text;
        [SerializeField] private TextMeshProUGUI slot2Text;

        [Header("파츠 구매 버튼")]
        [SerializeField] private Button buyForcedGuidanceSlot1Button;
        [SerializeField] private Button buyForcedGuidanceSlot2Button;
        [SerializeField] private Button buyTimeStopSlot1Button;
        [SerializeField] private Button buyTimeStopSlot2Button;

        [Header("파츠 프리팹")]
        [SerializeField] private GameObject forcedGuidancePrefab;
        [SerializeField] private GameObject timeStopPrefab;

        [Header("파츠 가격")]
        [SerializeField] private int forcedGuidancePrice = 100;
        [SerializeField] private int timeStopPrice = 150;

        [Header("가격 표시")]
        [SerializeField] private TextMeshProUGUI forcedGuidancePriceText;
        [SerializeField] private TextMeshProUGUI timeStopPriceText;

        [Header("다음 스테이지 버튼")]
        [SerializeField] private Button nextStageButton;

        private DataManager _data;

        private void Start()
        {
            _data = DataManager.Instance;

            RefreshUI();

            if (nextStageButton != null)
                nextStageButton.onClick.AddListener(GoToNextStage);

            if (buyForcedGuidanceSlot1Button != null)
                buyForcedGuidanceSlot1Button.onClick.AddListener(() => BuyParts(PartsType.ForcedGuidanceParts, 0));

            if (buyForcedGuidanceSlot2Button != null)
                buyForcedGuidanceSlot2Button.onClick.AddListener(() => BuyParts(PartsType.ForcedGuidanceParts, 1));

            if (buyTimeStopSlot1Button != null)
                buyTimeStopSlot1Button.onClick.AddListener(() => BuyParts(PartsType.SlowFieldParts, 0));

            if (buyTimeStopSlot2Button != null)
                buyTimeStopSlot2Button.onClick.AddListener(() => BuyParts(PartsType.SlowFieldParts, 1));

            if (forcedGuidancePriceText != null)
                forcedGuidancePriceText.text = $"{forcedGuidancePrice} G";

            if (timeStopPriceText != null)
                timeStopPriceText.text = $"{timeStopPrice} G";
        }

        private void RefreshUI()
        {
            if (_data == null) return;

            if (stageText != null)
                stageText.text = $"Stage {_data.stage + 1} 준비";

            if (goldText != null)
                goldText.text = $"골드: {_data.gold} G";

            RefreshSlotUI();
        }

        private void RefreshSlotUI()
        {
            if (slot1Text != null)
            {
                var slot1 = _data.GetSlot(0);
                slot1Text.text = slot1.type == PartsType.None ? "슬롯 1: 비어있음" : $"슬롯 1: {slot1.type}";
            }

            if (slot2Text != null)
            {
                var slot2 = _data.GetSlot(1);
                slot2Text.text = slot2.type == PartsType.None ? "슬롯 2: 비어있음" : $"슬롯 2: {slot2.type}";
            }
        }

        private void BuyParts(PartsType type, int slotIndex)
        {
            if (_data == null) return;

            int price = type == PartsType.ForcedGuidanceParts ? forcedGuidancePrice : timeStopPrice;

            if (_data.gold < price)
            {
                Debug.Log("골드가 부족합니다.");
                return;
            }

            _data.gold -= price;
            _data.SetSlotType(slotIndex, type);

            Debug.Log($"[상점] {type} 구매 완료 → 슬롯 {slotIndex + 1}");
            RefreshUI();
        }

        private void GoToNextStage()
        {
            SceneManager.LoadScene("MainGame");
        }
    }
}
