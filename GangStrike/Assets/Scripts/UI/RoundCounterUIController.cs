using DefaultNamespace;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RoundCounterUIController : MonoBehaviour
    {
        private GameRoot gameRoot;
        [SerializeField] private AttributeUIController attributeUIController;
        [SerializeField] private Image firstRoundSprite;
        [SerializeField] private Image secondRoundSprite;

        private void Awake()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            if (gameRoot == null)
            {
                Debug.LogError("RoundCounterUIController: GameRoot not found in the scene!");
                return;
            }
            gameRoot.scoreUpdatedEvent.AddListener(OnScoreUpdated);
        }

        private void Start()
        {
            UpdateRoundIndicators(gameRoot.GetScore());
        }
    
        private void OnScoreUpdated(Vector2 score)
        {
            UpdateRoundIndicators(score);
        }
    
        private void UpdateRoundIndicators(Vector2 score)
        {
            // activates the sprites based on the score
            if(attributeUIController.PlayerAttributeController.GetPlayerId() == 1)
            {
                firstRoundSprite.enabled = score.x >= 1;
                secondRoundSprite.enabled = score.x >= 2;
            }
            else
            {
                firstRoundSprite.enabled = score.y >= 1;
                secondRoundSprite.enabled = score.y >= 2;
            }
        }
    }
}
