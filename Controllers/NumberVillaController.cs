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
using System.Net;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberVillaController : ControllerBase
    {
        private readonly ILogger<NumberVillaController> _logger;
        //private readonly ApplicationDbContext _dbContext;
        private readonly IVillaRepository _villaRepo;
        private readonly INumberVillaRepository _villaNumberRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumberVillaController(ILogger<NumberVillaController> logger, IVillaRepository villaRepo, IMapper mapper, INumberVillaRepository villaNumberRepo)
        {

            _logger = logger;
            _villaRepo=villaRepo;
            _villaNumberRepo = villaNumberRepo;
            _mapper = mapper;
            _response=new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumberVillas()
        {
            try
            {
                _logger.LogInformation("Obtener Numero Villas");
                IEnumerable<NumberVilla> villaNumeroList = await _villaNumberRepo.GetAll();
                _response.Result = _mapper.Map<IEnumerable<NumberVillaDto>>(villaNumeroList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso=false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            
            }
            return _response;

        }

        [HttpGet("id:int", Name ="GetNumberVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetNumberVilla(int id)
        {
            try
            {
                _response.ErrorMessages = new List<string>();
                if (id == 0)
                {
                    _logger.LogError("Error al traer numero villa con Id: " + id);
                    _response.ErrorMessages.Clear();
                    _response.ErrorMessages.Add("Error al traer numerovilla con Id: " + id);
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villaNumber = await _villaNumberRepo.Get(v => v.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Clear();
                    _response.ErrorMessages.Add("No se encontro villa con el Id: " + id);
                    _response.IsExitoso= false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<NumberVillaDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateNumberVilla([FromBody] NumberVillaCreateDto createNumberVillaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _villaNumberRepo.Get(v => v.VillaNo == createNumberVillaDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("Nombre Existe", "El numero de villa ya existe!");
                    return BadRequest(ModelState);
                };

                if(await _villaRepo.Get(v=>v.Id== createNumberVillaDTO.VillaId)==null)
                {
                    ModelState.AddModelError("Clave Foranea", "El Id de la villa no existe!");
                    return BadRequest(ModelState);
                }

                if (createNumberVillaDTO == null)
                {
                    return BadRequest(createNumberVillaDTO);
                }


                NumberVilla modelo = _mapper.Map<NumberVilla>(createNumberVillaDTO);
                modelo.FechaCreacion=DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _villaNumberRepo.Create(modelo);

                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumberVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;


        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] NumberVillaUpdateDto updateNumberVillaDTO)
        {
            try
            {
                if (updateNumberVillaDTO == null || id != updateNumberVillaDTO.VillaNo)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _villaRepo.Get(v=>v.Id== updateNumberVillaDTO.VillaId)==null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la Villa no existe");
                    return BadRequest(ModelState);
                }


                NumberVilla modelo = _mapper.Map<NumberVilla>(updateNumberVillaDTO);

                await _villaNumberRepo.Update(modelo);
                _response.StatusCode= HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id,  JsonPatchDocument<NumberVillaUpdateDto> patchNumberDTO)
        {
            try
            {
                if (patchNumberDTO == null || id == 0)
                {
                    return BadRequest(patchNumberDTO);
                }

                var villa = await _villaRepo.Get(x => x.Id == id, tracked: false);

                NumberVillaUpdateDto villaNumberDTO = _mapper.Map<NumberVillaUpdateDto>(villa);

                if (villa == null)
                {
                    return BadRequest();
                }



                patchNumberDTO.ApplyTo(villaNumberDTO, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                NumberVilla modelo = _mapper.Map<NumberVilla>(villaNumberDTO);
                await _villaNumberRepo.Update(modelo);
                _response.StatusCode=HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNumberVilla(int id)
        {
            try
            {
                if (id==0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);    
                }
                var numberVilla = await _villaNumberRepo.Get(x => x.VillaNo == id);

                if (numberVilla == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _villaNumberRepo.Delete(numberVilla);
                _response.StatusCode=HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);

        }
    }
}
