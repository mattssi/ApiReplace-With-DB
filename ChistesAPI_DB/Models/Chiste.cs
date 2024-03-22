using System;
using System.Collections.Generic;

namespace ChistesAPI_DB.Models;

public partial class Chiste
{
    public int Id { get; set; }

    public string Contenido { get; set; } = null!;
}
