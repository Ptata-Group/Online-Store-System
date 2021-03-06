using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.DTOs.ProductImagesDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Interfaces.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/{productId}/Image")]
    public class ProductImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IGenericRepository<Product, int> _productRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public ProductImageController(IImageRepository imageRepository,
            IGenericRepository<Product, int> productRepo, IUserRepository userRepo, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IReadOnlyList<ProductImagesForReturnDto>> GetImagesForProduct(int productId)
        {
            var images = await _imageRepository.GetAllImagesForProduct(productId);
            var imagesForReturn =
                _mapper.Map<IReadOnlyList<ProductImage>, IReadOnlyList<ProductImagesForReturnDto>>(images);
            return imagesForReturn;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductImagesForReturnDto>> GetImage(int id)
        {
            var image = await _imageRepository.GetImageById(id);
            if (image == null)
                return NotFound("this image is not exist.");
            var imageForReturn = _mapper.Map<ProductImagesForReturnDto>(image);
            return imageForReturn;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddImage(int productId, [FromForm] ProductImageForAddDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var product = await _productRepo.GetById(productId);
            if (product == null)
                return BadRequest("You can't add image for product not exist.");

            if (product.UserId != user.Id)
                return Unauthorized("You cannot add an image product owned by another user");

            var images = await _imageRepository.GetAllImagesForProduct(productId);
            if (images.Count == 4)
                return BadRequest("Sorry, you can't add more than 4 images to your product.");

            var file = entity.File;
            if (file == null)
                return BadRequest("Please add photo to your product.");

            var path = Path.Combine("wwwroot/images/", file.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            await stream.DisposeAsync();

            var imageForAdd = _mapper.Map<ProductImage>(entity);
            imageForAdd.ProductId = productId;
            if (images.Count == 0)
                imageForAdd.IsMainPhoto = true;
            imageForAdd.ImageUrl = path.Substring(7);

            if (await _imageRepository.AddImage(imageForAdd))
                return Ok();

            throw new Exception("Error happen when add photo to your product");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> SetImageMain(int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var image = await _imageRepository.GetImageById(id);
            var product = await _productRepo.GetById(image.ProductId);

            if (product.UserId != user.Id)
                return Unauthorized("You cannot Update an image product owned by another user");

            if (await _imageRepository.SetMainImage(id))
                return Ok();
            throw new Exception("Error happen when set image main");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var image = await _imageRepository.GetImageById(id);
            var product = await _productRepo.GetById(image.ProductId);

            if (product.UserId != user.Id)
                return Unauthorized("You cannot Delete an image product owned by another user");

            if (System.IO.File.Exists("wwwroot" + image.ImageUrl))
                System.IO.File.Delete("wwwroot" + image.ImageUrl);
            else
                return NotFound("Not Found Image in Server.");

            if (await _imageRepository.DeleteImage(id))
                return Ok();
            throw new Exception("Error happen when remove image");
        }
    }
}