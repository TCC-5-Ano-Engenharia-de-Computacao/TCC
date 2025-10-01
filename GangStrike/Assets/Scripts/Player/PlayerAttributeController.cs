using UnityEngine;

namespace Player
{
    public class PlayerAttributeController : MonoBehaviour
    {
        [Header("Player Reference")]
        [SerializeField] private int playerId = 1; // 1 or 2 for player identification
        
        [Header("UI Settings")]
        [SerializeField] private AttributeUIController uiControllerPrefab;
        [SerializeField] private Transform uiParent; // Parent transform for UI elements
        [SerializeField] private Vector3 uiOffset = Vector3.zero; // Offset for UI positioning
        
        private AttributeSystem attributeSystem;
        private AttributeUIController uiController;
        
        // Public properties for easy access
        public AttributeSystem AttributeSystem => attributeSystem;
        public AttributeUIController UIController => uiController;

        private void Awake()
        {
            // Get or add the attribute system component
            attributeSystem = GetComponent<AttributeSystem>();
            if (attributeSystem == null)
            {
                attributeSystem = gameObject.AddComponent<AttributeSystem>();
            }
        }

        private void Start()
        {
            // Initialize UI after all components are ready
            InitializeUI();
        }

        private void InitializeUI()
        {
            if (uiControllerPrefab != null)
            {
                InstantiateUIController();
            }
            else
            {
                Debug.LogWarning($"PlayerAttributeController on {gameObject.name}: UI Controller is not assigned!");
            }
        }

        public void InstantiateUIController()
        {
            if (uiController != null)
            {
                Debug.LogWarning($"UI Controller already exists for player {playerId}");
                return;
            }

            // Determine parent for UI
            Transform parent = uiParent != null ? uiParent : FindUICanvas();
            
            if (parent == null)
            {
                Debug.LogError($"No UI parent found for player {playerId}. Please assign uiParent or ensure a Canvas exists in the scene.");
                return;
            }

            // Instantiate the UI controller
            GameObject uiObject = Instantiate(uiControllerPrefab.gameObject, parent);
            uiController = uiObject.GetComponent<AttributeUIController>();
            
            if (uiController != null)
            {
                // Initialize the UI controller with this player's data
                uiController.Initialize(this);
                
                // Position the UI based on player ID and offset
                PositionUI();
                
                Debug.Log($"UI Controller instantiated for player {playerId}");
            }
            else
            {
                Debug.LogError($"Instantiated UI object does not have AttributeUIController component!");
                Destroy(uiObject);
            }
        }

        private Transform FindUICanvas()
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            return canvas != null ? canvas.transform : null;
        }

        private void PositionUI()
        {
            if (uiController == null) return;

            RectTransform rectTransform = uiController.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Position based on player ID
                Vector2 anchoredPosition;
                
                switch (playerId)
                {
                    case 1:
                        // Player 1 - Left side
                        rectTransform.anchorMin = new Vector2(0f, 1f);
                        rectTransform.anchorMax = new Vector2(0f, 1f);
                        rectTransform.pivot = new Vector2(0f, 1f);
                        anchoredPosition = new Vector2(150f, -20f); // Top-left with margin
                        break;
                    case 2:
                        // Player 2 - Right side
                        rectTransform.anchorMin = new Vector2(1f, 1f);
                        rectTransform.anchorMax = new Vector2(1f, 1f);
                        rectTransform.pivot = new Vector2(1f, 1f);
                        anchoredPosition = new Vector2(-150f, -20f); // Top-right with margin
                        break;
                    default:
                        // Default positioning
                        rectTransform.anchorMin = new Vector2(0.5f, 1f);
                        rectTransform.anchorMax = new Vector2(0.5f, 1f);
                        rectTransform.pivot = new Vector2(0.5f, 1f);
                        anchoredPosition = new Vector2(0f, -20f); // Top-center
                        break;
                }
                
                rectTransform.anchoredPosition = anchoredPosition + (Vector2)uiOffset;
            }
        }

        public int GetPlayerId()
        {
            return playerId;
        }

        public void SetUIParent(Transform parent)
        {
            uiParent = parent;
        }

        public void SetUIOffset(Vector3 offset)
        {
            uiOffset = offset;
            if (uiController != null)
            {
                PositionUI();
            }
        }

        // Convenience methods for attribute access
        public void TakeDamage(float damage)
        {
            attributeSystem.TakeDamage(damage);
        }

        public void Heal(float amount)
        {
            attributeSystem.Heal(amount);
        }

        public void ConsumeStamina(float amount)
        {
            attributeSystem.ConsumeStamina(amount);
        }

        public void RestoreStamina(float amount)
        {
            attributeSystem.RestoreStamina(amount);
        }

        public void GainUltimate(float amount)
        {
            attributeSystem.GainUltimate(amount);
        }

        public void UseUltimate()
        {
            if (attributeSystem.CanUseUltimate())
            {
                attributeSystem.ConsumeUltimate(attributeSystem.Ultimate.MaxValue);
            }
        }

        // Status checks
        public bool IsAlive() => attributeSystem.IsAlive();
        public bool HasStamina() => attributeSystem.HasStamina();
        public bool CanUseUltimate() => attributeSystem.CanUseUltimate();

        private void OnDestroy()
        {
            // Clean up UI when player is destroyed
            if (uiController != null)
            {
                Destroy(uiController.gameObject);
            }
        }

        // Editor helper methods
        #if UNITY_EDITOR
        [ContextMenu("Test Damage")]
        private void TestDamage()
        {
            TakeDamage(10f);
        }

        [ContextMenu("Test Heal")]
        private void TestHeal()
        {
            Heal(10f);
        }

        [ContextMenu("Test Consume Stamina")]
        private void TestConsumeStamina()
        {
            ConsumeStamina(10f);
        }

        [ContextMenu("Test Gain Ultimate")]
        private void TestGainUltimate()
        {
            GainUltimate(10f);
        }
        #endif
    }
}

