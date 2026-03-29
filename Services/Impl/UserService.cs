using AskAgainApi.Entity.User;
using AskAgainApi.Enums;
using AskAgainApi.Exceptions;
using AskAgainApi.Models.DTO.NonBindToEntity;
using AskAgainApi.Models.DTO.User.Request;
using AskAgainApi.Models.DTO.User.Response;
using AskAgainApi.Repositories;
using AskAgainApi.Settings;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Yandex.Checkout.V3;

namespace AskAgainApi.Services.Impl;

public partial class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IOptions<FrontendSettings> _frontendSettings;
    private readonly IPasswordHashingService _passwordHasher;

    private static readonly AsyncClient PaymentClient = new Client(
        shopId: "324139",
        secretKey: "live_pqmw14rjX1oTntdrbJ2vBjdDHGUkUbpTySPUCuwHhuU").MakeAsync();

    public UserService(
        IUserRepository userRepository, 
        IMapper mapper, 
        IConfiguration configuration, 
        IOptions<FrontendSettings> frontendSettings, 
        IEmailService emailService, 
        IPasswordHashingService passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
        _frontendSettings = frontendSettings;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDTO> AuthUserAsync(AuthUserDTO authUserDTO)
    {
        var user = await _userRepository.GetByLoginAsync(authUserDTO.Login) ?? throw new HttpException("User not found.", 404);

        if (!user.Verifyed) 
            throw new HttpException("Email not verifyed.");

        var passwordMatch = _passwordHasher.Verify(authUserDTO.Pass, user.Pass);
        
        if (user == null || !passwordMatch) 
            throw new HttpException("Invalid login or password.");

        var token = GenerateJwtToken(user);

        return new LoginResponseDTO(token);
    }

    private string GenerateJwtToken(UserEntity user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = GetSecretKey();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new List<Claim> {
                    new Claim(ClaimTypes.Thumbprint, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Login)
                }),

            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<UserResponseDTO> RegUserAsync(UserRegistrationDTO regUserDTO)
    {
        var userEntity = _mapper.Map<UserEntity>(regUserDTO);

        if (!CheckEmailValid(regUserDTO.Login))
            throw new HttpException("Email is not valid.");

        if (!CheckPasswordStrong(regUserDTO.Pass))
            throw new HttpException("The password is not complex enough. The password must be at least 8 characters long. Use a different letter case and numbers.");

        if (null != await _userRepository.GetByLoginAsync(regUserDTO.Login))
            throw new HttpException("This email address already used.");

        var passwordHash = _passwordHasher.GenerateHash(userEntity.Pass);
        userEntity.Pass = passwordHash;

        await _userRepository.CreateAsync(userEntity);

        await _emailService.SendMessegeAsync(regUserDTO.Login, "Подтверждения почты - сайт \"Ещё?\"", GenereteEmailBodyForVerifyEmail(userEntity));

        return _mapper.Map<UserResponseDTO>(await _userRepository.GetByLoginAsync(userEntity.Login));
    }

    private static bool CheckEmailValid(string email)
    {
        string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        var ValidEmailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

        return ValidEmailRegex.IsMatch(email);
    }


    private static bool CheckPasswordStrong(string password)
    {
        var strongPassRegex = StrongPassRegex();

        return strongPassRegex.IsMatch(password);
    }

    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")]
    public static partial Regex StrongPassRegex();

    public async Task VerifyEmailAddress(Guid Id)
    {
        await _userRepository.VerifyAsync(Id);
    }

    private string GenereteEmailBodyForVerifyEmail(UserEntity user)
    {
        string body = "Перейдите по ссылке для подтверждения почты и активации аккаунта\n"
            + GenereteValidationLink(user);

        return body;
    }

    private string GenereteValidationLink(UserEntity user)
    {
        string link = _frontendSettings.Value.Link
                    + _frontendSettings.Value.PathForVerifyPage
                    + HttpUtility.UrlEncode(user.Id.ToString());

        return link;
    }


    private byte[] GetSecretKey()
    {
        var secretSection = _configuration.GetSection("Secret");
        if (secretSection.Value == null)
            throw new Exception("Secret not set.");

        var key = Encoding.ASCII.GetBytes(secretSection.Value);
        return key;
    }

    public async Task UpdatePassword(UserPasswordUpdateDTO userPasswordUpdate, Guid userId)
    {
        var userEntity = await _userRepository.GetByIdAsync(userId) ?? throw new HttpException("Auth is bad.", 401);

        var passwordMatch = _passwordHasher.Verify(userPasswordUpdate.OldPassword, userEntity.Pass);
        
        if (!passwordMatch) 
            throw new HttpException("Password wrong.");

        if (!CheckPasswordStrong(userPasswordUpdate.NewPassword))
            throw new HttpException("The password is not complex enough. The password must be at least 8 characters long. Use a different letter case and numbers.");

        var passwordHash = _passwordHasher.GenerateHash(userPasswordUpdate.NewPassword);
        
        await _userRepository.UpdatePasswordAsync(userId, passwordHash);
    }

    public async Task UpdateTariff(Guid userId, TariffEnum tariff)
    {
        await _userRepository.ChangeTariffAsync(userId, tariff);
    }

    public async Task<TariffEnum> GetUserTariff(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new HttpException("Auth is bad.", 401);

        return user.TariffId ?? TariffEnum.None;
    }

    public async Task<int> GetCountOfAvailableToCreateSessions(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new HttpException("Auth is bad.", 401);

        return user.CountSessionsAvailableToCreate;
    }

    public async Task<string> GetLinkForPayNewTariff(Guid userId, TariffEnum tariff)
    {
        var newPayment = new NewPayment
        {
            Confirmation = new Confirmation
            {
                Type = ConfirmationType.Redirect,
                ReturnUrl = "https://ещевопрос.рф/account/tariff"
            },
            Description = "Payment of tariff on platform",
            MerchantCustomerId = userId.ToString(),
            Metadata = new Dictionary<string, string>
            {
                {"userId", userId.ToString()},
                {"tariffId", tariff.ToString()}
            },
            Amount = tariff switch
            {
                TariffEnum.Annual => new Amount { Value = 20000.00m, Currency = "RUB" },
                TariffEnum.Single => new Amount { Value = 1000.00m, Currency = "RUB" },

                TariffEnum.None => throw new HttpException("Select tariff not available"),
                TariffEnum.Education => throw new HttpException("Select tariff not available"),
                _ => throw new HttpException("Select tariff not available")
            }
        };

        var payment = await PaymentClient.CreatePaymentAsync(newPayment);

        return payment.Confirmation.ConfirmationUrl;
    }

    public async Task ConfirmPayTariff(Message message)
    {
        var payment = message.Object;
        var userId = payment.Metadata["userId"];
        var tariffString = payment.Metadata["tariffId"];

        if (tariffString == null || userId == null)
        {
            return;
        }

        var tariff = (TariffEnum)Enum.Parse(typeof(TariffEnum), tariffString);

        if (message.Event == Event.PaymentWaitingForCapture && payment.Paid)
        {
            await PaymentClient.CapturePaymentAsync(payment.Id);

            switch (tariff)
            {
                case TariffEnum.Annual:
                    await _userRepository.IncrementCountOfAvailableToCreateSessions(new Guid(payment.Metadata["userId"]), 40);
                    break;
                case TariffEnum.Single:
                    await _userRepository.IncrementCountOfAvailableToCreateSessions(new Guid(payment.Metadata["userId"]));
                    break;
                case TariffEnum.None:
                    break;
                case TariffEnum.Education:
                    break;
            }
        }
    }
}

