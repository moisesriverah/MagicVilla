using Azure;
using MagicVillaAPI.Datos;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext dbContext)
        {

            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener las Villas");
            return Ok(_dbContext.Villas.ToList());
        }

        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
           if (id == 0)
            {
                _logger.LogError("Error al traer villa con Id: " + id);
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa =_dbContext.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDTO)
        {
            if(!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }
           if(_dbContext.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Nombre Existe", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            };
        
            if (villaDTO==null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0) 
            { 
                return BadRequest();
            }
            // villaDTO.Id= VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+1;
            //  VillaStore.villaList.Add(villaDTO);
            Villa modelo = new()
            {
                Nombre = villaDTO.Nombre,
                Amenidad = villaDTO.Amenidad,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados=villaDTO.MetrosCuadrados,
                Ocupantes=villaDTO.Ocupantes
            };
            _dbContext.Villas.Add(modelo);
            _dbContext.SaveChanges();


            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDTO)
        {
            if (villaDTO==null || id !=villaDTO.Id)
            {
                return BadRequest(villaDTO);
            }
            Villa modelo = new()
            {
              Id = id,
                Nombre = villaDTO.Nombre,
                Amenidad = villaDTO.Amenidad,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Ocupantes = villaDTO.Ocupantes
            };
            _dbContext.Villas.Update(modelo);

            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id,  JsonPatchDocument<VillaDto> patchDTO)
        {
            if (patchDTO == null || id ==0)
            {
                return BadRequest(patchDTO);
            }
            var villa = _dbContext.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            VillaDto villaDTO = new()
            {
                Id = id,
                Nombre = villa.Nombre,
                Amenidad = villa.Amenidad,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Ocupantes = villa.Ocupantes
            };

            if (villa==null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villaDTO,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = new()
            {
                Id=villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Amenidad = villaDTO.Amenidad,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Ocupantes = villaDTO.Ocupantes
            };
            _dbContext.Villas.Update(modelo);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteVilla(int id)
        {
            var villa = _dbContext.Villas.FirstOrDefault(x => x.Id == id);
           
            if (villa==null)
            {
                return NotFound();
            }
            _dbContext.Villas.Remove(villa);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
