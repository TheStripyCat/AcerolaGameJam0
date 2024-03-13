using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// CrazyMan Script.
/// </summary>
public class CrazyMan : Script
{
    [Serialize, ShowInEditor] AudioClip laugh, nonono, crazyTalk;
    [Serialize, ShowInEditor] AudioSource manSpeak;
    private AnimatedModel manModel;
    private AnimGraphParameter isLaughing, isRunningMad, isScared;
    
    private float newAngle, behavior;
    private float timer;
    public float rotationSpeed;
    public override void OnStart()
    {        
        manModel = Actor.GetChild<AnimatedModel>();
        isLaughing = manModel.GetParameter("isLaughing");
        isRunningMad = manModel.GetParameter("isRunningAround");
        isScared = manModel.GetParameter("isScared");
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        behavior = RandomUtil.Random.Next(1, 4);
        if (behavior == 1) 
        {
            isLaughing.Value = true;
            manSpeak.Clip = laugh;
            manSpeak.Volume = 0.6f;

        }
        else if (behavior == 2)
        {
            isRunningMad.Value = true;
            manSpeak.Clip = crazyTalk;
        }
        else if (behavior == 3)
        {
            isScared.Value = true;
            manSpeak.Clip = nonono;
        }        
        newAngle = Actor.Orientation.EulerAngles.Y;
        manSpeak.Play();
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        if (behavior == 2)
        {
            timer += Time.DeltaTime;
            if (timer > 10f) 
            {
                newAngle += RandomUtil.Random.Next(-359, 359);                
                timer = 0;
                Debug.Log("rotated");
            }
            Actor.Orientation = Quaternion.Lerp(Actor.Orientation, Quaternion.Euler(0, newAngle, 0), Time.DeltaTime);

        }
        if (PluginManager.GetPlugin<PortalPlugin>().youDidIt)
        {
            manSpeak.Stop();
            isLaughing.Value = false;
            isRunningMad.Value = false;
            isScared.Value = false;
        }



    }
}
