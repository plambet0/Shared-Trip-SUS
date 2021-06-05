

using SharedTrip.Services;
using SharedTrip.ViewModels.Trips;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Globalization;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService tripService;

        public TripsController(ITripService tripService)
        {
            this.tripService = tripService;
        }
        public HttpResponse All()
        {
            var trips = tripService.GetAll();
            return this.View(trips);
        }
        public HttpResponse Details(string tripId)
        {
            var trip = this.tripService.GetDetails(tripId);
            return this.View(trip);
        }
        public HttpResponse Add()
        {
            return this.View();
        }
        
        [HttpPost]
        public HttpResponse Add(AddTripInputModel input)
        {
            if (string.IsNullOrEmpty(input.StartPoint))
            {
                return this.Error("Start Point is requered!");
            }
            if (string.IsNullOrEmpty(input.EndPoint))
            {
                return this.Error("End Point is requered!");
            }
            if (input.Seats < 2 || input.Seats > 6)
            {
                return this.Error("Seats should be between 2 and 6!");
            }
            if (string.IsNullOrEmpty(input.Description)|| input.Description.Length > 80)
            {
                return this.Error("Description is required and has max length of 80 characters!");
            }
            if (!DateTime.TryParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return this.Error("Invalid Departure Time!");
            }
            this.tripService.Create(input);
            return this.Redirect("/Trips/All");
        }
    }
}
