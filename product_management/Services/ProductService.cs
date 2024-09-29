using ProductManagement.Models;
using ProductManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProductManagement.Services
{
    public class ProductService
    {
        private static readonly ProductRepository productRepository = new ProductRepository();

        public List<ProductDTO> GetAllProducts(Expression<Func<Product, bool>> predicate = null)
        {
            return productRepository.GetAllProducts(predicate);
        }

        public void SaveProduct(ProductDTO animalDTO)
        {
            productRepository.SaveProduct(animalDTO);
        }

        public void DeleteProduct(Func<Product, bool> predicate)
        {
            productRepository.DeleteProduct(predicate);
        }

        public ProductDTO GetMaxPriceProduct()
        {
            return productRepository.GetMaxPriceProduct();
        }

        public void DeleteAllProduct()
        {
            productRepository.DeleteAllProduct();
        }
    }
}
