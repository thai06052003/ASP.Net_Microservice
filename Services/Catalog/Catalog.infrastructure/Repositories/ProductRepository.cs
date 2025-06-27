
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypeRepository
    {
        public ICatalogContext _context { get; }
        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Lấy ra tất cả sản phẩm
        /// </summary>
        /// <param name="catalogSpecParams">Điều kiện lọc</param>
        /// <returns>Danh sách sản phẩm</returns>
        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;

            var filter = Builders<Product>.Filter.Empty;
            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                var brandFilter = builder.Eq(p => p.Brands.Id, catalogSpecParams.BrandId);
                filter &= brandFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                var typeFilter = builder.Eq(p => p.Types.Id, catalogSpecParams.TypeId);
                filter &= typeFilter;
            }
            var totalItems = await _context.Products.CountDocumentsAsync(filter);
            var data = await DataFilter(catalogSpecParams, filter);

            return new Pagination<Product>(
                catalogSpecParams.PageIndex,
                catalogSpecParams.PageSize,
                (int)totalItems,
                data
            );
        }
        /// <summary>
        /// Lấy ra sản phẩm tương ứng với id
        /// </summary>
        /// <param name="id">ID sản phẩm</param>
        /// <returns>Sản phẩm tương ứng</returns>
        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Lấy ra danh sách sản phẩm tương ứng với tên sản phẩm tìm kiếm
        /// </summary>
        /// <param name="name">Tên sản phẩm tím kiếm</param>
        /// <returns>Danh sách tên sản phẩm tương ứng</returns>
        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            return await _context.Products.Find(p => p.Name.ToLower() == name.ToLower()).ToListAsync();
        }
        /// <summary>
        /// Lấy ra danh sách sản phẩm tương ứng với thương hiệu sản phẩm tìm kiếm
        /// </summary>
        /// <param name="brandName">Thương hiệu sản phẩm tím kiếm</param>
        /// <returns>Danh sách thương hiệu sản phẩm tương ứng</returns>
        public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
        {
            return await _context.Products.Find(p => p.Brands.Name.ToLower() == brandName.ToLower()).ToListAsync();
        }
        /// <summary>
        /// Tạo sản phẩm mới
        /// </summary>
        /// <param name="product">Sản phẩm được tạo</param>
        /// <returns>Sản phẩm được tạo</returns>
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }
        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="id">ID sản phẩm bị xóa</param>
        /// <returns>True nếu xóa được; False nếu không xóa được</returns>
        public async Task<bool> DeleteProduct(string id)
        {
            var deletedProduct = await _context.Products.DeleteOneAsync(p => p.Id == id);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }
        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="product">Thông tin sản phẩm được cập nhật</param>
        /// <returns>True nếu xóa được; False nếu không xóa được</returns>
        public async Task<bool> UpdateProduct(Product product)
        {
            var updateProduct = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }
        /// <summary>
        /// Lấy ra danh sách thể loại
        /// </summary>
        /// <returns>Danh sách thể loại</returns>
        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.Types.Find(t => true).ToListAsync();
        }
        /// <summary>
        /// Danh sách thương hiệu
        /// </summary>
        /// <returns>Danh sách thương hiệu</returns>

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.Brands.Find(b => true).ToListAsync();
        }
        /// <summary>
        /// Thực hiện lọc và sắp xếp sản phẩm
        /// </summary>
        /// <param name="catalogSpecParams">Điều kiện lọc</param>
        /// <param name="filter">câu truy vấn</param>
        /// <returns>Danh sách sản phẩm tương ứng</returns>
        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sortDefn = Builders<Product>.Sort.Ascending("Name");
            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                switch (catalogSpecParams.Sort)
                {
                    case "priceAsc":
                        sortDefn = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sortDefn = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDefn = Builders<Product>.Sort.Ascending(p => p.Name);
                        break;
                }
            }
            return await _context.Products
                .Find(filter)
                .Sort(sortDefn)
                .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync();
        }
    }
}
