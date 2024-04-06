using Microsoft.AspNetCore.Mvc;
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
		IUserRepository userRepository;
		IOrderItemsRepository orderItemsRepository;
		public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, ICommentRepository commentRepo,IUserRepository userRepo, IOrderItemsRepository orderItemsRepo)
		{
			productRepository = productRepo;
			categoryRepository = categoryRepo;
			commentRepository = commentRepo;
			userRepository = userRepo;
			orderItemsRepository= orderItemsRepo;
        }

		public IActionResult ProductDash(int page = 1)
		{
			int content = 9;
			int skip = (page - 1) * content;
            User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Category> ParamCategory = categoryRepository.GetAll();
			List<Product> products = productRepository.GetSpeceficProduct(skip, content, ParamCategory);
            int total = productRepository.GetProductcount(ParamCategory);
            ViewData["User"] = user;
            ViewData["Page"] = page;
            ViewData["content"] = content;
            ViewData["TotalItems"] = total;
            return View("ProductDash", products);
		}
        public IActionResult AdminProduct(int page = 1)
        {
            int content = 9;
            int skip = (page - 1) * content;
            User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Category> ParamCategory = categoryRepository.GetAll();
            List<Product> products = productRepository.GetForUser(skip, content,user.Id);
            int total = productRepository.GetProductcount(ParamCategory);
            ViewData["User"] = user;
            ViewData["Page"] = page;
            ViewData["content"] = content;
            ViewData["TotalItems"] = total;
            return View("AdminProduct", products);
        }
        public IActionResult Index(int page = 1)
        {
            int content = 9;
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

		public IActionResult Details(int id)
		{
			ViewData["ProductDetails"] = productRepository.GetProduct(id);
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["productExists"]= orderItemsRepository.productExists(id, userId);
           // var comment = commentRepository.GetForProduct(id);
            #region
            int content = 4;
			int page = 1;
            int skip = (page - 1) * content;
            List<Comment> comments = commentRepository.GetForProduct(id, skip, content);
            int total =  commentRepository.GetForProduct(id).Count();
            ViewBag.ProductID = id;
            ViewBag.Page = page;
            ViewBag.Content = content;
            ViewBag.TotalItems = total;
        
            #endregion

            return View("Details", comments);
		}

        public IActionResult CommentsPagination( int productId,int page = 1)
		{
            int content = 4;
            int skip = (page - 1) * content;
            List<Comment> comments = commentRepository.GetForProduct(productId, skip, content);
            int total = commentRepository.GetForProduct(productId).Count();
            ViewBag.ProductID = productId;
            ViewBag.Page = page;
            ViewBag.Content = content;
            ViewBag.TotalItems = total;
            return PartialView("_AllCommentsPartial", comments);
        }

        public IActionResult New()
		{
            User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var categories = categoryRepository.GetAll();
			ViewData["Categories"] = new SelectList(categories, "Id", "Title");
            ViewData["User"] = user;
            return View("NewProduct");
		}

		[HttpPost]
		public IActionResult SaveNew(ProductWithCategoryList product, IFormFile Img)
		{
            User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (Img != null && Img.Length > 0)
			{
				string FileName = Path.GetFileName(Img.FileName);
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
				return RedirectToAction("ProductDash");
			}

			ViewData["Category"] = categoryRepository.GetAll();
            ViewData["User"] = user;
            return View("NewProduct", product);
		}

		public IActionResult deleteProduct(int id)
		{
			var product = productRepository.GetById(id);
			if (product != null)
			{
				productRepository.delete(id);
				productRepository.save();
                return RedirectToAction("ProductDash");
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
				CategoryId = productData.CategoryId,
				UserID= productData.UserID
			};
            User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewData["User"] = user;
            return View(productViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ProductWithCategoryList product)
		{

			if (ModelState.IsValid == true)
			{
                var updatedProduct = productRepository.GetProduct(product.Id);
                updatedProduct.Title = product.Title;
                updatedProduct.Description = product.Description;
                updatedProduct.Img = product.Img;
                updatedProduct.Price = product.Price;
                updatedProduct.Stock = product.Stock;
                updatedProduct.CategoryId = product.CategoryId;
				updatedProduct.UserID = product.UserID;
                productRepository.Update(updatedProduct);
				productRepository.Save();
				return RedirectToAction("ProductDash");
			}
            User user = userRepository.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewData["User"] = user;
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
				int retryDelayMs = 100;

				for (int i = 0; i < retryAttempts; i++)
				{
					try
					{
						System.IO.File.Delete(oldPath);
						break;
					}
					catch (IOException)
					{
						Task.Delay(retryDelayMs).Wait();
					}
				}
				string FileName = Path.GetFileName(NewImg.FileName);
				string path = $"wwwroot/Images/{FileName}";
				FileStream fs = new FileStream(path, FileMode.Create);
				NewImg.CopyTo(fs);
				Product updated = new Product();
				updated = productRepository.GetById(product.Id);
				updated.Img = FileName;
				productRepository.Update(updated);
				productRepository.Save();
                return RedirectToAction("ProductDash");
            }
			return PartialView("_EditImagePartial", product);
		}


		// ---- Comment ---- //

		[HttpGet]
		public IActionResult comments(int id)
		{
			var comment = commentRepository.GetForProduct(id);
			return PartialView("_AllCommentsPartial", comment);
		}

		[HttpGet]
		public IActionResult Addcomment(int productId)
		{
			var commentViewModel = new CommentWithItsUser { ProductID = productId };
			return PartialView("__ProductAddCommentPartial", commentViewModel);
		}


		[HttpPost]
		public IActionResult AddComment(CommentWithItsUser comment)
		{
			
			string user = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (comment.Body != null && comment.ProductID != null)
			{
				var newComment = new Comment
				{
					Body = comment.Body,
					ProductID = comment.ProductID,
					UserId = user

				};

				commentRepository.Insert(newComment);
				commentRepository.Save();
                HttpContext.Session.SetInt32("productID", comment.ProductID);
                return RedirectToAction("CommentsPagination", "Product");
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