using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Accounts
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        [Remote(action: "VerificaryAccount", controller: "Accounts")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int AccountTypeId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Balance { get; set; }

        [StringLength(maximumLength: 400, ErrorMessage = "You cannot enter more than 400 characters")]
        public string Description { get; set; }
    }
}
