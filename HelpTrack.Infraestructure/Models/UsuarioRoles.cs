using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class UsuarioRoles
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public virtual Roles IdRolNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
