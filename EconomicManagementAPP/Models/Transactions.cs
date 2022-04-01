using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Transactions
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        public string Total { get; set; }       

        [Required(ErrorMessage = "{0} is required")]
        public int OperationTypeId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        [StringLength(maximumLength:400, ErrorMessage = "You cannot enter more than 400 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }

    }
}
