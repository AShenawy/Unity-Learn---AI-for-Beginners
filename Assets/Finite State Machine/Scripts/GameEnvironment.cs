using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This script handles the game environment elements, such as AI patrol path
public sealed class GameEnvironment
{
    private static GameEnvironment instance;    // make this class a singleton
    private List<GameObject> checkpoints = new List<GameObject>();  // reference to the patrol path waypoints
    public List<GameObject> Checkpoints { get { return checkpoints; } }

    public static GameEnvironment Singleton 
    { 
        get 
        {
            // Check if an instance of the class already exists
            if(instance == null)
            {
                instance = new GameEnvironment();
                
                // fill the checkpoints list with waypoint scene objects tagged Checkpoint
                instance.checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));

                // order the list of waypoints ascendingly by name to make AI move in the same sequence
                instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();
            }

            return instance;
        } 
    }
    
}
