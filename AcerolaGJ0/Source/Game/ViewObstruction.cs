using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// ViewObstruction Script.
/// </summary>
public class ViewObstruction : Script
{
    private Vector3 camPos, camJoint, viewDir;    
    private List<MaterialBase> materials;
    public MaterialBase seeThroughMat;
    private RayCastHit hit;
    private Actor currentObject, lastObject;
    private float camDistance, matNumber;


    public override void OnStart()
    {
        materials = new List<MaterialBase>();
        camPos = Actor.Position;
        camJoint = Actor.Parent.Position;
        camDistance = Vector3.Distance(camPos, camJoint);        
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        // Here you can add code that needs to be called when script is enabled (eg. register for events)
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        camPos = Actor.Position;
        camJoint = Actor.Parent.Position;
        viewDir = camJoint - camPos;
        viewDir = viewDir.Normalized;
        
        if (Physics.RayCast(camPos, viewDir, out hit, camDistance, hitTriggers : false))
        {

            if (hit.Collider.HasTag("Player"))
            {                
                if (lastObject != null)
                {
                    if (lastObject is StaticModel)
                    {
                        for (int i = 0; i < matNumber; i++)
                        {
                            lastObject.As<StaticModel>().SetMaterial(i, materials[i]);
                        }                       

                    }
                    else if (lastObject is AnimatedModel)
                    {
                        for (int i = 0; i < matNumber; i++)
                        {
                            lastObject.As<AnimatedModel>().SetMaterial(i, materials[i]);
                        }                        
                    }
                    materials.Clear();
                    currentObject = null;
                    lastObject = null;
                }
            }
            else
            {   // Get current object.
                if (hit.Collider.HasTag("Door"))
                {
                    currentObject = hit.Collider.Parent.Parent;                    
                }
                else
                {
                    currentObject = hit.Collider.Parent;
                }

                if (lastObject != currentObject)
                {
                    // Reset old object's material.
                    if (lastObject is StaticModel)
                    {
                        for (int i = 0; i < matNumber; i++)
                        {
                            lastObject.As<StaticModel>().SetMaterial(i, materials[i]);                            
                        }
                        
                    }
                    else if (lastObject is AnimatedModel)
                    {
                        for (int i = 0; i < matNumber; i++)
                        {
                            lastObject.As<AnimatedModel>().SetMaterial(i, materials[i]);                                                      
                        }
                        
                    }

                    materials.Clear();
                    // Set new material and store the new object's original material.
                    if (currentObject is StaticModel)
                    {
                        matNumber = currentObject.As<StaticModel>().MaterialSlots.Length;                        
                            
                        for (int i = 0; i < matNumber; i++)
                        {
                            materials.Add(currentObject.As<StaticModel>().GetMaterial(i, 0));                            
                        }
                                                
                        for (int i = 0; i < matNumber; i++)
                        {
                            currentObject.As<StaticModel>().SetMaterial(i, seeThroughMat);
                        }
                            
                    }
                    else if (currentObject is AnimatedModel)
                    {
                        matNumber = currentObject.As<AnimatedModel>().MaterialSlots.Length;
                        for (int i = 0;i < matNumber; i++)
                        {
                            materials.Add(currentObject.As<AnimatedModel>().GetMaterial(i));
                        }
                        
                        for (int i = 0; i < matNumber; i++)
                        {
                            currentObject.As<AnimatedModel>().SetMaterial(i, seeThroughMat);
                        }
                        
                    }
                }

                // Store new state                
                lastObject = currentObject;
            }
        }
            
        
    }
}
