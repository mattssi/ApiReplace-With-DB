namespace ChistesAPI_DB.Models
{
    public class AuditTables
    {
        public int? IdAudit { get; set; }
        public string? Tipo { get; set; }
        public string? Tabla { get; set; }
        public int? Registro { get; set; }
        public string? Campo { get; set; }
        public string? ValorAntes { get; set; }
        public string? ValorDespues { get; set; }
        public DateTime Fecha { get; set; }
        public string? Usuario { get; set; }
        public string? PC { get; set; }
        public string? DireccionIP { get; set; }

    }
}