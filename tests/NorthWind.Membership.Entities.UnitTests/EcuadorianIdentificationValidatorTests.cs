using NorthWind.Sales.Validators.Entities.Common;

namespace NorthWind.Membership.Entities.UnitTests;

public class EcuadorianIdentificationValidatorTests
{
    [Theory]
    [InlineData("0102030400")] // Cédula Azuay válida (suma 10 -> verificador 0)
    [InlineData("1803456787")] // Cédula Tungurahua válida (suma 43 -> verificador 7)
    [InlineData("1712345675")] // Cédula Pichincha válida (suma 35 -> verificador 5)
    [InlineData("1710034065")] // Cédula Pichincha válida
    public void IsValidNatural_WithValidCedula_ShouldReturnTrue(string cedula)
    {
        var result = EcuadorianIdentificationValidator.IsValidNatural(cedula);
        Assert.True(result);
    }

    [Theory]
    [InlineData("0102030409")] // Verificador incorrecto (debería ser 0)
    [InlineData("1803456782")] // Verificador incorrecto (debería ser 7)
    [InlineData("9902030409")] // Código de provincia inválido (99)
    [InlineData("0162030409")] // Tercer dígito >= 6 no es persona natural
    [InlineData("010203040")]   // Menos de 10 dígitos
    [InlineData("0102030400A")] // Carácter no numérico
    [InlineData("")]
    [InlineData(null)]
    public void IsValidNatural_WithInvalidCedula_ShouldReturnFalse(string? cedula)
    {
        var result = EcuadorianIdentificationValidator.IsValidNatural(cedula);
        Assert.False(result);
    }

    [Theory]
    [InlineData("1712345675001")] // RUC Natural con sucursal 001
    [InlineData("1712345675002")] // RUC Natural con sucursal 002
    public void IsValidNatural_WithValidNaturalRuc_ShouldReturnTrue(string ruc)
    {
        var result = EcuadorianIdentificationValidator.IsValidNatural(ruc);
        Assert.True(result);
    }

    [Theory]
    [InlineData("1712345676001")] // Base de cédula con dígito verificador inválido (esperado 5, dado 6)
    [InlineData("1712345675000")] // Sucursal 000 es inválida
    [InlineData("171234567500")]  // Longitud 12 dígitos
    public void IsValidNatural_WithInvalidNaturalRuc_ShouldReturnFalse(string ruc)
    {
        var result = EcuadorianIdentificationValidator.IsValidNatural(ruc);
        Assert.False(result);
    }

    [Theory]
    [InlineData("1790011674001")] // RUC Privado Pichincha Módulo 11 válido (verificador 4)
    [InlineData("1790087654001")] // RUC Privado Pichincha Módulo 11 válido (verificador 4)
    [InlineData("1790011674002")] // RUC Privado con establecimiento 002
    public void IsValidLegal_WithValidRuc_ShouldReturnTrue(string ruc)
    {
        var result = EcuadorianIdentificationValidator.IsValidLegal(ruc);
        Assert.True(result);
    }

    [Theory]
    [InlineData("1790012345001")] // Caso borde: suma da residuo 1 -> 11 - 1 = 10 (inválido SRI)
    [InlineData("1790087657001")] // Verificador incorrecto (esperado 4, dado 7)
    [InlineData("1780011674001")] // Tercer dígito no es 9 (es 8)
    [InlineData("1790011674000")] // Sucursal 000 inválida
    [InlineData("9990011674001")] // Provincia inválida (99)
    [InlineData("179001167400")]  // Longitud menor a 13
    [InlineData("")]
    [InlineData(null)]
    public void IsValidLegal_WithInvalidRuc_ShouldReturnFalse(string? ruc)
    {
        var result = EcuadorianIdentificationValidator.IsValidLegal(ruc);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_GeneralMethod_ShouldValidateBothNaturalAndLegal()
    {
        Assert.True(EcuadorianIdentificationValidator.IsValid("0102030400"));    // Cédula Natural
        Assert.True(EcuadorianIdentificationValidator.IsValid("1712345675001")); // RUC Natural
        Assert.True(EcuadorianIdentificationValidator.IsValid("1790011674001")); // RUC Jurídico

        Assert.False(EcuadorianIdentificationValidator.IsValid("0102030405"));   // Cédula inválida
        Assert.False(EcuadorianIdentificationValidator.IsValid("1790012345001"));// RUC Jurídico inválido (resto 10)
    }
}
