using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Interfaces.ACAD;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using DTOs.ACAD.ACAD_ClassReservation.Requests;
using DTOs.ACAD.ACAD_ClassReservation.Responses;
using DTOs.ACAD.ACAD_Course.Responses;
using Microsoft.EntityFrameworkCore;
using DTOs.ACAD.ACAD_Course.Requests;
using AutoMapper.QueryableExtensions;

namespace Application.Implementations.ACAD
{
    public class ACAD_ClassReservationService : IACAD_ClassReservationService
    {
        private readonly IACAD_ClassReservationRepository _reservationRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_ClassReservationService(
            IACAD_ClassReservationRepository reservationRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _reservationRepo = reservationRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public IQueryable<ClassReservationResponse> GetAllReservationAsync()
        {
           
            return _reservationRepo.GetAllReservation()
                    .ProjectTo<ClassReservationResponse>(_mapper.ConfigurationProvider);
        }

        public async Task<ClassReservationResponse?> GetReservationByStudentId(Guid id)
        {
            var reservation = await _reservationRepo.GetReservationByStudentId(id);
            return _mapper.Map<ClassReservationResponse>(reservation);
        }
        public async Task<ClassReservationResponse?> GetReservationById(Guid id)
        {
            var reservation = await _reservationRepo.GetByIdAsync(id);
            return _mapper.Map<ClassReservationResponse>(reservation);
        }
        public async Task<Guid> CreateReservationAsync(CreateClassReservationRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(() =>
            {
                var entity = _mapper.Map<ACAD_ClassReservation>(request);
                

                _reservationRepo.Add(entity);
                return Task.FromResult(entity.Id);
            });
        }
        public async Task<ClassReservationResponse?> UpdateReservationAsync(UpdateClassReservationRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _reservationRepo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new KeyNotFoundException("Reservation not found");

                _mapper.Map(request, entity);
                _reservationRepo.Update(entity);
            });

            // Lấy lại đối tượng sau khi đã được cập nhật và lưu
            var updatedEntity = await _reservationRepo.GetByIdAsync(request.Id);

            // Map sang DTO và trả về
            return _mapper.Map<ClassReservationResponse>(updatedEntity);
        }

        public IQueryable<ClassReservationResponse> GetAllAsQueryable()
        {
            var query = _reservationRepo.GetAllReservation(); 

            
            return query.ProjectTo<ClassReservationResponse>(_mapper.ConfigurationProvider);
        }
    }
}
