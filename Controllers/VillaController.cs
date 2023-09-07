using AutoMapper;
using Azure;
using MagicVillaAPI.Datos;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dto;
using MagicVillaAPI.Repository.IRepository;
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
        //private readonly ApplicationDbContext _dbContext;
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepo, IMapper mapper)
        {

            _logger = logger;
            _villaRepo=villaRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las Villas");
            IEnumerable<Villa> villaList = await _villaRepo.GetAll();
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }

        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
           if (id == 0)
            {
                _logger.LogError("Error al traer villa con Id: " + id);
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa =await _villaRepo.Get(v=>v.Id==id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createVillaDTO)
        {
            if(!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }
           if(await _villaRepo.Get(v => v.Nombre.ToLower() == createVillaDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Nombre Existe", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            };
        
            if (createVillaDTO == null)
            {
                return BadRequest(createVillaDTO);
            }
     
            // villaDTO.Id= VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+1;
            //  VillaStore.villaList.Add(villaDTO);
          /*  Villa modelo = new()
            {
                Nombre = villaDTO.Nombre,
                Amenidad = villaDTO.Amenidad,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados=villaDTO.MetrosCuadrados,
                Ocupantes=villaDTO.Ocupantes
            };*/

            Villa modelo=_mapper.Map<Villa>(createVillaDTO);
            await _villaRepo.Create(modelo);



            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateVillaDTO)
        {
            if (updateVillaDTO == null || id != updateVillaDTO.Id)
            {
                return BadRequest(updateVillaDTO);
            }
          /*  Villa modelo = new()
            {
              Id = id,
                Nombre = villaDTO.Nombre,
                Amenidad = villaDTO.Amenidad,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Ocupantes = villaDTO.Ocupantes
            };*/

            Villa modelo = _mapper.Map<Villa>(updateVillaDTO);

            _dbContext.Villas.Update(modelo);

           await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id,  JsonPatchDocument<VillaUpdateDto> patchDTO)
        {
            if (patchDTO == null || id ==0)
            {
                return BadRequest(patchDTO);
            }
            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            /* VillaUpdateDto villaDTO = new()
             {
                 Id = id,
                 Nombre = villa.Nombre,
                 Amenidad = villa.Amenidad,
                 Detalle = villa.Detalle,
                 ImagenUrl = villa.ImagenUrl,
                 Tarifa = villa.Tarifa,
                 MetrosCuadrados = villa.MetrosCuadrados,
                 Ocupantes = villa.Ocupantes
             };*/
            VillaUpdateDto villaDTO = _mapper.Map<VillaUpdateDto>(villa);

            if (villa==null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villaDTO,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*Villa modelo = new()
            {
                Id=villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Amenidad = villaDTO.Amenidad,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Ocupantes = villaDTO.Ocupantes
            };*/
            Villa modelo = _mapper.Map<Villa>(villaDTO);
            _dbContext.Villas.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var villa = await _villaRepo.Get(x => x.Id == id);
           
            if (villa==null)
            {
                return NotFound();
            }
            await _villaRepo.Delete(villa);
          
            return NoContent();
        }
    }
}
