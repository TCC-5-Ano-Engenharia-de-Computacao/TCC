using UnityEngine;

namespace UI
{
    public class PhysicalCameraBoundary : MonoBehaviour
    {
        [Header("Boundary Settings")]
        [SerializeField] private LayerMask playerLayer = -1;
        [SerializeField] private PhysicsMaterial2D boundaryPhysicsMaterial;
        [SerializeField] private float boundaryThickness = 0.5f;
        [SerializeField] private float boundaryHeight = 20f;
        
        [Header("Movement Settings")]
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private bool followCameraInstantly = false;
        
        // Boundary objects
        private GameObject leftBoundary;
        private GameObject rightBoundary;
        private BoxCollider2D leftCollider;
        private BoxCollider2D rightCollider;
        
        // Movement tracking
        private Vector3 targetPosition;
        private Vector3 velocity = Vector3.zero;
        private Camera mainCamera;
        private FightingGameCameraController cameraController;
        
        // Properties
        public float BoundaryThickness { get => boundaryThickness; set => boundaryThickness = value; }
        public float BoundaryHeight { get => boundaryHeight; set => boundaryHeight = value; }
        public LayerMask PlayerLayer { get => playerLayer; set => playerLayer = value; }
        
        private void Awake()
        {
            mainCamera = GetComponent<Camera>();
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            
            cameraController = GetComponent<FightingGameCameraController>();
            
            CreateBoundaries();
        }
        
        private void Start()
        {
            if (mainCamera != null)
            {
                targetPosition = transform.position;
                UpdateBoundaryPositions();
            }
        }
        
        private void LateUpdate()
        {
            if (mainCamera == null) return;
            
            UpdateBoundarySystem();
        }
        
        private void CreateBoundaries()
        {
            // Create left boundary
            leftBoundary = new GameObject("LeftCameraBoundary");
            leftBoundary.transform.SetParent(transform);
            leftBoundary.layer = gameObject.layer;
            
            leftCollider = leftBoundary.AddComponent<BoxCollider2D>();
            leftCollider.isTrigger = false;
            
            if (boundaryPhysicsMaterial != null)
            {
                leftCollider.sharedMaterial = boundaryPhysicsMaterial;
            }
            
            // Create right boundary
            rightBoundary = new GameObject("RightCameraBoundary");
            rightBoundary.transform.SetParent(transform);
            rightBoundary.layer = gameObject.layer;
            
            rightCollider = rightBoundary.AddComponent<BoxCollider2D>();
            rightCollider.isTrigger = false;
            
            if (boundaryPhysicsMaterial != null)
            {
                rightCollider.sharedMaterial = boundaryPhysicsMaterial;
            }
            
            // Set up collision layers
            SetupCollisionLayers();
            
            Debug.Log("Physical camera boundaries created");
        }
        
        private void SetupCollisionLayers()
        {
            // Ensure boundaries only collide with players
            int boundaryLayer = gameObject.layer;
            
            // Get all layers that should collide with boundaries
            for (int i = 0; i < 32; i++)
            {
                if ((playerLayer.value & (1 << i)) != 0)
                {
                    Physics2D.IgnoreLayerCollision(boundaryLayer, i, false);
                }
                else
                {
                    Physics2D.IgnoreLayerCollision(boundaryLayer, i, true);
                }
            }
        }
        
        private void UpdateBoundarySystem()
        {
            Vector3 currentCameraPos = transform.position;
            
            if (followCameraInstantly)
            {
                targetPosition = currentCameraPos;
            }
            else
            {
                // Smooth follow the camera
                targetPosition = Vector3.SmoothDamp(targetPosition, currentCameraPos, ref velocity, smoothTime);
            }
            
            UpdateBoundaryPositions();
        }
        
        private void UpdateBoundaryPositions()
        {
            if (mainCamera == null || leftBoundary == null || rightBoundary == null) return;
            
            // Calculate camera bounds
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float cameraLeft = targetPosition.x - cameraHalfWidth;
            float cameraRight = targetPosition.x + cameraHalfWidth;
            
            // Position left boundary
            Vector3 leftPos = new Vector3(cameraLeft - boundaryThickness * 0.5f, targetPosition.y, targetPosition.z);
            leftBoundary.transform.position = leftPos;
            
            // Position right boundary
            Vector3 rightPos = new Vector3(cameraRight + boundaryThickness * 0.5f, targetPosition.y, targetPosition.z);
            rightBoundary.transform.position = rightPos;
            
            // Update collider sizes
            leftCollider.size = new Vector2(boundaryThickness, boundaryHeight);
            rightCollider.size = new Vector2(boundaryThickness, boundaryHeight);
        }
        
