

using SharedTrip.ViewModels.Trips;
using System.Collections;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface ITripService
    {
        void Create(AddTripInputModel trip);

        IEnumerable<TripViewModel> GetAll();

        TripsDetailsViewModel GetDetails(string id);
    }
}
