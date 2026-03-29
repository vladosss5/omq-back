using AskAgainApi.Enums;
using AskAgainApi.Models.DTO.NonBindToEntity;
using AskAgainApi.Models.DTO.User.Request;
using AskAgainApi.Models.DTO.User.Response;
using Yandex.Checkout.V3;

namespace AskAgainApi.Services
{
    public interface IUserService
    {
        public Task<UserResponseDTO> RegUserAsync(UserRegistrationDTO regUserDTO);
        public Task<LoginResponseDTO> AuthUserAsync(AuthUserDTO authUserDTO);
        public Task VerifyEmailAddress(Guid userId);
        public Task UpdatePassword(UserPasswordUpdateDTO userPasswordUpdate, Guid userId);
        public Task UpdateTariff(Guid userId, TariffEnum tariff);
        public Task<TariffEnum> GetUserTariff(Guid userId);
        public Task<int> GetCountOfAvailableToCreateSessions(Guid userId);
        public Task<string> GetLinkForPayNewTariff(Guid userId, TariffEnum tariff);
        public Task ConfirmPayTariff(Message message);
    }
}
