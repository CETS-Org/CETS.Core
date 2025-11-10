using System;
using System.Collections.Generic;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class SpeakingSubmissionUploadUrlsResponse
    {
        public string AnswersJsonUploadUrl { get; set; } = null!;
        public string AnswersJsonFilePath { get; set; } = null!;
        public Dictionary<string, AudioUploadInfo> AudioUploadUrls { get; set; } = new Dictionary<string, AudioUploadInfo>();
    }

    public class AudioUploadInfo
    {
        public string UploadUrl { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string ContentType { get; set; } = "audio/webm";
    }
}

