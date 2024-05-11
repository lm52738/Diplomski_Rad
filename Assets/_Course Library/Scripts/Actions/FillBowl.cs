using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the content and properties of the bowl containing various liquid particles.
/// </summary>
public class FillBowl : MonoBehaviour
{
    private Dictionary<string, ParticleInfo> particlesInsideBowl;

    // Class to store information about each particle
    public class ParticleInfo
    {
        public ParticleSystem particle;
        public double phValue;
        public int count;
    }

    // Class to store information about each type of particle
    public class ParticleType
    {
        public string type;
        public double phValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        particlesInsideBowl = new Dictionary<string, ParticleInfo>();
    }

    private void OnDisable()
    {
        EmptyBowl();
    }

    private void OnParticleCollision(GameObject other)
    {
        // Get particles
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
        if (particleSystem == null)
            return;

        int particleCount = particleSystem.particleCount;

        ParticleType particleType = DetermineParticleType(other);

        ParticleInfo particleInfo = new ParticleInfo();
        particleInfo.particle = particleSystem;
        particleInfo.count = particleCount;
        particleInfo.phValue = particleType.phValue;

        // Log the information
        // Debug.Log($"Particles inside the bowl - Type: {particleType.type}, Count: {particleCount}");

        particlesInsideBowl[particleType.type] = particleInfo;
    }

    // Calculate the average pH level of particles in the bowl based on the ratio of each type of particle
    public double getPhLevel()
    {
        if (particlesInsideBowl.Count == 0)
        {
            // Debug.LogWarning("No particles in the bowl.");
            return 0;
        }

        double totalPhLevel = 0;
        int totalParticleCount = 0;

        foreach (var particle in particlesInsideBowl)
        {
            totalPhLevel += particle.Value.phValue * particle.Value.count;
            totalParticleCount += particle.Value.count;
        }

        double averagePhLevel = totalPhLevel / totalParticleCount;

        // Debug.Log($"Average pH level of particles in the bowl: {averagePhLevel}");

        return averagePhLevel;
    }

    // Get the list of unique liquids present in the bowl
    public List<String> getLiquids()
    {
        List<string> liquids = new List<string>();

        foreach (var particle in particlesInsideBowl)
        {
            if (!liquids.Contains(particle.Key))
            {
                liquids.Add(particle.Key);
            }
        }

        return liquids;
    }

    private ParticleType DetermineParticleType(GameObject particle)
    {
        ParticleType particleType = new ParticleType();
        particleType.type = particle.tag;

        switch (particle.tag)
        {
            case "Soda":
                particleType.phValue = 2.5;
                break;
            case "Bleach":
                particleType.phValue = 9;
                break;
            case "Coffee":
                particleType.phValue = 5;
                break;
            case "Orange Juice":
                particleType.phValue = 3.5;
                break;
            case "Apple Juice":
                particleType.phValue = 3.7;
                break;
            case "Lemon Juice":
                particleType.phValue = 2.3;
                break;
            case "Milk":
                particleType.phValue = 6.5;
                break;
            case "Detergent":
                particleType.phValue = 12;
                break;
            case "Molasses":
                particleType.phValue = 5.8;
                break;
            case "Saliva":
                particleType.phValue = 6;
                break;
            case "Ammonia":
                particleType.phValue = 11;
                break;
            case "Blood":
                particleType.phValue = 7.4;
                break;
            case "Mouthwash":
                particleType.phValue = 4.5;
                break;
            case "Soap":
                particleType.phValue = 10;
                break;
            default:
                particleType.phValue = 7; // if the type cannot be determined
                break; 
        }

        return particleType;
    }

    private void EmptyBowl()
    {
        // Update particle lifetimes and remove expired particles
        // Debug.Log("Empty the bowl");
        if (particlesInsideBowl.Count > 0)
        {
            foreach (var particle in particlesInsideBowl)
            {
                particle.Value.particle.Clear(true);
            }

            particlesInsideBowl.Clear();
        }
    }
}
