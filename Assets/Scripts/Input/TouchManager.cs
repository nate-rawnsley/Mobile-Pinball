using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

//This script controls the player's inputs and their use in controlling flippers and launchers.
//touchPositonAction returns the position that was touched, used to determine which flipper to move.
//touchPressAction detects when the screen is being touched, triggering either event.
//touchReleaseAction returns the delta value when the input is held, used to move the launchers.
public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction touchReleaseAction;

    public UnityEvent leftFlipper;
    public UnityEvent rightFlipper;

    private bool launcherActive = true;
    private bool dead;

    [SerializeField] private Launcher[] launchers;
    
    //Awake through OnDisable (and part of TouchPressed) are largely taken from a tutorial by 'samyam' on youtube.
    //"How to use TOUCH with the Input System in Unity" (28/11/2022)
    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        touchPositionAction = playerInput.actions.FindAction("TouchPosition");
        touchPressAction = playerInput.actions.FindAction("TouchPress");
        touchReleaseAction = playerInput.actions.FindAction("TouchRelease");
    }

    private void OnEnable() {
        //Tells the code to start listening to whether the screen has been pressed.
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable() {
        touchPressAction.performed -= TouchPressed;
    }

    private void Update() {
        if (touchPressAction.IsInProgress()) {
            float delta = touchReleaseAction.ReadValue<Vector2>().y;
            if (launcherActive) {
                foreach (Launcher launcher in launchers) {
                    launcher.OnHold(delta);
                }
            }
        } else {
            foreach (Launcher launcher in launchers) {
                launcher.OnRelease();
            }
        }
    }

    private void TouchPressed(InputAction.CallbackContext context) {
        if (!launcherActive && !dead && Time.timeScale == 1) {
            Vector2 position = touchPositionAction.ReadValue<Vector2>();
            if (position.x < Screen.width / 2) {
                leftFlipper.Invoke();
            } else {
                rightFlipper.Invoke();
            }
        }
    }

    //Both functions below are called by events from Ball
    public void SetLaunchActive(bool state) {
        launcherActive = state;
    }

    public void GameOver() {
        dead = true;
    }
}
