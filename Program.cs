class Program
{
    static void Main(string[] args)
    {
        Vehicle vehicle1 = new Vehicle("license 1", false);
        Vehicle vehicle2 = new Vehicle("license 2", false);
        VehicleTracker vehicleTracker = new VehicleTracker(10, "123 Fake St");
        vehicleTracker.AddVehicle(vehicle1);
        Console.WriteLine(vehicleTracker.PassholderPercentage());
        vehicleTracker.AddVehicle(vehicle2);
        Console.WriteLine(vehicleTracker.PassholderPercentage());
        vehicleTracker.RemoveVehicle("license 1");
        Console.WriteLine(vehicleTracker.PassholderPercentage());
        vehicleTracker.RemoveVehicle(2); 
        Console.WriteLine(vehicleTracker.PassholderPercentage());
    }
}

public class Vehicle
{
    public string Licence { get; set; }
    public bool Pass { get; set; }
    public Vehicle(string licence, bool pass)
    {
        this.Licence = licence;
        this.Pass = pass;
    }
}

public class VehicleTracker
{
    //PROPERTIES
    public string Address { get; set; }
    public int Capacity { get; set; }
    public int SlotsAvailable { get; set; }
    public Dictionary<int, Vehicle> VehicleList { get; set; }

    public VehicleTracker(int capacity, string address)
    {
        this.Capacity = capacity;
        this.Address = address;
        this.SlotsAvailable = capacity;
        this.VehicleList = new Dictionary<int, Vehicle>();

        this.GenerateSlots();
    }

    // STATIC PROPERTIES
    public static string BadSearchMessage = "Error: Search did not yield any result.";
    public static string BadSlotNumberMessage = "Error: No slot with number ";
    public static string SlotsFullMessage = "Error: no slots available.";

    // METHODS
    public void GenerateSlots()
    {
        for (int i = 0; i < this.Capacity; i++)
        {
            this.VehicleList.Add(i+1, null);
        }
    }

    public void AddVehicle(Vehicle vehicle)
    {
        foreach (KeyValuePair<int, Vehicle> slot in this.VehicleList)
        {
            if (slot.Value == null)
            {
                this.VehicleList[slot.Key] = vehicle;
                this.VehicleList[slot.Key].Pass = true;
                this.SlotsAvailable--;
                return;
            }
        }
        throw new IndexOutOfRangeException(SlotsFullMessage);
    }

    public void RemoveVehicle(string licence)
    {
        try
        {
            int slot = this.VehicleList.First(v => v.Value.Licence == licence).Key;
            //VehicleList.Remove(slot);
            this.SlotsAvailable++;
            this.VehicleList[slot].Pass = false;
            this.VehicleList[slot] = null;
        }
        catch
        {
            throw new NullReferenceException(BadSearchMessage);
        }
    }

    public bool RemoveVehicle(int slotNumber)
    {
        if (slotNumber > this.Capacity)
        {
            return false;
        }
        this.VehicleList[slotNumber].Pass = false;
        this.VehicleList[slotNumber] = null;
        this.SlotsAvailable++;
        return true; 
    }

    public List<Vehicle> ParkedPassholders()
    {
        List<Vehicle> passHolders = new List<Vehicle>(100);
        foreach(KeyValuePair<int,Vehicle> entry in this.VehicleList)
        {
            if (entry.Value != null)
            {
                passHolders.Add(entry.Value);
            }
        }
        //passHolders.Add(this.VehicleList.FirstOrDefault(v => v.Value.Pass).Value);
        return passHolders;
    }

    public int PassholderPercentage()
    {
        int passHolders = ParkedPassholders().Count();
        int percentage = (passHolders * 100 / this.Capacity) ;
        return percentage;
    }
}