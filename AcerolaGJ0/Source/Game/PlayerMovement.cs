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
    [Serialize, ShowInEditor] Actor camera, playerModel;
    [Serialize, ShowInEditor] Prefab portalPrefab;
    private AnimGraphParameter isRunning, isWalking, isReading;
    [Serialize, ShowInEditor] AudioSource playerAudioSource;

    public Float2 camVertMinMax;

    public float rotationSpeed, playerSpeed, mouseSensitivity, grav;
    private float hInput, vInput, hLookDir, vLookDir, targetAngle, incantationTimer, insanity, maxInsaneAngle, camZrot;
    private Vector3 viewDir, camForward, movement, inputDir;
    private bool portalSpawned, camZup;
    
    /// <inheritdoc/>
    public override void OnStart()
    {
        player = Actor as CharacterController;
        isRunning = playerModel.As<AnimatedModel>().GetParameter("isRunning");
        isWalking = playerModel.As<AnimatedModel>().GetParameter("isWalking");
        isReading = playerModel.As<AnimatedModel>().GetParameter("isReading");        
        incantationTimer = 0f;
        insanity = 0f; maxInsaneAngle = 0f;
    }
    
    
    public override void OnUpdate()
    {
        Screen.CursorLock = CursorLockMode.Locked;
        Screen.CursorVisible = false;

        //get direction of the camera
        viewDir = player.Position - new Vector3(camera.Position.X, player.Position.Y, camera.Position.Z);
        camForward = viewDir.Normalized;

        insanity += 0.0001f* Time.DeltaTime;
        
        //calculate camera Z rotation (depends on insanity)
        if ((camZup) && (camZrot < maxInsaneAngle))
        {
            camZrot += insanity;
        }
        else if (camZrot >= maxInsaneAngle)
        {
            camZup = false;
        }
        if ((!camZup) && (camZrot > -maxInsaneAngle))
        {
            camZrot -= insanity;
        }
        else if (camZrot <= -maxInsaneAngle)
        {
            camZup = true;
        }
        //rotate camera
        hLookDir += Input.GetAxis("Mouse X") * mouseSensitivity * Time.DeltaTime;
        vLookDir += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.DeltaTime;
        vLookDir = Mathf.Clamp(vLookDir, camVertMinMax.X, camVertMinMax.Y);
        camera.Parent.Orientation = Quaternion.Lerp(camera.Parent.Orientation, Quaternion.Euler(vLookDir, hLookDir, camZrot), Time.DeltaTime * 20f);

        //find movement vector relative to camera direction
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        inputDir = camForward * vInput + camera.Transform.Right * hInput;
        movement = new Vector3(inputDir.X, grav, inputDir.Z) * playerSpeed;

        //reading incantation and spawning the portal
        if (Input.GetKey(KeyboardKeys.R))
        {
            playerSpeed = 0f;
            isReading.Value = true;
            isRunning.Value = false;
            isWalking.Value = false;            
            incantationTimer += Time.DeltaTime;
            playerAudioSource.Play();
            if (incantationTimer >= 4.2f)
            {
                playerAudioSource.Stop();
                if ((portalSpawned == false) &(PluginManager.GetPlugin<PortalPlugin>().inPortalSpace))
                {
                    PrefabManager.SpawnPrefab(portalPrefab, PluginManager.GetPlugin<PortalPlugin>().portalPosition, PluginManager.GetPlugin<PortalPlugin>().portalSpawnEmpty.Orientation);
                    portalSpawned = true;
                    Debug.Log("portal spawned");
                }
            }
        }
        
        //rotate the character and set speed
        else if (inputDir != Vector3.Zero && Input.GetKey(KeyboardKeys.Shift))
        {
            playerAudioSource.Stop();
            incantationTimer = 0f;
            playerSpeed = 3f;
            isReading.Value = false;
            isRunning.Value = false;
            isWalking.Value = true;
            targetAngle = Mathf.Atan2(inputDir.X, inputDir.Z) * Mathf.RadiansToDegrees;
            playerModel.Orientation = Quaternion.Lerp(playerModel.Orientation, Quaternion.Euler(0f, targetAngle, 0f), rotationSpeed * Time.DeltaTime);
        }
        else if (inputDir != Vector3.Zero)
        {
            playerAudioSource.Stop();
            incantationTimer = 0f;
            playerSpeed = 7f;
            isReading.Value = false;
            isWalking.Value = false;
            isRunning.Value = true;
            targetAngle = Mathf.Atan2(inputDir.X, inputDir.Z) * Mathf.RadiansToDegrees;
            playerModel.Orientation = Quaternion.Lerp(playerModel.Orientation, Quaternion.Euler(0f, targetAngle, 0f), rotationSpeed * Time.DeltaTime);
        }
        else
        {
            playerAudioSource.Stop();
            incantationTimer = 0f;
            isReading.Value = false;
            isRunning.Value = false;
            isWalking.Value = false;
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
