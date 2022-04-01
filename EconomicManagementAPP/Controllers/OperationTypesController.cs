using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Controllers
{
    public class OperationTypesController : Controller
    {
        private readonly IRepositorieOperationTypes repositorieOperationTypes;


        //Inicializamos los Repositories para inyectarlas funcionalidades de interfaz
        //Las funcionalidades de interfaz Ejecutan querys para el CRUD
        public OperationTypesController(IRepositorieOperationTypes repositorieOperationTypes)
        {
            this.repositorieOperationTypes = repositorieOperationTypes;
        }


        //Creamos index para ejecutar la interfaz asincrona
        //Mediante metodos Get traemos ciertos datos mediante las querys de la interfaz
        //VISTA DEL CREATE
        public async Task<IActionResult> Index()
        {
            var operationTypes = await repositorieOperationTypes.getOperationTypes();
            return View(operationTypes);
        }
        public IActionResult Create()
        {
            return View();
        }


        //EJECUCION DEL CREATE
        [HttpPost]
        public async Task<IActionResult> Create(OperationTypes operationTypes)
        {
            //Con el IsValid ejecutamos las validaciones de formulario que tambien se encuentran asociadas en las vistas del Repositorie y el Model
            if (!ModelState.IsValid)
            {
                return View(operationTypes);
            }

            //En esta variable gurdamos el dato para despues compararlo
            var operationTypeExist = await repositorieOperationTypes.Exist(operationTypes.Description);

            if (operationTypeExist)
            {
                ModelState.AddModelError(nameof(operationTypes.Description),
                    $"The operation types {operationTypes.Description} already exist.");

                return View(operationTypes);
            }

            await repositorieOperationTypes.Create(operationTypes);
            //Redireccionamos a la lista index de la carpeta Create
            return RedirectToAction("Index");
        }


        //La validacion se activa automaticamente en el front con la comparacion del dato
        [HttpGet]
        public async Task<IActionResult> VerificaryOperationType(string Description)
        {
            //En esta variable gurdamos el dato para despues compararlo
            var operationType = await repositorieOperationTypes.Exist(Description);

            if (operationType)
            {
                //Este es el mensaje que arroja la validacion anterior
                return Json($"The Operation {Description} already exist");
            }

            return Json(true);
        }


        //VISTA DEL MODIFY
        [HttpGet]
        public async Task<ActionResult> Modify(int Id)
        {
            // Traemos toda la info mediante el ID, el cual se pasa como parametro en la query del Repositorie
            var operationType = await repositorieOperationTypes.getOperationTypesById(Id);

            if (operationType is null)
            {
                //Redireccio cuando esta vacio
                return RedirectToAction("NotFound", "Home");
            }

            return View(operationType);
        }


        //EJECUCON DEL MODIFY
        [HttpPost]
        public async Task<ActionResult> Modify(OperationTypes operationTypes)
        {
            var operationType = await repositorieOperationTypes.getOperationTypesById(operationTypes.Id);

            if (operationType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la modificacion con este parametro que lleva toda la info a la query del Repositorie
            await repositorieOperationTypes.Modify(operationTypes);
            return RedirectToAction("Index");
        }


        //VISTA DEL DELETE
        //Traemos toda la info a travez del ID
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var operation = await repositorieOperationTypes.getOperationTypesById(Id);

            //Validamos si hemos encontrado el registro perteneciente a esa ID
            if (operation is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Retornamos la vista de lo que vamos a eliminar
            return View(operation);
        }


        //EJECUCION DEL DELETE
        [HttpPost]
        //DeleteUser es el parametro que se jecuta en el asp-action del form
        public async Task<IActionResult> DeleteOperation(int Id)
        {
            var operation = await repositorieOperationTypes.getOperationTypesById(Id);

            //Validamos que el id con el cual estamos buscando el dato relacionado exista.
            if (operation is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la query de eliminacion que se encuentra en el Repositorie
            await repositorieOperationTypes.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}

