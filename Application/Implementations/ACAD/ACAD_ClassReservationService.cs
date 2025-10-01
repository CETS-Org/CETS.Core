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
using Domain.Interfaces.CORE;
using Domain.Constants;

namespace Application.Implementations.ACAD
{
    public class ACAD_ClassReservationService : IACAD_ClassReservationService
    {
        private readonly IACAD_ClassReservationRepository _reservationRepo;
        private readonly IACAD_ReservationItemRepository _reservationItemRepo;
        private readonly ICORE_LookUpRepository _lookUpRepo;
        private readonly IACAD_CourseRepository _courseRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_ClassReservationService(
            IACAD_ClassReservationRepository reservationRepo,
            IACAD_ReservationItemRepository reservationItemRepo,
            ICORE_LookUpRepository lookUpRepo,
            IACAD_CourseRepository courseRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _reservationRepo = reservationRepo;
            _reservationItemRepo = reservationItemRepo;
            _lookUpRepo = lookUpRepo;
            _courseRepo = courseRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<ClassReservationResponse> GetAllReservationAsync()
        {

            var list = _reservationRepo.GetAllReservation();
            return _mapper.Map<IEnumerable<ClassReservationResponse>>(list);

        }

        public IQueryable<ClassReservationResponse> GetReservationByStudentId(Guid id)
        {
            return _reservationRepo.GetReservationByStudentId(id)
                    .ProjectTo<ClassReservationResponse>(_mapper.ConfigurationProvider);
        }
        public async Task<ClassReservationResponse?> GetReservationById(Guid id)
        {
            var reservation = await _reservationRepo.GetReservationById(id);
            return _mapper.Map<ClassReservationResponse>(reservation);
        }
        public async Task<Guid> CreateReservationAsync(CreateClassReservationRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(() =>
            {
                if (request.CoursePackageID != null)
                {
                    var existingReservation = _reservationRepo.GetReservationByStudentId(request.StudentID)
                        .FirstOrDefault(r => r.CoursePackageID == request.CoursePackageID && r.ExpiresAt > DateTime.Now);
                    if (existingReservation != null)
                    {
                        throw new InvalidOperationException("A reservation for this course package already exists and is still valid.");
                    }
                }
               

                var status = _lookUpRepo.GetByCodeAsync(LookUpTypes.ReservationStatus, "Paying");
                var entity = _mapper.Map<ACAD_ClassReservation>(request);

                entity.ReservationStatusID = status.Result?.Id;
                entity.ExpiresAt = DateTime.Now.AddDays(7);

                _reservationRepo.Add(entity);
                return Task.FromResult(entity.Id);
            });
        }


        public async Task<Guid> CreateReservationWithItemsAsync(CreateClassReservationWithItemsRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                if (request.CoursePackageID != null)
                {
                    var existingReservation = _reservationRepo.GetReservationByStudentId(request.StudentID)
                        .FirstOrDefault(r => r.CoursePackageID == request.CoursePackageID && r.ExpiresAt > DateTime.Now);
                    if (existingReservation != null)
                    {
                        throw new InvalidOperationException("A reservation for this course package already exists and is still valid.");
                    }
                }

                var status = await _lookUpRepo.GetByCodeAsync(LookUpTypes.ReservationStatus, "Paying");
                if (status == null)
                {
                    throw new InvalidOperationException("Reservation status 'Paying' not found in system.");
                }

                // Create the class reservation entity
                var entity = _mapper.Map<ACAD_ClassReservation>(request);
                entity.ReservationStatusID = status.Id;
                entity.ExpiresAt = DateTime.Now.AddDays(7);

                _reservationRepo.Add(entity);
                
                var courseIds = request.Items.Select(i => i.CourseID).ToList();
              
                // Check if student already has courses
                var existingCourseIds = await _reservationItemRepo.GetActiveReservationCoursesForStudentAsync(request.StudentID, DateTime.Now);

                var conflictingCourses = courseIds.Where(c => existingCourseIds.Contains(c)).ToList();
                
                if (conflictingCourses.Any())
                {
                    throw new InvalidOperationException(
                        $"Student already has active reservations for these courses: {string.Join(", ", conflictingCourses)}");
                }

                // Create reservation items
                foreach (var itemRequest in request.Items)
                {
                    // Validate: Check if course exists
                    var courseExists = await _courseRepo.ExistsByIdAsync(itemRequest.CourseID);
                    if (!courseExists)
                    {
                        throw new KeyNotFoundException($"Course with ID {itemRequest.CourseID} not found.");
                    }

                    //// Validate: Check if plan type exists (if provided)
                    //if (itemRequest.PlanTypeID.HasValue)
                    //{
                    //    var planType = await _lookUpRepo.GetByIdAsync(itemRequest.PlanTypeID.Value);
                    //    if (planType == null || planType.LookUpType.Code != LookUpTypes.PlanType)
                    //    {
                    //        throw new KeyNotFoundException($"Plan type with ID {itemRequest.PlanTypeID.Value} not found.");
                    //    }
                    //}

                    var reservationItemEntity = _mapper.Map<ACAD_ReservationItem>(itemRequest);
                    reservationItemEntity.ClassReservationID = entity.Id;

                    entity.ACAD_ReservationItems.Add(reservationItemEntity);
                }

                if (!entity.ACAD_ReservationItems.Any())
                {
                    throw new InvalidOperationException("At least one reservation item is required.");
                }

                return entity.Id;
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

        public async Task<ClassReservationResponse> UpdateReservationStatusAsync(Guid id, Guid lookupId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new Exception("Invoice not found");
            }
            reservation.ReservationStatusID = lookupId;
            _reservationRepo.Update(reservation);
            await _uow.SaveChangesAsync();
            return _mapper.Map<ClassReservationResponse>(reservation);
        }
    }
}

