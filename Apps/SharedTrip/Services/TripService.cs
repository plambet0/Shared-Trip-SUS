

using SharedTrip.Data;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharedTrip.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext db;

        public TripService(ApplicationDbContext db)
        {
            this.db = db;
        }
        
        public void Create(AddTripInputModel trip)
        {
            var dbTrip = new Trip
            {
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                DepartureTime = DateTime.ParseExact(trip.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                ImagePath = trip.ImagePath,
                Seats = trip.Seats,
                Description = trip.Description

            };
            this.db.Trips.Add(dbTrip);
            this.db.SaveChanges();
        }

        public IEnumerable<TripViewModel> GetAll()
        {
            var trips = this.db.Trips.Select(x => new TripViewModel
            {
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                DepartureTime = x.DepartureTime,
                AvailableSeats = x.Seats - x.UserTrips.Count(),
                Id = x.Id

            }).ToList();

            return trips;
        }

        public TripsDetailsViewModel GetDetails(string id)
        {
            var trip = db.Trips.Where(x => x.Id == id)
                .Select(x => new TripsDetailsViewModel
                {
                    ImagePath = x.ImagePath,
                    StartingPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    DepartureTime = x.DepartureTime,
                    Seats = x.Seats - x.UserTrips.Count(),
                    Description = x.Description,
                    Id = x.Id
                    
                    
                    
                })
                .FirstOrDefault();
                return trip;
        }
    }
}
