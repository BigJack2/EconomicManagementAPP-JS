using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;


        //Inicializamos los Repositories para inyectarlas funcionalidades de interfaz
        //Las funcionalidades de interfaz Ejecutan querys para el CRUD
        public AccountTypesController(IRepositorieAccountTypes repositorieAccountTypes)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
        }


        //Creamos index para ejecutar la interfaz asincrona
        //Mediante metodos Get traemos ciertos datos mediante las querys de la interfaz
        //VISTA DEL CREATE
        public async Task<IActionResult> Index()
        {           
            var accountTypes = await repositorieAccountTypes.getAccounts();
            return View(accountTypes);
        }
        public IActionResult Create()
        {
            return View();
        }


        //EJECUCION DEL CREATE
        [HttpPost]
        public async Task<IActionResult> Create(AccountTypes accountTypes)
        {
            //Con el IsValid ejecutamos las validaciones de formulario que tambien se encuentran asociadas en las vistas del Repositorie y el Model
            if (!ModelState.IsValid)
            {
                return View(accountTypes);
            }

            //En esta variable gurdamos el dato para despues comparar si existen
            var accountTypeExist = await repositorieAccountTypes.Exist(accountTypes.Name);

            if (accountTypeExist)
            {
                ModelState.AddModelError(nameof(accountTypes.Name),
                    $"The account {accountTypes.Name} already exist.");

                return View(accountTypes);
            }
            await repositorieAccountTypes.Create(accountTypes);
            //Redireccionamos a la lista index de la carpeta Create
            return RedirectToAction("Index");
        }


        //La validacion se activa automaticamente en el front con la comparacion del dato
        [HttpGet]
        public async Task<IActionResult> VerificaryAccountType(string Name)
        {
            
            //En esta variable gurdamos el dato para despues compararlo
            var accountTypeExist = await repositorieAccountTypes.Exist(Name);

            if (accountTypeExist)
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
            var accountType = await repositorieAccountTypes.getAccountById(Id);

            if (accountType is null)
            {
                //Redireccio cuando esta vacio
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }


        //EJECUCON DEL MODIFY
        [HttpPost]
        public async Task<ActionResult> Modify(AccountTypes accountTypes)
        {           
            var accountType = await repositorieAccountTypes.getAccountById(accountTypes.Id);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la modificacion con este parametro que lleva toda la info a la query del Repositorie
            await repositorieAccountTypes.Modify(accountTypes);
            return RedirectToAction("Index");
        }


        //VISTA DEL DELETE
        //Traemos toda la info a travez del ID
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {            
            var account = await repositorieAccountTypes.getAccountById(Id);

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
            var account = await repositorieAccountTypes.getAccountById(Id);

            //Validamos que el id con el cual estamos buscando el dato relacionado exista.
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la query de eliminacion que se encuentra en el Repositorie
            await repositorieAccountTypes.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
