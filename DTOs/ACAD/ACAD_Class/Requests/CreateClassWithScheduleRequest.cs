using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Class.Requests
{
    public class CreateClassWithScheduleRequest : CreateClassRequest // Kế thừa các trường cơ bản
    {
        // Danh sách lịch học đi kèm
        public List<ClassMeetingScheduleDto> Schedules { get; set; } = new();

        // Danh sách học sinh cần enroll luôn (Tuỳ chọn - nếu bạn muốn gộp cả bước 3)
        public List<Guid> StudentIds { get; set; } = new();
    }

    public class ClassMeetingScheduleDto
    {
        public Guid SlotID { get; set; }
        public DateOnly Date { get; set; }
        public Guid? RoomID { get; set; }
        public Guid SyllabusItemID { get; set; }

        // Nếu cần lưu chuỗi mô tả lịch học vào từng meeting (như code cũ Frontend gửi)
        public string? ScheduleDescription { get; set; }
    }
}
