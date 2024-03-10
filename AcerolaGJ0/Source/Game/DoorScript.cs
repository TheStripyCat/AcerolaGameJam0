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
    public override void OnStart()
    {
        openDoor = Actor.As<AnimatedModel>().GetParameter("doorOpened");
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
            if (Input.GetKeyDown(KeyboardKeys.Spacebar))
            {
                if (isClosed)
                {
                    openDoor.Value = true;
                    isClosed = false;
                }
                else
                { 
                    openDoor.Value = false;
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
        }
            
    }


}
