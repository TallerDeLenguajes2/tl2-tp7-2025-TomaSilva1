using System.Formats.Asn1;
using Microsoft.Data.Sqlite;

public class PresupuestoRepositorio
{
    private string _connexionDb = "Data Source=DB/nueva.db";

    public int crearPresupuesto(Presupuestos P)
    {
        int nuevoID;

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        string sql = "INSERT INTO presupuestos (nombreDestinatario, fechaCreacion) VALUES (@nombre, @fecha); SELECT last_insert_rowid();";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@nombre", P.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@fecha", P.FechaCreacion));

        nuevoID = Convert.ToInt32(comando.ExecuteScalar());

        return nuevoID;
    }

    public List<Presupuestos> obtenerPresupuestos()
    {
        string sql = "SELECT * FROM presupuestos";
        List<Presupuestos> presupuestos = [];

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);

        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            var p = new Presupuestos
            {
                IdPresupuesto = Convert.ToInt32(lector["idPresupuesto"]),
                NombreDestinatario = lector["nombreDestinatario"].ToString(),
                FechaCreacion = lector["fechaCreacion"].ToString(),
                Detalle = new List<PresupuestoDetalle>()
            };

            string sql2 = "SELECT idProducto, descripcion, precio, cantidad FROM presupuestoDetalle INNER JOIN productos p ON p.id_prod = id_prod WHERE idPresupuesto = @id";

            using var comando2 = new SqliteCommand(sql2, conexion);
            comando2.Parameters.Add(new SqliteParameter("@id", p.IdPresupuesto));
            using var lector2 = comando2.ExecuteReader();

            while (lector2.Read())
            {
                var pp = new Productos()
                {
                    IdProducto = Convert.ToInt32(lector2["idProducto"]),
                    Descripcion = lector2["descripcion"].ToString(),
                    Precio = Convert.ToInt32(lector2["precio"])
                };
                var pd = new PresupuestoDetalle()
                {
                    Producto = pp,
                    Cantidad = Convert.ToInt32(lector2["cantidad"])
                };
                p.Detalle.Add(pd);
            }

            presupuestos.Add(p);
        }

        return presupuestos;
    }

    public Presupuestos obtenerPresupuestoPorId(int id)
    {
        string sql = "SELECT * FROM presupuestos WHERE idPresupuesto = @id";

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", id));

        using var lector = comando.ExecuteReader();

        if (!lector.Read())
        {
            return null;
        }

        var presupuesto = new Presupuestos
        {
            IdPresupuesto = Convert.ToInt32(lector["idPresupuesto"]),
            NombreDestinatario = lector["nombreDestinatario"].ToString(),
            FechaCreacion = lector["fechaCreacion"].ToString(),
            Detalle = new List<PresupuestoDetalle>()
        };

        string sql2 = "SELECT idProducto, descripcion, precio, cantidad FROM presupuestoDetalle INNER JOIN productos p ON p.id_prod = id_prod WHERE idPresupuesto = @id";

        using var comando2 = new SqliteCommand(sql2, conexion);
        comando2.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));
        using var lector2 = comando2.ExecuteReader();

        while (lector2.Read())
        {
            var pp = new Productos()
            {
                IdProducto = Convert.ToInt32(lector2["idProducto"]),
                Descripcion = lector2["descripcion"].ToString(),
                Precio = Convert.ToInt32(lector2["precio"])
            };
            var pd = new PresupuestoDetalle()
            {
                Producto = pp,
                Cantidad = Convert.ToInt32(lector2["cantidad"])
            };
            presupuesto.Detalle.Add(pd);
        }

        return presupuesto;
    }
}