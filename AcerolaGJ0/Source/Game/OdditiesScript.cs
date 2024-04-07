using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// OdditiesScript Script.
/// </summary>
public class OdditiesScript : Script
{
    public int oddityListNumber, tileOccupied;
    [Serialize, ShowInEditor] Collider oddityCollider;
    public override void OnStart()
    {
        
    }

    /// <inheritdoc/>
    public override void OnEnable()
    {
        if (oddityCollider != null)
        {
            oddityCollider.TriggerEnter += OddityClose;
        }
        tileOccupied = 9;
    }
        

    /// <inheritdoc/>
    public override void OnDisable()
    {
        if (oddityCollider != null)
        {
            oddityCollider.TriggerEnter -= OddityClose;
        }
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        // Here you can add code that needs to be called every frame
    }
    private void OddityClose(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            other.GetScript<PlayerMovement>().insanitySpeed += 0.001f;
            other.GetScript<PlayerMovement>().insanity += 0.01f;
            other.GetScript<PlayerMovement>().maxInsaneAngle += 2f;
            if (other.GetScript<PlayerMovement>().maxInsaneAngle > 20)
            {
                other.GetScript<PlayerMovement>().maxInsaneAngle = 20;
            }
        }
    }
}
