namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class UpdateDetails
    {
        public FieldUpdate<decimal?> Score { get; set; } = new FieldUpdate<decimal?>();
        public FieldUpdate<string?> Feedback { get; set; } = new FieldUpdate<string?>();
    }
}


