using Microsoft.AspNetCore.Http;

namespace Models.DTO.Development
{
    public class SampleFile
    {
        public IFormFile ImageFile { get; set; }

        public string Name { get; set; }
    }
}
