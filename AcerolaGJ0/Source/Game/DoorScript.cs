using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// DoorScript Script.
/// </summary>
public class DoorScript : Script
{   
    private AnimGraphParameter openDoor;
    private bool isClosed = true;
    private bool isInteractable;
    [Serialize, ShowInEditor] Collider interractionTrigger;
    [Serialize, ShowInEditor] UIControl openPrompt, closePrompt;
    [Serialize, ShowInEditor] AudioClip openDoorSound, closeDoorSound;
    private AudioSource doorAudioSource;
    public override void OnStart()
    {
        openDoor = Actor.As<AnimatedModel>().GetParameter("doorOpened");
        doorAudioSource = Actor.AddChild<AudioSource>();
    }
    public override void OnEnable()
    {
        interractionTrigger.TriggerEnter += IsInteractable;
        interractionTrigger.TriggerExit += IsNotInteractable;
    }

    public override void OnDisable()
    {
        interractionTrigger.TriggerEnter -= IsInteractable;
        interractionTrigger.TriggerExit -= IsNotInteractable;
    }
    public override void OnUpdate()
    {
        
        if (isInteractable)
        {
            if(isClosed)
            {
                if (!openPrompt.IsActive)
                {
                    openPrompt.IsActive = true;                    
                }                
            }
            else if (!isClosed)
            {                
                if (!closePrompt.IsActive)
                {
                    closePrompt.IsActive = true;                    
                }
            }
            if (Input.GetKeyDown(KeyboardKeys.E))
            {
                if (isClosed)
                {
                    openDoor.Value = true;
                    doorAudioSource.Clip = openDoorSound;
                    doorAudioSource.Play();
                    openPrompt.IsActive = false;
                    closePrompt.IsActive = true;
                    isClosed = false;
                }
                else
                {
                    closePrompt.IsActive = false;
                    openPrompt.IsActive = true;
                    openDoor.Value = false;
                    doorAudioSource.Clip = closeDoorSound;
                    doorAudioSource.Play();
                    isClosed = true;
                }
            }

        }        
        
    }
    public void IsInteractable(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            isInteractable = true;            
        }
        
    }
    public void IsNotInteractable(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            isInteractable = false;
            openPrompt.IsActive = false;
            closePrompt.IsActive = false;
        }
            
    }


}
