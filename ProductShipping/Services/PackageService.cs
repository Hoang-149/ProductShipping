using ProductShipping.Models;

namespace ProductShipping.Services
{
    public class PackageService
    {
        private readonly DBContext _dbcontext;

        public PackageService(DBContext db)
        {
            _dbcontext = db;
        }

        public List<Package> SplitPackages(List<Product> products)
        {
            var packages = new List<Package>();
            var currentPackage = new Package();
            var totalWeight = products.Sum(p => p.Weight.GetValueOrDefault());
            var numberOfPackages = (int)Math.Ceiling(products.Sum(p => p.Price.GetValueOrDefault()) / 250);
            var targetWeight = totalWeight / numberOfPackages;

            foreach (var product in products.OrderByDescending(p => p.Price))
            {
                if (currentPackage.TotalPrice + product.Price > 250 || currentPackage.TotalWeight + product.Weight > targetWeight)
                {
                    AddPackage(currentPackage, packages);
                    currentPackage = new Package();
                }
                currentPackage.Items.Add(product);
                currentPackage.TotalPrice += product.Price.GetValueOrDefault();
                currentPackage.TotalWeight += product.Weight.GetValueOrDefault();
            }

            if (currentPackage.Items.Any())
            {
                AddPackage(currentPackage, packages);
            }

            return packages;
        }

        public void AddPackage(Package package, List<Package> packages)
        {
            package.CourierPrice = GetCourierPrice(package.TotalWeight);
            packages.Add(package);
        }

        public decimal GetCourierPrice(int weight)
        {
            var charge = _dbcontext.CourierCharges.FirstOrDefault(c => weight >= c.MinWeight && weight <= c.MaxWeight);
            return charge?.Charge ?? 0;
        }
    }
}
