using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// PlayerMovement Script.
/// </summary>
public class PlayerMovement : Script
{
    CharacterController player;
    [Serialize, ShowInEditor] Actor camera;
    
    public Float2 camVertMinMax;

    public float rotationSpeed, playerSpeed, mouseSensitivity, grav;
    private float hInput, vInput, hLookDir, vLookDir, targetAngle;
    private Vector3 viewDir, camForward, movement, inputDir;
    
    /// <inheritdoc/>
    public override void OnStart()
    {
        player = Actor as CharacterController;

    }
    
    
    public override void OnUpdate()
    {
        Screen.CursorLock = CursorLockMode.Locked;
        Screen.CursorVisible = false;

        //get direction of the camera
        viewDir = player.Position - new Vector3(camera.Position.X, player.Position.Y, camera.Position.Z);
        camForward = viewDir.Normalized;

        //rotate camera
        hLookDir += Input.GetAxis("Mouse X") * mouseSensitivity * Time.DeltaTime;
        vLookDir += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.DeltaTime;
        vLookDir = Mathf.Clamp(vLookDir, camVertMinMax.X, camVertMinMax.Y);
        camera.Parent.Orientation = Quaternion.Lerp(camera.Parent.Orientation, Quaternion.Euler(vLookDir, hLookDir, 0f), Time.DeltaTime * 10f);

        //find movement vector relative to camera direction
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        inputDir = camForward * vInput + camera.Transform.Right * hInput;
        movement = new Vector3(inputDir.X, grav, inputDir.Z) * playerSpeed;

        //rotate the character
        if (inputDir != Vector3.Zero)
        {
            targetAngle = Mathf.Atan2(inputDir.X, inputDir.Z) * Mathf.RadiansToDegrees;
            player.LocalOrientation = Quaternion.Lerp(player.LocalOrientation, Quaternion.Euler(0f, targetAngle, 0f), rotationSpeed * Time.DeltaTime);
        }
    
    }
    public override void OnFixedUpdate()
    {
        if (inputDir != Vector3.Zero)
        {
            player.Move(movement);
        }
            
    }
}
