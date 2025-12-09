using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Application.Resources;
using HelpTrack.Helpers;
using HelpTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using X.PagedList;
using X.PagedList.Extensions;
using System.Linq;

namespace HelpTrack.Web.Controllers
{
    [RequireAdmin]
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceRol _serviceRol;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private const int PageSize = 10;

        public UsuarioController(IServiceUsuario serviceUsuario, IServiceRol serviceRol, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _serviceUsuario = serviceUsuario ?? throw new ArgumentNullException(nameof(serviceUsuario));
            _serviceRol = serviceRol ?? throw new ArgumentNullException(nameof(serviceRol));
            _sharedLocalizer = sharedLocalizer ?? throw new ArgumentNullException(nameof(sharedLocalizer));
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string? searchString)
        {
            try
            {
                int pageNumber = page ?? 1;
                var usuarios = await _serviceUsuario.ListAsync();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    ViewData["CurrentFilter"] = searchString;
                    usuarios = usuarios
                        .Where(u =>
                            (!string.IsNullOrEmpty(u.Nombre) && u.Nombre.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                var pagedList = usuarios.ToPagedList(pageNumber, PageSize);
                return View(pagedList);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, _sharedLocalizer["ErrorLoadingUsers"]);
                return View(new PagedList<UsuarioDTO>(new List<UsuarioDTO>(), 1, 1));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var usuario = await _serviceUsuario.FindByIdAsync(id);
                if (usuario == null)
                {
                    TempData["ErrorMessage"] = _sharedLocalizer["UserNotFound"].Value;
                    return RedirectToAction(nameof(Index));
                }

                var roles = await _serviceUsuario.GetUsuarioRolesAsync(id);
                ViewBag.Roles = roles.OrderBy(r => r.Nombre).ToList();

                return View(usuario);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, _sharedLocalizer["ErrorLoadingUser"]);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UsuarioDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Nombre,Activo,Password")] UsuarioDTO usuarioDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe un usuario con ese email
                    var existing = await _serviceUsuario.FindByEmailAsync(usuarioDto.Email);
                    if (existing != null)
                    {
                        ModelState.AddModelError("Email", _sharedLocalizer["EmailExists"]);
                        return View(usuarioDto);
                    }

                    if (string.IsNullOrWhiteSpace(usuarioDto.Password))
                    {
                        ModelState.AddModelError("Password", _sharedLocalizer["PasswordRequired"]);
                        return View(usuarioDto);
                    }

                    await _serviceUsuario.RegisterAsync(usuarioDto, usuarioDto.Password);
                    TempData["SuccessMessage"] = _sharedLocalizer["UserCreatedSuccess"].Value;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, string.Format(_sharedLocalizer["ErrorCreatingUser"], ex.Message));
                }
            }
            return View(usuarioDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var usuario = await _serviceUsuario.FindByIdAsync(id);
                if (usuario == null)
                {
                    TempData["ErrorMessage"] = _sharedLocalizer["UserNotFound"].Value;
                    return RedirectToAction(nameof(Index));
                }

                var rolesDisponibles = await _serviceRol.ListAsync();
                var rolesUsuario = await _serviceUsuario.GetUsuarioRolesAsync(id);

                var viewModel = new UsuarioEditViewModel
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Activo = usuario.Activo,
                    FechaCreacion = usuario.FechaCreacion,
                    UltimoInicioSesion = usuario.UltimoInicioSesion,
                    Roles = rolesDisponibles.Select(r => new RolSeleccionableViewModel
                    {
                        IdRol = r.IdRol,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Seleccionado = rolesUsuario.Any(ru => ru.IdRol == r.IdRol)
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, _sharedLocalizer["ErrorLoadingUser"]);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioEditViewModel viewModel)
        {
            if (id != viewModel.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDto = new UsuarioDTO
                    {
                        IdUsuario = viewModel.IdUsuario,
                        Nombre = viewModel.Nombre,
                        Email = viewModel.Email,
                        Activo = viewModel.Activo,
                        FechaCreacion = viewModel.FechaCreacion,
                        UltimoInicioSesion = viewModel.UltimoInicioSesion
                    };

                    await _serviceUsuario.UpdateAsync(id, usuarioDto);

                    var rolesActuales = await _serviceUsuario.GetUsuarioRolesAsync(id);
                    var rolesSeleccionadosIds = viewModel.Roles.Where(r => r.Seleccionado).Select(r => r.IdRol).ToList();

                    foreach (var rol in rolesActuales)
                    {
                        if (!rolesSeleccionadosIds.Contains(rol.IdRol))
                        {
                            await _serviceUsuario.RemoveRoleFromUsuarioAsync(id, rol.IdRol);
                        }
                    }

                    foreach (var rolId in rolesSeleccionadosIds)
                    {
                        if (!rolesActuales.Any(r => r.IdRol == rolId))
                        {
                            await _serviceUsuario.AssignRoleToUsuarioAsync(id, rolId);
                        }
                    }

                    TempData["SuccessMessage"] = _sharedLocalizer["UserUpdatedSuccessfully"].Value;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error al actualizar usuario: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError(string.Empty, $"Detalle: {ex.InnerException.Message}");
                    }
                }
            }

            var rolesDisponibles = await _serviceRol.ListAsync();
            viewModel.Roles = rolesDisponibles.Select(r => new RolSeleccionableViewModel
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                Seleccionado = viewModel.Roles.Any(vr => vr.IdRol == r.IdRol && vr.Seleccionado)
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _serviceUsuario.DeleteAsync(id);
                if (!result)
                {
                    TempData["Error"] = _sharedLocalizer["ErrorDeletingUserNotFound"].Value;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = _sharedLocalizer["UserDeletedSuccess"].Value;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = _sharedLocalizer["ErrorDeletingUser"].Value;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
