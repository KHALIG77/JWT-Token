using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.DTOs.BrandDTOs;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Data;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandsController(IBrandRepository brandRepository,IMapper mapper)
        {
            
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        [HttpPost("")]
        public ActionResult<BrandPostDTO> Create(BrandPostDTO brandPostDTO)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Id", "Is Required");
                return BadRequest(ModelState);
            }
            Brand brand = _mapper.Map<Brand>(brandPostDTO);
            _brandRepository.Add(brand);
            _brandRepository.Commit();
            return StatusCode(201, brand.Id);
        }
        [HttpPut("{id}")]
        public IActionResult Edit(int id,BrandPutDTO brandPutDTO)
        {
            Brand existBrand = _brandRepository.Get(x=>x.Id==id);
            if(existBrand == null)
            {
                return NotFound();
            }
            existBrand.Name=brandPutDTO.Name;
            _brandRepository.Commit();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Brand brand=_brandRepository.Get(x=>x.Id==id);
            if(brand == null)
            {
                return NotFound();

            }
            _brandRepository.Remove(brand);
            _brandRepository.Commit();
            return NoContent();
        }
        [HttpGet("all")]
        public ActionResult<List<BrandGetAllItemDTO>> GetAll()
        {
            var data = _mapper.Map<List<BrandGetAllItemDTO>>(_brandRepository.GetAll(x=>true));
            return Ok(data);
        }

    }
}
