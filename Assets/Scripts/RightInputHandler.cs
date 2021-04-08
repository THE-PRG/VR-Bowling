using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RightInputHandler : MonoBehaviour
{
    public InputDevice controller;

    private GameObject hand;
    private GameObject handmodel;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            controller = devices[0];
        }

        handmodel = Resources.Load<GameObject>("Hands/Right Hand Model");

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

    void HandAnimUpdater ()
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
            } else { anim.SetBool("Primary_Button", false); }
        }
        else { anim.SetBool("Primary_Button", false); }
    }
}
