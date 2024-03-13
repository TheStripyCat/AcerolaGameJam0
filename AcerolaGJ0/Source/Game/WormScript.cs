using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// WormScript Script.
/// </summary>
public class WormScript : Script
{
    [Serialize, ShowInEditor] AnimatedModel wormModel;
    private AnimGraphParameter wormSpeed;
    private int wormRotation;
    public override void OnStart()
    {
        wormSpeed = wormModel.GetParameter("animSpeed");
        wormSpeed.Value = RandomUtil.Random.Next(3, 16) / 10;
        wormRotation = RandomUtil.Random.Next(0, 356);
        Actor.RotateAround(Actor.Position, Vector3.Up, wormRotation);
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        wormSpeed = wormModel.GetParameter("animSpeed");
        wormSpeed.Value = RandomUtil.Random.Next(3, 16) / 10;
        wormRotation = RandomUtil.Random.Next(0, 356);
        Actor.RotateAround(Actor.Position, Vector3.Up, wormRotation);
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        // Here you can add code that needs to be called every frame
    }
}
