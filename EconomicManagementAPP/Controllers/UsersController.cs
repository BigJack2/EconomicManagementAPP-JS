using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.IRepositories;

namespace EconomicManagementAPP.Controllers
{
    public class UsersController : Controller
    {

        private readonly IRepositorieUsers repositorieUsers;


        //Inicializamos los Repositories para inyectarlas funcionalidades de interfaz
        //Las funcionalidades de interfaz Ejecutan querys para el CRUD
        public UsersController(IRepositorieUsers repositorieUsers)
        {
            this.repositorieUsers = repositorieUsers;
        }


        //Creamos index para ejecutar la interfaz asincrona
        //Mediante metodos Get traemos ciertos datos mediante las querys de la interfaz
        //VISTA DEL CREATE
        public async Task<IActionResult> Index()
        {

            var users = await repositorieUsers.getUser();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }


        //EJECUCION DEL CREATE
        [HttpPost]
        public async Task<IActionResult> Create(Users users)
        {
            //Con el IsValid ejecutamos las validaciones de formulario que tambien se encuentran asociadas en las vistas del Repositorie y el Model
            if (!ModelState.IsValid)
            {
                return View(users);
            }

            //En esta variable gurdamos el Email para despues compararlo
            var usersExist = await repositorieUsers.Exist(users.Email);

            if (usersExist)
            {
                //Se valida si el usuario existe con el Email como parametro unico perteneciente al usuario
                ModelState.AddModelError(nameof(users.Email),
                    $"The account {users.Email} already exist.");

                return View(users);
            }
            await repositorieUsers.Create(users);
            //Redireccionamos a la lista de usuarios que en este caso es index de la carpeta del Views
            return RedirectToAction("Index");
        }


        //La validacion se activa automaticamente en el front con la comparacion del Email
        [HttpGet]
        public async Task<IActionResult> VerificaryUsers(string Email)
        {
            var usersExist = await repositorieUsers.Exist(Email);

            if (usersExist)
            {   
                //Este es el mensaje que arroja la validacion anterior
                return Json($"The account {Email} already exist.");
            }

            return Json(true);
        }


        //VISTA DEL MODIFY
        [HttpGet]
        public async Task<ActionResult> Modify(int Id)
        {

            //Traemos toda la info mediante el ID, el cual se pasa como parametro en la query del Repositorie
            var user = await repositorieUsers.getUserById(Id);

            if (user is null)
            {
                //Redireccio cuando esta vacio redirecciona a NotFound
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }


        //EJECUCON DEL MODIFY
        [HttpPost]
        public async Task<ActionResult> Modify(Users users)
        {
            var user = await repositorieUsers.getUserById(users.Id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(users);
            }

            //Ejecutamos la modificacion con este parametro que lleva toda la info a la query del Repositorie
            await repositorieUsers.Modify(users);
            return RedirectToAction("Index");
        }


        //VISTA DEL DELETE
        //Traemos toda la info a travez del ID
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var user = await repositorieUsers.getUserById(Id);

            //Validamos si hemos encontrado el registro perteneciente a esa ID
            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Retornamos la vista de lo que vamos a eliminar
            return View(user);
        }


        //EJECUCION DEL DELETE
        [HttpPost]
        //DeleteUser es el parametro que se jecuta en el asp-action del form
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var user = await repositorieUsers.getUserById(Id);

            //Validamos que el id con el cual estamos buscando el dato relacionado exista.
            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            //Ejecutamos la query de eliminacion que se encuentra en el Repositorie
            await repositorieUsers.Delete(Id);
            return RedirectToAction("Index");
        }


        //VISTA DEL LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        //EJECUCION DEL LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            //Ejecutamos el IsValid para que los datos digitados sean validos 
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            //Mediante esta variable Capturamos los datos digitados
            var result = await repositorieUsers.Login(loginViewModel.Email, loginViewModel.Password);

            //Valida que los datos no esten Null
            if (result is null)
            { //Revisar si El correo o la Contraseña son correctos o incorrectos
                ModelState.AddModelError(String.Empty, "Worng Email or Password");
                return View(loginViewModel);
            }
            else
            {
                //A donde lo llevamos cuando ya se ha logueado
                //Lo llevara a la carpeta Create de AccounTypes
                return RedirectToAction("Create", "AccountTypes");
            }


        }
    }
}
