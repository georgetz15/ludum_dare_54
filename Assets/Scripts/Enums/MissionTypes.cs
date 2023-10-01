
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
	public enum TaskType
	{
		// FUEL,
		MEDICAL,
		AGRICULTURAL,
		POWER_GENERATION,
		TOOLS,
		// BOOKS,
		// ROBOTS,
		SCIENCE,
	}
	
	public class TaskDescriptions
	{
		private static Dictionary<TaskType, string> _desc = new Dictionary<TaskType, string>() {
			// {TaskType.FUEL, "Fuel and Propellant" },
			{TaskType.MEDICAL, "Medical Equipment" },
			{TaskType.AGRICULTURAL, "Greenhouses and Agricultural Equipment" },
			{TaskType.POWER_GENERATION, "Power Generation and Storage" },
			{TaskType.TOOLS, "Tools and Equipment" },
			// {TaskType.BOOKS, "Books" },
			// {TaskType.ROBOTS, "Rovers and Landers"},
			{TaskType.SCIENCE, "Scientific Instruments" }
		};

		public static Dictionary<TaskType, string> GetDescriptions()
		{
			return _desc;
		}
	}
}