        public void SetBoundaryMaterial(PhysicsMaterial2D material)
        {
            boundaryPhysicsMaterial = material;
            
            if (leftCollider != null)
            {
                leftCollider.sharedMaterial = material;
            }
            
            if (rightCollider != null)
            {
                rightCollider.sharedMaterial = material;
            }
        }
        
        public void SetBoundaryLayer(int layer)
        {
            if (leftBoundary != null)
            {
                leftBoundary.layer = layer;
            }
            
            if (rightBoundary != null)
            {
                rightBoundary.layer = layer;
            }
            
            SetupCollisionLayers();
        }
        
        public void EnableBoundaries(bool enable)
        {
            if (leftCollider != null)
            {
                leftCollider.enabled = enable;
            }
            
            if (rightCollider != null)
            {
                rightCollider.enabled = enable;
            }
        }
        
        public void SetBoundarySize(float thickness, float height)
        {
            boundaryThickness = thickness;
            boundaryHeight = height;
            UpdateBoundaryPositions();
        }
        
        // Get boundary positions for other systems
        public float GetLeftBoundaryPosition()
        {
            if (mainCamera == null) return 0f;
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            return targetPosition.x - cameraHalfWidth;
        }
        
        public float GetRightBoundaryPosition()
        {
            if (mainCamera == null) return 0f;
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            return targetPosition.x + cameraHalfWidth;
        }
        
        public Vector2 GetBoundaryRange()
        {
            return new Vector2(GetLeftBoundaryPosition(), GetRightBoundaryPosition());
        }
        
        // Debug visualization
        private void OnDrawGizmosSelected()
        {
            if (mainCamera == null) return;
            
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            Vector3 pos = Application.isPlaying ? targetPosition : transform.position;
            
            // Draw boundary positions
            Gizmos.color = Color.red;
            
            // Left boundary
            Vector3 leftCenter = new Vector3(pos.x - cameraHalfWidth - boundaryThickness * 0.5f, pos.y, pos.z);
            Gizmos.DrawWireCube(leftCenter, new Vector3(boundaryThickness, boundaryHeight, 1f));
            
            // Right boundary
            Vector3 rightCenter = new Vector3(pos.x + cameraHalfWidth + boundaryThickness * 0.5f, pos.y, pos.z);
            Gizmos.DrawWireCube(rightCenter, new Vector3(boundaryThickness, boundaryHeight, 1f));
            
            // Draw camera bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                new Vector3(pos.x - cameraHalfWidth, pos.y - boundaryHeight * 0.5f, pos.z),
                new Vector3(pos.x - cameraHalfWidth, pos.y + boundaryHeight * 0.5f, pos.z)
            );
            Gizmos.DrawLine(
                new Vector3(pos.x + cameraHalfWidth, pos.y - boundaryHeight * 0.5f, pos.z),
                new Vector3(pos.x + cameraHalfWidth, pos.y + boundaryHeight * 0.5f, pos.z)
            );
        }
        
        private void OnDestroy()
        {
            if (leftBoundary != null)
            {
                DestroyImmediate(leftBoundary);
            }
            
            if (rightBoundary != null)
            {
                DestroyImmediate(rightBoundary);
            }
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Recreate Boundaries")]
        private void RecreateBoundaries()
        {
            if (leftBoundary != null)
            {
                DestroyImmediate(leftBoundary);
            }
            
            if (rightBoundary != null)
            {
                DestroyImmediate(rightBoundary);
            }
            
            CreateBoundaries();
            UpdateBoundaryPositions();
        }
        
        [ContextMenu("Test Boundary Positions")]
        private void TestBoundaryPositions()
        {
            Debug.Log($"Left Boundary: {GetLeftBoundaryPosition()}, Right Boundary: {GetRightBoundaryPosition()}");
        }
        #endif
    }
}

