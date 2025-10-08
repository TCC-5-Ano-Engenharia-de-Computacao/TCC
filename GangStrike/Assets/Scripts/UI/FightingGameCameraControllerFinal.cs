using System.Collections.Generic;
using StateMachine;
using UnityEngine;

// For PlayerRoot

namespace UI
{
    public class FightingGameCameraController : MonoBehaviour
    {
        [Header("Player References")]
        [SerializeField] private Transform player1;
        [SerializeField] private Transform player2;
        [SerializeField] private List<Transform> additionalPlayers = new List<Transform>();
        [SerializeField] private bool autoFindPlayers = true;
        [SerializeField] private float playerSearchInterval = 1f; // How often to search for new players
        
        [Header("Camera Settings")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float cameraHeight = 5f;
        [SerializeField] private float minZoom = 3f;
        [SerializeField] private float maxZoom = 8f;
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float panSpeed = 3f;
        
        [Header("Player Separation")]
        [SerializeField] private float maxPlayerSeparation = 12f; // Maximum distance players can be apart
        [SerializeField] private float separationZoomFactor = 0f; // How much separation affects zoom
        
        [Header("Background Bounds")]
        [SerializeField] private Transform backgroundLeft;   // Left boundary of the background
        [SerializeField] private Transform backgroundRight;  // Right boundary of the background
        [SerializeField] private float backgroundPadding = 2f; // Padding from background edges
        
        [Header("Camera Bounds")]
        [SerializeField] private bool useCameraBounds = true;
        [SerializeField] private float leftBound = -10f;
        [SerializeField] private float rightBound = 10f;
        
        [Header("Movement Settings")]
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float deadZone = 0.5f; // Dead zone for camera movement
        [SerializeField] private bool followBothPlayers = true;
        [SerializeField] private float playerMeanInfluence = 0.8f; // How much the player mean affects camera position
        
        [Header("Physical Boundaries")]
        [SerializeField] private bool usePhysicalBoundaries = true;
        [SerializeField] private PhysicalCameraBoundary physicalBoundary;
        
        // Private variables
        private Vector3 velocity = Vector3.zero;
        private Vector3 targetPosition;
        private float targetZoom;
        private Vector3 lastCenterPosition;
        private bool isInitialized = false;
        private float lastPlayerSearchTime = 0f;
        
        // Player tracking
        private List<PlayerRoot> trackedPlayerRoots = new List<PlayerRoot>();
        
        // Properties
        public Transform Player1 
        { 
            get => player1; 
            set => player1 = value; 
        }
        
        public Transform Player2 
        { 
            get => player2; 
            set => player2 = value; 
        }
        
        public Camera MainCamera 
        { 
            get => mainCamera; 
            set => mainCamera = value; 
        }
        
        public float MaxPlayerSeparation 
        { 
            get => maxPlayerSeparation; 
            set => maxPlayerSeparation = value; 
        }
        
        public PhysicalCameraBoundary PhysicalBoundary 
        { 
            get => physicalBoundary; 
            set => physicalBoundary = value; 
        }
        
        public List<PlayerRoot> TrackedPlayers => trackedPlayerRoots;
        
        private void Awake()
        {
            if (mainCamera == null)
            {
                mainCamera = GetComponent<Camera>();
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                }
            }
            
            // Get or create physical boundary component
            if (usePhysicalBoundaries && physicalBoundary == null)
            {
                physicalBoundary = GetComponent<PhysicalCameraBoundary>();
                if (physicalBoundary == null)
                {
                    physicalBoundary = gameObject.AddComponent<PhysicalCameraBoundary>();
                }
            }
            
            Initialize();
        }
        
        private void Start()
        {
            if (!isInitialized)
            {
                Initialize();
            }
            
            // Initial player search
            if (autoFindPlayers)
            {
                FindAllPlayerRoots();
            }
        }
        
        private void Update()
        {
            // Periodically search for new players if auto-find is enabled
            if (autoFindPlayers && Time.time > lastPlayerSearchTime + playerSearchInterval)
            {
                FindAllPlayerRoots();
                lastPlayerSearchTime = Time.time;
            }
        }
        
        private void LateUpdate()
        {
            if (!isInitialized || !HasValidPlayers())
            {
                return;
            }
            
            UpdateCameraPosition();
            UpdateCameraZoom();
        }
        
        public void Initialize()
        {
            if (mainCamera == null)
            {
                Debug.LogError("FightingGameCameraController: No camera assigned!");
                return;
            }
            
            // Calculate initial bounds if background objects are assigned
            if (backgroundLeft != null && backgroundRight != null)
            {
                leftBound = backgroundLeft.position.x + backgroundPadding;
                rightBound = backgroundRight.position.x - backgroundPadding;
                useCameraBounds = true;
            }
            
            // Set initial camera position
            if (HasValidPlayers())
            {
                Vector3 centerPosition = GetPlayersCenter();
                transform.position = new Vector3(centerPosition.x, cameraHeight, centerPosition.z);
                lastCenterPosition = centerPosition;
                targetPosition = transform.position;
                targetZoom = mainCamera.orthographicSize;
            }
            
            // Initialize physical boundaries
            if (usePhysicalBoundaries && physicalBoundary != null)
            {
                physicalBoundary.EnableBoundaries(true);
            }
            
            isInitialized = true;
            Debug.Log("FightingGameCameraController initialized successfully");
        }
        
