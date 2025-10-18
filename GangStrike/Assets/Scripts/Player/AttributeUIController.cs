using UnityEngine;

namespace Player
{
    public class AttributeUIController : MonoBehaviour
    {
        [Header("Attribute Bars")]
        [SerializeField] private AttributeBar healthBar;
        [SerializeField] private AttributeBar staminaBar;
        [SerializeField] private AttributeBar ultimateBar;
        
        [Header("Player Info")]
        [SerializeField] private TMPro.TextMeshProUGUI playerNameText;
        
        [Header("Bar Colors")]
        [SerializeField] private Color healthColor = Color.green;
        [SerializeField] private Color staminaColor = Color.magenta;
        [SerializeField] private Color ultimateColor = Color.cyan;
        
        [Header("Effects")]
        [SerializeField] private bool enableDamageFlash = true;
        [SerializeField] private bool enableHealPulse = false;
        [SerializeField] private bool enableUltimatePulse = false;
        [SerializeField] private Color damageFlashColor = Color.white;
        [SerializeField] private Color healPulseColor = Color.green;
        
        private PlayerAttributeController playerAttributeController;
        private AttributeSystem attributeSystem;
        private bool isInitialized = false;
        
        // Previous values for effect detection
        private float previousHealth;
        private float previousStamina;
        private float previousUltimate;
        
        public PlayerAttributeController PlayerAttributeController => playerAttributeController;
        public bool IsInitialized => isInitialized;
        
        public void Initialize(PlayerAttributeController controller)
        {
            if (isInitialized)
            {
                Debug.LogWarning("AttributeUIController is already initialized!");
                return;
            }
            
            playerAttributeController = controller;
            attributeSystem = controller.AttributeSystem;
            
            if (attributeSystem == null)
            {
                Debug.LogError("AttributeSystem is null in PlayerAttributeController!");
                return;
            }
            
            SetupBars();
            SubscribeToEvents();
            UpdatePlayerName();
            InitializeValues();
            
            isInitialized = true;
            
            //Debug.Log($"AttributeUIController initialized for Player {controller.GetPlayerId()}");
        }
        
        private void SetupBars()
        {
            if (healthBar != null)
            {
                healthBar.Initialize("Health", healthColor);
            }
            
            if (staminaBar != null)
            {
                staminaBar.Initialize("Stamina", staminaColor);
            }
            
            if (ultimateBar != null)
            {
                ultimateBar.Initialize("Ultimate", ultimateColor);
            }
        }
        
        private void SubscribeToEvents()
        {
            // Subscribe to attribute change events
            attributeSystem.HealthChangeEvent += HealthChanged;
            attributeSystem.StaminaChangeEvent += StaminaChanged;
            attributeSystem.UltimateChangeEvent += UltimateChanged;
        }
        
        private void UnsubscribeFromEvents()
        {
            if (attributeSystem != null)
            {
                attributeSystem.HealthChangeEvent -= HealthChanged;
                attributeSystem.StaminaChangeEvent -= StaminaChanged;
                attributeSystem.UltimateChangeEvent -= UltimateChanged;
            }
        }
        
        private void UpdatePlayerName()
        {
            if (playerNameText != null && playerAttributeController != null)
            {
                playerNameText.text = $"Player {playerAttributeController.GetPlayerId()}";
            }
        }
        
        private void InitializeValues()
        {
            // Set initial values and store them for effect detection
            if (attributeSystem != null)
            {
                previousHealth = attributeSystem.HealthValue;
                previousStamina = attributeSystem.StaminaValue;
                previousUltimate = attributeSystem.UltimateValue;
                
                HealthChanged(attributeSystem.HealthValue, attributeSystem.Health.MaxValue);
                StaminaChanged(attributeSystem.StaminaValue, attributeSystem.Stamina.MaxValue);
                UltimateChanged(attributeSystem.UltimateValue, attributeSystem.Ultimate.MaxValue);
            }
        }
        
