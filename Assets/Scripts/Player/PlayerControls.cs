using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    public KeyCode getJumpKey()
    {
        return jumpKey;
    }

    public KeyCode getSprintKey()
    {
        return sprintKey;
    }
}
