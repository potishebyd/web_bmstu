using web_bmstu.DTO;
using web_bmstu.ModelsBL;
using web_bmstu.Services;

namespace web_bmstu.ModelsConverters
{
    public class RecordingStudioConverters
    {
        private readonly IRecordingStudioService recordingStudioService;

        public RecordingStudioConverters(IRecordingStudioService recordingStudioService)
        {
            this.recordingStudioService = recordingStudioService;
        }

        public RecordingStudioBL convertPatch(int id, RecordingStudioBaseDto recordingStudio)
        {
            var existedRecordingStudio = recordingStudioService.GetByID(id);

            return new RecordingStudioBL
            {
                Id = id,
                Name = recordingStudio.Name ?? existedRecordingStudio.Name,
                Country = recordingStudio.Country ?? existedRecordingStudio.Country,
                YearFounded = recordingStudio.YearFounded ?? existedRecordingStudio.YearFounded
            };
        }
    }
}