        private void HealthChanged(float currentValue, float maxValue)
        {
            if (healthBar != null)
            {
                healthBar.SetValue(currentValue, maxValue);
                
                // Trigger effects based on change
                if (isInitialized)
                {
                    if (currentValue < previousHealth && enableDamageFlash)
                    {
                        // Damage taken
                        healthBar.FlashEffect(damageFlashColor);
                    }
                    else if (currentValue > previousHealth && enableHealPulse)
                    {
                        // Healing received
                        healthBar.PulseEffect();
                    }
                }
                
                previousHealth = currentValue;
            }
        }
        
        private void StaminaChanged(float currentValue, float maxValue)
        {
            if (staminaBar != null)
            {
                staminaBar.SetValue(currentValue, maxValue);
                previousStamina = currentValue;
            }
        }
        
        private void UltimateChanged(float currentValue, float maxValue)
        {
            if (ultimateBar != null)
            {
                ultimateBar.SetValue(currentValue, maxValue);
                
                // Trigger effects for ultimate changes
                if (isInitialized && enableUltimatePulse)
                {
                    if (currentValue > previousUltimate)
                    {
                        // Ultimate gained
                        ultimateBar.PulseEffect();
                    }
                    else if (currentValue == 0f && previousUltimate > 0f)
                    {
                        // Ultimate used
                        ultimateBar.FlashEffect(ultimateColor);
                    }
                }
                
                previousUltimate = currentValue;
            }
        }
        
        public void SetBarColors(Color health, Color stamina, Color ultimate)
        {
            healthColor = health;
            staminaColor = stamina;
            ultimateColor = ultimate;
            
            if (isInitialized)
            {
                healthBar?.SetColors(health, health * 0.7f, Color.green, Color.gray);
                staminaBar?.SetColors(stamina, stamina * 0.7f, Color.magenta, Color.gray);
                ultimateBar?.SetColors(ultimate, ultimate * 0.7f, Color.cyan, Color.gray);
            }
        }
        
        public void SetEffectSettings(bool damageFlash, bool healPulse, bool ultimatePulse)
        {
            enableDamageFlash = damageFlash;
            enableHealPulse = healPulse;
            enableUltimatePulse = ultimatePulse;
        }
        
        public void SetPlayerName(string name)
        {
            if (playerNameText != null)
            {
                playerNameText.text = name;
            }
        }
        
        // Manual update methods (in case events don't fire)
        public void ForceUpdateAllBars()
        {
            if (!isInitialized || attributeSystem == null) return;
            
            HealthChanged(attributeSystem.HealthValue, attributeSystem.Health.MaxValue);
            StaminaChanged(attributeSystem.StaminaValue, attributeSystem.Stamina.MaxValue);
            UltimateChanged(attributeSystem.UltimateValue, attributeSystem.Ultimate.MaxValue);
        }
        
        public void ShowBars(bool show)
        {
            gameObject.SetActive(show);
        }
        
        public void SetBarVisibility(bool health, bool stamina, bool ultimate)
        {
            if (healthBar != null) healthBar.gameObject.SetActive(health);
            if (staminaBar != null) staminaBar.gameObject.SetActive(stamina);
            if (ultimateBar != null) ultimateBar.gameObject.SetActive(ultimate);
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }
        
        
        // Editor helper methods
        #if UNITY_EDITOR
        [ContextMenu("Test Damage Effect")]
        private void TestDamageEffect()
        {
            if (healthBar != null)
            {
                healthBar.FlashEffect(damageFlashColor);
            }
        }
        
        [ContextMenu("Test Heal Effect")]
        private void TestHealEffect()
        {
            if (healthBar != null)
            {
                healthBar.PulseEffect();
            }
        }
        
        [ContextMenu("Test Ultimate Effect")]
        private void TestUltimateEffect()
        {
            if (ultimateBar != null)
            {
                ultimateBar.PulseEffect();
            }
        }
        
        [ContextMenu("Force Update All Bars")]
        private void EditorForceUpdate()
        {
            ForceUpdateAllBars();
        }
        #endif
        
        
    }
}

