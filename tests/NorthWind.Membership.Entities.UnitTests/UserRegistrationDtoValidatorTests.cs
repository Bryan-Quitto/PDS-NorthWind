using NorthWind.Membership.Entities.Dtos;
using NorthWind.Membership.Entities.Resources;
using NorthWind.Membership.Entities.Validators;
using Xunit;

namespace NorthWind.Membership.Entities.UnitTests;

public class UserRegistrationDtoValidatorTests
{
    private readonly UserRegistrationDtoValidator _validator = new();

    private static UserRegistrationDto CreateValidDto() => new()
    {
        FirstName = "Juan",
        LastName = "Pérez",
        Email = "juan.perez@northwind.com",
        Password = "Password123!"
    };

    [Theory]
    [InlineData("Juan1", false)]       // Contiene números
    [InlineData("   ", false)]         // Solo espacios
    [InlineData("----", false)]        // Solo guiones
    [InlineData(" Pedro", false)]      // Espacio al inicio (no trimmeado)
    [InlineData("Pedro ", false)]      // Espacio al final (no trimmeado)
    [InlineData("Ana-María", true)]    // Válido con guion y tilde
    [InlineData("O'Connor", true)]     // Válido con apóstrofe
    [InlineData("María José", true)]   // Válido con espacio intermedio y tilde
    public async Task ValidateAsync_FirstNameCases_ExpectedValidity(string firstName, bool expectedValid)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.FirstName = firstName;

        // Act
        var errors = await _validator.ValidateAsync(dto);

        // Assert
        var firstNameErrors = errors.Where(e => e.PropertyName == nameof(UserRegistrationDto.FirstName)).ToList();
        if (expectedValid)
        {
            Assert.Empty(firstNameErrors);
        }
        else
        {
            Assert.NotEmpty(firstNameErrors);
        }
    }

    [Fact]
    public async Task ValidateAsync_FirstNameExceeds100Characters_ReturnsError()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.FirstName = new string('A', 101);

        // Act
        var errors = await _validator.ValidateAsync(dto);

        // Assert
        Assert.Contains(errors, e => e.PropertyName == nameof(UserRegistrationDto.FirstName) &&
                                     e.ErrorMessage == MembershipMessages.FirstNameMaxLength);
    }

    [Theory]
    [InlineData("Pérez1", false)]       // Contiene números
    [InlineData("   ", false)]          // Solo espacios
    [InlineData("----", false)]         // Solo guiones
    [InlineData(" López", false)]       // Espacio al inicio
    [InlineData("De la Torre", true)]   // Válido compuesto
    [InlineData("Núñez-García", true)]  // Válido compuesto con guion y ñ
    public async Task ValidateAsync_LastNameCases_ExpectedValidity(string lastName, bool expectedValid)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.LastName = lastName;

        // Act
        var errors = await _validator.ValidateAsync(dto);

        // Assert
        var lastNameErrors = errors.Where(e => e.PropertyName == nameof(UserRegistrationDto.LastName)).ToList();
        if (expectedValid)
        {
            Assert.Empty(lastNameErrors);
        }
        else
        {
            Assert.NotEmpty(lastNameErrors);
        }
    }

    [Fact]
    public async Task ValidateAsync_LastNameExceeds100Characters_ReturnsError()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.LastName = new string('B', 101);

        // Act
        var errors = await _validator.ValidateAsync(dto);

        // Assert
        Assert.Contains(errors, e => e.PropertyName == nameof(UserRegistrationDto.LastName) &&
                                     e.ErrorMessage == MembershipMessages.LastNameMaxLength);
    }

    [Theory]
    [InlineData("test+alias@domain.com", false)]   // Contiene '+' (rechazado por criterio estricto)
    [InlineData("test..user@domain.com", false)]   // Puntos consecutivos '..'
    [InlineData("sin_arroba.com", false)]          // Sin '@'
    [InlineData("correo@dominio.c", false)]        // TLD de solo 1 letra
    [InlineData("correo@dominio..com", false)]     // Puntos consecutivos en dominio
    [InlineData("correo con espacio@dom.com", false)] // Espacios en blanco
    [InlineData("usuario@dominio.com", true)]      // Válido estándar
    [InlineData("usuario.nombre_123@sub.dominio.org", true)] // Válido con punto, guion bajo y números
    [InlineData("cliente-vip@empresa.com.ec", true)] // Válido con guiones y subdominio de país
    public async Task ValidateAsync_EmailCases_ExpectedValidity(string email, bool expectedValid)
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Email = email;

        // Act
        var errors = await _validator.ValidateAsync(dto);

        // Assert
        var emailErrors = errors.Where(e => e.PropertyName == nameof(UserRegistrationDto.Email)).ToList();
        if (expectedValid)
        {
            Assert.Empty(emailErrors);
        }
        else
        {
            Assert.NotEmpty(emailErrors);
        }
    }

    [Fact]
    public async Task ValidateAsync_EmailExceeds256Characters_ReturnsError()
    {
        // Arrange
        var dto = CreateValidDto();
        var longUser = new string('a', 250);
        dto.Email = $"{longUser}@domain.com"; // > 256 chars

        // Act
        var errors = await _validator.ValidateAsync(dto);

        // Assert
        Assert.Contains(errors, e => e.PropertyName == nameof(UserRegistrationDto.Email) &&
                                     e.ErrorMessage == MembershipMessages.EmailMaxLength);
    }

    [Fact]
    public async Task ValidateAsync_EmailWithConsecutiveDots_ReturnsSpecificErrorMessage()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Email = "test..user@domain.com";

        // Act
        var errors = await _validator.ValidateAsync(dto);

        // Assert
        Assert.Contains(errors, e => e.PropertyName == nameof(UserRegistrationDto.Email) &&
                                     e.ErrorMessage == MembershipMessages.EmailConsecutiveDots);
    }
}
