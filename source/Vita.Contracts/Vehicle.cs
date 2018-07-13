namespace Vita.Contracts
{
    public class Vehicle : ValueObject
    {
        public static Vehicle None = new Vehicle(VehicleType.None, "", "", "", "", "", "", "");

        public VehicleType VehicleType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Registration { get; set; }
        public string Vin { get; set; }
        public string EngineNumber { get; set; }
        public string Description { get; set; }

        public Vehicle(VehicleType vehicleType, string make, string model, string year, string registration, string vin, string engineNumber, string description)
        {
            VehicleType = vehicleType;
            Make = make;
            Model = model;
            Year = year;
            Registration = registration;
            Vin = vin;
            EngineNumber = engineNumber;
            Description = description;
        }
    }
}