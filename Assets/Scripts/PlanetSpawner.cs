using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlanetSpawner : MonoBehaviour
{
	// Grid controls
	public GameObject[] planetPrefabs; // Array of different planet prefabs
	public int numPlanets = 6; // Number of planets to generate
	public Vector2 gridSize = new Vector2(20f, 20f); // Size of the grid
	
	// Size controls
	public float minSize = 1f;
	public float maxSize = 2.5f;
	private float minSpacing = 0f;

    // Animatioon controls
	public float minAnimSpeed = 0.05f;
	public float maxAnimSpeed = 0.3f;
	public float minAnimCycleOffset = 0.0f;
	public float maxAnimCycleOffset = 1.0f;

	[SerializeField] private UnityEvent onPlanetsSpawned = new();
	private int minClusterSize;
	private	int planetNameCounter = 0;
	private List<Collider2D> planetColliders = new List<Collider2D>();
	private List<Vector2> usedPositions = new List<Vector2>();

	private float range;
	private float defaultRange = 3f;

	public static PlanetSpawner Instance;
	public GameObject initialPlanet;
	void Start()
	{
		minSpacing = maxSize + 0.5f;		

		var sc = SpaceShipController.Instance;
		if (sc != null)
		{
			range = SpaceShipController.Instance.Range;
		} else
		{
			range = defaultRange;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	public void GeneratePlanets()
	{
		minClusterSize = (int)(numPlanets * 0.15f);
		Vector2 position;
		planetNameCounter = 0;
		// Generate connected planets around the origin
		for (int i = 0; i < minClusterSize; i++)
		{
			do
			{
				position = Random.insideUnitCircle * range;
			} while (IsOverlapping(position) && !IsValidSpacing(position));
			usedPositions.Add(position);
			InstantiateRandomPlanet(position, i);
		}

		// Generate remaining planets randomly
		for (int i = minClusterSize; i < numPlanets; i++)
		{
			position = GetRandomValidPosition();
			usedPositions.Add(position);
			InstantiateRandomPlanet(position, i);
		}

		onPlanetsSpawned.Invoke();
	}

	void InstantiateRandomPlanet(Vector2 position, int idx)
	{
		// Generate random planet with name
		GameObject planetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Count())];
		GameObject planet = Instantiate(planetPrefab, position, Quaternion.identity);
		planetNameCounter = planetNameCounter % PlanetNames.names.Length;
		planet.name = $"{PlanetNames.names[planetNameCounter++]}";

		// Collect colliders
		Collider2D[] colliders = planet.GetComponentsInChildren<Collider2D>();
		foreach (Collider2D collider in colliders)
		{
			planetColliders.Add(collider);
		}

		// Adjust the scale of the planet
		float scale = Random.Range(minSize, maxSize);
		planet.transform.localScale = new Vector3(scale, scale, 1f);
		
		// Randomize the animation
		float randomAnimSpeed = Random.Range(minAnimSpeed, maxAnimSpeed);
		float randomAnimOffset = Random.Range(minAnimCycleOffset, maxAnimCycleOffset);
		planet.GetComponent<PlanetController>().Initialize(randomAnimSpeed, randomAnimOffset);

		if (idx == 0)
		{
			initialPlanet = planet;
		}
	}

	Vector2 GetRandomValidPosition()
	{
		Vector2 position;
		int maxAttempts = 300;
		int currentAttempt = 0;

		do
		{
			float x = Random.Range(-gridSize.x * 0.5f, gridSize.x * 0.5f);
			float y = Random.Range(-gridSize.y * 0.5f, gridSize.y * 0.5f);
			position = new Vector2(x, y);
			currentAttempt++;
		}
		while (usedPositions.Contains(position) &&
			   IsOverlapping(position)          && 
			   !IsValidSpacing(position)        &&
			   currentAttempt < maxAttempts);

		return position;
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

	bool IsValidSpacing(Vector2 position)
	{
		foreach (var pos in usedPositions)
		{
			if (Vector2.Distance(position, pos) >= minSpacing)
				return false;
		}

		return true;
	}
}