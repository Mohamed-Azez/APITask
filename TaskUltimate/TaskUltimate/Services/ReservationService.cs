using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskUltimate.Interfaces;
using TaskUltimate.Models;
using TaskUltimate.ViewModel;

namespace TaskUltimate.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IMapper _mapReservationDetails = new MapperConfiguration(cnf => cnf.CreateMap<ReservationDetailsDto, ReservationDetails>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())).CreateMapper();

        private readonly IMapper _mapReservation = new MapperConfiguration(cnf => cnf.CreateMap<ReservationDto, Reservation>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())).CreateMapper();

        private readonly ApplicatoinDbContext _db;
        public ReservationService(ApplicatoinDbContext db)
        {
            _db = db;
        }

        public int AddReservation(ReservationDto reservation)
        {
            var r = _mapReservation.Map<ReservationDto, Reservation>(reservation);
            _db.Reservation.Add(r);
            _db.SaveChanges();
            return r.Id;
        }

        public bool AddReservationDetails(ReservationDetailsDto reservationdetails)
        {
            _db.ReservationDetails.Add(_mapReservationDetails.Map<ReservationDetailsDto, ReservationDetails>(reservationdetails));
            _db.SaveChanges();
            return true;
        }
    }
}