        private void FindAllPlayerRoots()
        {
            // Find all PlayerRoot components in the scene
            PlayerRoot[] allPlayerRoots = FindObjectsOfType<PlayerRoot>();
            
            // Clear current tracking
            trackedPlayerRoots.Clear();
            additionalPlayers.Clear();
            
            // Add all found players to tracking
            foreach (PlayerRoot playerRoot in allPlayerRoots)
            {
                if (playerRoot != null && playerRoot.gameObject.activeInHierarchy)
                {
                    trackedPlayerRoots.Add(playerRoot);
                }
            }
            
            // Assign first two players to player1 and player2
            if (trackedPlayerRoots.Count >= 1)
            {
                player1 = trackedPlayerRoots[0].transform;
            }
            
            if (trackedPlayerRoots.Count >= 2)
            {
                player2 = trackedPlayerRoots[1].transform;
            }
            
            // Add remaining players to additional players list
            for (int i = 2; i < trackedPlayerRoots.Count; i++)
            {
                additionalPlayers.Add(trackedPlayerRoots[i].transform);
            }
            
            if (trackedPlayerRoots.Count > 0)
            {
                Debug.Log($"Found and tracking {trackedPlayerRoots.Count} PlayerRoot objects");
                
                // Log player names for debugging
                for (int i = 0; i < trackedPlayerRoots.Count; i++)
                {
                    Debug.Log($"Player {i + 1}: {trackedPlayerRoots[i].gameObject.name}");
                }
            }
            else
            {
                Debug.LogWarning("No PlayerRoot objects found in scene");
            }
        }
        
        private bool HasValidPlayers()
        {
            return player1 != null && player2 != null;
        }
        
        private Vector3 GetPlayersCenter()
        {
            if (!HasValidPlayers()) return transform.position;
            
            Vector3 center = (player1.position + player2.position) * 0.5f;
            
            // Include additional players in center calculation
            if (additionalPlayers.Count > 0)
            {
                Vector3 totalPosition = player1.position + player2.position;
                int totalPlayers = 2;
                
                foreach (Transform player in additionalPlayers)
                {
                    if (player != null)
                    {
                        totalPosition += player.position;
                        totalPlayers++;
                    }
                }
                
                center = totalPosition / totalPlayers;
            }
            
            return center;
        }
        
        private Vector3 GetPlayersMean()
        {
            // Calculate the X mean of the two main players for boundary positioning
            if (!HasValidPlayers()) return transform.position;
            
            float meanX = (player1.position.x + player2.position.x) * 0.5f;
            Vector3 center = GetPlayersCenter();
            
            return new Vector3(meanX, center.y, center.z);
        }
        
        private float GetPlayersMaxDistance()
        {
            if (!HasValidPlayers()) return 0f;
            
            float maxDistance = Vector3.Distance(player1.position, player2.position);
            
            // Check distance to additional players
            foreach (Transform player in additionalPlayers)
            {
                if (player != null)
                {
                    float dist1 = Vector3.Distance(player1.position, player.position);
                    float dist2 = Vector3.Distance(player2.position, player.position);
                    maxDistance = Mathf.Max(maxDistance, dist1, dist2);
                }
            }
            
            return maxDistance;
        }
        
        private void UpdateCameraPosition()
        {
            Vector3 centerPosition = GetPlayersCenter();
            Vector3 meanPosition = GetPlayersMean();
            
            // Blend between center position and mean position based on playerMeanInfluence
            Vector3 targetCameraPosition = Vector3.Lerp(centerPosition, meanPosition, playerMeanInfluence);
            
            // Check if both players are moving in the same direction
            if (followBothPlayers)
            {
                Vector3 movement = targetCameraPosition - lastCenterPosition;
                
                // Only move camera if movement is significant (outside dead zone)
                if (movement.magnitude > deadZone)
                {
                    targetPosition.x = targetCameraPosition.x;
                }
            }
            else
            {
                targetPosition.x = targetCameraPosition.x;
            }
            
            // Apply camera bounds
            if (useCameraBounds)
            {
                float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
                float minX = leftBound + cameraHalfWidth;
                float maxX = rightBound - cameraHalfWidth;
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            }
            
            targetPosition.y = cameraHeight;
            targetPosition.z = targetCameraPosition.z;
            
            // Smooth camera movement
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            
            lastCenterPosition = targetCameraPosition;
        }
        
        private void UpdateCameraZoom()
        {
            float playersDistance = GetPlayersMaxDistance();
            
            // Calculate desired zoom based on player separation
            // Use a more sophisticated zoom calculation that considers the separation factor
            float normalizedDistance = Mathf.Clamp01(playersDistance / maxPlayerSeparation);
            float desiredZoom = Mathf.Lerp(minZoom, maxZoom, normalizedDistance * separationZoomFactor);
            targetZoom = Mathf.Clamp(desiredZoom, minZoom, maxZoom);
            
            // Smooth zoom transition
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        }
        
