﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using OutfitO.Models;
using OutfitO.Repository;
using OutfitO.ViewModels;
using System.IO;
using System.Security.Claims;

namespace OutfitO.Controllers
{
	public class ProductController : Controller
	{
		IProductRepository productRepository;
		ICategoryRepository categoryRepository;
		ICommentRepository commentRepository;
		public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, ICommentRepository commentRepo)
		{
			productRepository = productRepo;
			categoryRepository = categoryRepo;
			commentRepository = commentRepo;

		}
		public IActionResult Index(int page = 1)
		{
			int content = 3;
			int skip = (page - 1) * content;
			List<Category> ParamCategory = categoryRepository.GetAll();
			List<Product> products = productRepository.GetSpeceficProduct(skip, content, ParamCategory);


			int total = productRepository.GetProductcount(ParamCategory);
			ViewData["Page"] = page;
			ViewData["content"] = content;
			ViewData["TotalItems"] = total;

			ViewData["Category"] = categoryRepository.GetAll();

            return View("Index", products);
		}

		[HttpPost]
        public IActionResult Filter(List<string> Params ,int page = 1)
        {
            int content = 3;
			int total = 0;
            int skip = (page - 1) * content;
			List<Product> products;

            if (Params.Count == 0)
			{
                List<Category> ParamCategory = categoryRepository.GetAll();
                total = productRepository.GetProductcount(ParamCategory);
				products = productRepository.GetSpeceficProduct(skip, content, ParamCategory);
			}
			else
			{
                total = productRepository.GetProductcount(Params);
                products = productRepository.GetSpeceficProduct(skip, content, Params);
			}


            ViewData["Page"] = page;
            ViewData["content"] = content;
            ViewData["TotalItems"] = total;

            return PartialView("_ProductStorePartial", products);
        }


        // details
        public IActionResult Details(int id)
		{
			var ProductDetails = productRepository.GetProduct(id);
			return View("Details", ProductDetails);
		}


		public IActionResult New()
		{

			var categories = categoryRepository.GetAll();
			ViewData["Categories"] = new SelectList(categories, "Id", "Title");
			return View("NewProduct");

		}

		[HttpPost]
		public IActionResult SaveNew(ProductWithCategoryList product, IFormFile Img)
		{
			if (Img != null && Img.Length > 0)
			{
				string FileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Img.FileName);
				string path = $"wwwroot/Images/{FileName}";
				FileStream fs = new FileStream(path, FileMode.Create);
				Img.CopyTo(fs);
				product.Img = FileName;
				//ModelState.SetModelValue("ProfileImage", new ValueProviderResult(FileName));
			}
			if (product.Title != null && product.Price != null && product.Stock != null && product.Id != null && product.Description != null && product.CategoryId != null)
			{
				var newProduct = new Product
				{

					Id = product.Id,
					Title = product.Title,
					Description = product.Description,
					Img = product.Img,
					Price = product.Price,
					Stock = product.Stock,
					CategoryId = product.CategoryId,
					UserID = User.FindFirstValue(ClaimTypes.NameIdentifier)
					//categories = categoryRepository.GetAll(),

				};
				productRepository.Insert(newProduct);
				productRepository.Save();
				return RedirectToAction("Index");
			}

