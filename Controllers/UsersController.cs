using AskAgainApi.Helpers;
using AskAgainApi.Models.DTO.NonBindToEntity;
using AskAgainApi.Models.DTO.User.Request;
using AskAgainApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yandex.Checkout.V3;

namespace AskAgainApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : UserApiController
    {
        private readonly IUserService _usersService;

        public UsersController(IUserService usersService) =>
            _usersService = usersService;

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(AuthUserDTO authUserDTO)
        {

            var loginResponseDTO = await _usersService.AuthUserAsync(authUserDTO);

            return loginResponseDTO;
        }

        [HttpPost("reg")]
        public async Task<ActionResult> Registration(UserRegistrationDTO regUserDTO)
        {
            var newRegUserDTO = await _usersService.RegUserAsync(regUserDTO);

            return Created("user", newRegUserDTO);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyUserDTO verifyUser)
        {
            await _usersService.VerifyEmailAddress(verifyUser.Token);

            return Ok();
        }

        [Authorize]
        [HttpPatch("password")]
        public async Task<IActionResult> UpdatePassword(UserPasswordUpdateDTO userPasswordUpdate)
        {
            await _usersService.UpdatePassword(userPasswordUpdate, GetUserId());

            return Ok();
        }

        [Authorize]
        [HttpPatch("tariff")]
        public async Task<IActionResult> ChangeTariff(ChangeTariffDTO changeTariffDto)
        {
            await _usersService.UpdateTariff(GetUserId(), changeTariffDto.Tariff);

            return Ok();
        }


        [Authorize]
        [HttpGet("tariff")]
        public async Task<ActionResult<TariffResponseDTO>> GetTariff()
        {
            var userId = GetUserId();
            var tariff = await _usersService.GetUserTariff(userId);
            var remains = await _usersService.GetCountOfAvailableToCreateSessions(userId);

            return new TariffResponseDTO() { Tariff = tariff, CountCourses = remains };
        }

        [Authorize]
        [HttpPost("tariff/pay")]
        public async Task<ActionResult<PayResponseDTO>> GetLinkForPayNewTariff(ChangeTariffDTO changeTariffDto)
        {
            var link = await _usersService.GetLinkForPayNewTariff(GetUserId(), changeTariffDto.Tariff);

            return new PayResponseDTO() { Url = link };
        }

        [HttpPost("tariff/pay/confirm")]
        public async Task<ActionResult> ConfirmPay(Message message)
        {
            await _usersService.ConfirmPayTariff(message);

            return Ok();
        }
    }
}
