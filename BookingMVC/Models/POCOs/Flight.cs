namespace BookingMVC.Models.POCOs
{
    public class RootObject
    {
        public Pagination? pagination { get; set; }
        public Datum[]? data { get; set; }
    }

    public class Pagination
    {
        public int? limit { get; set; }
        public int? offset { get; set; }
        public int? count { get; set; }
        public int? total { get; set; }
    }

    public class Datum
    {
        public string? weekday { get; set; }
        public Departure? departure { get; set; }
        public Arrival? arrival { get; set; }
        public Aircraft?  aircraft { get; set; }
        public Airline? airline { get; set; }
        public Flight? flight { get; set; }
        public Codeshared? codeshared { get; set; }
    }

    public class Departure
    {
        public string? iataCode { get; set; }
        public string? icaoCode { get; set; }
        public string? terminal { get; set; }
        public string? gate { get; set; }
        public string? scheduledTime { get; set; }
    }

    public class Arrival
    {
        public string? iataCode { get; set; }
        public string? icaoCode { get; set; }
        public string? terminal { get; set; }
        public string? gate { get; set; }
        public string? scheduledTime { get; set; }
    }

    public class Aircraft
    {
        public string? modelCode { get; set; }
        public string? modelText { get; set; }
    }

    public class Airline
    {
        public string? name { get; set; }
        public string? iataCode { get; set; }
        public string? icaoCode { get; set; }
    }

    public class Flight
    {
        public string? number { get; set; }
        public string? iataNumber { get; set; }
        public string? icaoNumber { get; set; }
    }

    public class Codeshared
    {
        public Airline1? airline { get; set; }
        public Flight1? flight { get; set; }
    }

    public class Airline1
    {
        public string? name { get; set; }
        public string? iataCode { get; set; }
        public string? icaoCode { get; set; }
    }

    public class Flight1
    {
        public string? number { get; set; }
        public string? iataNumber { get; set; }
        public string? icaoNumber { get; set; }
    }
}
