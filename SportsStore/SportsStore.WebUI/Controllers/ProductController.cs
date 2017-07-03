using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public int PageSize = 2;
        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            var model = new ProductsListViewModel()
            {
                Products = repository.Products.Where(x => category == null || x.Category == category)
                                           .OrderBy(p => p.ProductID)
                                           .Skip((page - 1) * PageSize)
                                           .Take(PageSize),
                CurrentCategory = category,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Where(x => category == null || x.Category == category).Count()

                }
            };
            return View(model);
        }
    }
}