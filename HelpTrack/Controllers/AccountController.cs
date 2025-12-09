using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HelpTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public AccountController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _serviceUsuario.LoginAsync(model.Email, model.Password);

                if (user != null)
                {
                    if (!user.Activo)
                    {
                        TempData["ErrorMessage"] = "Su cuenta está inactiva. Por favor contacte al administrador.";
                        ModelState.AddModelError(string.Empty, "Su cuenta está inactiva.");
                        return View(model);
                    }

                    // Determinar si es administrador (por ahora basado en email)
                    var isAdmin = user.Email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase);
                    
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Nombre),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("IdUsuario", user.IdUsuario.ToString()),
                        new Claim(ClaimTypes.Role, isAdmin ? "Administrador" : "Usuario")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    await _serviceUsuario.UpdateLastLoginAsync(user.IdUsuario);

                    TempData["SuccessMessage"] = $"¡Bienvenido de nuevo, {user.Nombre}!";
                    return RedirectToAction("Index", "Home");
                }

                TempData["ErrorMessage"] = "Correo o contraseña incorrectos. Por favor, verifica tus credenciales.";
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if email exists
                var existingUser = await _serviceUsuario.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "El correo electrónico ya está registrado. Por favor usa otro correo.";
                    ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
                    return View(model);
                }

                var newUser = new UsuarioDTO
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    Activo = true
                };

                await _serviceUsuario.RegisterAsync(newUser, model.Password);

                // Auto login after register
                TempData["SuccessMessage"] = "¡Cuenta creada exitosamente!";
                return await Login(new LoginViewModel { Email = model.Email, Password = model.Password });
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _serviceUsuario.UpdatePasswordAsync(model.Email, model.NewPassword);
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Contraseña actualizada exitosamente. Ahora puedes iniciar sesión con la nueva contraseña.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "No se encontró un usuario con ese correo electrónico.";
                    ModelState.AddModelError(string.Empty, "No se encontró un usuario con ese correo electrónico.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var usuario = await _serviceUsuario.FindByEmailAsync(email);
            if (usuario == null)
            {
                return RedirectToAction("Logout");
            }

            var userDto = new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion,
                UltimoInicioSesion = usuario.UltimoInicioSesion
            };

            return View(userDto);
        }
        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
