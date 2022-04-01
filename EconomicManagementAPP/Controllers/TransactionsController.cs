using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Controllers
{
    public class TransactionsController : Controller
    {

        private readonly IRepositorieTransactions repositorieTransactions;

        //Inicializamos los Repositories para inyectarlas funcionalidades de interfaz
        //Las funcionalidades de interfaz Ejecutan querys para el CRUD
        public TransactionsController(IRepositorieTransactions repositorieTransactions)
        {
            this.repositorieTransactions = repositorieTransactions;
        }


        //Creamos index para ejecutar la interfaz asincrona
        //Mediante metodos Get traemos ciertos datos mediante las querys de la interfaz
        //VISTA DEL CREATE
        public async Task<IActionResult> Index()
        {
            var transactions = await repositorieTransactions.getTransactions();
            return View(transactions);
        }
        public IActionResult Create()
        {
            return View();
        }


        //EJECUCION DEL CREATE
        [HttpPost]
        public async Task<IActionResult> Create(Transactions transactions)
        {
            //Con el IsValid ejecutamos las validaciones de formulario que tambien se encuentran asociadas en las vistas del Repositorie y el Model
            if (!ModelState.IsValid)
            {
                return View(transactions);
            }

            await repositorieTransactions.Create(transactions);
            //Redireccionamos a la lista index de la carpeta Create
            return RedirectToAction("Index");
        }


        //VISTA DEL MODIFY
        [HttpGet]
        public async Task<ActionResult> Modify(int Id)
        {
            // Traemos toda la info mediante el ID, el cual se pasa como parametro en la query del Repositorie
            var transaction = await repositorieTransactions.getTransactionsById(Id);

            if (transaction is null)
            {
                //Redireccio cuando esta vacio
                return RedirectToAction("NotFound", "Home");
            }

            return View(transaction);
        }


        //EJECUCON DEL MODIFY
        [HttpPost]
        public async Task<ActionResult> Modify(Transactions transactions)
        {
            var transaction = await repositorieTransactions.getTransactionsById(transactions.Id);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la modificacion con este parametro que lleva toda la info a la query del Repositorie
            await repositorieTransactions.Modify(transactions);
            return RedirectToAction("Index");
        }


        //VISTA DEL DELETE
        //Traemos toda la info a travez del ID
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var transaction = await repositorieTransactions.getTransactionsById(Id);

            //Validamos si hemos encontrado el registro perteneciente a esa ID
            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Retornamos la vista de lo que vamos a eliminar
            return View(transaction);
        }


        //EJECUCION DEL DELETE
        [HttpPost]
        //DeleteUser es el parametro que se jecuta en el asp-action del form
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            var transaction = await repositorieTransactions.getTransactionsById(Id);

            //Validamos que el id con el cual estamos buscando el dato relacionado exista.
            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la query de eliminacion que se encuentra en el Repositorie
            await repositorieTransactions.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
