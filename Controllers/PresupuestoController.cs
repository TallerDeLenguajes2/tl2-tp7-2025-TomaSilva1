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
}
