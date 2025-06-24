// See https://aka.ms/new-console-template for more information

// Abstraction: Interface defining common behavior for all smart devices
public interface ISmartDevice
{
    string DeviceName { get; }
    void TurnOn();
    void TurnOff();
    string GetStatus();
}

// Encapsulation: Base class with protected fields and public methods
public abstract class SmartDevice : ISmartDevice
{
    protected string? deviceName;
    protected bool isOn;
    protected DateTime lastActivated;

    //Read only property encapsulation
#pragma warning disable CS8603 // Possible null reference return.
    public string DeviceName => deviceName;
#pragma warning restore CS8603 // Possible null reference return.

    protected SmartDevice(string name)
    {
        deviceName = name;
        isOn = false;
        lastActivated = DateTime.MinValue;
    }

    public virtual void TurnOn()
    {
        isOn = true;
        lastActivated = DateTime.Now;
        Console.WriteLine($"{deviceName} turned ON at {lastActivated}");
    }

    public virtual void TurnOff()
    {
        isOn = false;
        Console.WriteLine($"{deviceName} turned OFF");
    }

    public virtual string GetStatus()
    {
        return $"{deviceName} is {(isOn ? "ON" : "OFF")}";
    }
}

// Inheritance: Derived class from SmartDevice
public class SmartLight : SmartDevice
{
    private int brightnessLevel;

    public SmartLight(string name) : base(name)
    {
        brightnessLevel = 50; // Default brightness
    }

    // Additional functionality specific to SmartLight
    public void SetBrightness(int level)
    {
        if (level < 0 || level > 100)
            throw new ArgumentException("Brightness must be between 0 and 100");

        brightnessLevel = level;
        Console.WriteLine($"{DeviceName} brightness set to {brightnessLevel}%");
    }

    // Method overriding (polymorphism)
    public override string GetStatus()
    {
        return base.GetStatus() + $", Brightness: {brightnessLevel}%";
    }
}

// Another inherited class demonstrating polymorphism differently
public class SmartThermostat : SmartDevice
{
    private double currentTemperature;
    private double targetTemperature;

    public SmartThermostat(string name) : base(name)
    {
        currentTemperature = 20.0; // Default temperature
        targetTemperature = 22.0;
    }

    public void SetTemperature(double temp)
    {
        targetTemperature = temp;
        Console.WriteLine($"{DeviceName} target temperature set to {temp}°C");
    }

    public void UpdateCurrentTemperature(double temp)
    {
        currentTemperature = temp;
        Console.WriteLine($"{DeviceName} current temperature is now {temp}°C");
    }

    // Overriding with different implementation
    public override void TurnOn()
    {
        base.TurnOn();
        Console.WriteLine($"{DeviceName} is now maintaining {targetTemperature}°C");
    }

    public override string GetStatus()
    {
        return $"{base.GetStatus()}, Current: {currentTemperature}°C, Target: {targetTemperature}°C";
    }
}

// Composition: A class that contains other objects
public class SmartRoom
{
    private string roomName;
    private List<ISmartDevice> devices;

    public SmartRoom(string name)
    {
        roomName = name;
        devices = new List<ISmartDevice>();
    }

    public void AddDevice(ISmartDevice device)
    {
        devices.Add(device);
        Console.WriteLine($"Added {device.DeviceName} to {roomName}");
    }

    public void TurnOnAllDevices()
    {
        Console.WriteLine($"Turning on all devices in {roomName}:");
        foreach (var device in devices)
        {
            device.TurnOn();
        }
    }

    public void DisplayAllStatuses()
    {
        Console.WriteLine($"Status of all devices in {roomName}:");
        foreach (var device in devices)
        {
            Console.WriteLine($"- {device.GetStatus()}");
        }
    }
}

// Demonstrating polymorphism through interface usage
public class SmartHome
{
    private List<SmartRoom> rooms;

    public SmartHome()
    {
        rooms = new List<SmartRoom>();
    }

    public void AddRoom(SmartRoom room)
    {
        rooms.Add(room);
        Console.WriteLine($"Added room: {room}");
    }

    public void DisplayHomeStatus()
    {
        Console.WriteLine("\n=== SMART HOME STATUS ===");
        foreach (var room in rooms)
        {
            room.DisplayAllStatuses();
        }
        Console.WriteLine("=======================\n");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a smart home
        SmartHome myHome = new SmartHome();

        // Create some rooms
        SmartRoom livingRoom = new SmartRoom("Living Room");
        SmartRoom bedroom = new SmartRoom("Master Bedroom");

        // Create various smart devices (polymorphism - all treated as ISmartDevice)
        ISmartDevice livingRoomLight = new SmartLight("Living Room Main Light");
        ISmartDevice bedroomLight = new SmartLight("Bedroom Night Light");
        ISmartDevice livingRoomThermostat = new SmartThermostat("Living Room Thermostat");

        // Add devices to rooms
        livingRoom.AddDevice(livingRoomLight);
        livingRoom.AddDevice(livingRoomThermostat);
        bedroom.AddDevice(bedroomLight);

        // Add rooms to home
        myHome.AddRoom(livingRoom);
        myHome.AddRoom(bedroom);

        // Interact with devices
        livingRoomLight.TurnOn();
        ((SmartLight)livingRoomLight).SetBrightness(75); // Casting to access specific method
        
        livingRoomThermostat.TurnOn();
        ((SmartThermostat)livingRoomThermostat).SetTemperature(21.5);
        
        bedroomLight.TurnOn();

        // Display status
        myHome.DisplayHomeStatus();

        // Turn on all devices in living room
        livingRoom.TurnOnAllDevices();

        // Final status check
        myHome.DisplayHomeStatus();
    }
}