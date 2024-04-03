using System;
using System.Collections.Generic;

namespace TicketingConsoleApp
{
    public enum SeatType
    {
        Window,
        Aisle
    }

    public class Seat
    {
        public string Label { get; }
        public bool IsBooked { get; set; }
        public Passenger Passenger { get; set; }

        public Seat(string label)
        {
            Label = label;
            IsBooked = false;
        }
    }

    public class Passenger
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public SeatType SeatPreference { get; set; }
        public Seat Seat { get; set; }
    }

    public class Row
    {
        public List<Seat> Seats { get; }

        public Row()
        {
            Seats = new List<Seat>();
        }
    }

    public class Plane
    {
        private readonly List<Row> _rows;

        public Plane()
        {
            _rows = new List<Row>();

            for (int i = 0; i < 12; i++)
            {
                var row = new Row();
                row.Seats.Add(new Seat("A"));
                row.Seats.Add(new Seat("B"));
                row.Seats.Add(new Seat("C"));
                row.Seats.Add(new Seat("D"));
                _rows.Add(row);
            }
        }

        public Seat FindAvailableSeat(SeatType preference)
        {
            foreach (var row in _rows)
            {
                foreach (var seat in row.Seats)
                {
                    if (!seat.IsBooked)
                    {
                        if (preference == SeatType.Window && (seat.Label == "A" || seat.Label == "D"))
                            return seat;
                        else if (preference == SeatType.Aisle && (seat.Label == "B" || seat.Label == "C"))
                            return seat;
                    }
                }
            }
            return null;
        }

        public bool BookTicket(Passenger passenger)
        {
            Seat availableSeat = FindAvailableSeat(passenger.SeatPreference);
            if (availableSeat != null)
            {
                availableSeat.IsBooked = true;
                availableSeat.Passenger = passenger;
                passenger.Seat = availableSeat;
                return true;
            }
            return false;
        }

        public void DisplaySeatingChart()
        {
            foreach (var row in _rows)
            {
                foreach (var seat in row.Seats)
                {
                    if (seat.IsBooked)
                        Console.Write($"{seat.Passenger.FirstName[0]}{seat.Passenger.LastName[0]} ");
                    else
                        Console.Write($"{seat.Label} ");
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Plane plane = new Plane();

            while (true)
            {
                Console.WriteLine("\nPlease enter 1 to book a ticket.");
                Console.WriteLine("Please enter 2 to see seating chart.");
                Console.WriteLine("Please enter 3 exit the application.");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\nPlease enter the passenger's first name:");
                        string firstName = Console.ReadLine();
                        Console.WriteLine("Please enter the passenger's last name:");
                        string lastName = Console.ReadLine();
                        Console.WriteLine("Please enter 1 for a Window seat preference, 2 for an Aisle seat preference, or hit enter to pick the first available seat:");
                        string preferenceInput = Console.ReadLine();

                        SeatType preference = SeatType.Window;
                        if (!string.IsNullOrEmpty(preferenceInput))
                            preference = (SeatType)int.Parse(preferenceInput);

                        Passenger passenger = new Passenger
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            SeatPreference = preference
                        };

                        if (plane.BookTicket(passenger))
                            Console.WriteLine($"The seat located in {passenger.Seat.Label} has been booked.");
                        else
                            Console.WriteLine("Sorry, the plane is fully booked.");

                        break;
                    case "2":
                        plane.DisplaySeatingChart();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
    }
}

