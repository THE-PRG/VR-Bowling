using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VR_RigHandler : MonoBehaviour
{
    private XRRig rig;

    private float speed = 2.8f;
    private float fallingSpeed;
    private CharacterController characterController;
    private Vector3 direction;
    private LayerMask groundLayer;

    private float gravity = 9.81f;

    public bool heightDivider = false;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        CharacterControllerFollow();

        Quaternion quaternion = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);

        GetComponentInChildren<LeftInputHandler>().controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);

        direction = quaternion * new Vector3(primary2DAxisValue.x, 0, primary2DAxisValue.y);

        characterController.Move(direction * Time.fixedDeltaTime * speed);

        //gravity
        if (isGrounded())
        {
            fallingSpeed = 0;
        }
        else { fallingSpeed += gravity * Time.fixedDeltaTime; }

        characterController.Move(Vector3.down * fallingSpeed * Time.fixedDeltaTime);
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.TransformPoint(characterController.center), Vector3.down, characterController.height / 2f + 0.02f, groundLayer);
    }

    void CharacterControllerFollow()
    {
        if (heightDivider) { characterController.height = 0.6f; }
        else { characterController.height = rig.cameraInRigSpaceHeight; }
        Vector3 center = new Vector3(rig.cameraInRigSpacePos.x, rig.cameraInRigSpaceHeight / 2, rig.cameraInRigSpacePos.z);
        characterController.center = center;
    }
}
