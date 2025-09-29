using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Interfaces.ACAD;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.ACAD.ACAD_ClassReservation.Requests;
using DTOs.ACAD.ACAD_ClassReservation.Responses;
using DTOs.ACAD.ACAD_ReservationItem.Responses;
using AutoMapper.QueryableExtensions;
using Domain.Entities;

namespace Application.Implementations.ACAD
{
    public class ACAD_ReservationItemService : IACAD_ReservationItemService
    {
        private readonly IACAD_ReservationItemRepository _reservationItemRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_ReservationItemService(
            IACAD_ReservationItemRepository reservationItemRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _reservationItemRepo = reservationItemRepo;
            _uow = uow;
            _mapper = mapper;
        }


        public IQueryable<ReservationItemResponse?> GetAllReservationItemAsync()
        {
            return _reservationItemRepo.GetAllReservationItem()
                    .ProjectTo<ReservationItemResponse>(_mapper.ConfigurationProvider);
        }

        public async Task<ReservationItemResponse?> GetReservationItemByIdAsync(Guid reservationItemId)
        {
            var reservation = await _reservationItemRepo.GetReservationItemByIdAsync(reservationItemId);
            return _mapper.Map<ReservationItemResponse>(reservation);
        }
        
        public async Task<ReservationItemResponse?> GetReservationItemByReservationId(Guid id)
        {
            var reservation = await _reservationItemRepo.GetReservationItemByReservationId(id);
            return _mapper.Map<ReservationItemResponse>(reservation);
        }
        public async Task<ReservationItemResponse> CreateReservationItemAsync(CreateClassReservationRequest request)
        {
            var entity = _mapper.Map<ACAD_ReservationItem>(request);

            await _uow.ExecuteInTransactionAsync(async () =>
            {
                _reservationItemRepo.Add(entity);
                await _uow.SaveChangesAsync();
            });

            return _mapper.Map<ReservationItemResponse>(entity);
        }
        public async Task<List<ReservationItemResponse>> CreateListReservationItemAsync(List<CreateClassReservationRequest> request)
        {
            var entity = _mapper.Map<List<ACAD_ReservationItem>>(request);

            await _uow.ExecuteInTransactionAsync(async () =>
            {
                _reservationItemRepo.AddRange(entity);
                await _uow.SaveChangesAsync();
            });

            return _mapper.Map<List<ReservationItemResponse>>(entity);
        }

        public async Task<bool> DeleteReservationItemAsync(Guid id)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var existingEntity = await _reservationItemRepo.GetByIdAsync(id);
                if (existingEntity == null)
                    throw new KeyNotFoundException("Reservation Item not found");
                _reservationItemRepo.Remove(existingEntity);
                await _uow.SaveChangesAsync();
                return true;
            });
        }
        public async Task<ClassReservationResponse?> UpdateReservationItemAsync(UpdateClassReservationRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var existingEntity = await _reservationItemRepo.GetByIdAsync(request.Id);
                if (existingEntity == null)
                    throw new KeyNotFoundException("Reservation Item not found");
                _mapper.Map(request, existingEntity);
                _reservationItemRepo.Update(existingEntity);
                await _uow.SaveChangesAsync();
                return _mapper.Map<ClassReservationResponse>(existingEntity);
            });
        }

    }
}
