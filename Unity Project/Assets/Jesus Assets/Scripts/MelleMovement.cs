using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.IO;

public class MelleMovement : MonoBehaviour
{


    private AIPath aiPath;                  // AIPath component for movement
    private Transform playerTransform;      // Reference to the player's transform
    private float nextFireTime;             // Time for the next shot
    private AIDestinationSetter destinationSetter; // Reference to the destination setter

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        // Find the player by tag and set as target
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            destinationSetter.target = playerTransform; // Set the player as the target
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player has the 'Player' tag.");
        }
    }
}