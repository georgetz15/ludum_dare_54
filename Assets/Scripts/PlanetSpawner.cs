using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
	private List<Vector2> usedPositions = new List<Vector2>();

	[SerializeField] private UnityEvent onPlanetsSpawned = new();
	private List<Collider2D> planetColliders = new List<Collider2D>();


	void Start()
	{
		minSpacing = maxSize * 0.5f;
	}

	public void GeneratePlanets()
	{
		// Create a list of positions to track where planets are placed
		Vector2 currentPosition = initialPosition;
		usedPositions.Add(currentPosition);
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
			Collider2D[] colliders = planet.GetComponentsInChildren<Collider2D>();
			foreach (Collider2D collider in colliders)
			{
				planetColliders.Add(collider);
			}

			currentPosition = GenerateRandomPosition(currentPosition);
		}
		
		onPlanetsSpawned.Invoke();
	}

	// Generate a random position within the constraints
	private Vector2 GenerateRandomPosition(Vector2 previousPosition)
	{
		Vector2 newPosition;
		int maxRetries = 500;

		float xMinBound = -gridSize.x / 2 + maxSize;
		float xMaxBound = gridSize.x / 2 - maxSize;
		float yMinBound = -gridSize.y / 2 + maxSize;
		float yMaxBound = gridSize.y / 2 - maxSize;

		for (int retry = 0; retry < maxRetries; ++retry)
		{

			// Generate a random position within the circular range
			float angle = Random.Range(0f, 360f);
			float distance = Random.Range(minSpacing, range);

			float xOffset = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
			float yOffset = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

			newPosition = previousPosition + new Vector2(xOffset, yOffset);

			// Ensure the planet, including its maximum size, remains within the grid boundaries
			newPosition.x = Mathf.Clamp(newPosition.x, xMinBound, xMaxBound);
			newPosition.y = Mathf.Clamp(newPosition.y, yMinBound, yMaxBound);

			if (!IsOverlapping(newPosition) && IsSpacingValid(newPosition))
			{
				// Valid position found, add it to used positions and return
				usedPositions.Add(newPosition);
				return newPosition;
			}
		}

		// Return random non-overlapping position
		Vector3 randomPos;
		do
		{
			var randomX = Random.Range(xMinBound, xMaxBound);
			var randomY = Random.Range(yMinBound, yMaxBound);
			randomPos = new Vector2(randomX, randomY);
		} while (usedPositions.Contains(randomPos) || 
				!IsSpacingValid(randomPos)		   ||
				IsOverlapping(randomPos));
		usedPositions.Add(randomPos);
		return randomPos;
	}
	
	bool IsSpacingValid(Vector2 newPosition)
	{
		foreach (Vector2 usedPosition in usedPositions)
		{
			if (Vector2.Distance(newPosition, usedPosition) <= minSpacing)
			{
				return false;
			}
		}
		return true;
	}
	bool IsOverlapping(Vector2 position)
	{
		foreach (Collider2D collider in planetColliders)
		{
			if (collider.bounds.Contains(position))
			{
				return true;
			}
		}
		return false;
	}
}