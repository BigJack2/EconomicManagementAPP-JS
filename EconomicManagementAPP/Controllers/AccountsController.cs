using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {

        private readonly IRepositorieAccounts repositorieAccounts;


        //Inicializamos los Repositories para inyectarlas funcionalidades de interfaz
        //Las funcionalidades de interfaz Ejecutan querys para el CRUD
        public AccountsController(IRepositorieAccounts repositorieAccounts)
        {
            this.repositorieAccounts = repositorieAccounts;
        }


        //Creamos index para ejecutar la interfaz asincrona
        //Mediante metodos Get traemos ciertos datos mediante las querys de la interfaz
        //VISTA DEL CREATE
        public async Task<IActionResult> Index()
        {
            var accounts = await repositorieAccounts.getAccounts();
            return View(accounts);
        }
        public IActionResult Create()
        {
            return View();
        }


        //EJECUCION DEL CREATE
        [HttpPost]
        public async Task<IActionResult> Create(Accounts accounts)
        {
            //Con el IsValid ejecutamos las validaciones de formulario que tambien se encuentran asociadas en las vistas del Repositorie y el Model
            if (!ModelState.IsValid)
            {
                return View(accounts);
            }

            // Validamos si ya existe antes de registrar
            var accountExist =
               await repositorieAccounts.Exist(accounts.Name);

            if (accountExist)
            {
                ModelState.AddModelError(nameof(accounts.Name),
                    $"The account {accounts.Name} already exist.");

                return View(accounts);
            }
            await repositorieAccounts.Create(accounts);
            //Redireccionamos a la lista index de la carpeta Create
            return RedirectToAction("Index");
        }


        //La validacion se activa automaticamente en el front con la comparacion del dato
        [HttpGet]
        public async Task<IActionResult> VerificaryAccount(string Name)
        {
            //En esta variable gurdamos el dato para despues compararlo
            var accountExist = await repositorieAccounts.Exist(Name);

            if (accountExist)
            {
                //Este es el mensaje que arroja la validacion anterior
                return Json($"The account {Name} already exist");
            }

            return Json(true);
        }


        //VISTA DEL MODIFY
        [HttpGet]
        public async Task<ActionResult> Modify(int Id)
        {
            // Traemos toda la info mediante el ID, el cual se pasa como parametro en la query del Repositorie
            var account = await repositorieAccounts.getAccountsById(Id);

            if (account is null)
            {
                //Redireccio cuando esta vacio
                return RedirectToAction("NotFound", "Home");
            }

            return View(account);
        }


        //EJECUCON DEL MODIFY
        [HttpPost]
        public async Task<ActionResult> Modify(Accounts accounts)
        {
            var account = await repositorieAccounts.getAccountsById(accounts.Id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la modificacion con este parametro que lleva toda la info a la query del Repositorie
            await repositorieAccounts.Modify(accounts);
            return RedirectToAction("Index");
        }


        //VISTA DEL DELETE
        //Traemos toda la info a travez del ID
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var account = await repositorieAccounts.getAccountsById(Id);

            //Validamos si hemos encontrado el registro perteneciente a esa ID
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Retornamos la vista de lo que vamos a eliminar
            return View(account);
        }


        //EJECUCION DEL DELETE
        [HttpPost]
        //DeleteUser es el parametro que se jecuta en el asp-action del form
        public async Task<IActionResult> DeleteAccount(int Id)
        {
            var account = await repositorieAccounts.getAccountsById(Id);

            //Validamos que el id con el cual estamos buscando el dato relacionado exista.
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la query de eliminacion que se encuentra en el Repositorie
            await repositorieAccounts.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
