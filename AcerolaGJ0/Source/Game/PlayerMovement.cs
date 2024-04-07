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
    [Serialize, ShowInEditor] Actor camera, playerModel, worms;
    [Serialize, ShowInEditor] Prefab portalPrefab;
    [Serialize, ShowInEditor] AnimatedModel vines1, vines2;
    private AnimGraphParameter isRunning, isWalking, isReading, insaneRun, isRestoringSanity, vines1Mad, vines2Mad;
    [Serialize, ShowInEditor] AudioSource playerAudioSource;
    [Serialize, ShowInEditor] AudioClip incantation, madness;

    public Float2 camVertMinMax;
    public float insanity, maxInsaneAngle, insanitySpeed;

    public float rotationSpeed, mouseSensitivity, grav;
    private float hInput, vInput, hLookDir, vLookDir, playerSpeed, targetAngle, incantationTimer, sanityRestoringTimer, camZrot;
    private Vector3 viewDir, camForward, movement, inputDir;
    private bool portalSpawned, camZup;
    private const float runningSpeed = 500f;
    private const float walkingSpeed = 220f;
    
    /// <inheritdoc/>
    public override void OnStart()
    {
        playerSpeed = runningSpeed;
        Screen.CursorLock = CursorLockMode.Locked;
        Screen.CursorVisible = false;
        player = Actor as CharacterController;
        isRunning = playerModel.As<AnimatedModel>().GetParameter("isRunning");
        isWalking = playerModel.As<AnimatedModel>().GetParameter("isWalking");
        isReading = playerModel.As<AnimatedModel>().GetParameter("isReading");
        insaneRun = playerModel.As<AnimatedModel>().GetParameter("insane");
        isRestoringSanity = playerModel.As<AnimatedModel>().GetParameter("restoringSanity");
        vines1Mad = vines1.GetParameter("tooMad");
        vines2Mad = vines2.GetParameter("tooMad");
        incantationTimer = 0f;
        insanity = 0f; maxInsaneAngle = 0f; insanitySpeed = 0.0002f;
    }
    
    
    public override void OnUpdate()
    {
        
        //get direction of the camera
        viewDir = player.Position - new Vector3(camera.Position.X, player.Position.Y, camera.Position.Z);
        camForward = viewDir.Normalized;

        if (insanity < 0.15f)
        {
            insanity += insanitySpeed * Time.DeltaTime;
        }
        if (maxInsaneAngle < 20f)
        {
            maxInsaneAngle += 0.00001f;
        }

        //set parameter for insane running animation
        if (insanity * 5f > 1)
        {
            insaneRun.Value = 1f;
        }
        else
        {
            insaneRun.Value = insanity * 8f;
        } 
        //enable worms
        if ((insanity >= 0.05f)&&(!worms.IsActive))
        {
            worms.IsActive = true;
            playerAudioSource.Clip = madness;
            playerAudioSource.IsLooping = true;
            if (!Input.GetKey(KeyboardKeys.R))
            {
                playerAudioSource.Play();
            }
                      
        }
        else if ((insanity < 0.05f)&&(worms.IsActive))
        {
            worms.IsActive = false;
            if (playerAudioSource.Clip == madness)
            {
                playerAudioSource.Stop();
                playerAudioSource.IsLooping = false;
            }
        }
        //enable vines
        if (insanity >= 0.085f)
        {
            vines1Mad.Value = true;
            vines2Mad.Value = true;
        }
        else if (insanity < 0.085f)
        {
            vines1Mad.Value = false;
            vines2Mad.Value = false;
        }

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
        inputDir = inputDir.Normalized;
        movement += new Vector3(inputDir.X, grav, inputDir.Z) * playerSpeed * Time.DeltaTime;

        //reading incantation and spawning the portal
        if ((Input.GetKey(KeyboardKeys.R))&&(!portalSpawned))
        {
            playerSpeed = 0f;
            isReading.Value = true;
            isRunning.Value = false;
            isRestoringSanity.Value = false;
            isWalking.Value = false;            
            incantationTimer += Time.DeltaTime;
            playerAudioSource.Clip = incantation;
            playerAudioSource.Play();
            if (incantationTimer >= 4.2f)
            {
                playerAudioSource.Stop();
                if ((portalSpawned == false) &(PluginManager.GetPlugin<PortalPlugin>().inPortalSpace))
                {
                    PrefabManager.SpawnPrefab(portalPrefab, PluginManager.GetPlugin<PortalPlugin>().portalPosition, PluginManager.GetPlugin<PortalPlugin>().portalSpawnEmpty.Orientation);
                    portalSpawned = true;
                    insanitySpeed += 0.002f;
                    maxInsaneAngle += 5f;
                    insanity += 0.02f;
                    if (maxInsaneAngle > 20)
                    {
                        maxInsaneAngle = 20;
                    }


                }
            }
        }
        //restoring sanity
        else if (Input.GetKey(KeyboardKeys.F))
        {
            playerSpeed = 0f;
            isRestoringSanity.Value = true;
            isRunning.Value = false;
            isWalking.Value = false;
            isReading.Value = false;
            sanityRestoringTimer += Time.DeltaTime;
            if (sanityRestoringTimer >= 5.3f)
            {
                insanity = 0.01f; maxInsaneAngle = 1f; insanitySpeed = 0.0002f; camZrot = 0f;
                isRestoringSanity.Value = false;
            }
        }
        
        //rotate the character and set speed
        else if (inputDir != Vector3.Zero && Input.GetKey(KeyboardKeys.Shift))
        {
            
            incantationTimer = 0f;
            sanityRestoringTimer = 0f;
            playerSpeed = walkingSpeed;
            isReading.Value = false;
            isRestoringSanity.Value = false;
            isRunning.Value = false;
            isWalking.Value = true;
            targetAngle = Mathf.Atan2(inputDir.X, inputDir.Z) * Mathf.RadiansToDegrees;
            playerModel.Orientation = Quaternion.Lerp(playerModel.Orientation, Quaternion.Euler(0f, targetAngle, 0f), rotationSpeed * Time.DeltaTime);
        }
        else if (inputDir != Vector3.Zero)
        {
            
            incantationTimer = 0f;
            sanityRestoringTimer = 0f;
            playerSpeed = runningSpeed;
            isReading.Value = false;
            isRestoringSanity.Value = false;
            isWalking.Value = false;
            isRunning.Value = true;
            targetAngle = Mathf.Atan2(inputDir.X, inputDir.Z) * Mathf.RadiansToDegrees;
            playerModel.Orientation = Quaternion.Lerp(playerModel.Orientation, Quaternion.Euler(0f, targetAngle, 0f), rotationSpeed * Time.DeltaTime);
        }
        else
        {
            if (playerAudioSource.Clip == incantation)
            {
                playerAudioSource.Stop();
            }
            incantationTimer = 0f;
            sanityRestoringTimer = 0f;
            isReading.Value = false;
            isRestoringSanity.Value = false;
            isRunning.Value = false;
            isWalking.Value = false;
        }
        

    }
    public override void OnFixedUpdate()
    {
        if (inputDir != Vector3.Zero)
        {
            player.Move(movement);
            movement = Vector3.Zero;
        }
            
    }
}
