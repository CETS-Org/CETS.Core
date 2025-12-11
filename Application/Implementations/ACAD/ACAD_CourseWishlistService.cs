using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.IDN;
using DTOs.ACAD.ACAD_CourseWishlist.Requests;
using DTOs.ACAD.ACAD_CourseWishlist.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseWishlistService : IACAD_CourseWishlistService
    {
        private readonly IACAD_CourseWishlistRepository _wishlistRepo;
        private readonly IACAD_CourseRepository _courseRepo;
        private readonly IIDN_StudentRepository _studentRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ACAD_CourseWishlistService(
            IACAD_CourseWishlistRepository wishlistRepo,
            IACAD_CourseRepository courseRepo,
            IIDN_StudentRepository studentRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _courseRepo = courseRepo;
            _studentRepo = studentRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<WishlistItemResponse> AddCourseToWishlistAsync(AddToWishlistRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                // Validate student exists
                var studentExists = await _studentRepo.ExistsByIdAsync(request.StudentId);
                if (!studentExists)
                    throw new KeyNotFoundException($"Student with ID {request.StudentId} not found");

                // Validate course exists
                var course = await _courseRepo.GetByIdAsync(request.CourseId);
                if (course == null)
                    throw new KeyNotFoundException($"Course with ID {request.CourseId} not found");

                // Check if course is already in wishlist
                var alreadyInWishlist = await _wishlistRepo.IsCourseInWishlistAsync(request.StudentId, request.CourseId);
                if (alreadyInWishlist)
                    throw new InvalidOperationException("Course is already in the wishlist");

                // Create wishlist item
                var wishlistItem = new ACAD_CourseWishlist
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    CreatedAt = DateTime.Now
                };

                _wishlistRepo.Add(wishlistItem);
                await _uow.SaveChangesAsync();

                // Retrieve the created item with course details
                var createdItem = await _wishlistRepo.GetWishlistItemAsync(request.StudentId, request.CourseId);
                return _mapper.Map<WishlistItemResponse>(createdItem);
            });
        }

        public async Task<bool> IsCourseInWishlistAsync(Guid studentId, Guid courseId)
        {
            return await _wishlistRepo.IsCourseInWishlistAsync(studentId, courseId);
        }

        public async Task<IEnumerable<WishlistItemResponse>> GetStudentWishlistAsync(Guid studentId)
        {
            var wishlistItems = await _wishlistRepo.GetWishlistByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<WishlistItemResponse>>(wishlistItems);
        }

        public async Task RemoveCourseFromWishlistAsync(Guid studentId, Guid courseId)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var wishlistItem = await _wishlistRepo.GetWishlistItemAsync(studentId, courseId);
                if (wishlistItem == null)
                    throw new KeyNotFoundException("Wishlist item not found");

                _wishlistRepo.Remove(wishlistItem);
                await _uow.SaveChangesAsync();
            });
        }
    }
}