			ViewData["Category"] = categoryRepository.GetAll();
			return View("NewProduct", product);
		}

		[HttpPost]
		public IActionResult deleteProduct(int id)
		{
			var product = productRepository.GetById(id);
			if (product != null)
			{
				productRepository.delete(id);
				productRepository.save();
				return RedirectToAction("index");
			}
			else
			{
				return NotFound();
			}
		}

		public IActionResult Edit(int id)
		{
			Product productData = productRepository.GetById(id);
			if (productData == null)
			{
				return NotFound();
			}



			var categories = categoryRepository.GetAll();
			ViewBag.Categories = new SelectList(categories, "Id", "Title");

			ProductWithCategoryList productViewModel = new ProductWithCategoryList
			{
				Id = productData.Id,
				Title = productData.Title,
				Description = productData.Description,
				Img = productData.Img,
				Price = productData.Price,
				Stock = productData.Stock,
				CategoryId = productData.CategoryId
			};

			return View(productViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ProductWithCategoryList product)
		{
			if (ModelState.IsValid == true)
			{
				Product updatedProduct = new Product
				{
					Id = product.Id,
					Title = product.Title,
					Description = product.Description,
					Img = product.Img,
					Price = product.Price,
					Stock = product.Stock,
					CategoryId = product.CategoryId
				};

				productRepository.Update(updatedProduct);
				productRepository.Save();
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}


		[HttpGet]
		public IActionResult EditImage(int id)
		{
			var ProductDetails = productRepository.GetById(id);
			productImageVM vm = new productImageVM()
			{
				Id = id,
				OldImg = ProductDetails.Img,
			};
			return PartialView("_EditImagePartial", vm);
		}
		[HttpPost]
		public IActionResult EditImage(productImageVM product, IFormFile NewImg)
		{
			if (NewImg != null && NewImg.Length > 0)
			{
				string oldImg = product.OldImg;
				string oldPath = $"wwwroot/Images/{oldImg}";
				int retryAttempts = 3;
				int retryDelayMs = 100; // milliseconds

				for (int i = 0; i < retryAttempts; i++)
				{
					try
					{
						System.IO.File.Delete(oldPath);
						break; // Break out of the loop if deletion is successful
					}
					catch (IOException)
					{
						// Log or handle the exception
						// Wait for a short delay before retrying
						Task.Delay(retryDelayMs).Wait();
					}
				}
				string FileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(NewImg.FileName);
				string path = $"wwwroot/Images/{FileName}";
				FileStream fs = new FileStream(path, FileMode.Create);
				NewImg.CopyTo(fs);
				Product updated = new Product();
				updated = productRepository.GetById(product.Id);
				updated.Img = FileName;
				productRepository.Update(updated);
				productRepository.Save();
				return RedirectToAction("Index");
			}
			return PartialView("_EditImagePartial", product);
		}



		// ---- Comment ---- //

		[HttpGet]
		public IActionResult Addcomment(int productId)
		{
			var commentViewModel = new CommentWithItsUser { ProductID = productId };
			return PartialView("__ProductAddCommentPartial", commentViewModel);
		}

		//[HttpPost]
		//public IActionResult Addcomment(CommentWithItsUser comment)
		//{
		//    if (ModelState.IsValid == true)
		//    {
		//        var newComment = new Comment
		//        {
		//            Body = comment.Body,
		//            ProductID = comment.ProductID,
		//        };

		//        commentRepository.Insert(newComment);
		//        commentRepository.Save();
		//        return RedirectToAction("Details", "Product", new { id = comment.ProductID });
		//    }
		//    return PartialView("_ProductAddCommentPartial", comment);
		//}
		[HttpPost]
		public IActionResult AddComment(CommentWithItsUser comment)
		{
			if (ModelState.IsValid)
			{
				var newComment = new Comment
				{
					Body = comment.Body,
					ProductID = comment.ProductID,
				};

				commentRepository.Insert(newComment);
				commentRepository.Save();
				return RedirectToAction("Details", "Product", new { id = comment.ProductID });
			}
			return PartialView("__ProductAddCommentPartial", comment);
		}
		public IActionResult Editcomment(int commentId)
		{
			Comment commentData = commentRepository.GetById(commentId);
			if (commentData == null)
			{
				return NotFound();
			}

			CommentWithItsUser commentViewModel = new CommentWithItsUser
			{
				Id = commentData.Id,
				Body = commentData.Body,
				ProductID = commentData.ProductID
			};

			return PartialView("_EditCommentPartial", commentViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditComment(CommentWithItsUser comment)
		{
			if (ModelState.IsValid)
			{
				Comment existingComment = commentRepository.GetById(comment.Id);
				if (existingComment == null)
				{
					return NotFound();
				}

				existingComment.Body = comment.Body;

				commentRepository.Update(existingComment);
				commentRepository.Save();
				return RedirectToAction("Details", "Product", new { id = existingComment.ProductID });
			}
			return PartialView("_EditCommentPartial", comment);
		}


		[HttpPost]
		public IActionResult DeleteComment(int commentId)
		{
			var comment = commentRepository.GetById(commentId);
			if (comment != null)
			{
				commentRepository.Delete(commentId);
				commentRepository.Save();
			}
			// Redirect back to the product details page or wherever appropriate
			return RedirectToAction("Details", "Product", new { id = comment?.ProductID });
		}





	}
}