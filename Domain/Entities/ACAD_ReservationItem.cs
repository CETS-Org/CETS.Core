using Domain.Entities.EntityBases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class ACAD_ReservationItem : EntityBase
    {
        public Guid? InvoiceID { get; set; }

        public Guid CourseID { get; set; }

        public int? PaymentSequence { get; set; }

        public Guid? PlanTypeID { get; set; }

        [ForeignKey(nameof(InvoiceID))]
        public virtual FIN_Invoice Invoice { get; set; } = null!;

        [ForeignKey(nameof(CourseID))]
        public virtual ACAD_Course Course { get; set; } = null!;

        [ForeignKey(nameof(PlanTypeID))]
        public virtual CORE_LookUp? PlanType { get; set; }
    }
}
