using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class HealthWeapon : MonoBehaviour
{
    PlayerMovement playermovement;
    Animator animator;
    [SerializeField] private Joystick aimJoystick;
    private bool used = false;
    private void Start()
    {
        playermovement = FindFirstObjectByType<PlayerMovement>();
        animator = GetComponent<Animator>();
        if (aimJoystick == null)
        {
            GameObject joystickObj = GameObject.FindGameObjectWithTag("AimJoystick");
            if (joystickObj != null)
            {
                aimJoystick = joystickObj.GetComponent<Joystick>();
            }
        }
    }

    private void Update()
    {
        bool joystickMoved = aimJoystick.Direction.magnitude > 0.3f;
        if (!used && joystickMoved)
        {
            used = true;
            animator.SetTrigger("Use");
            if(playermovement.health<100)
            {
                playermovement.health += 30;
                if(playermovement.health>=playermovement.maxHealth)
                {
                    playermovement.health = playermovement.maxHealth;
                }
            }
            Destroy(gameObject, 1f);
        }
    }
}
