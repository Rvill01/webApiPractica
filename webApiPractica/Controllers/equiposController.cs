using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public equiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;


        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<equipos> listadoEquipo = (from e in _equiposContext.equipos
                                           select e).ToList();
            if (listadoEquipo.Count()== 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("GetbyId¨/{id}")]
        public IActionResult Get(int id)
        {
            equipos? equipo=(from e in _equiposContext.equipos
                             where e.id_equipos==id
                             select e).FirstOrDefault();
            if(equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription (string filtro) {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Guardar([FromBody]equipos equipo)
        {
            try {
            _equiposContext.equipos.Add(equipo);
            _equiposContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("Actualizar/{id}")]

        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoMod) 
        {
            equipos? equipoActual=(from e in _equiposContext.equipos
                                   where e.id_equipos ==id
                                   select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();

            }
            equipoActual.nombre=equipoMod.nombre;
            equipoActual.descripcion = equipoMod.descripcion;
            equipoActual.marca_id = equipoMod.marca_id;
            equipoActual.tipo_equipo_id = equipoMod.tipo_equipo_id;
            equipoActual.anio_compra = equipoMod.anio_compra;
            equipoActual.costo = equipoMod.costo;

            _equiposContext.Entry(equipoActual).State= EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(equipoMod);
        }

        [HttpDelete]
        [Route ("Eliminar/{id}")]

        public IActionResult eliminarEquipo(int id)
        {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();

            }
            _equiposContext.equipos.Attach(equipo);
            _equiposContext.equipos.Remove(equipo);
            _equiposContext.SaveChanges();
            return Ok(equipo);
        }

    }
}
