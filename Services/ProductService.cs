using BankProductsAPI.Models;

namespace BankProductsAPI.Services;

public class ProductService
{
    private readonly object _lock = new();
    private int _nextId = 6;

    private readonly List<BankProduct> _products = new()
    {
        new BankProduct { Id = 1, Name = "Потребительский кредит", Description = "Кредит на любые цели без залога", InterestRate = 14.5m, IsActive = true },
        new BankProduct { Id = 2, Name = "Ипотека", Description = "Кредит на покупку жилья с первоначальным взносом", InterestRate = 9.8m, IsActive = true },
        new BankProduct { Id = 3, Name = "Накопительный вклад", Description = "Вклад с ежемесячной капитализацией процентов", InterestRate = 12m, IsActive = true },
        new BankProduct { Id = 4, Name = "Кредитная карта", Description = "Карта с льготным периодом до 55 дней", InterestRate = 22.9m, IsActive = true },
        new BankProduct { Id = 5, Name = "Автокредит", Description = "Кредит на покупку нового автомобиля", InterestRate = 16.5m, IsActive = false }
    };

    public List<BankProduct> GetAll()
    {
        lock (_lock)
        {
            return _products.ToList();
        }
    }

    public BankProduct? Get(int id)
    {
        lock (_lock)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }

    public BankProduct Add(BankProduct product)
    {
        lock (_lock)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }
    }

    public BankProduct? Update(int id, BankProduct product)
    {
        lock (_lock)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.InterestRate = product.InterestRate;
            existing.IsActive = product.IsActive;
            return existing;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _products.RemoveAll(p => p.Id == id) > 0;
        }
    }
}
