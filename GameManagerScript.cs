﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public float timeScale = 1f;
    public int initialPopulationSize = 10;
    public GameObject creaturePrefab;
    private List<GameObject> population;
    public Transform spawnLocation;
    public float spawnNoise = 10f;
    public int worldX, worldY, worldZ;
    private float xOffset, yOffset, zOffset;

    void Start() {
        spawnInitialPopulation();
        this.xOffset = 0f;
        this.yOffset = worldX * worldY;
        this.zOffset = worldX * worldY * 2;
    }

    void spawnInitialPopulation() {
        this.population = new List<GameObject>();
        for (int i = 0; i < this.initialPopulationSize; i++) {
            GameObject newCreature = Instantiate(creaturePrefab,
                                                 randomSpawnLocation(spawnLocation.position, spawnNoise),
                                                 Quaternion.identity);
            this.population.Add(newCreature);
        }
    }

    Vector3 randomSpawnLocation(Vector3 center, float noise) {
        float x = center.x + Random.Range(-noise, noise);
        float y = center.y + Random.Range(-noise, noise);
        float z = center.z;
        return new Vector3(x, y, z);
    }

    void FixedUpdate() {
        // fixed update won't get called if timescale is set to 0
        if (timeScale != 0)
            Time.timeScale = timeScale;
        applyFlowField();
    }

    void applyFlowField() {
        foreach (GameObject creature in this.population) {
            Vector3 creaturePos = creature.transform.position;
            creature.GetComponent<Rigidbody>().AddForce(calcFlowFieldVector(creaturePos), ForceMode.Acceleration);
        }
    }

    Vector3 calcFlowFieldVector(Vector3 position) {
        float noiseX = Mathf.PerlinNoise(position.x + position.z + this.xOffset, position.y + this.xOffset) - 0.5f;
        float noiseY = Mathf.PerlinNoise(position.x + position.z + this.yOffset, position.y + this.yOffset) - 0.5f;
        float noiseZ = Mathf.PerlinNoise(position.x + position.z + this.zOffset, position.y + this.zOffset) - 0.5f;
        return new Vector3(noiseX, noiseY, noiseZ);
    }

}
