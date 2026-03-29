using AskAgainApi.Enums;

namespace AskAgainApi.Models.DTO.NonBindToEntity
{
    public class TariffResponseDTO
    {
        public TariffEnum Tariff { get; set; }
        public int CountCourses { get; set; }
    }
}
