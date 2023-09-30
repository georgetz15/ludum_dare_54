using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetSpawner : MonoBehaviour
{
	// Grid controls
	public GameObject[] planetPrefabs; // Array of different planet prefabs
	public int numPlanets = 6; // Number of planets to generate
	public Vector2 gridSize = new Vector2(20f, 20f); // Size of the grid
	public Vector2 initialPosition = Vector2.zero; // Initial position to start generating planets	
	
	// Size controls
	public float minSize = 1f;
	public float maxSize = 2.5f;

    // Animatioon controls
	public float minAnimSpeed = 0.05f;
	public float maxAnimSpeed = 0.3f;
	public float minAnimCycleOffset = 0.0f;
	public float maxAnimCycleOffset = 1.0f;

	// Position controls
	// TODO : Get range from spaceship controller
	private float minSpacing;
	public float range = 7; // Maximum range from spaceship
	private List<Vector3> usedPositions = new List<Vector3>();
	private float probOutsideRange = 0.05f;

	void Start()
	{
		minSpacing = maxSize;
		GeneratePlanets();
	}

	void GeneratePlanets()
	{
		// Create a list of positions to track where planets are placed
		Vector3 currentPosition = initialPosition;
		int planetNameCounter = 0;
		for (int i = 0; i < numPlanets; i++)
		{
			// Randomly choose a planet prefab from the array
			GameObject randomPlanetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];

			GameObject planet = Instantiate(randomPlanetPrefab, currentPosition, Quaternion.identity);
			planetNameCounter = i% PlanetNames.names.Length;
			planet.name = $"{PlanetNames.names[planetNameCounter]}";

			// Set random size, animation speed, and cycle offset
			float randomSize = Random.Range(minSize, maxSize);
			planet.transform.localScale = new Vector3(randomSize, randomSize, 1f);

			float randomAnimSpeed = Random.Range(minAnimSpeed, maxAnimSpeed);
			float randomAnimOffset = Random.Range(minAnimCycleOffset, maxAnimCycleOffset);
			planet.GetComponent<PlanetController>().Initialize(randomAnimSpeed, randomAnimOffset);

			// Update the current position for the next planet
			currentPosition = GenerateRandomPosition(currentPosition);
		}
	}

	// Generate a random position within the constraints
	private Vector3 GenerateRandomPosition(Vector3 previousPosition)
	{
		Vector3 newPosition;
		int retries = 0;
		int max_retries = 250;
		do
		{	

			// Generate a random position within the circular range
			float angle = Random.Range(0f, 360f);
			float distance = Random.Range(minSpacing, range);

			float xOffset = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
			float yOffset = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

			newPosition = previousPosition + new Vector3(xOffset, yOffset, 0f);

			// Ensure the planet, including its maximum size, remains within the grid boundaries
			float xMinBound = -gridSize.x / 2 + maxSize;
			float xMaxBound = gridSize.x / 2 - maxSize;
			float yMinBound = -gridSize.y / 2 + maxSize;
			float yMaxBound = gridSize.y / 2 - maxSize;

			newPosition.x = Mathf.Clamp(newPosition.x, xMinBound, xMaxBound);
			newPosition.y = Mathf.Clamp(newPosition.y, yMinBound, yMaxBound);

			if (retries > max_retries)
			{
				return newPosition;
			}
			retries++;

			// Check if the position is not valid
			if (Vector3.Distance(newPosition, previousPosition) < minSpacing || IsPositionUsed(newPosition) || Random.Range(0f, 1f) <= 0.05f)
			{
				// Backtrack to a previously tried position or start over
				if (usedPositions.Count > 0)
				{
					newPosition = usedPositions[Random.Range(0, usedPositions.Count)];
				}
				else
				{
					// If no previous positions to backtrack to, start over
					newPosition = Vector3.zero; // You can set it to your initial position or any suitable value
				}
			}
		} while (Vector3.Distance(newPosition, previousPosition) < minSpacing || IsPositionUsed(newPosition) || Random.Range(0f, 1f) <= probOutsideRange);
		usedPositions.Add(newPosition);
		return newPosition;
	}

	private bool IsPositionUsed(Vector3 position)
	{
		foreach (Vector3 usedPosition in usedPositions)
		{
			if (Vector3.Distance(position, usedPosition) < minSpacing)
			{
				return true; // Position is already used
			}
		}
		return false; // Position is not used
	}
}