        // Public methods for external control
        public void SetPlayers(Transform p1, Transform p2)
        {
            player1 = p1;
            player2 = p2;
            Initialize();
        }
        
        public void AddPlayer(Transform player)
        {
            if (player != null && !additionalPlayers.Contains(player))
            {
                additionalPlayers.Add(player);
            }
        }
        
        public void RemovePlayer(Transform player)
        {
            additionalPlayers.Remove(player);
            
            // Also remove from tracked players
            PlayerRoot playerRoot = player.GetComponent<PlayerRoot>();
            if (playerRoot != null)
            {
                trackedPlayerRoots.Remove(playerRoot);
            }
            
            // Reassign main players if needed
            if (player == player1 && additionalPlayers.Count > 0)
            {
                player1 = additionalPlayers[0];
                additionalPlayers.RemoveAt(0);
            }
            else if (player == player2 && additionalPlayers.Count > 0)
            {
                player2 = additionalPlayers[0];
                additionalPlayers.RemoveAt(0);
            }
        }
        
        public void ForcePlayerSearch()
        {
            FindAllPlayerRoots();
        }
        
        public void SetAutoFindPlayers(bool enable)
        {
            autoFindPlayers = enable;
        }
        
        public void SetPlayerSearchInterval(float interval)
        {
            playerSearchInterval = interval;
        }
        
        public void SetBackgroundBounds(Transform left, Transform right)
        {
            backgroundLeft = left;
            backgroundRight = right;
            
            if (left != null && right != null)
            {
                leftBound = left.position.x + backgroundPadding;
                rightBound = right.position.x - backgroundPadding;
                useCameraBounds = true;
            }
        }
        
        public void SetCameraBounds(float left, float right)
        {
            leftBound = left;
            rightBound = right;
            useCameraBounds = true;
        }
        
        public void FocusOnPlayers()
        {
            if (HasValidPlayers())
            {
                Vector3 center = GetPlayersCenter();
                targetPosition = new Vector3(center.x, cameraHeight, center.z);
            }
        }
        
        public void EnablePhysicalBoundaries(bool enable)
        {
            usePhysicalBoundaries = enable;
            
            if (physicalBoundary != null)
            {
                physicalBoundary.EnableBoundaries(enable);
            }
        }
        
        public Vector2 GetCameraBounds()
        {
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            return new Vector2(
                transform.position.x - cameraHalfWidth,
                transform.position.x + cameraHalfWidth
            );
        }
        
        // Get the current player mean position (useful for other systems)
        public Vector3 GetCurrentPlayerMean()
        {
            return GetPlayersMean();
        }
        
        // Get information about tracked players
        public int GetPlayerCount()
        {
            return trackedPlayerRoots.Count;
        }
        
        public PlayerRoot GetPlayerRoot(int index)
        {
            if (index >= 0 && index < trackedPlayerRoots.Count)
            {
                return trackedPlayerRoots[index];
            }
            return null;
        }
        
        public List<PlayerRoot> GetAllPlayerRoots()
        {
            return new List<PlayerRoot>(trackedPlayerRoots);
        }
        
        // Debug visualization
        private void OnDrawGizmosSelected()
        {
            if (!isInitialized) return;
            
            // Draw camera bounds
            if (useCameraBounds)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(new Vector3(leftBound, 0, -10), new Vector3(leftBound, 10, -10));
                Gizmos.DrawLine(new Vector3(rightBound, 0, -10), new Vector3(rightBound, 10, -10));
            }
            
            // Draw player separation constraint
            if (HasValidPlayers())
            {
                Vector3 center = GetPlayersCenter();
                Vector3 mean = GetPlayersMean();
                
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(center, maxPlayerSeparation * 0.5f);
                
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(mean, 0.5f);
                
                // Draw line between center and mean
                Gizmos.color = Color.green;
                Gizmos.DrawLine(center, mean);
                
                // Draw connections to all tracked players
                Gizmos.color = Color.cyan;
                foreach (PlayerRoot playerRoot in trackedPlayerRoots)
                {
                    if (playerRoot != null)
                    {
                        Gizmos.DrawLine(center, playerRoot.transform.position);
                    }
                }
            }
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Force Find Players")]
        private void EditorForceFindPlayers()
        {
            FindAllPlayerRoots();
        }
        
        [ContextMenu("Log Tracked Players")]
        private void LogTrackedPlayers()
        {
            Debug.Log($"Currently tracking {trackedPlayerRoots.Count} players:");
            for (int i = 0; i < trackedPlayerRoots.Count; i++)
            {
                if (trackedPlayerRoots[i] != null)
                {
                    Debug.Log($"Player {i + 1}: {trackedPlayerRoots[i].gameObject.name}");
                }
            }
        }
        #endif
    }
}

