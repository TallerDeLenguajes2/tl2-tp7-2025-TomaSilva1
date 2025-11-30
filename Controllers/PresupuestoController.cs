using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class PresupuestoController : ControllerBase
{
    private PresupuestoRepositorio _presupuestoRepositorio;
    public PresupuestoController()
    {
        _presupuestoRepositorio = new PresupuestoRepositorio();
    }

    [HttpGet]
    public ActionResult<List<Presupuestos>> GetAll()
    {
        var presupuestos = _presupuestoRepositorio.obtenerPresupuestos();
        return Ok(presupuestos);
    }

    [HttpPost]
    public ActionResult<int> PostP(Presupuestos P)
    {
        int res = _presupuestoRepositorio.crearPresupuesto(P);
        return Ok(res);
    }

    [HttpGet]
    [Route("/ObtenerPorId")]
    public ActionResult<Presupuestos> ObetenerUno(int id)
    {
        var res = _presupuestoRepositorio.obtenerPresupuestoPorId(id);
        return Ok(res);
    }

    [HttpPost]
    [Route("/crearYAgregarProductoAPresupuesto")]
    public ActionResult<int> agregarProductoAPresupuesto(Productos p, int cant, int idPresupuesto)
    {
        _presupuestoRepositorio.crearYAgregarProductoAPresupuesto(p, cant, idPresupuesto);
        return Ok();
    }

    [HttpPost]
    [Route("/AgregarProductoAPresupuesto")]
    public ActionResult<int> agregarProducto(int idPro, int idPre, int cant)
    {
        int real = _presupuestoRepositorio.agregarProductoAPresupuesto(idPro, idPre, cant);
        if(real == -1)
        {
            return BadRequest();
        }

        return Ok(real);
    }

    [HttpDelete]
    public ActionResult<int> EliminarPresupuesto(int idPres)
    {
        int real = _presupuestoRepositorio.eliminarPresupuesto(idPres);

        if (real == -1)
        {
            return BadRequest("FIRST");
        }

        if(real == -2)
        {
            return BadRequest("SECOND");
        }

        return Ok("Filas afectadas: "+real);
    }
}
