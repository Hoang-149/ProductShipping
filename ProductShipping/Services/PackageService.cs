using ProductShipping.Models;

namespace ProductShipping.Services
{
    public class PackageService
    {
        private readonly DBContext _dbcontext;
        private const decimal MaxPackagePrice = 250m;

        public PackageService(DBContext db)
        {
            _dbcontext = db;
        }

        public List<Package> SplitPackages(List<Product> products)
        {
            var totalPrice = products.Sum(p => p.Price.GetValueOrDefault());
            var totalWeight = products.Sum(p => p.Weight.GetValueOrDefault());

            if (totalPrice <= MaxPackagePrice)
            {
                return new List<Package> { CreatePackage(products) };
            }

            int numberOfPackages = (int)Math.Ceiling(totalPrice / MaxPackagePrice);
            return DistributeProducts(products, numberOfPackages);
        }

        private List<Package> DistributeProducts(List<Product> products, int numberOfPackages)
        {
            var packages = Enumerable.Range(0, numberOfPackages).Select(_ => new Package()).ToList();
            var sortedProducts = products.OrderByDescending(p => p.Price * p.Weight).ToList();

            foreach (var product in sortedProducts)
            {
                var targetPackage = packages
                    .Where(p => p.TotalPrice + product.Price <= MaxPackagePrice)
                    .OrderBy(p => p.TotalWeight)
                    .FirstOrDefault();

                if (targetPackage == null)
                {
                    targetPackage = new Package();
                    packages.Add(targetPackage);
                }

                targetPackage.Items.Add(product);
                targetPackage.TotalPrice += product.Price.GetValueOrDefault();
                targetPackage.TotalWeight += product.Weight.GetValueOrDefault();
            }

            packages.RemoveAll(p => !p.Items.Any());

            foreach (var package in packages)
            {
                package.CourierPrice = GetCourierPrice(package.TotalWeight);
            }

            return packages;
        }

        private Package CreatePackage(List<Product> products)
        {
            var package = new Package
            {
                Items = products,
                TotalPrice = products.Sum(p => p.Price.GetValueOrDefault()),
                TotalWeight = products.Sum(p => p.Weight.GetValueOrDefault())
            };
            package.CourierPrice = GetCourierPrice(package.TotalWeight);
            return package;
        }

        public decimal GetCourierPrice(int weight)
        {
            var charge = _dbcontext.CourierCharges.FirstOrDefault(c => weight >= c.MinWeight && weight <= c.MaxWeight);
            return charge?.Charge ?? 0;
        }
    }
}