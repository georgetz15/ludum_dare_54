using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetSpawner : MonoBehaviour
{
	public GameObject[] planetPrefabs; // Array of different planet prefabs
	public int numPlanets = 6; // Number of planets to generate
	public float minSize = 1.5f;
	public float maxSize = 3.5f;
	public float minAnimSpeed = 0.8f;
	public float maxAnimSpeed = 1.2f;
	public float minAnimCycleOffset = 0.0f;
	public float maxAnimCycleOffset = 1.0f;
	public float minSpacing = 4f; // Minimum spacing between planets
	public float maxRange = 10f; // Maximum range from spaceship
	public Vector2 gridSize = new Vector2(20f, 20f); // Size of the grid
	public Vector2 initialPosition = Vector2.zero; // Initial position to start generating planets

	private List<Vector3> usedPositions = new List<Vector3>();
	private float probOutsideRange = 0.05f;

	void Start()
	{
		GeneratePlanets();
	}

	void GeneratePlanets()
	{
		// Create a list of positions to track where planets are placed
		Vector3 currentPosition = initialPosition;

		for (int i = 0; i < numPlanets; i++)
		{
			// Randomly choose a planet prefab from the array
			GameObject randomPlanetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];

			GameObject planet = Instantiate(randomPlanetPrefab, currentPosition, Quaternion.identity);
			planet.name = $"Planet_{i}";

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
		// Todo: Change this to something smarter? :P 
		float planetRadius = minSpacing;
		do
		{
			float angle = Random.Range(0f, 360f);
			float distance = Random.Range(minSpacing + planetRadius, maxRange);

			float xOffset = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
			float yOffset = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

			newPosition = previousPosition + new Vector3(xOffset, yOffset, 0f);

			// Ensure the center of the planet remains within the grid boundaries
			newPosition.x = Mathf.Clamp(newPosition.x, -gridSize.x / 2 + planetRadius, gridSize.x / 2 - planetRadius);
			newPosition.y = Mathf.Clamp(newPosition.y, -gridSize.y / 2 + planetRadius, gridSize.y / 2 - planetRadius);
		} while (Vector3.Distance(newPosition, previousPosition) < minSpacing ||
												  IsPositionUsed(newPosition) || 
												  Random.Range(0f, 1f) <= probOutsideRange);
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