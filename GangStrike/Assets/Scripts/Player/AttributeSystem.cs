using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerAttribute
    {
        [SerializeField] private float currentValue;
        [SerializeField] private float maxValue;
        [SerializeField] private float minValue;

        public event Action<float, float> OnValueChanged; // (currentValue, maxValue)

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                float oldValue = currentValue;
                currentValue = Mathf.Clamp(value, minValue, maxValue);
                if (Math.Abs(oldValue - currentValue) > 0.001f)
                {
                    OnValueChanged?.Invoke(currentValue, maxValue);
                }
            }
        }

        public float MaxValue
        {
            get => maxValue;
            set
            {
                float oldMax = maxValue;
                maxValue = Mathf.Max(value, minValue);
                currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
                if (Math.Abs(oldMax - maxValue) > 0.001f)
                {
                    OnValueChanged?.Invoke(currentValue, maxValue);
                }
            }
        }

        public float MinValue
        {
            get => minValue;
            set
            {
                float oldMin = minValue;
                minValue = Mathf.Min(value, maxValue);
                currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
                if (Math.Abs(oldMin - minValue) > 0.001f)
                {
                    OnValueChanged?.Invoke(currentValue, maxValue);
                }
            }
        }

        public float NormalizedValue => maxValue > 0 ? currentValue / maxValue : 0f;

        public PlayerAttribute(float initialValue, float maxValue, float minValue = 0f)
        {
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.currentValue = Mathf.Clamp(initialValue, minValue, maxValue);
        }

        public void SetValues(float current, float max, float min = 0f)
        {
            this.maxValue = max;
            this.minValue = min;
            CurrentValue = current;
        }

        public void Modify(float amount)
        {
            CurrentValue += amount;
        }

        public void SetToMax()
        {
            CurrentValue = maxValue;
        }

        public void SetToMin()
        {
            CurrentValue = minValue;
        }

        public bool IsAtMax() => Math.Abs(currentValue - maxValue) < 0.001f;
        public bool IsAtMin() => Math.Abs(currentValue - minValue) < 0.001f;
        public bool IsEmpty() => IsAtMin();
        public bool IsFull() => IsAtMax();
    }

    public class AttributeSystem : MonoBehaviour
    {
        [Header("Attribute Settings")]
        [SerializeField] private float healthMax = 100f;
        [SerializeField] private float staminaMax = 100f;
        [SerializeField] private float ultimateMax = 100f;

        [SerializeField] private PlayerAttribute health;
        [SerializeField] private PlayerAttribute stamina;
        [SerializeField] private PlayerAttribute ultimate;

        // Events for each attribute
        public event Action<float, float> HealthChangeEvent;
        public event Action<float, float> StaminaChangeEvent;
        public event Action<float, float> UltimateChangeEvent;

        // Properties with getters and setters
        public PlayerAttribute Health
        {
            get => health;
            private set => health = value;
        }

        public PlayerAttribute Stamina
        {
            get => stamina;
            private set => stamina = value;
        }

        public PlayerAttribute Ultimate
        {
            get => ultimate;
            private set => ultimate = value;
        }

        // Convenience properties for direct value access
        public float HealthValue
        {
            get => health.CurrentValue;
            set => health.CurrentValue = value;
        }

        public float StaminaValue
        {
            get => stamina.CurrentValue;
            set => stamina.CurrentValue = value;
        }

        public float UltimateValue
        {
            get => ultimate.CurrentValue;
            set => ultimate.CurrentValue = value;
        }

        private void Awake()
        {
            InitializeAttributes();
        }

        private void InitializeAttributes()
        {
            // Initialize attributes with max values
            health = new PlayerAttribute(healthMax, healthMax);
            stamina = new PlayerAttribute(staminaMax, staminaMax);
            ultimate = new PlayerAttribute(0f, ultimateMax); // Ultimate starts at 0

            // Subscribe to attribute change events and forward them
            health.OnValueChanged += (current, max) => HealthChangeEvent?.Invoke(current, max);
            stamina.OnValueChanged += (current, max) => StaminaChangeEvent?.Invoke(current, max);
            ultimate.OnValueChanged += (current, max) => UltimateChangeEvent?.Invoke(current, max);
        }

        // Public methods for modifying attributes
        public void ModifyHealth(float amount)
        {
            health.Modify(amount);
        }

        public void ModifyStamina(float amount)
        {
            stamina.Modify(amount);
        }

        public void ModifyUltimate(float amount)
        {
            ultimate.Modify(amount);
        }

        // Heal and damage methods
        public void Heal(float amount)
        {
            ModifyHealth(amount);
        }

        public void TakeDamage(float amount)
        {
            ModifyHealth(-amount);
        }

        public void ConsumeStamina(float amount)
        {
            ModifyStamina(-amount);
        }

        public void RestoreStamina(float amount)
        {
            ModifyStamina(amount);
        }

        public void GainUltimate(float amount)
        {
            ModifyUltimate(amount);
        }

        public void ConsumeUltimate(float amount)
        {
            ModifyUltimate(-amount);
        }

        // Reset methods
        public void ResetToFull()
        {
            health.SetToMax();
            stamina.SetToMax();
            ultimate.SetToMin(); // Ultimate resets to 0
        }

        public void ResetHealth()
        {
            health.SetToMax();
        }

        public void ResetStamina()
        {
            stamina.SetToMax();
        }

        public void ResetUltimate()
        {
            ultimate.SetToMin();
        }

        // Status check methods
        public bool IsAlive() => !health.IsEmpty();
        public bool HasStamina() => !stamina.IsEmpty();
        public bool CanUseUltimate() => ultimate.IsFull();
    }
}

