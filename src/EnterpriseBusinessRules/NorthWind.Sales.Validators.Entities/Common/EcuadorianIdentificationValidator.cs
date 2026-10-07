namespace NorthWind.Sales.Validators.Entities.Common;

public static class EcuadorianIdentificationValidator
{
    public static bool IsValid(string? identification)
    {
        if (string.IsNullOrWhiteSpace(identification) || identification.Length != 10)
        {
            return false;
        }

        foreach (char c in identification)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }
        }

        int province = int.Parse(identification[..2]);
        if ((province < 1 || province > 24) && province != 30)
        {
            return false;
        }

        int thirdDigit = identification[2] - '0';
        if (thirdDigit >= 6)
        {
            return false;
        }

        int[] coefficients = [2, 1, 2, 1, 2, 1, 2, 1, 2];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            int digit = identification[i] - '0';
            int product = digit * coefficients[i];
            sum += product >= 10 ? product - 9 : product;
        }

        int checkDigit = (10 - (sum % 10)) % 10;
        int lastDigit = identification[9] - '0';

        return checkDigit == lastDigit;
    }
}
