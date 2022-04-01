using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Validations
{
    public class FirstCapitalLetter: ValidationAttribute
    {
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext) 
        { 
            // Validamos que el String no se aNull
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            //Validamos que el caracter en posicion 0 sea Mayuscula con el firstLetter.ToUpper
            var firstLetter = value.ToString()[0].ToString();

            if (firstLetter != firstLetter.ToUpper())
            {   
                //Este es el mensaje que se muestra cuando la primera letra es minuscula
                return new ValidationResult("The first letter must be in uppercase");
            }

            //Retornamos una validacion exitosa si todo se cumple
            return ValidationResult.Success;
        }
    }




}
