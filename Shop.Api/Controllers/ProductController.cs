using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.DTOs.ProductDTOs;
using Shop.Core.Entities;
using Shop.Core.Repositories;

namespace Shop.Api.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository,IBrandRepository brandRepository,IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        [HttpPost("")]
        
        public ActionResult<ProductPostDTO> Create(ProductPostDTO postDTO)
        {
            if (!_brandRepository.IsExist(x=>x.Id==postDTO.BrandId))
            {
                ModelState.AddModelError("BrandId", "BrandId not found");
                return BadRequest(ModelState);
            }
            Product product= _mapper.Map<Product>(postDTO); 
            _productRepository.Add(product);
            _productRepository.Commit();


            return StatusCode(201,new { id = product.Id });
        }
        [HttpGet("all")]
      
        public ActionResult<List<ProductGetAllItemDTO>> GetAll()
        {
            var data = _mapper.Map<List<ProductGetAllItemDTO>>(_productRepository.GetAll(x=>true,"Brand"));

            return Ok(data);
        }
        [HttpPut("{id}")]

        public ActionResult Update(int id,ProductPutDTO productPutDTO)
        {
            Product entity=_productRepository.Get(x=>x.Id==id);
            if (entity == null) return NotFound();

            if(!_brandRepository.IsExist(x => x.Id == productPutDTO.BrandId))
            {
                ModelState.AddModelError("BrandId", "BrandId not found");
                return BadRequest(ModelState);
            }
            entity.SalePrice = productPutDTO.SalePrice;
            entity.DiscountPercent = productPutDTO.DiscountPercent;
            entity.CostPrice= productPutDTO.CostPrice;
            entity.Name= productPutDTO.Name;
            entity.BrandId= productPutDTO.BrandId;

            _productRepository.Commit();
            return NoContent();
        }
        [HttpGet("{id}")]
        public ActionResult<ProductGetDTO> Get(int id)
        {
            Product product=_productRepository.Get(x=>x.Id == id,"Brand");
            if(product == null) return NotFound();
            var data = _mapper.Map<ProductGetDTO>(product);
            return StatusCode(201, data);
        }
    }
}
