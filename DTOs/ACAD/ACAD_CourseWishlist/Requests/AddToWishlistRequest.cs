using System;

namespace DTOs.ACAD.ACAD_CourseWishlist.Requests
{
    public class AddToWishlistRequest
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}

