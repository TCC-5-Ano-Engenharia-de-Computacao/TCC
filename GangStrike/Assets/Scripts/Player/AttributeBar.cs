using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class AttributeBar : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image fillImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private TextMeshProUGUI labelText;
        
        [Header("Animation Settings")]
        [SerializeField] private bool useAnimation = true;
        [SerializeField] private float animationSpeed = 5f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        [Header("Color Settings")]
        [SerializeField] private Color fullColor = Color.green;
        [SerializeField] private Color halfColor = Color.yellow;
        [SerializeField] private Color lowColor = Color.red;
        [SerializeField] private Color emptyColor = Color.gray;
        [SerializeField] private Color cantUseColor = Color.red;
        
        [SerializeField] private float lowThreshold = 0.25f;
        [SerializeField] private float halfThreshold = 0.5f;
        
        private float currentValue;
        private float maxValue;
        private float targetFillAmount;
        private float currentFillAmount;
        private bool isAnimating;
        [SerializeField] public bool canUseStamina = true;

        public string AttributeName { get; private set; }
        
        private void Update()
        {
            if (useAnimation && isAnimating)
            {
                AnimateFill();
            }
        }
        
        public void Initialize(string attributeName, Color barColor)
        {
            AttributeName = attributeName;
            
            if (labelText != null)
            {
                labelText.text = attributeName;
            }
            
            if (fillImage != null)
            {
                fullColor = barColor;
            }
            
            
            
            SetValue(0f, 100f);
        }
        
        public void SetValue(float current, float max)
        {
            currentValue = current;
            maxValue = max;
            
            float normalizedValue = max > 0 ? current / max : 0f;
            targetFillAmount = Mathf.Clamp01(normalizedValue);
            
            UpdateValueText();
            UpdateFillAmount();
            UpdateColor();
        }
        
        private void UpdateValueText()
        {
            if (valueText != null)
            {
                valueText.text = $"{currentValue:F0}/{maxValue:F0}";
            }
        }
        
        private void UpdateFillAmount()
        {
            if (fillImage == null) return;
            
            if (useAnimation)
            {
                isAnimating = true;
            }
            else
            {
                fillImage.fillAmount = targetFillAmount;
                currentFillAmount = targetFillAmount;
            }
        }
        
        private void AnimateFill()
        {
            if (fillImage == null) return;
            
            float difference = Mathf.Abs(targetFillAmount - currentFillAmount);
            if (difference < 0.001f)
            {
                fillImage.fillAmount = targetFillAmount;
                currentFillAmount = targetFillAmount;
                isAnimating = false;
                return;
            }
            
            float animationStep = animationSpeed * Time.deltaTime;
            currentFillAmount = Mathf.MoveTowards(currentFillAmount, targetFillAmount, animationStep);
            
            // Apply animation curve
            float t = 1f - (difference / Mathf.Max(difference, 0.001f));
            float curveValue = animationCurve.Evaluate(t);
            
            fillImage.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, curveValue * Time.deltaTime * animationSpeed);
        }
        
        private void UpdateColor()
        {
            if (fillImage == null) return;
            
            Color targetColor;
            if (!canUseStamina)
            {
                targetColor = cantUseColor;
            }
            else
            {
                if (targetFillAmount <= 0f)
                {
                    targetColor = emptyColor;
                }
                else if (targetFillAmount <= lowThreshold)
                {
                    targetColor = lowColor;
                }
                else if (targetFillAmount <= halfThreshold)
                {
                    targetColor = Color.Lerp(lowColor, halfColor,
                        (targetFillAmount - lowThreshold) / (halfThreshold - lowThreshold));
                }
                else
                {
                    targetColor = Color.Lerp(halfColor, fullColor,
                        (targetFillAmount - halfThreshold) / (1f - halfThreshold));
                }
            }

            fillImage.color = targetColor;
        }
        
        public void SetColors(Color full, Color half, Color low, Color empty)
        {
            fullColor = full;
            halfColor = half;
            lowColor = low;
            emptyColor = empty;
            UpdateColor();
        }
        
        public void SetThresholds(float low, float half)
        {
            lowThreshold = Mathf.Clamp01(low);
            halfThreshold = Mathf.Clamp01(half);
            UpdateColor();
        }
        
        public void SetAnimationSettings(bool animate, float speed, AnimationCurve curve)
        {
            useAnimation = animate;
            animationSpeed = speed;
            animationCurve = curve;
        }
        
        // Pulse effect for special events (like taking damage or gaining ultimate)
        public void PulseEffect(float duration = 0.5f, float intensity = 1.2f)
        {
            if (fillImage != null)
            {
                StartCoroutine(PulseCoroutine(duration, intensity));
            }
        }
        
        private System.Collections.IEnumerator PulseCoroutine(float duration, float intensity)
        {
            Vector3 originalScale = fillImage.transform.localScale;
            Vector3 targetScale = originalScale * intensity;
            
            float elapsed = 0f;
            
            // Scale up
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                fillImage.transform.localScale = Vector3.Lerp(originalScale, targetScale, animationCurve.Evaluate(t));
                yield return null;
            }
            
            elapsed = 0f;
            
            // Scale down
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                fillImage.transform.localScale = Vector3.Lerp(targetScale, originalScale, animationCurve.Evaluate(t));
                yield return null;
            }
            
            fillImage.transform.localScale = originalScale;
        }
        
        // Flash effect for critical events
        public void FlashEffect(Color flashColor, float duration = 0.3f)
        {
            if (fillImage != null)
            {
                StartCoroutine(FlashCoroutine(flashColor, duration));
            }
        }
        
        private System.Collections.IEnumerator FlashCoroutine(Color flashColor, float duration)
        {
            Color originalColor = fillImage.color;
            
            float elapsed = 0f;
            
            // Flash to color
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                fillImage.color = Color.Lerp(originalColor, flashColor, t);
                yield return null;
            }
            
            elapsed = 0f;
            
            // Flash back
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                fillImage.color = Color.Lerp(flashColor, originalColor, t);
                yield return null;
            }
            
            UpdateColor(); // Restore proper color based on current value
        }
    }
}

