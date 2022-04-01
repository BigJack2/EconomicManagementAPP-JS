using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class OperationTypes
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        [Remote(action: "VerificaryOperationType", controller: "OperationTypes")]
        [StringLength(maximumLength: 400, ErrorMessage = "You cannot enter more than 400 characters")]
        //Validamos que la casilla no este vacia y que el correo inicie por mayuscula
        public string Description { get; set; }

    }
}
