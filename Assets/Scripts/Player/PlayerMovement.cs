using UnityEngine;

/**
 * A component of the Player that handles its movement.
 */
public class PlayerMovement : MonoBehaviour {

    private const float MOVE_SPEED = 3.5f;

    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Rigidbody body;


    private Vector2 currentMoveDirection; 
    private bool isWalking;

    private void Start() {
        GameInput.Instance.OnMove += GameInput_OnMove;

        currentMoveDirection = Vector2.zero;
        isWalking = false;
        body.freezeRotation = true;
    }

    /**
     * Next 3 functions deal with player movement:
     * 1) Reacting when movement event is fired
     * 2) Handling Movement of Player
     * 3) Setting IsWalking State of Player
     */

    private void GameInput_OnMove(object sender, Vector2 e) {
        currentMoveDirection = e;
    }

    public void HandleWalking() {
        //Move relative to camera looking direction and not strict x-y values
        //X is right, Y is up, Z is forward
        Vector3 moveDirection = new Vector3(currentMoveDirection.x, 0, currentMoveDirection.y);
        Vector3 cameraForward = playerCamera.GetViewCamera().transform.forward;
        Vector3 cameraRight = playerCamera.GetViewCamera().transform.right;

        // Project the forward and right vectors onto the XZ plane (ignoring Y component) to prevent moving up or down
        cameraRight.y = 0;
        cameraForward.y = 0;

        // Handle issue of movement stopping when looking up/down
        if (cameraForward == Vector3.zero) {
            float dotUp = Vector3.Dot(playerCamera.GetViewCamera().transform.forward, Vector3.up);
            if (dotUp > 0.9f) {
                // If facing up, use -camera.transform.up
                cameraForward = -playerCamera.GetViewCamera().transform.up;
            } else if (dotUp < -0.9f) {
                // If facing down, use camera.transform.up
                cameraForward = playerCamera.GetViewCamera().transform.up;
            }
        }

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 relativeMoveDirection = (cameraRight * moveDirection.x + cameraForward * moveDirection.z).normalized;

        Vector3 velocity = relativeMoveDirection * MOVE_SPEED;
        body.velocity = new Vector3(velocity.x, body.velocity.y, velocity.z);

        isWalking = relativeMoveDirection != Vector3.zero;
    }

    public bool IsWalking() {
        return isWalking;
    }
}
