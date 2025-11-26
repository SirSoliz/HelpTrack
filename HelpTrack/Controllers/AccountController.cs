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
                        ModelState.AddModelError(string.Empty, "Su cuenta está inactiva.");
                        return View(model);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Nombre),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("IdUsuario", user.IdUsuario.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    await _serviceUsuario.UpdateLastLoginAsync(user.IdUsuario);

                    return RedirectToAction("Index", "Home");
                }

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
        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
