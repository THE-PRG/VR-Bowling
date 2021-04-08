using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeftInputHandler : MonoBehaviour
{
    public InputDevice controller;

    private GameObject hand;
    private GameObject handmodel;

    private Animator anim;

    private float lastPressTime;
    private bool lastTickPressed;

    // Start is called before the first frame update
    void Start()
    {
        lastPressTime = 0;
        lastTickPressed = false;

        Initialize();
    }

    void Initialize ()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            controller = devices[0];
        }

        handmodel = Resources.Load<GameObject>("Hands/Left Hand Model");

        hand = Instantiate(handmodel, transform);
        anim = hand.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isValid)
        {
            HandAnimUpdater();
        }
        else { Initialize(); }
    }

    private void FixedUpdate()
    {
        if (controller.isValid)
        {
            if (controller.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool pressed) && pressed)
            {
                if ((Time.fixedTime - lastPressTime) > 0.1f && !lastTickPressed)
                {
                    if (GameObject.Find("VR_Rig").GetComponent<VR_RigHandler>().heightDivider)
                    {
                        GameObject.Find("VR_Rig").GetComponent<VR_RigHandler>().heightDivider = false;
                    }
                    else
                    {
                        GameObject.Find("VR_Rig").GetComponent<VR_RigHandler>().heightDivider = true;
                    }

                    lastPressTime = Time.fixedTime;
                }

                lastTickPressed = true;
            }
            else { lastTickPressed = false; }
        }   
    }

    void HandAnimUpdater()
    {
        if (controller.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            anim.SetFloat("Grip", gripValue);
        }
        else { anim.SetFloat("Grip", 0); }

        if (controller.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            anim.SetFloat("Trigger", triggerValue);
        }
        else { anim.SetFloat("Trigger", 0); }

        if (controller.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue))
        {
            if (primaryButtonValue)
            {
                anim.SetBool("Primary_Button", true);
            }
            else { anim.SetBool("Primary_Button", false); }
        }
        else { anim.SetBool("Primary_Button", false); }
    }
}
