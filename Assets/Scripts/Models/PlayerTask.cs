
using UnityEngine;
using Tasks;

namespace Models
{
    public class PlayerTask
    {
        public string CargoName { get; set; }
        public int CargoUnits { get; set; }
        public GameObject PlanetFrom { get; set; }
        public GameObject PlanetTo { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }

        public int Deadline {  get; set; }
        public CargoItem CargoItem { get; set; }
        public string QuestDescription { get; set; }
        public int Reward { get; set; }

        public TaskStatus Status { get; set; }
    } 
}
